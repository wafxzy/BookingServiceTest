export interface Statistics {
  totalBookings: number;
  totalRevenue: number;
  bookingsByCity: CityStatistics[];
  monthlyBookings: MonthlyStatistics[];
  totalHotels?: number;
  totalRooms?: number;
  activeBookings?: number;
  availableRooms?: number;
  activeUsers?: number;
}

export interface CityStatistics {
  city: string;
  bookingsCount: number;
}

export interface MonthlyStatistics {
  month: number;
  year: number;
  bookingsCount: number;
  monthName: string;
}
