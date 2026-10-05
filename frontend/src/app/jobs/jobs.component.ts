import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { JobsService, JobSummaryDto, SkillDto } from '../services/jobs.service';
import { SkillsService } from '../services/skills.service';

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
    private skillsService: SkillsService,
    private fb: FormBuilder,
    private changeDetector: ChangeDetectorRef,
  ) {
    this.filterForm = this.fb.group({
      selectedSkills: [[]],
    });
  }

  ngOnInit(): void {
    this.loadJobs();
    this.loadSkills();
  }

  loadJobs(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.jobsService.browse().subscribe({
      next: (jobs) => {
        this.jobs = jobs;
        this.filteredJobs = jobs;
        this.isLoading = false;
        this.changeDetector.markForCheck();
      },
      error: (error) => {
        this.errorMessage = 'Failed to load jobs. Please try again.';
        this.isLoading = false;
        this.changeDetector.markForCheck();
      },
    });
  }

  loadSkills(): void {
    this.skillsService.list().subscribe({
      next: (skills) => {
        this.allSkills = skills.sort((a, b) => a.name.localeCompare(b.name));
        this.changeDetector.markForCheck();
      },
      error: (error) => {
        console.error('Failed to load skills', error);
        this.changeDetector.markForCheck();
      },
    });
  }

  onFilterChange(): void {
    const selectedSkillIds = this.filterForm.value.selectedSkills;

    if (selectedSkillIds.length === 0) {
      this.filteredJobs = this.jobs;
    } else {
      this.filteredJobs = this.jobs.filter((job) =>
        job.requiredSkills.some((skill) => selectedSkillIds.includes(skill.id)),
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

  applyToJob(jobId: string): void {
    this.jobsService.apply(jobId).subscribe({
      next: () => {
        alert('Application submitted successfully!');
      },
      error: (error) => {
        alert('Failed to submit application. Please try again.');
        console.error(error);
      },
    });
  }
}
