import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface SkillDto {
  id: string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class SkillsService {
  constructor(private apiService: ApiService) {}

  list(): Observable<SkillDto[]> {
    return this.apiService.get<SkillDto[]>('/skills');
  }
}
