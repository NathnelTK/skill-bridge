import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

/**
 * Temporary placeholder for feature screens (jobs, create job, applicants, applications).
 * The route guards are what this PR exercises; the real screens arrive in later PRs.
 */
@Component({
  selector: 'app-placeholder',
  template: `
    <section class="placeholder">
      <h1>{{ title }}</h1>
      <p>This screen is coming in a later PR.</p>
    </section>
  `,
  styles: `
    .placeholder {
      max-width: 40rem;
      margin: 3rem auto;
      padding: 2rem;
      border: 1px dashed #cbd5e1;
      border-radius: 0.75rem;
      text-align: center;
      color: #475569;
    }

    h1 {
      margin: 0 0 0.5rem;
      font-size: 1.35rem;
      color: #0f172a;
    }
  `
})
export class Placeholder {
  protected readonly title =
    inject(ActivatedRoute).snapshot.data['title'] ?? 'Coming soon';
}
