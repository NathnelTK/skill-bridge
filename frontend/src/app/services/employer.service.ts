import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { JobSummaryDto, SkillDto } from './jobs.service';

export type { JobSummaryDto, SkillDto };

export interface ApplicantDto {
  id: string;
  fullName: string;
  email: string;
  location?: string;
  skills: SkillDto[];
}

@Injectable({
  providedIn: 'root'
})
export class EmployerService {
  constructor(private apiService: ApiService) {}

  listJobs(): Observable<JobSummaryDto[]> {
    return this.apiService.get<JobSummaryDto[]>('/employer/jobs');
  }

  getApplicants(jobId: string): Observable<ApplicantDto[]> {
    return this.apiService.get<ApplicantDto[]>(`/employer/jobs/${jobId}/applicants`);
  }
}
