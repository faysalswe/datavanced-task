import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink],
  template: `
    <div class="app-container">
      <header class="app-header">
        <h1>Datavanced Task</h1>
        <nav class="app-nav">
          <a class="nav-button" routerLink="/search" routerLinkActive="active">
            Search
          </a>
          <a class="nav-button" routerLink="/patients" routerLinkActive="active">
            Patients
          </a>
        </nav>
      </header>
      <main class="app-main">
        <router-outlet></router-outlet>
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
      margin: 0 0 15px 0;
      font-size: 24px;
    }

    .app-nav {
      display: flex;
      gap: 10px;
    }

    .nav-button {
      display: inline-block;
      padding: 10px 20px;
      background-color: rgba(255, 255, 255, 0.2);
      color: white;
      border: 2px solid transparent;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      font-weight: 500;
      transition: all 0.3s;
      text-decoration: none;
    }

    .nav-button:hover {
      background-color: rgba(255, 255, 255, 0.3);
    }

    .nav-button.active {
      background-color: white;
      color: #2c3e50;
      border-color: white;
    }
    
    .app-main {
      padding: 20px;
    }
  `]
})
export class AppComponent {
  title = 'healthcare-app';
}
