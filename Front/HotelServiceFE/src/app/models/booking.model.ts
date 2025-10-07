export enum BookingStatus {
  Pending = 0,
  Confirmed = 1,
  Cancelled = 2,
  Completed = 3
}

export interface Booking {
  id: number;
  userId: string;
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: BookingStatus;
  createdAt: string;
  userEmail: string;
  roomNumber: string;
  hotelName: string;
}

export interface CreateBookingRequest {
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
}

export interface UpdateBookingStatusRequest {
  status: BookingStatus;
}
