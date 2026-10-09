import { Injectable, signal } from '@angular/core';
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

export interface UserDto {
  id: string;
  fullName: string;
  email: string;
  role: string;
  companyName?: string;
  location?: string;
}

export interface AuthResponse {
  token: string;
  expiresAtUtc: string;
  user: UserDto;
}

const TokenKey = 'auth_token';
const UserKey = 'auth_user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly currentUserSignal = signal<UserDto | null>(this.readStoredUser());

  readonly currentUser = this.currentUserSignal.asReadonly();

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

  saveSession(response: AuthResponse): void {
    localStorage.setItem(TokenKey, response.token);
    localStorage.setItem(UserKey, JSON.stringify(response.user));
    this.currentUserSignal.set(response.user);
  }

  getToken(): string | null {
    return localStorage.getItem(TokenKey);
  }

  getUser(): UserDto | null {
    return this.currentUserSignal();
  }

  getRole(): string | null {
    return this.currentUserSignal()?.role ?? null;
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem(TokenKey);
    localStorage.removeItem(UserKey);
    this.currentUserSignal.set(null);
  }

  private readStoredUser(): UserDto | null {
    const raw = localStorage.getItem(UserKey);
    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as UserDto;
    } catch {
      return null;
    }
  }
}
