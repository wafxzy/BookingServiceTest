import { Room } from './room.model';

export interface Hotel {
  id: number;
  name: string;
  address: string;
  city: string;
  description: string;
  pricePerNight?: number;
  rooms?: Room[];
}

export interface CreateHotelRequest {
  name: string;
  address: string;
  city: string;
  description: string;
}

export interface UpdateHotelRequest {
  name: string;
  address: string;
  city: string;
  description: string;
}
