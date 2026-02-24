import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from '@core/services/http.service';
import { API_ENDPOINTS, APP_CONSTANTS } from '@core/constants/app.constants';
import { AgendarCitaRequest, CitaResponse, HorarioDisponibleResponse, RespuestaApi } from '@models/interfaces/cita.model';

@Injectable({
    providedIn: 'root'
})
export class CitaService {
    private http = inject(HttpService);

    consultarPorPlaca(
        placa: string,
        numeroPagina: number = APP_CONSTANTS.PAGINACION.PAGINA_DEFECTO,
        tamanoPagina: number = APP_CONSTANTS.PAGINACION.TAMANO_DEFECTO
    ): Observable<RespuestaApi<CitaResponse[]>> {
        return this.http.get<RespuestaApi<CitaResponse[]>>(
            API_ENDPOINTS.CITAS.POR_PLACA(placa),
            { numeroPagina, tamanoPagina }
        );
    }

    obtenerHorariosDisponibles(fecha: string): Observable<RespuestaApi<HorarioDisponibleResponse[]>> {
        return this.http.get<RespuestaApi<HorarioDisponibleResponse[]>>(
            API_ENDPOINTS.CITAS.HORARIOS,
            { fecha }
        );
    }

    agendarCita(solicitud: AgendarCitaRequest): Observable<RespuestaApi<{ citaId: number }>> {
        return this.http.post<RespuestaApi<{ citaId: number }>>(
            API_ENDPOINTS.CITAS.BASE,
            solicitud
        );
    }
}
