import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { LoginRequest } from '../../models/auth.model';
import { AuthService } from '../../services/auth';
import { AutoUnsubscribe } from 'ngx-auto-unsubscribe';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})

//@AutoUnsubscribe()
export class Login {
  loginData: LoginRequest = {
    email: '',
    password: ''
  };
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (!this.loginData.email || !this.loginData.password) {
      this.errorMessage = 'Пожалуйста, заполните все поля';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginData).subscribe({
      next: () => {
        this.router.navigate(['/hotels']);
      },
      error: (error) => {
        this.errorMessage = 'Неверный email или пароль';
        this.isLoading = false;
      }
    });
  }

  fillTestClient(): void {
    this.loginData = {
      email: 'client1@t.com',
      password: 'Client123!'
    };
  }

  fillTestAdmin(): void {
    this.loginData = {
      email: 'admin@g.com',
      password: 'Admin123!'
    };
  }
}