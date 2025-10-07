import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/environment';
import { Room, CreateRoomRequest, UpdateRoomRequest } from '../models/room.model';


@Injectable({
  providedIn: 'root'
})
export class RoomService {
  private readonly API_URL = `${environment.apiUrl}/rooms`;

  constructor(private http: HttpClient) {}

  getRoomsByHotelId(hotelId: number): Observable<Room[]> {
    const params = new HttpParams().set('hotelId', hotelId.toString());
    return this.http.get<Room[]>(this.API_URL, { params });
  }

  getRoomById(id: number): Observable<Room> {
    return this.http.get<Room>(`${this.API_URL}/${id}`);
  }

  searchAvailableRooms(checkInDate: string, checkOutDate: string): Observable<Room[]> {
    const params = new HttpParams()
      .set('checkInDate', checkInDate)
      .set('checkOutDate', checkOutDate);
    return this.http.get<Room[]>(`${this.API_URL}/available`, { params });
  }

  createRoom(request: CreateRoomRequest): Observable<Room> {
    return this.http.post<Room>(this.API_URL, request);
  }

  updateRoom(id: number, request: UpdateRoomRequest): Observable<Room> {
    return this.http.put<Room>(`${this.API_URL}/${id}`, request);
  }

  deleteRoom(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
