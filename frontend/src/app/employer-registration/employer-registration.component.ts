import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-employer-registration',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './employer-registration.component.html',
  styleUrl: '../candidate-registration/candidate-registration.component.scss',
})
export class EmployerRegistrationComponent {
  private readonly router = inject(Router);

  companyName = '';
  fullName = '';
  email = '';
  password = '';
  confirmPassword = '';
  errorMessage = '';

  submit(): void {
    const companyName = this.companyName.trim();
    const fullName = this.fullName.trim();
    const email = this.email.trim();

    if (!companyName || !fullName || !email || !this.password || !this.confirmPassword) {
      this.errorMessage = 'Complete all fields to create your employer account.';
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    localStorage.setItem('skillbridgeEmployerCompany', companyName);
    localStorage.setItem('skillbridgeEmployerName', fullName);
    localStorage.setItem('skillbridgeEmployerEmail', email);
    this.router.navigateByUrl('/employer-login');
  }
}
