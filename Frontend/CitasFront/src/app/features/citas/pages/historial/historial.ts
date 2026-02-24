import { Component, inject, signal, ViewChild } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { DatePipe, UpperCasePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { CitaService } from '@features/citas/services/cita.service';
import { CitaResponse } from '@models/interfaces/cita.model';
import { APP_CONSTANTS } from '@core/constants/app.constants';

@Component({
    selector: 'app-historial',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        MatTableModule,
        MatPaginatorModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatIconModule,
        MatCardModule,
        MatChipsModule,
        DatePipe,
        UpperCasePipe
    ],
    templateUrl: './historial.html',
    styleUrl: './historial.css'
})
export class Historial {
    private citaService = inject(CitaService);

    @ViewChild(MatPaginator) paginador!: MatPaginator;

    campoPlaca = new FormControl('', [
        Validators.required,
        Validators.pattern(/^[A-Z]{3}-\d{3,4}$/)
    ]);

    fuenteDatos = new MatTableDataSource<CitaResponse>([]);
    columnasVisibles = ['placa', 'estado', 'fechaCita', 'horaCita', 'descripcion'];
    totalRegistros = signal<number>(0);
    paginaActual = signal<number>(APP_CONSTANTS.PAGINACION.PAGINA_DEFECTO);
    tamanoPagina = signal<number>(APP_CONSTANTS.PAGINACION.TAMANO_DEFECTO);
    opcionesTamano = APP_CONSTANTS.PAGINACION.OPCIONES_TAMANO;
    buscado = signal(false);
    sinResultados = signal(false);

    buscar(): void {
        if (this.campoPlaca.invalid) {
            this.campoPlaca.markAsTouched();
            return;
        }
        this.paginaActual.set(1);
        this.ejecutarBusqueda();
    }

    cambiarPagina(evento: PageEvent): void {
        this.paginaActual.set(evento.pageIndex + 1);
        this.tamanoPagina.set(evento.pageSize);
        this.ejecutarBusqueda();
    }

    private ejecutarBusqueda(): void {
        const placa = this.campoPlaca.value!.toUpperCase();

        this.citaService.consultarPorPlaca(placa, this.paginaActual(), this.tamanoPagina()).subscribe({
            next: (respuesta) => {
                this.buscado.set(true);
                this.sinResultados.set(false);
                this.fuenteDatos.data = respuesta.datos ?? [];
                this.totalRegistros.set(respuesta.paginacion?.totalRegistros ?? 0);
            },
            error: (error) => {
                this.buscado.set(true);
                if (error.status === 404) {
                    this.sinResultados.set(true);
                    this.fuenteDatos.data = [];
                    this.totalRegistros.set(0);
                }
            }
        });
    }
}
