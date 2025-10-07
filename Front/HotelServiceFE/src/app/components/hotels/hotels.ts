import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { CreateBookingRequest } from "../../models/booking.model";
import { Hotel } from "../../models/hotel.model";
import { Room } from "../../models/room.model";
import { AuthService } from "../../services/auth";
import { BookingService } from "../../services/booking";
import { HotelService } from "../../services/hotel";



@Component({
  selector: 'app-hotels',
  imports: [CommonModule, FormsModule],
  templateUrl: './hotels.html',
  styleUrl: './hotels.css'
})
export class Hotels implements OnInit {
  hotels: Hotel[] = [];
  filteredHotels: Hotel[] = [];
  searchCity: string = '';
  searchCheckIn: string = '';
  searchCheckOut: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';
  
  // Booking modal
  selectedRoom: Room | null = null;
  showBookingModal: boolean = false;
  bookingData = {
    checkInDate: '',
    checkOutDate: ''
  };

  constructor(
    private hotelService: HotelService,
    private bookingService: BookingService,
    public authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadHotels();
  }

  loadHotels(): void {
    this.isLoading = true;
    this.hotelService.getAllHotels().subscribe({
      next: (hotels) => {
        this.hotels = hotels;
        this.filteredHotels = hotels;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Ошибка загрузки отелей';
        this.isLoading = false;
      }
    });
  }

  searchHotels(): void {
    if (!this.searchCity.trim()) {
      this.filteredHotels = this.hotels;
      return;
    }

    this.isLoading = true;
    
    if (this.searchCheckIn && this.searchCheckOut) {
      // Поиск с датами - получаем отели с доступными номерами
      this.searchHotelsWithAvailability();
    } else {
      // Обычный поиск по городу
      this.hotelService.searchHotelsByCity(this.searchCity).subscribe({
        next: (hotels) => {
          this.filteredHotels = hotels;
          this.isLoading = false;
        },
        error: (error) => {
          this.errorMessage = 'Ошибка поиска';
          this.isLoading = false;
        }
      });
    }
  }

  private searchHotelsWithAvailability(): void {
    // Поиск отелей с учетом доступности номеров на указанные даты
    this.hotelService.searchAvailableHotels(this.searchCity, this.searchCheckIn, this.searchCheckOut).subscribe({
      next: (hotels) => {
        this.filteredHotels = hotels;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Ошибка поиска доступных номеров';
        this.isLoading = false;
      }
    });
  }

  clearSearch(): void {
    this.searchCity = '';
    this.searchCheckIn = '';
    this.searchCheckOut = '';
    this.filteredHotels = this.hotels;
  }

  openBookingModal(room: Room): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    
    this.selectedRoom = room;
    this.showBookingModal = true;
  }

  closeBookingModal(): void {
    this.selectedRoom = null;
    this.showBookingModal = false;
    this.bookingData = {
      checkInDate: '',
      checkOutDate: ''
    };
  }

  bookRoom(): void {
    if (!this.selectedRoom || !this.bookingData.checkInDate || !this.bookingData.checkOutDate) {
      return;
    }

    const request: CreateBookingRequest = {
      roomId: this.selectedRoom.id,
      checkInDate: this.bookingData.checkInDate,
      checkOutDate: this.bookingData.checkOutDate
    };

    this.bookingService.createBooking(request).subscribe({
      next: () => {
        alert('Номер успешно забронирован!');
        this.closeBookingModal();
        this.router.navigate(['/my-bookings']);
      },
      error: (error) => {
        alert('Ошибка бронирования');
      }
    });
  }

  viewHotelDetails(hotelId: number): void {
    this.router.navigate(['/hotels', hotelId]);
  }

  quickBook(hotel: Hotel): void {
    // Быстрое бронирование - можно открыть модальное окно с первым доступным номером
    if (hotel.rooms && hotel.rooms.length > 0) {
      const availableRoom = hotel.rooms.find(room => room.isAvailable);
      if (availableRoom) {
        this.openBookingModal(availableRoom);
      } else {
        alert('В данный момент нет доступных номеров');
      }
    } else {
      this.viewHotelDetails(hotel.id);
    }
  }
}


