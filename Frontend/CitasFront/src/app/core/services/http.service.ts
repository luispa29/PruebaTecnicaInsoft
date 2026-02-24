import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { APP_CONSTANTS } from '@core/constants/app.constants';

@Injectable({
    providedIn: 'root'
})
export class HttpService {
    private http = inject(HttpClient);
    private urlBase = APP_CONSTANTS.API_URL;

    get<T>(ruta: string, parametros?: Record<string, string | number | boolean>): Observable<T> {
        let params = new HttpParams();
        if (parametros) {
            Object.entries(parametros).forEach(([clave, valor]) => {
                params = params.set(clave, String(valor));
            });
        }
        return this.http.get<T>(`${this.urlBase}${ruta}`, { params });
    }

    post<T>(ruta: string, cuerpo: unknown): Observable<T> {
        return this.http.post<T>(`${this.urlBase}${ruta}`, cuerpo);
    }
}
