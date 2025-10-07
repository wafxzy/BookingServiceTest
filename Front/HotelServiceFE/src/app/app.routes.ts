import { Routes } from '@angular/router';
import { authGuard } from './guards/auth-guard';
import { adminGuard } from './guards/admin-guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'hotels',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login/login').then(m => m.Login)
  },
  {
    path: 'register',
    loadComponent: () => import('./components/register/register').then(m => m.Register)
  },
  {
    path: 'hotels',
    loadComponent: () => import('./components/hotels/hotels').then(m => m.Hotels)
  },
  {
    path: 'hotels/:id',
    loadComponent: () => import('./components/hotel-detail/hotel-detail').then(m => m.HotelDetail)
  },
  {
    path: 'my-bookings',
    canActivate: [authGuard],
    loadComponent: () => import('./components/my-bookings/my-bookings').then(m => m.MyBookings)
  },
  {
    path: 'admin',
    canActivate: [authGuard, adminGuard],
    loadComponent: () => import('./components/admin-dashboard/admin-dashboard').then(m => m.AdminDashboard)
  }
];
