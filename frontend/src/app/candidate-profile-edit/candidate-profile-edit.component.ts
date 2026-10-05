import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CandidatesService, CandidateProfileDto, UpdateCandidateProfileRequest } from '../services/candidates.service';
import { SkillsService, SkillDto } from '../services/skills.service';

@Component({
  selector: 'app-candidate-profile-edit',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './candidate-profile-edit.component.html',
  styleUrl: './candidate-profile-edit.component.scss'
})
export class CandidateProfileEditComponent implements OnInit {
  private router = inject(Router);
  private candidatesService = inject(CandidatesService);
  private skillsService = inject(SkillsService);

  profile: CandidateProfileDto | null = null;
  allSkills: SkillDto[] = [];
  selectedSkillIds: string[] = [];
  loading = false;
  submitting = false;
  error: string | null = null;

  formData = {
    fullName: '',
    location: ''
  };

  ngOnInit(): void {
    this.loadProfile();
    this.loadSkills();
  }

  loadProfile(): void {
    this.loading = true;
    this.candidatesService.getProfile().subscribe({
      next: (profile) => {
        this.profile = profile;
        this.formData.fullName = profile.fullName;
        this.formData.location = profile.location || '';
        this.selectedSkillIds = profile.skills.map(s => s.id);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load profile';
        this.loading = false;
        console.error(err);
      }
    });
  }

  loadSkills(): void {
    this.skillsService.list().subscribe({
      next: (skills) => {
        this.allSkills = skills.sort((a, b) => a.name.localeCompare(b.name));
      },
      error: (err) => {
        console.error('Failed to load skills', err);
      }
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

    if (!this.formData.fullName) {
      this.error = 'Name is required';
      return;
    }

    this.submitting = true;
    this.error = null;

    const request: UpdateCandidateProfileRequest = {
      fullName: this.formData.fullName,
      location: this.formData.location || undefined,
      skillIds: this.selectedSkillIds
    };

    this.candidatesService.updateProfile(request).subscribe({
      next: () => {
        this.router.navigate(['/candidate/dashboard']);
      },
      error: (err) => {
        this.error = 'Failed to update profile';
        this.submitting = false;
        console.error(err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/candidate/dashboard']);
  }
}
