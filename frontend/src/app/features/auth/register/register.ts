import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../../core/auth/auth.service';
import { RegisterRequest, UserRole } from '../../../core/auth/auth.models';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  private readonly formBuilder = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  readonly submitting = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly role = signal<UserRole>('Candidate');
  readonly isEmployer = computed(() => this.role() === 'Employer');

  readonly form = this.formBuilder.nonNullable.group({
    fullName: ['', [Validators.required, Validators.maxLength(160)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(320)]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(128)]],
    companyName: ['', [Validators.maxLength(160)]],
    location: ['', [Validators.maxLength(160)]]
  });

  setRole(role: UserRole): void {
    this.role.set(role);
    const companyName = this.form.controls.companyName;

    if (role === 'Employer') {
      companyName.addValidators(Validators.required);
    } else {
      companyName.removeValidators(Validators.required);
      companyName.setValue('');
    }

    companyName.updateValueAndValidity();
  }

  submit(): void {
    this.errorMessage.set(null);

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);

    const value = this.form.getRawValue();
    const request: RegisterRequest = {
      fullName: value.fullName,
      email: value.email,
      password: value.password,
      role: this.role(),
      companyName: value.companyName || null,
      location: value.location || null
    };

    this.auth.register(request).subscribe({
      next: () => {
        this.submitting.set(false);
        void this.router.navigateByUrl(this.auth.homeRoute());
      },
      error: (error: unknown) => {
        this.submitting.set(false);
        this.errorMessage.set(messageFor(error, 'Unable to create your account. Please try again.'));
      }
    });
  }
}

function messageFor(error: unknown, fallback: string): string {
  if (error instanceof HttpErrorResponse) {
    if (error.status === 409) {
      return 'An account with this email already exists.';
    }

    const detail = (error.error as { detail?: string } | null)?.detail;
    if (detail) {
      return detail;
    }
  }

  return fallback;
}
