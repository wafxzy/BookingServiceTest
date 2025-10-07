import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BookingStatus, Booking, UpdateBookingStatusRequest } from '../../models/booking.model';
import { Hotel, CreateHotelRequest } from '../../models/hotel.model';
import { Room, CreateRoomRequest, UpdateRoomRequest } from '../../models/room.model';
import { Statistics } from '../../models/statistics.model';
import { AuthService } from '../../services/auth';
import { BookingService } from '../../services/booking';
import { HotelService } from '../../services/hotel';
import { RoomService } from '../../services/room';
import { StatisticsService } from '../../services/statistics';


@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboard implements OnInit {
  BookingStatus = BookingStatus; // Экспорт для использования в шаблоне
  activeTab: string = 'statistics';
  isLoading = false;
  errorMessage = '';

  // Статистика
  statistics: Statistics | null = null;
  recentBookings: Booking[] = [];

  // Отели
  hotels: Hotel[] = [];
  selectedHotel: Hotel | null = null;
  hotelForm: CreateHotelRequest = {
    name: '',
    description: '',
    address: '',
    city: ''
  };
  isEditingHotel = false;

  // Комнаты
  rooms: Room[] = [];
  selectedRoom: Room | null = null;
  roomForm: CreateRoomRequest = {
    roomNumber: '',
    description: '',
    type: '',
    maxOccupancy: 1,
    pricePerNight: 0,
    hotelId: 0
  };
  roomUpdateForm: UpdateRoomRequest = {
    roomNumber: '',
    description: '',
    type: '',
    maxOccupancy: 1,
    pricePerNight: 0,
    isAvailable: true
  };
  isEditingRoom = false;

  // Бронирования
  allBookings: Booking[] = [];
  bookingFilter = {
    status: '',
    hotelId: 0,
    startDate: '',
    endDate: ''
  };

  constructor(
    private hotelService: HotelService,
    private bookingService: BookingService,
    private roomService: RoomService,
    private statisticsService: StatisticsService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (!this.authService.isAdmin()) {
      this.router.navigate(['/hotels']);
      return;
    }
    this.loadStatistics();
    this.loadHotels();
    this.loadAllBookings();
  }

  setActiveTab(tab: string): void {
    this.activeTab = tab;
    if (tab === 'statistics') {
      this.loadStatistics();
    } else if (tab === 'hotels') {
      this.loadHotels();
    } else if (tab === 'bookings') {
      this.loadAllBookings();
    }
  }

  // СТАТИСТИКА
  loadStatistics(): void {
    this.isLoading = true;
    this.statisticsService.getStatistics().subscribe({
      next: (stats) => {
        this.statistics = stats;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Ошибка загрузки статистики';
        this.isLoading = false;
      }
    });

    // Загрузка последних бронирований через booking service
    this.bookingService.getAllBookings().subscribe({
      next: (bookings) => {
        // Берем последние 5 бронирований, отсортированные по дате создания
        this.recentBookings = bookings
          .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
          .slice(0, 5);
      },
      error: (error) => {
        console.error('Ошибка загрузки последних бронирований:', error);
      }
    });
  }

  // УПРАВЛЕНИЕ ОТЕЛЯМИ
  loadHotels(): void {
    this.isLoading = true;
    this.hotelService.getAllHotels().subscribe({
      next: (hotels) => {
        this.hotels = hotels;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Ошибка загрузки отелей';
        this.isLoading = false;
      }
    });
  }

  createHotel(): void {
    this.selectedHotel = null;
    this.isEditingHotel = true;
    this.hotelForm = {
      name: '',
      description: '',
      address: '',
      city: ''
    };
  }

  editHotel(hotel: Hotel): void {
    this.selectedHotel = hotel;
    this.isEditingHotel = true;
    this.hotelForm = {
      name: hotel.name,
      description: hotel.description,
      address: hotel.address,
      city: hotel.city || ''
    };
  }

  saveHotel(): void {
    if (this.selectedHotel) {
      // Обновление
      this.hotelService.updateHotel(this.selectedHotel.id, this.hotelForm).subscribe({
        next: () => {
          this.loadHotels();
          this.cancelHotelEdit();
          alert('Отель успешно обновлен');
        },
        error: (error) => {
          alert('Ошибка обновления отеля');
        }
      });
    } else {
      // Создание
      this.hotelService.createHotel(this.hotelForm).subscribe({
        next: () => {
          this.loadHotels();
          this.cancelHotelEdit();
          alert('Отель успешно создан');
        },
        error: (error) => {
          alert('Ошибка создания отеля');
        }
      });
    }
  }

  deleteHotel(hotelId: number): void {
    if (confirm('Вы уверены, что хотите удалить этот отель?')) {
      this.hotelService.deleteHotel(hotelId).subscribe({
        next: () => {
          this.loadHotels();
          alert('Отель успешно удален');
        },
        error: (error) => {
          alert('Ошибка удаления отеля');
        }
      });
    }
  }

  cancelHotelEdit(): void {
    this.isEditingHotel = false;
    this.selectedHotel = null;
  }

  // УПРАВЛЕНИЕ КОМНАТАМИ
  loadRooms(hotelId: number): void {
    this.roomService.getRoomsByHotelId(hotelId).subscribe({
      next: (rooms) => {
        this.rooms = rooms;
      },
      error: (error) => {
        console.error('Ошибка загрузки комнат:', error);
      }
    });
  }

  createRoom(hotelId: number): void {
    this.selectedRoom = null;
    this.isEditingRoom = true;
    this.roomForm = {
      roomNumber: '',
      description: '',
      type: '',
      maxOccupancy: 1,
      pricePerNight: 0,
      hotelId: hotelId
    };
  }

  editRoom(room: Room): void {
    this.selectedRoom = room;
    this.isEditingRoom = true;
    this.roomUpdateForm = {
      roomNumber: room.roomNumber,
      description: room.description,
      type: room.type,
      maxOccupancy: room.maxOccupancy,
      pricePerNight: room.pricePerNight,
      isAvailable: room.isAvailable
    };
  }

  saveRoom(): void {
    if (this.selectedRoom) {
      // Обновление
      this.roomService.updateRoom(this.selectedRoom.id, this.roomUpdateForm).subscribe({
        next: () => {
          this.loadRooms(this.selectedRoom!.hotelId);
          this.cancelRoomEdit();
          alert('Комната успешно обновлена');
        },
        error: (error) => {
          alert('Ошибка обновления комнаты');
        }
      });
    } else {
      // Создание
      this.roomService.createRoom(this.roomForm).subscribe({
        next: () => {
          this.loadRooms(this.roomForm.hotelId);
          this.cancelRoomEdit();
          alert('Комната успешно создана');
        },
        error: (error) => {
          alert('Ошибка создания комнаты');
        }
      });
    }
  }

  deleteRoom(roomId: number, hotelId: number): void {
    if (confirm('Вы уверены, что хотите удалить эту комнату?')) {
      this.roomService.deleteRoom(roomId).subscribe({
        next: () => {
          this.loadRooms(hotelId);
          alert('Комната успешно удалена');
        },
        error: (error) => {
          alert('Ошибка удаления комнаты');
        }
      });
    }
  }

  cancelRoomEdit(): void {
    this.isEditingRoom = false;
    this.selectedRoom = null;
  }

  // УПРАВЛЕНИЕ БРОНИРОВАНИЯМИ
  loadAllBookings(): void {
    this.isLoading = true;
    this.bookingService.getAllBookings().subscribe({
      next: (bookings) => {
        this.allBookings = bookings;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Ошибка загрузки бронирований';
        this.isLoading = false;
      }
    });
  }

  updateBookingStatus(bookingId: number, status: string): void {
    const statusEnum = status as keyof typeof BookingStatus;
    const updateRequest: UpdateBookingStatusRequest = {
      status: BookingStatus[statusEnum]
    };
    
    this.bookingService.updateBookingStatus(bookingId, updateRequest).subscribe({
      next: () => {
        this.loadAllBookings();
        alert('Статус бронирования обновлен');
      },
      error: (error) => {
        alert('Ошибка обновления статуса');
      }
    });
  }

  getFilteredBookings(): Booking[] {
    return this.allBookings.filter(booking => {
      if (this.bookingFilter.status && BookingStatus[booking.status] !== this.bookingFilter.status) {
        return false;
      }
      // Фильтр по отелю убран так как в Booking нет прямой связи с отелем
      return true;
    });
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Confirmed': return 'bg-green-100 text-green-800';
      case 'Pending': return 'bg-yellow-100 text-yellow-800';
      case 'Cancelled': return 'bg-red-100 text-red-800';
      case 'Completed': return 'bg-blue-100 text-blue-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  }

  logout(): void {
    this.authService.logout();
  }
}
