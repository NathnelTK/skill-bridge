import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import {
  JobsService,
  CreateJobRequest,
  UpdateJobRequest,
  JobDto,
  SkillDto,
} from '../services/jobs.service';
import { SkillsService } from '../services/skills.service';

@Component({
  selector: 'app-job-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './job-form.component.html',
  styleUrl: './job-form.component.scss',
})
export class JobFormComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private jobsService = inject(JobsService);
  private skillsService = inject(SkillsService);
  private changeDetector = inject(ChangeDetectorRef);

  jobId: string | null = null;
  isEditMode = false;
  loading = false;
  submitting = false;
  error: string | null = null;

  availableSkills: SkillDto[] = [];
  selectedSkillIds: string[] = [];

  formData = {
    title: '',
    description: '',
    location: '',
  };

  ngOnInit(): void {
    this.jobId = this.route.snapshot.paramMap.get('id');
    if (this.jobId) {
      this.isEditMode = true;
      this.loadJob();
    }
    this.loadSkills();
  }

  loadSkills(): void {
    this.skillsService.list().subscribe({
      next: (skills) => {
        this.availableSkills = skills;
        this.changeDetector.markForCheck();
      },
      error: (err) => {
        console.error('Failed to load skills', err);
        this.changeDetector.markForCheck();
      },
    });
  }

  loadJob(): void {
    if (!this.jobId) return;
    this.loading = true;
    this.jobsService.get(this.jobId).subscribe({
      next: (job) => {
        this.formData.title = job.title;
        this.formData.description = job.description;
        this.formData.location = job.location;
        this.selectedSkillIds = job.requiredSkills.map((s) => s.id);
        this.loading = false;
        this.changeDetector.markForCheck();
      },
      error: (err) => {
        this.error = 'Failed to load job';
        this.loading = false;
        console.error(err);
        this.changeDetector.markForCheck();
      },
    });
  }

  toggleSkill(skillId: string): void {
    const index = this.selectedSkillIds.indexOf(skillId);
    if (index > -1) {
      this.selectedSkillIds.splice(index, 1);
    } else {
      this.selectedSkillIds.push(skillId);
    }
  }

  onSubmit(): void {
    if (this.submitting) return;

    if (!this.formData.title || !this.formData.description || !this.formData.location) {
      this.error = 'Please fill in all required fields';
      return;
    }

    this.submitting = true;
    this.error = null;

    if (this.isEditMode && this.jobId) {
      const request: UpdateJobRequest = {
        title: this.formData.title,
        description: this.formData.description,
        location: this.formData.location,
        requiredSkillIds: this.selectedSkillIds,
      };
      this.jobsService.update(this.jobId, request).subscribe({
        next: () => {
          this.router.navigate(['/employer/dashboard']);
        },
        error: (err) => {
          this.error = 'Failed to update job';
          this.submitting = false;
          console.error(err);
          this.changeDetector.markForCheck();
        },
      });
    } else {
      const request: CreateJobRequest = {
        title: this.formData.title,
        description: this.formData.description,
        location: this.formData.location,
        requiredSkillIds: this.selectedSkillIds,
      };
      this.jobsService.create(request).subscribe({
        next: () => {
          this.router.navigate(['/employer/dashboard']);
        },
        error: (err) => {
          this.error = 'Failed to create job';
          this.submitting = false;
          console.error(err);
          this.changeDetector.markForCheck();
        },
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/employer/dashboard']);
  }
}
