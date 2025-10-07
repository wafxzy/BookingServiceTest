import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../enviroments/environment";
import { Hotel, CreateHotelRequest, UpdateHotelRequest } from "../models/hotel.model";

@Injectable({
  providedIn: 'root'
})
export class HotelService {
  private readonly API_URL = `${environment.apiUrl}/hotels`;

  constructor(private http: HttpClient) {}

  getAllHotels(): Observable<Hotel[]> {
    return this.http.get<Hotel[]>(this.API_URL);
  }

  getHotelById(id: number): Observable<Hotel> {
    return this.http.get<Hotel>(`${this.API_URL}/${id}`);
  }

  searchHotelsByCity(city: string): Observable<Hotel[]> {
    const params = new HttpParams().set('city', city);
    return this.http.get<Hotel[]>(`${this.API_URL}/search`, { params });
  }

  searchAvailableHotels(city: string, checkIn?: string, checkOut?: string): Observable<Hotel[]> {
    let params = new HttpParams().set('city', city);
    
    if (checkIn) {
      params = params.set('checkIn', checkIn);
    }
    if (checkOut) {
      params = params.set('checkOut', checkOut);
    }
    
    return this.http.get<Hotel[]>(`${this.API_URL}/search-available`, { params });
  }

  createHotel(request: CreateHotelRequest): Observable<Hotel> {
    return this.http.post<Hotel>(this.API_URL, request);
  }

  updateHotel(id: number, request: UpdateHotelRequest): Observable<Hotel> {
    return this.http.put<Hotel>(`${this.API_URL}/${id}`, request);
  }

  deleteHotel(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}

