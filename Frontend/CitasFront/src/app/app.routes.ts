import { Routes } from '@angular/router';
import { LayoutPrincipal } from '@shared/layout/layout-principal/layout-principal';

export const routes: Routes = [
    {
        path: '',
        component: LayoutPrincipal,
        children: [
            {
                path: 'historial',
                loadComponent: () => import('@features/citas/pages/historial/historial').then(m => m.Historial)
            },
            {
                path: 'agendar',
                loadComponent: () => import('@features/citas/pages/agendar/agendar').then(m => m.Agendar)
            },
            {
                path: '',
                redirectTo: 'historial',
                pathMatch: 'full'
            }
        ]
    }
];
