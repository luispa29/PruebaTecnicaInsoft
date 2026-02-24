import { Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CitaService } from '@features/citas/services/cita.service';
import { HorarioDisponibleResponse } from '@models/interfaces/cita.model';

@Component({
    selector: 'app-agendar',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatIconModule,
        MatCardModule,
        MatSelectModule,
        MatDatepickerModule,
        MatNativeDateModule
    ],
    templateUrl: './agendar.html',
    styleUrl: './agendar.css'
})
export class Agendar {
    private citaService = inject(CitaService);
    private fb = inject(FormBuilder);
    private snackBar = inject(MatSnackBar);

    horarios = signal<HorarioDisponibleResponse[]>([]);
    cargandoHorarios = signal(false);
    fechaMinima = new Date();

    formulario = this.fb.group({
        placa: ['', [Validators.required, Validators.pattern(/^[A-Z]{3}-\d{3,4}$/)]],
        marca: ['', Validators.required],
        modelo: ['', Validators.required],
        anio: [new Date().getFullYear(), [Validators.required, Validators.min(1900), Validators.max(new Date().getFullYear() + 1)]],
        fechaCita: [null as Date | null, Validators.required],
        horaCita: ['', Validators.required],
        descripcion: ['']
    });

    filtroFecha = (fecha: Date | null): boolean => {
        if (!fecha) return false;
        const dia = fecha.getDay();
        return dia !== 0 && dia !== 6;
    };

    onFechaCambio(): void {
        const fecha = this.formulario.get('fechaCita')?.value;
        if (!fecha) return;

        this.formulario.get('horaCita')?.reset();
        this.cargandoHorarios.set(true);

        const fechaIso = fecha.toISOString().split('T')[0];
        this.citaService.obtenerHorariosDisponibles(fechaIso).subscribe({
            next: (respuesta) => {
                this.horarios.set(respuesta.datos ?? []);
                this.cargandoHorarios.set(false);
            },
            error: () => {
                this.horarios.set([]);
                this.cargandoHorarios.set(false);
            }
        });
    }

    agendar(): void {
        if (this.formulario.invalid) {
            this.formulario.markAllAsTouched();
            return;
        }

        const valores = this.formulario.value;
        const fechaIso = (valores.fechaCita as Date).toISOString().split('T')[0];

        this.citaService.agendarCita({
            placa: valores.placa!.toUpperCase(),
            marca: valores.marca!,
            modelo: valores.modelo!,
            anio: Number(valores.anio),
            fechaCita: `${fechaIso}T00:00:00`,
            horaCita: valores.horaCita!,
            descripcion: valores.descripcion ?? ''
        }).subscribe({
            next: (respuesta) => {
                this.snackBar.open(respuesta.mensaje || 'Cita agendada exitosamente', 'Cerrar', {
                    duration: 4000,
                    panelClass: ['snack-exito']
                });
                this.formulario.reset();
                this.horarios.set([]);
            },
            error: () => { }
        });
    }
}
