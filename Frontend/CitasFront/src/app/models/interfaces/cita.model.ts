export interface RespuestaApi<T> {
    codigo: number;
    mensaje: string;
    datos: T | null;
    paginacion: InfoPaginacion | null;
}

export interface InfoPaginacion {
    totalRegistros: number;
    pagina: number;
    tamanoPagina: number;
    totalPaginas: number;
}

export interface CitaResponse {
    citaID: number;
    placa: string;
    marca: string;
    modelo: string;
    anio: number;
    fechaCita: string;
    horaCita: string;
    descripcion: string;
    estado: string;
    fechaCreacion: string;
}

export interface HorarioDisponibleResponse {
    hora: string;
    disponible: boolean;
}

export interface AgendarCitaRequest {
    placa: string;
    marca: string;
    modelo: string;
    anio: number;
    fechaCita: string;
    horaCita: string;
    descripcion: string;
}
