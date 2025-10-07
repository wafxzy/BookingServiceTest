import { Component, signal, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { AuthService } from './services/auth';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, AsyncPipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('HotelServiceFE');
  protected readonly authService = inject(AuthService);

  logout(): void {
    this.authService.logout();
  }
}
