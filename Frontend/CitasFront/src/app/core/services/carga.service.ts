import { Injectable, signal } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class CargaService {
    private contador = 0;
    estaCargando = signal<boolean>(false);

    mostrar(): void {
        this.contador++;
        this.estaCargando.set(true);
    }

    ocultar(): void {
        this.contador--;
        if (this.contador <= 0) {
            this.contador = 0;
            this.estaCargando.set(false);
        }
    }
}

