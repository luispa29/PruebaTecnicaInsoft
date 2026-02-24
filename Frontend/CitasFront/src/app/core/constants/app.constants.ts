import { environment } from '../../../environments/environment';

export const API_ENDPOINTS = {
    CITAS: {
        POR_PLACA: (placa: string) => `/citas/placa/${placa}`,
        HORARIOS: '/citas/horarios-disponibles',
        BASE: '/citas'
    }
} as const;

export const APP_CONSTANTS = {
    API_URL: environment.apiUrl,
    PAGINACION: {
        PAGINA_DEFECTO: 1,
        TAMANO_DEFECTO: 10,
        OPCIONES_TAMANO: [5, 10, 25, 50]
    }
} as const;

export const MENSAJES = {
    EXITO: {
        CITA_AGENDADA: 'Cita agendada exitosamente'
    },
    ERROR: {
        GENERICO: 'Ha ocurrido un error. Por favor, intente nuevamente',
        CONEXION: 'Error de conexión. Verifique su conexión a internet',
        SIN_CITAS: 'No se encontraron citas para la placa indicada'
    }
} as const;
