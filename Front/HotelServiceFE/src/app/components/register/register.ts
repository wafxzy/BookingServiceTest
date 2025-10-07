import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';
import { RegisterRequest } from '../../models/auth.model';
import { AuthService } from '../../services/auth';


@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})

export class Register {
  registerData: RegisterRequest = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: ''
  };
  
  isLoading = false;
  errorMessage = '';
  showPassword = false;
  showConfirmPassword = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (!this.validateForm()) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.register(this.registerData).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.router.navigate(['/hotels']);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Ошибка регистрации';
      }
    });
  }

  private validateForm(): boolean {
    this.errorMessage = '';

    if (!this.registerData.firstName.trim()) {
      this.errorMessage = 'Введите имя';
      return false;
    }

    if (!this.registerData.lastName.trim()) {
      this.errorMessage = 'Введите фамилию';
      return false;
    }

    if (!this.registerData.email.trim()) {
      this.errorMessage = 'Введите email';
      return false;
    }

    if (!this.isValidEmail(this.registerData.email)) {
      this.errorMessage = 'Введите корректный email';
      return false;
    }

    if (!this.registerData.password) {
      this.errorMessage = 'Введите пароль';
      return false;
    }

    if (this.registerData.password.length < 6) {
      this.errorMessage = 'Пароль должен содержать минимум 6 символов';
      return false;
    }

    if (this.registerData.password !== this.registerData.confirmPassword) {
      this.errorMessage = 'Пароли не совпадают';
      return false;
    }

    return true;
  }

  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }
}
