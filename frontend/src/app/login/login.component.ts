import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private readonly router = inject(Router);

  email = '';
  password = '';

  submit(): void {
    if (!this.email.trim() || !this.password.trim()) {
      return;
    }

    this.router.navigateByUrl('/candidate-dashboard');
  }
}
