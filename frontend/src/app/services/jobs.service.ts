import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface SkillDto {
  id: string;
  name: string;
}

export interface JobSummaryDto {
  id: string;
  title: string;
  location: string;
  companyName: string;
  createdAtUtc: string;
  requiredSkillCount: number;
  requiredSkills: SkillDto[];
}

export interface JobDto {
  id: string;
  title: string;
  description: string;
  location: string;
  companyName: string;
  createdAtUtc: string;
  requiredSkills: SkillDto[];
}

export interface CreateJobRequest {
  title: string;
  description: string;
  location: string;
  requiredSkillIds: string[];
}

export interface UpdateJobRequest {
  title?: string;
  description?: string;
  location?: string;
  requiredSkillIds?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class JobsService {
  constructor(private apiService: ApiService) {}

  browse(skillIds?: string[]): Observable<JobSummaryDto[]> {
    const params = skillIds && skillIds.length > 0
      ? skillIds.map(id => `skillId=${id}`).join('&')
      : '';
    return this.apiService.get<JobSummaryDto[]>(`/jobs${params ? '?' + params : ''}`);
  }

  get(id: string): Observable<JobDto> {
    return this.apiService.get<JobDto>(`/jobs/${id}`);
  }

  create(request: CreateJobRequest): Observable<JobDto> {
    return this.apiService.post<JobDto>('/jobs', request);
  }

  update(id: string, request: UpdateJobRequest): Observable<JobDto> {
    return this.apiService.put<JobDto>(`/jobs/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.apiService.delete<void>(`/jobs/${id}`);
  }

  apply(id: string): Observable<any> {
    return this.apiService.post<any>(`/jobs/${id}/applications`, {});
  }
}
