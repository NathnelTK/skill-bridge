import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { JobsService, JobSummaryDto, SkillDto } from '../services/jobs.service';

@Component({
  selector: 'app-jobs',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './jobs.component.html',
  styleUrl: './jobs.component.scss',
})
export class JobsComponent implements OnInit {
  jobs: JobSummaryDto[] = [];
  allSkills: SkillDto[] = [];
  filteredJobs: JobSummaryDto[] = [];
  filterForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  constructor(
    private jobsService: JobsService,
    private fb: FormBuilder
  ) {
    this.filterForm = this.fb.group({
      selectedSkills: [[]]
    });
  }

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.jobsService.browse().subscribe({
      next: (jobs) => {
        this.jobs = jobs;
        this.filteredJobs = jobs;
        this.extractAllSkills(jobs);
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load jobs. Please try again.';
        this.isLoading = false;
      }
    });
  }

  extractAllSkills(jobs: JobSummaryDto[]): void {
    const skillMap = new Map<string, SkillDto>();

    jobs.forEach(job => {
      job.requiredSkills.forEach(skill => {
        if (!skillMap.has(skill.id)) {
          skillMap.set(skill.id, skill);
        }
      });
    });

    this.allSkills = Array.from(skillMap.values()).sort((a, b) =>
      a.name.localeCompare(b.name)
    );
  }

  onFilterChange(): void {
    const selectedSkillIds = this.filterForm.value.selectedSkills;

    if (selectedSkillIds.length === 0) {
      this.filteredJobs = this.jobs;
    } else {
      this.filteredJobs = this.jobs.filter(job =>
        job.requiredSkills.some(skill => selectedSkillIds.includes(skill.id))
      );
    }
  }

  onSkillToggle(skillId: string): void {
    const currentSelection = this.filterForm.value.selectedSkills;
    const newSelection = currentSelection.includes(skillId)
      ? currentSelection.filter((id: string) => id !== skillId)
      : [...currentSelection, skillId];

    this.filterForm.patchValue({ selectedSkills: newSelection });
    this.onFilterChange();
  }

  isSkillSelected(skillId: string): boolean {
    return this.filterForm.value.selectedSkills.includes(skillId);
  }

  clearFilters(): void {
    this.filterForm.patchValue({ selectedSkills: [] });
    this.onFilterChange();
  }
}
