import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { AuthService, AuthResponse } from '../services/auth.service';
import { LoginComponent } from './login.component';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let savedToken: string | undefined;
  let navigations: string[][];
  let loginResponse: AuthResponse;
  let loginError: HttpErrorResponse | undefined;

  beforeEach(() => {
    savedToken = undefined;
    navigations = [];
    loginError = undefined;
    loginResponse = {
      token: 'test-token',
      user: {
        id: 'user-id',
        fullName: 'Test User',
        email: 'test@example.com',
        role: 'Candidate',
      },
    };

    TestBed.configureTestingModule({
      providers: [
        {
          provide: AuthService,
          useValue: {
            login: () => (loginError ? throwError(() => loginError) : of(loginResponse)),
            saveToken: (token: string) => {
              savedToken = token;
            },
          },
        },
        {
          provide: Router,
          useValue: {
            navigate: (commands: string[]) => {
              navigations.push(commands);
              return Promise.resolve(true);
            },
          },
        },
        {
          provide: ChangeDetectorRef,
          useValue: { markForCheck: () => undefined },
        },
      ],
    });

    component = new LoginComponent(
      new FormBuilder(),
      TestBed.inject(AuthService),
      TestBed.inject(Router),
      TestBed.inject(ChangeDetectorRef),
    );
  });

  it('shows validation feedback when submitted with invalid fields', () => {
    component.onSubmit();

    expect(component.loginForm.get('email')?.touched).toBe(true);
    expect(component.loginForm.get('password')?.touched).toBe(true);
    expect(component.isLoading).toBe(false);
  });

  it('stores the token and routes to the authenticated user dashboard', () => {
    component.loginForm.setValue({
      email: '  candidate@example.com  ',
      password: 'Password123!',
    });

    component.onSubmit();

    expect(savedToken).toBe('test-token');
    expect(navigations).toEqual([['/candidate/dashboard']]);
    expect(component.isLoading).toBe(false);
  });

  it('routes employers to the employer dashboard', () => {
    loginResponse.user.role = 'Employer';
    component.loginForm.setValue({
      email: 'employer@example.com',
      password: 'Password123!',
    });

    component.onSubmit();

    expect(navigations).toEqual([['/employer/dashboard']]);
  });

  it('shows a useful message when credentials are rejected', () => {
    loginError = new HttpErrorResponse({
      status: 401,
      error: { detail: 'Invalid email or password.' },
    });
    component.loginForm.setValue({
      email: 'candidate@example.com',
      password: 'wrong-password',
    });

    component.onSubmit();

    expect(component.errorMessage).toBe('Incorrect email or password.');
    expect(component.isLoading).toBe(false);
  });

  it('distinguishes an unavailable API from invalid credentials', () => {
    loginError = new HttpErrorResponse({ status: 0 });
    component.loginForm.setValue({
      email: 'candidate@example.com',
      password: 'Password123!',
    });

    component.onSubmit();

    expect(component.errorMessage).toContain('Unable to reach the server');
    expect(component.isLoading).toBe(false);
  });
});
