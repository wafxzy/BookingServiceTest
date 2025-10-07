import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { CreateBookingRequest } from "../../models/booking.model";
import { Hotel } from "../../models/hotel.model";
import { Room } from "../../models/room.model";
import { AuthService } from "../../services/auth";
import { BookingService } from "../../services/booking";
import { HotelService } from "../../services/hotel";
import { RoomService } from "../../services/room";

@Component({
  selector: 'app-hotel-detail',
  imports: [CommonModule, FormsModule],
  templateUrl: './hotel-detail.html',
  styleUrl: './hotel-detail.css'
})
export class HotelDetail implements OnInit {
  hotel: Hotel | null = null;
  rooms: Room[] = [];
  isLoading: boolean = false;
  isLoadingRooms: boolean = false;
  errorMessage: string = '';
  
  // Search parameters
  searchParams = {
    checkInDate: '',
    checkOutDate: '',
    guests: 2
  };
  
  // Booking modal
  selectedRoom: Room | null = null;
  showBookingModal: boolean = false;
  isBooking: boolean = false;
  totalPrice: number = 0;
  numberOfNights: number = 0;
  
  bookingData = {
    checkInDate: '',
    checkOutDate: '',
    numberOfGuests: 2
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private hotelService: HotelService,
    private roomService: RoomService,
    private bookingService: BookingService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    const hotelId = this.route.snapshot.params['id'];
    if (hotelId) {
      this.loadHotel(+hotelId);
      this.loadRooms(+hotelId);
    }
  }

  loadHotel(id: number): void {
    this.isLoading = true;
    this.hotelService.getHotelById(id).subscribe({
      next: (hotel) => {
        this.hotel = hotel;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Отель не найден';
        this.isLoading = false;
      }
    });
  }

  loadRooms(hotelId: number): void {
    this.roomService.getRoomsByHotelId(hotelId).subscribe({
      next: (rooms) => {
        this.rooms = rooms;
      },
      error: (error) => {
        console.error('Ошибка загрузки номеров:', error);
      }
    });
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
      checkOutDate: '',
      numberOfGuests: 2
    };
    this.totalPrice = 0;
    this.numberOfNights = 0;
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

  calculateNights(): number {
    if (!this.bookingData.checkInDate || !this.bookingData.checkOutDate) {
      return 0;
    }
    
    const checkIn = new Date(this.bookingData.checkInDate);
    const checkOut = new Date(this.bookingData.checkOutDate);
    const diffTime = checkOut.getTime() - checkIn.getTime();
    this.numberOfNights = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return this.numberOfNights;
  }

  calculateTotal(): number {
    if (!this.selectedRoom) return 0;
    this.totalPrice = this.selectedRoom.pricePerNight * this.calculateNights();
    return this.totalPrice;
  }

  goBack(): void {
    this.router.navigate(['/hotels']);
  }

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }

  searchRooms(): void {
    if (!this.hotel?.id) return;
    
    this.isLoadingRooms = true;
    this.roomService.getRoomsByHotelId(this.hotel.id).subscribe({
      next: (rooms) => {
        this.rooms = rooms;
        this.isLoadingRooms = false;
      },
      error: (error) => {
        console.error('Ошибка загрузки комнат:', error);
        this.isLoadingRooms = false;
      }
    });
  }

  confirmBooking(): void {
    if (!this.selectedRoom || !this.bookingData.checkInDate || !this.bookingData.checkOutDate) {
      alert('Заполните все поля');
      return;
    }

    this.calculateTotal();
    
    this.isBooking = true;
    const request: CreateBookingRequest = {
      roomId: this.selectedRoom.id,
      checkInDate: this.bookingData.checkInDate,
      checkOutDate: this.bookingData.checkOutDate
    };

    this.bookingService.createBooking(request).subscribe({
      next: () => {
        alert('Бронирование успешно создано!');
        this.closeBookingModal();
        this.isBooking = false;
        // Перенаправляем на страницу бронирований
        this.router.navigate(['/my-bookings']);
      },
      error: (error) => {
        alert('Ошибка создания бронирования');
        this.isBooking = false;
      }
    });
  }
}
