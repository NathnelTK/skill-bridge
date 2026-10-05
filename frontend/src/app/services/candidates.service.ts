import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { SkillDto } from './jobs.service';

export interface CandidateProfileDto {
  id: string;
  fullName: string;
  email: string;
  location?: string;
  skills: SkillDto[];
}

export interface UpdateCandidateProfileRequest {
  fullName?: string;
  location?: string;
  skillIds?: string[];
}

export interface CvUploadResultDto {
  addedSkills: string[];
  alreadyPresentSkills: string[];
  extractedCharacterCount: number;
  profile: CandidateProfileDto;
}

export interface CandidateApplicationDto {
  id: string;
  jobId: string;
  jobTitle: string;
  companyName: string;
  status: string;
  appliedAtUtc: string;
}

@Injectable({
  providedIn: 'root'
})
export class CandidatesService {
  constructor(private apiService: ApiService) {}

  getProfile(): Observable<CandidateProfileDto> {
    return this.apiService.get<CandidateProfileDto>('/candidates/me');
  }

  updateProfile(request: UpdateCandidateProfileRequest): Observable<CandidateProfileDto> {
    return this.apiService.put<CandidateProfileDto>('/candidates/me', request);
  }

  uploadCv(file: File): Observable<CvUploadResultDto> {
    return this.apiService.upload<CvUploadResultDto>('/candidates/me/cv', file);
  }

  listApplications(): Observable<CandidateApplicationDto[]> {
    return this.apiService.get<CandidateApplicationDto[]>('/candidates/me/applications');
  }
}
