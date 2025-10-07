import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/environment';
import { Booking, CreateBookingRequest, UpdateBookingStatusRequest } from '../models/booking.model';


@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private readonly API_URL = `${environment.apiUrl}/bookings`;

  constructor(private http: HttpClient) {}

  getAllBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(this.API_URL);
  }

  getBookingById(id: number): Observable<Booking> {
    return this.http.get<Booking>(`${this.API_URL}/${id}`);
  }

  getUserBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.API_URL}/my-bookings`);
  }

  createBooking(request: CreateBookingRequest): Observable<Booking> {
    return this.http.post<Booking>(this.API_URL, request);
  }

  updateBookingStatus(id: number, request: UpdateBookingStatusRequest): Observable<Booking> {
    return this.http.put<Booking>(`${this.API_URL}/${id}/status`, request);
  }

  cancelBooking(id: number): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}/cancel`, {});
  }
}
