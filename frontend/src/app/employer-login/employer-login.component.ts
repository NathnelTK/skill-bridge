import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-employer-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './employer-login.component.html',
  styleUrl: '../login/login.component.scss',
})
export class EmployerLoginComponent {
  private readonly router = inject(Router);

  email = '';
  password = '';

  submit(): void {
    if (!this.email.trim() || !this.password.trim()) {
      return;
    }

    this.router.navigateByUrl('/employer-dashboard');
  }
}
