import { computed, inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import {
  API_BASE_URL,
  AuthResponse,
  AuthUser,
  LoginRequest,
  RegisterRequest,
  SESSION_TOKEN_KEY,
  SESSION_USER_KEY,
  UserRole
} from './auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly tokenState = signal<string | null>(readStoredToken());
  private readonly userState = signal<AuthUser | null>(readStoredUser());

  readonly token = this.tokenState.asReadonly();
  readonly currentUser = this.userState.asReadonly();
  readonly isAuthenticated = computed(() => this.tokenState() !== null);
  readonly role = computed<UserRole | null>(() => this.userState()?.role ?? null);

  login(request: LoginRequest) {
    return this.http
      .post<AuthResponse>(`${API_BASE_URL}/api/auth/login`, request)
      .pipe(tap((response) => this.setSession(response)));
  }

  register(request: RegisterRequest) {
    return this.http
      .post<AuthResponse>(`${API_BASE_URL}/api/auth/register`, request)
      .pipe(tap((response) => this.setSession(response)));
  }

  logout(): void {
    this.clearSession();
    void this.router.navigate(['/login']);
  }

  clearSession(): void {
    this.tokenState.set(null);
    this.userState.set(null);
    localStorage.removeItem(SESSION_TOKEN_KEY);
    localStorage.removeItem(SESSION_USER_KEY);
  }

  homeRoute(): string {
    return this.role() === 'Employer' ? '/applicants' : '/jobs';
  }

  private setSession(response: AuthResponse): void {
    this.tokenState.set(response.token);
    this.userState.set(response.user);
    localStorage.setItem(SESSION_TOKEN_KEY, response.token);
    localStorage.setItem(SESSION_USER_KEY, JSON.stringify(response.user));
  }
}

function readStoredToken(): string | null {
  return localStorage.getItem(SESSION_TOKEN_KEY);
}

function readStoredUser(): AuthUser | null {
  const raw = localStorage.getItem(SESSION_USER_KEY);
  if (!raw) {
    return null;
  }

  try {
    return JSON.parse(raw) as AuthUser;
  } catch {
    return null;
  }
}
