import { HttpEventType, HttpInterceptorFn } from '@angular/common/http';
import { tap } from 'rxjs';

export const requestInterceptor: HttpInterceptorFn = (req, next) => {
  console.log("REQUEST");
  console.log("URL:", req.url);
  console.log("BODY:", req.body);

  return next(req).pipe(
    tap(event => {
      if (event.type === HttpEventType.Response) {
        console.log("RESPONSE:", event.body);
      }
    })
  );
};
