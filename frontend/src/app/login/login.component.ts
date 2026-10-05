import { ChangeDetectorRef, Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs';
import { AuthService, LoginRequest } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  readonly seedAccounts = [
    {
      role: 'Candidate',
      email: 'candidate@skillbridge.demo',
      password: 'Password123!',
      description: 'For testing candidate features',
    },
    {
      role: 'Employer',
      email: 'employer@skillbridge.demo',
      password: 'Password123!',
      description: 'For testing employer features',
    },
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private changeDetector: ChangeDetectorRef,
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  useSeedAccount(account: any): void {
    this.loginForm.patchValue({
      email: account.email,
      password: account.password,
    });
  }

  onSubmit(): void {
    const emailControl = this.loginForm.get('email');
    if (typeof emailControl?.value === 'string') {
      emailControl.setValue(emailControl.value.trim());
    }

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const { email, password } = this.loginForm.getRawValue();

    const request: LoginRequest = {
      email,
      password,
    };

    this.authService
      .login(request)
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.changeDetector.markForCheck();
        }),
      )
      .subscribe({
        next: (response) => {
          this.authService.saveToken(response.token);
          this.successMessage = 'Login successful! Redirecting...';
          this.changeDetector.markForCheck();

          const dashboard =
            response.user.role === 'Employer' ? '/employer/dashboard' : '/candidate/dashboard';
          void this.router.navigate([dashboard]);
        },
        error: (error) => {
          this.errorMessage = this.getErrorMessage(error);
          this.changeDetector.markForCheck();
        },
      });
  }

  private getErrorMessage(error: HttpErrorResponse): string {
    if (error.status === 0) {
      return 'Unable to reach the server. Check your connection and try again.';
    }

    if (error.status === 401) {
      return 'Incorrect email or password.';
    }

    return (
      error.error?.detail ||
      error.error?.title ||
      `Login failed (HTTP ${error.status}). Please try again.`
    );
  }
}
