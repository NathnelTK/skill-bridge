import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { finalize, forkJoin } from 'rxjs';
import {
  CandidatesService,
  CandidateApplicationDto,
  CandidateProfileDto,
} from '../services/candidates.service';
import { AuthService } from '../services/auth.service';
import { JobSummaryDto, JobsService } from '../services/jobs.service';

@Component({
  selector: 'app-candidate-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './candidate-dashboard.component.html',
  styleUrl: './candidate-dashboard.component.scss',
})
export class CandidateDashboardComponent implements OnInit {
  private candidatesService = inject(CandidatesService);
  private jobsService = inject(JobsService);
  private authService = inject(AuthService);
  private changeDetector = inject(ChangeDetectorRef);

  profile: CandidateProfileDto | null = null;
  applications: CandidateApplicationDto[] = [];
  jobs: JobSummaryDto[] = [];
  appliedJobIds = new Set<string>();
  searchTerm = '';
  applyingJobId: string | null = null;
  applyError = '';
  loading = true;
  error: string | null = null;

  ngOnInit(): void {
    this.loadProfile();
  }

  get firstName(): string {
    return this.profile?.fullName.trim().split(/\s+/u)[0] ?? 'there';
  }

  get initials(): string {
    const name = this.profile?.fullName.trim() ?? '';
    return name
      .split(/\s+/u)
      .filter(Boolean)
      .slice(0, 2)
      .map((part) => part[0].toLocaleUpperCase())
      .join('');
  }

  get shortlistedCount(): number {
    return this.applications.filter(
      (application) => application.status.toLowerCase() === 'shortlisted',
    ).length;
  }

  get rejectedCount(): number {
    return this.applications.filter(
      (application) => application.status.toLowerCase() === 'rejected',
    ).length;
  }

  get visibleJobs(): JobSummaryDto[] {
    const search = this.searchTerm.trim().toLocaleLowerCase();

    return this.jobs
      .filter((job) => !this.appliedJobIds.has(job.id))
      .filter((job) => {
        if (!search) {
          return true;
        }

        return [
          job.title,
          job.companyName,
          job.location,
          ...job.requiredSkills.map((skill) => skill.name),
        ].some((value) => value.toLocaleLowerCase().includes(search));
      })
      .sort((a, b) => this.matchingSkillCount(b) - this.matchingSkillCount(a));
  }

  loadDashboard(): void {
    this.loading = true;
    this.error = null;

    forkJoin({
      profile: this.candidatesService.getProfile(),
      applications: this.candidatesService.listApplications(),
      jobs: this.jobsService.browse(),
    }).subscribe({
      next: ({ profile, applications, jobs }) => {
        this.profile = profile;
        this.applications = applications;
        this.jobs = jobs;
        this.appliedJobIds = new Set(applications.map((application) => application.jobId));
        this.loading = false;
        this.changeDetector.markForCheck();
      },
      error: (err) => {
        this.error = err.error?.detail ?? 'Please check your connection and try again.';
        this.loading = false;
        console.error(err);
        this.changeDetector.markForCheck();
      },
    });
  }

  onSearch(event: Event): void {
    this.searchTerm = (event.target as HTMLInputElement).value;
  }

  clearSearch(): void {
    this.searchTerm = '';
  }

  matchingSkillCount(job: JobSummaryDto): number {
    const candidateSkillIds = new Set(this.profile?.skills.map((skill) => skill.id) ?? []);
    return job.requiredSkills.filter((skill) => candidateSkillIds.has(skill.id)).length;
  }

  companyInitial(companyName: string): string {
    return companyName.trim().charAt(0).toLocaleUpperCase() || 'S';
  }

  applyToJob(jobId: string): void {
    if (this.applyingJobId) {
      return;
    }

    this.applyingJobId = jobId;
    this.applyError = '';

    this.jobsService
      .apply(jobId)
      .pipe(
        finalize(() => {
          this.applyingJobId = null;
          this.changeDetector.markForCheck();
        }),
      )
      .subscribe({
        next: () => {
          this.appliedJobIds = new Set(this.appliedJobIds).add(jobId);
          this.changeDetector.markForCheck();
        },
        error: (err) => {
          this.applyError =
            err.error?.detail ?? 'We couldn’t submit your application. Please try again.';
          console.error(err);
        },
      });
  }

  logout(): void {
    this.authService.logout();
    window.location.assign('/login');
  }
}
