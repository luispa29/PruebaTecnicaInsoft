USE pruebatecnicaInsoft;
GO

CREATE OR ALTER PROCEDURE SP_Vehiculo_ObtenerOCrearPorPlaca
    @Placa    VARCHAR(20),
    @Marca    VARCHAR(50) = NULL,
    @Modelo   VARCHAR(50) = NULL,
    @Anio     INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @VehiculoID INT;

    SELECT @VehiculoID = VehiculoID
    FROM Vehiculos
    WHERE Placa = @Placa;

    IF @VehiculoID IS NULL
    BEGIN
        INSERT INTO Vehiculos (Placa, Marca, Modelo, Anio)
        VALUES (@Placa, @Marca, @Modelo, @Anio);

        SET @VehiculoID = SCOPE_IDENTITY();
    END

    SELECT @VehiculoID AS VehiculoID;
END;
GO

CREATE OR ALTER PROCEDURE SP_Cita_ConsultarPorPlaca
    @Placa         VARCHAR(20),
    @NumeroPagina  INT = 1,
    @TamanoPagina  INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@NumeroPagina - 1) * @TamanoPagina;

    SELECT
        c.CitaID,
        v.Placa,
        v.Marca,
        v.Modelo,
        v.Anio,
        c.FechaCita,
        CONVERT(VARCHAR(5), c.HoraCita, 108) AS HoraCita,
        c.Descripcion,
        c.Estado,
        c.FechaCreacion
    FROM Citas c
    INNER JOIN Vehiculos v ON v.VehiculoID = c.VehiculoID
    WHERE v.Placa = @Placa
    ORDER BY c.FechaCita DESC, c.HoraCita DESC
    OFFSET @Offset ROWS FETCH NEXT @TamanoPagina ROWS ONLY;

    SELECT COUNT(*) AS Total
    FROM Citas c
    INNER JOIN Vehiculos v ON v.VehiculoID = c.VehiculoID
    WHERE v.Placa = @Placa;
END;
GO

CREATE OR ALTER PROCEDURE SP_Cita_ExisteEnHorario
    @FechaCita DATE,
    @HoraCita  TIME
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM Citas
        WHERE FechaCita = @FechaCita
          AND HoraCita = @HoraCita
          AND Estado <> 'Cancelada'
    )
        SELECT CAST(1 AS BIT) AS Existe;
    ELSE
        SELECT CAST(0 AS BIT) AS Existe;
END;
GO

CREATE OR ALTER PROCEDURE SP_Cita_Crear
    @VehiculoID  INT,
    @FechaCita   DATE,
    @HoraCita    TIME,
    @Descripcion NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Citas (VehiculoID, FechaCita, HoraCita, Descripcion, Estado)
    VALUES (@VehiculoID, @FechaCita, @HoraCita, @Descripcion, 'Pendiente');

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS CitaID;
END;
GO

CREATE OR ALTER PROCEDURE SP_Horario_ObtenerOcupados
    @FechaCita DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CONVERT(VARCHAR(5), HoraCita, 108) AS HoraCita
    FROM Citas
    WHERE FechaCita = @FechaCita
      AND Estado <> 'Cancelada';
END;
GO
