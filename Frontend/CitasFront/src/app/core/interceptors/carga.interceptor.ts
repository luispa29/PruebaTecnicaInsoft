import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { CargaService } from '@core/services/carga.service';

export const cargaInterceptor: HttpInterceptorFn = (req, next) => {
    const cargaService = inject(CargaService);
    cargaService.mostrar();
    return next(req).pipe(finalize(() => cargaService.ocultar()));
};
