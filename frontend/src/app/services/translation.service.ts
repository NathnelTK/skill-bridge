import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private apiUrl = 'https://api.mymemory.translated.net/get';

  constructor(private http: HttpClient) {}

  translate(text: string, targetLang: string = 'en'): Observable<any> {
    return this.http.get(this.apiUrl, {
      params: {
        q: text,
        langpair: `autodetect|${targetLang}`
      }
    });
  }

  extractSkillsFromText(text: string, knownSkills: string[]): string[] {
    const normalizedText = text.toLowerCase();
    const foundSkills: string[] = [];

    knownSkills.forEach(skill => {
      const skillVariants = this.getSkillVariants(skill);
      const found = skillVariants.some(variant =>
        normalizedText.includes(variant.toLowerCase())
      );

      if (found) {
        foundSkills.push(skill);
      }
    });

    return [...new Set(foundSkills)];
  }

  private getSkillVariants(skill: string): string[] {
    const variants: Record<string, string[]> = {
      'C#': ['c#', 'c sharp', 'csharp', 'c-sharp'],
      'ASP.NET Core': ['asp.net core', 'asp.net', 'aspnet core', 'aspnet', 'asp net core', '.net core'],
      'PostgreSQL': ['postgresql', 'postgres', 'psql'],
      'Angular': ['angular', 'angularjs', 'angular.js'],
      'TypeScript': ['typescript', 'type script', 'ts'],
      'REST APIs': ['rest api', 'rest apis', 'restful', 'rest', 'web api'],
      'Docker': ['docker', 'containerization', 'containerisation'],
      'Git': ['git', 'github', 'gitlab', 'version control'],
      'React': ['react', 'reactjs', 'react.js'],
      'Node.js': ['node', 'nodejs', 'node.js'],
      'Python': ['python', 'py'],
      'Java': ['java'],
      'JavaScript': ['javascript', 'js', 'ecmascript'],
      'SQL': ['sql', 'structured query language'],
      'MongoDB': ['mongodb', 'mongo'],
      'AWS': ['aws', 'amazon web services'],
      'Azure': ['azure', 'microsoft azure'],
      'Kubernetes': ['kubernetes', 'k8s', 'k8s']
    };

    return variants[skill] || [skill];
  }
}
