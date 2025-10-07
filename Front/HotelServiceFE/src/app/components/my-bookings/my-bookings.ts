import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterLink, Router, NavigationEnd } from '@angular/router';
import { Subject, filter, takeUntil } from 'rxjs';
import { Booking, BookingStatus } from '../../models/booking.model';
import { BookingService } from '../../services/booking';

@Component({
  selector: 'app-my-bookings',
  imports: [CommonModule, RouterLink],
  templateUrl: './my-bookings.html',
  styleUrl: './my-bookings.css'
})
export class MyBookings implements OnInit, OnDestroy {
  bookings: Booking[] = [];
  isLoading: boolean = false;
  errorMessage: string = '';
  private destroy$ = new Subject<void>();

  constructor(
    private bookingService: BookingService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadBookings();
    
    // Подписываемся на изменения роута, чтобы обновлять данные при каждом посещении
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        if (this.router.url === '/my-bookings') {
          this.loadBookings();
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadBookings(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.bookingService.getUserBookings().subscribe({
      next: (bookings) => {
        this.bookings = bookings;
        this.isLoading = false;
        console.log('Загружены бронирования:', bookings);
      },
      error: (error) => {
        console.error('Ошибка загрузки бронирований:', error);
        this.errorMessage = 'Ошибка загрузки бронирований';
        this.isLoading = false;
      }
    });
  }

  refreshBookings(): void {
    this.loadBookings();
  }

  getStatusText(status: BookingStatus): string {
    switch (status) {
      case BookingStatus.Pending:
        return 'Ожидание';
      case BookingStatus.Confirmed:
        return 'Подтверждено';
      case BookingStatus.Cancelled:
        return 'Отменено';
      case BookingStatus.Completed:
        return 'Завершено';
      default:
        return 'Неизвестно';
    }
  }

  getStatusClass(status: BookingStatus): string {
    switch (status) {
      case BookingStatus.Pending:
        return 'bg-yellow-100 text-yellow-800';
      case BookingStatus.Confirmed:
        return 'bg-green-100 text-green-800';
      case BookingStatus.Cancelled:
        return 'bg-red-100 text-red-800';
      case BookingStatus.Completed:
        return 'bg-blue-100 text-blue-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  }

  cancelBooking(id: number): void {
    if (confirm('Вы уверены, что хотите отменить бронирование?')) {
      this.bookingService.cancelBooking(id).subscribe({
        next: () => {
          this.loadBookings();
          alert('Бронирование успешно отменено');
        },
        error: (error) => {
          console.error('Ошибка отмены бронирования:', error);
          if (error.status === 400) {
            alert('Невозможно отменить это бронирование');
          } else if (error.status === 404) {
            alert('Бронирование не найдено');
          } else {
            alert('Ошибка отмены бронирования. Попробуйте позже.');
          }
        }
      });
    }
  }
}
