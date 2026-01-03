import { Routes } from '@angular/router';
import { UnifiedSearchComponent } from './components/unified-search/unified-search.component';
import { PatientGridComponent } from './components/patient-grid/patient-grid.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'search',
    pathMatch: 'full'
  },
  {
    path: 'search',
    component: UnifiedSearchComponent,
    data: { title: 'Search' }
  },
  {
    path: 'patients',
    component: PatientGridComponent,
    data: { title: 'Patients' }
  },
  {
    path: '**',
    redirectTo: 'search'
  }
];
