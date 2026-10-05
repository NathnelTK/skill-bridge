import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: 'Candidate' | 'Employer';
  companyName?: string;
  location?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  user: {
    id: string;
    fullName: string;
    email: string;
    role: string;
    companyName?: string;
    location?: string;
  };
}

export interface UserDto {
  id: string;
  fullName: string;
  email: string;
  role: string;
  companyName?: string;
  location?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private apiService: ApiService) {}

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.apiService.post<AuthResponse>('/auth/register', request);
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.apiService.post<AuthResponse>('/auth/login', request);
  }

  getCurrentUser(): Observable<UserDto> {
    return this.apiService.get<UserDto>('/auth/me');
  }

  saveToken(token: string): void {
    localStorage.setItem('auth_token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  logout(): void {
    localStorage.removeItem('auth_token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}
