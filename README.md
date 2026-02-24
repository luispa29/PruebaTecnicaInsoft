# Sistema de Gestion de Citas Vehiculares

Este proyecto es una solucion integral para la gestion de citas de mantenimiento vehicular, compuesta por un backend en .NET 8 y un frontend en Angular 20.

## Video de Explicacion

Para una guia detallada sobre el funcionamiento, configuracion y uso del sistema, por favor consulte el archivo: `Video de explicacion.mp4` que se encuentra en la raiz de este repositorio.

## Estructura del Proyecto

- **Backend**: Carpeta `Backend/` que contiene la solucion `ApiCitas` implementada con Arquitectura Limpia.
- **Frontend**: Carpeta `Frontend/CitasFront` con la aplicacion web desarrollada en Angular utilizando Angular Material.
- **Base de Datos**: Carpeta `Backend/BaseDatos` con los scripts SQL necesarios para la configuracion.

## Requisitos Previos

- SQL Server (con soporte para Windows Authentication).
- .NET 8 SDK.
- Node.js (Version LTS recomendada).
- Angular CLI (Version 19 o superior).

## Configuracion de la Base de Datos

1. Abra SQL Server Management Studio (SSMS).
2. Conectese a su instancia local.
3. Ejecute el script de base de datos ubicado en:
   `Backend/BaseDatos/BaseInicial.sql`
   Este script creara la base de datos `pruebatecnicaInsoft`, las tablas `Vehiculos` y `Citas` con datos de prueba, ademas de todos los procedimientos almacenados necesarios.

## Levantamiento del Backend

1. Dirijase a la carpeta `Backend/`.
2. Abra la solucion `ApiCitas.sln` en Visual Studio 2022 o VS Code.
3. Revise el archivo `ApiCitas/appsettings.json` y asegurese de que la cadena de conexion `DatabaseConnection` apunte correctamente a su servidor SQL Server.
4. Ejecute el proyecto `ApiCitas`. Por defecto, la API estara disponible en `http://localhost:5005` o el puerto configurado en sus Launch Settings.
5. Puede verificar los endpoints utilizando la interfaz de Swagger en `http://localhost:5005/swagger`.

## Levantamiento del Frontend

1. Abra una terminal y dirijase a la carpeta:
   `Frontend/CitasFront`
2. Instale las dependencias del proyecto:
   ```bash
   npm install
   ```
3. Inicie el servidor de desarrollo:
   ```bash
   ng serve -o
   ```
4. La aplicacion se abrira automaticamente en `http://localhost:4200`.

## Uso del Sistema

### Historial de Citas

1. Ingrese al modulo **Historial de Citas** desde el menu lateral.
2. Ingrese el numero de placa del vehiculo en el formato solicitado (ejemplo: ABC-123 o ABC-1234).
3. Presione el boton **Consultar Historial** para visualizar todas las citas agendadas, su estado, fecha y hora.

### Agendar Nueva Cita

1. Ingrese al modulo **Agendar Cita** desde el menu lateral.
2. **Informacion del Vehiculo**: Complete la placa, marca, modelo y año del vehiculo.
3. **Detalles de la Cita**:
   - Seleccione una fecha (el sistema solo permite de lunes a viernes).
   - El sistema cargara los horarios disponibles para esa fecha (intervalos de 30 minutos entre 08:00 y 14:00).
   - Seleccione un horario que no este marcado como ocupado.
4. Presione **Confirmar Agendamiento** para guardar la cita. El sistema validara que no existan cruces de horarios.

## Notas Adicionales

- Todas las respuestas de la API estan estandarizadas bajo el modelo `RespuestaApi`.
- El frontend gestiona estados de carga y errores de conexion mediante interceptores globales.
