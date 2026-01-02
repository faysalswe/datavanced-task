import { Component } from '@angular/core';
import { PatientGridComponent } from './components/patient-grid/patient-grid.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [PatientGridComponent],
  template: `
    <div class="app-container">
      <header class="app-header">
        <h1>Healthcare Management System</h1>
      </header>
      <main class="app-main">
        <app-patient-grid></app-patient-grid>
      </main>
    </div>
  `,
  styles: [`
    .app-container {
      min-height: 100vh;
      background-color: #f5f5f5;
    }
    
    .app-header {
      background-color: #2c3e50;
      color: white;
      padding: 20px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }
    
    .app-header h1 {
      margin: 0;
      font-size: 24px;
    }
    
    .app-main {
      padding: 20px;
    }
  `]
})
export class AppComponent {
  title = 'healthcare-app';
}
