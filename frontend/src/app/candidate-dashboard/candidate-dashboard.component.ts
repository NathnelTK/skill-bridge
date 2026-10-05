import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

interface CandidateProfile {
  skills: string[];
  resumeFileName: string;
}

@Component({
  selector: 'app-candidate-dashboard',
  standalone: true,
  templateUrl: './candidate-dashboard.component.html',
  styleUrl: './candidate-dashboard.component.scss',
})
export class CandidateDashboardComponent {
  private readonly router = inject(Router);
  private readonly applicationsStorageKey = 'skillbridgeCandidateApplications';
  private readonly profileStorageKey = 'skillbridgeCandidateProfile';

  readonly candidateName = this.getStoredName();
  profile = this.getStoredProfile();
  activeSection = 'Dashboard';
  searchTerm = '';
  selectedJobType = 'All types';
  filtersOpen = false;
  editingProfile = false;
  skillsInput = this.profile.skills.join(', ');
  pendingResumeFileName = '';
  applications: { title: string; company: string; status: string }[] = this.getStoredApplications();

  readonly stats = [
    { label: 'Available Jobs', value: 12 },
    { label: 'Applied', value: 4 },
    { label: 'Shortlisted', value: 1 },
    { label: 'Rejected', value: 0 },
  ];

  readonly jobs = [
    {
      title: 'Frontend Developer Intern',
      company: 'TechSolutions',
      location: 'Remote',
      type: 'Full-time',
      skills: ['Angular', 'TypeScript', 'HTML'],
      action: 'Apply Now',
    },
    {
      title: 'Junior Backend Developer',
      company: 'CodeHub',
      location: 'Hybrid',
      type: 'Part-time',
      skills: ['Node.js', 'PostgreSQL', 'API'],
      action: 'Apply Now',
    },
  ];

  get filteredJobs() {
    const query = this.searchTerm.trim().toLowerCase();
    return this.jobs.filter((job) => {
      const matchesQuery = !query || `${job.title} ${job.company} ${job.location} ${job.skills.join(' ')}`.toLowerCase().includes(query);
      const matchesType = this.selectedJobType === 'All types' || job.type === this.selectedJobType;
      return matchesQuery && matchesType;
    });
  }

  setSearchTerm(event: Event): void {
    this.searchTerm = (event.target as HTMLInputElement).value;
  }

  setJobType(event: Event): void {
    this.selectedJobType = (event.target as HTMLSelectElement).value;
  }

  applyToJob(job: (typeof this.jobs)[number]): void {
    if (this.applications.some((application) => application.title === job.title)) {
      return;
    }

    this.applications = [...this.applications, { title: job.title, company: job.company, status: 'Submitted' }];
    localStorage.setItem(this.applicationsStorageKey, JSON.stringify(this.applications));
  }

  logout(): void {
    this.router.navigateByUrl('/');
  }

  startProfileEdit(): void {
    this.skillsInput = this.profile.skills.join(', ');
    this.pendingResumeFileName = '';
    this.editingProfile = true;
  }

  updateSkills(event: Event): void {
    this.skillsInput = (event.target as HTMLTextAreaElement).value;
  }

  selectResume(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    this.pendingResumeFileName = file?.name ?? '';
  }

  saveProfile(event: Event): void {
    event.preventDefault();
    this.profile = {
      skills: this.skillsInput.split(',').map((skill) => skill.trim()).filter(Boolean),
      resumeFileName: this.pendingResumeFileName || this.profile.resumeFileName,
    };
    localStorage.setItem(this.profileStorageKey, JSON.stringify(this.profile));
    this.pendingResumeFileName = '';
    this.editingProfile = false;
  }

  cancelProfileEdit(): void {
    this.pendingResumeFileName = '';
    this.editingProfile = false;
  }

  private getStoredName(): string {
    const saved = localStorage.getItem('skillbridgeCandidateName');
    return saved && saved.trim().length > 0 ? saved.trim() : 'Roman';
  }

  private getStoredApplications(): { title: string; company: string; status: string }[] {
    try {
      const saved = localStorage.getItem(this.applicationsStorageKey);
      const applications: unknown = saved ? JSON.parse(saved) : [];
      if (!Array.isArray(applications)) {
        return [];
      }

      return applications.filter((application): application is { title: string; company: string; status: string } =>
        typeof application?.title === 'string' &&
        typeof application?.company === 'string' &&
        typeof application?.status === 'string'
      );
    } catch {
      return [];
    }
  }

  private getStoredProfile(): CandidateProfile {
    try {
      const saved = localStorage.getItem(this.profileStorageKey);
      const profile: unknown = saved ? JSON.parse(saved) : null;
      if (typeof profile !== 'object' || profile === null) {
        return { skills: [], resumeFileName: '' };
      }

      const value = profile as Partial<CandidateProfile>;
      return {
        skills: Array.isArray(value.skills) ? value.skills.filter((skill): skill is string => typeof skill === 'string') : [],
        resumeFileName: typeof value.resumeFileName === 'string' ? value.resumeFileName : '',
      };
    } catch {
      return { skills: [], resumeFileName: '' };
    }
  }
}
