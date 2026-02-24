import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const snackBar = inject(MatSnackBar);

    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            let mensajeError = 'Ha ocurrido un error inesperado';

            if (error.status === 0) {
                mensajeError = 'Error de conexión. Verifique su conexión a internet';
            } else if (error.status === 400) {
                mensajeError = error.error?.mensaje || 'Error de validación. Verifique los datos ingresados';
            } else if (error.status === 404) {
                mensajeError = error.error?.mensaje || 'No se encontraron resultados';
            } else if (error.status === 500) {
                mensajeError = 'Error interno del servidor';
            }

            snackBar.open(mensajeError, 'Cerrar', { duration: 5000, panelClass: ['snack-error'] });
            return throwError(() => error);
        })
    );
};
