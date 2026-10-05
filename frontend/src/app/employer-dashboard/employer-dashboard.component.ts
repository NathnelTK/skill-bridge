import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employer-dashboard',
  standalone: true,
  templateUrl: './employer-dashboard.component.html',
  styleUrl: './employer-dashboard.component.scss',
})
export class EmployerDashboardComponent {
  private readonly router = inject(Router);

  readonly companyName = localStorage.getItem('skillbridgeEmployerCompany') || 'Your company';
  readonly contactName = localStorage.getItem('skillbridgeEmployerName') || 'Employer';
  activeSection = 'Dashboard';

  readonly stats = [
    { label: 'Today', value: 2, accent: 'blue' },
    { label: 'Total Applications', value: 24, accent: 'sky' },
    { label: 'Shortlisted', value: 5, accent: 'green' },
    { label: 'Rejected', value: 3, accent: 'red' },
  ];

  readonly applications = [
    { candidate: 'Rohan Anderson', initials: 'RA', position: 'Frontend Developer Intern', match: 85, status: 'Review', date: 'Apr 26, 2025' },
    { candidate: 'Sara Muhammad', initials: 'SM', position: 'Frontend Developer Intern', match: 70, status: 'Shortlisted', date: 'Apr 25, 2025' },
    { candidate: 'Abel Tesfaye', initials: 'AT', position: 'Product Designer', match: 60, status: 'Rejected', date: 'Apr 25, 2025' },
  ];

  readonly jobs = [
    { title: 'Frontend Developer Intern', department: 'Engineering', applications: 12, status: 'Active' },
    { title: 'Product Designer', department: 'Design', applications: 8, status: 'Active' },
    { title: 'Junior Backend Developer', department: 'Engineering', applications: 4, status: 'Draft' },
  ];

  setSection(section: string): void {
    this.activeSection = section;
  }

  logout(): void {
    this.router.navigateByUrl('/');
  }
}
