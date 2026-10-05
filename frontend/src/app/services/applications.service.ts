import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface ApplicationDto {
  id: string;
  jobId: string;
  jobTitle: string;
  candidateId: string;
  candidateName: string;
  status: string;
  appliedAtUtc: string;
}

export interface UpdateApplicationStatusRequest {
  status: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApplicationsService {
  constructor(private apiService: ApiService) {}

  updateStatus(applicationId: string, request: UpdateApplicationStatusRequest): Observable<ApplicationDto> {
    return this.apiService.patch<ApplicationDto>(`/applications/${applicationId}/status`, request);
  }
}
