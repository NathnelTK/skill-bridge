export type UserRole = 'Candidate' | 'Employer';

export interface AuthUser {
  id: string;
  fullName: string;
  email: string;
  role: UserRole;
  companyName: string | null;
  location: string | null;
}

export interface AuthResponse {
  token: string;
  expiresAtUtc: string;
  user: AuthUser;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: UserRole;
  companyName?: string | null;
  location?: string | null;
}

export const API_BASE_URL = 'http://localhost:5000';

export const SESSION_TOKEN_KEY = 'skillbridge.token';
export const SESSION_USER_KEY = 'skillbridge.user';
