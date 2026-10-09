import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export type AppRole = 'Candidate' | 'Employer';

/** Allows access only when a JWT session exists; otherwise redirects to login. */
export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return auth.isAuthenticated() ? true : router.createUrlTree(['/login']);
};

/** Allows access only when the signed-in user has the required role. */
export function roleGuard(role: AppRole): CanActivateFn {
  return () => {
    const auth = inject(AuthService);
    const router = inject(Router);

    if (!auth.isAuthenticated()) {
      return router.createUrlTree(['/login']);
    }

    const currentRole = auth.getRole();
    if (currentRole === role) {
      return true;
    }

    return router.createUrlTree([
      currentRole === 'Employer' ? '/employer/dashboard' : '/candidate/dashboard',
    ]);
  };
}
