import { ChangeDetectorRef, Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService, RegisterRequest } from '../services/auth.service';

@Component({
  selector: 'app-employer-registration',
  standalone: true,
  imports: [RouterLink, CommonModule, ReactiveFormsModule],
  templateUrl: './employer-registration.component.html',
  styleUrl: './employer-registration.component.scss',
})
export class EmployerRegistrationComponent {
  registerForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  readonly checklist = [
    'Post job openings and reach skilled candidates.',
    'Manage applications efficiently',
    'Find the best talent for your team',
    'Build your employer brand',
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private changeDetector: ChangeDetectorRef,
  ) {
    this.registerForm = this.fb.group(
      {
        fullName: ['', [Validators.required, Validators.minLength(2)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
        companyName: ['', [Validators.required, Validators.minLength(2)]],
        location: [''],
      },
      { validators: this.passwordMatchValidator },
    );
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const { fullName, email, password, companyName, location } = this.registerForm.value;

    const request: RegisterRequest = {
      fullName,
      email,
      password,
      role: 'Employer',
      companyName,
      location: location || undefined,
    };

    this.authService.register(request).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token);
        this.successMessage = 'Registration successful! Redirecting...';
        this.changeDetector.markForCheck();
        setTimeout(() => {
          this.router.navigate(['/employer/dashboard']);
        }, 1500);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage =
          error.error?.detail || error.error?.message || 'Registration failed. Please try again.';
        this.changeDetector.markForCheck();
      },
      complete: () => {
        this.isLoading = false;
        this.changeDetector.markForCheck();
      },
    });
  }
}
