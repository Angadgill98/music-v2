import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Api } from '../api/api';
import { catchError, map, of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const api = inject(Api);
  const router = inject(Router);

  return api.auth.IsTokenValid().pipe(
    map((valid) => {
      console.log('Token valid:', valid);

      if (valid) {
        return true;
      }

      return router.createUrlTree(['/auth']);
    }),

    catchError((err) => {
      console.log('Token validation failed:', err);

      return of(router.createUrlTree(['/auth']));
    })
  );
};