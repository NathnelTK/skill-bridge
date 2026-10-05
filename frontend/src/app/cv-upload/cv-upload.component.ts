import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidatesService, CvUploadResultDto } from '../services/candidates.service';
import { TranslationService } from '../services/translation.service';

@Component({
  selector: 'app-cv-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cv-upload.component.html',
  styleUrl: './cv-upload.component.scss',
})
export class CvUploadComponent {
  selectedFile: File | null = null;
  isUploading = false;
  uploadResult: CvUploadResultDto | null = null;
  errorMessage = '';
  translateBeforeUpload = false;
  targetLanguage = 'en';

  constructor(
    private candidatesService: CandidatesService,
    private translationService: TranslationService
  ) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];

      if (file.type !== 'application/pdf' && !file.name.endsWith('.pdf')) {
        this.errorMessage = 'Only PDF files are supported.';
        this.selectedFile = null;
        return;
      }

      if (file.size > 8 * 1024 * 1024) {
        this.errorMessage = 'File size must be less than 8MB.';
        this.selectedFile = null;
        return;
      }

      this.selectedFile = file;
      this.errorMessage = '';
      this.uploadResult = null;
    }
  }

  onUpload(): void {
    if (!this.selectedFile) {
      return;
    }

    this.isUploading = true;
    this.errorMessage = '';
    this.uploadResult = null;

    this.candidatesService.uploadCv(this.selectedFile).subscribe({
      next: (result) => {
        this.uploadResult = result;
        this.isUploading = false;
        this.selectedFile = null;
      },
      error: (error) => {
        this.isUploading = false;
        this.errorMessage = error.error?.detail || 'Failed to upload CV. Please try again.';
      }
    });
  }

  onTranslateText(text: string): void {
    this.translationService.translate(text, this.targetLanguage).subscribe({
      next: (response) => {
        console.log('Translated text:', response.responseData.translatedText);
      },
      error: (error) => {
        console.error('Translation failed:', error);
      }
    });
  }

  onCancel(): void {
    this.selectedFile = null;
    this.errorMessage = '';
    this.uploadResult = null;
  }
}
