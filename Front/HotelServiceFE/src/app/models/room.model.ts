export interface Room {
  id: number;
  hotelId: number;
  roomNumber: string;
  name?: string;
  type: string;
  pricePerNight: number;
  maxOccupancy: number;
  capacity?: number;
  description: string;
  isAvailable: boolean;
}

export interface CreateRoomRequest {
  hotelId: number;
  roomNumber: string;
  type: string;
  pricePerNight: number;
  maxOccupancy: number;
  description: string;
}

export interface UpdateRoomRequest {
  roomNumber: string;
  type: string;
  pricePerNight: number;
  maxOccupancy: number;
  description: string;
  isAvailable: boolean;
}
