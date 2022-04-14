import { Inject, Injectable } from "@angular/core"
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, RouterStateSnapshot } from "@angular/router"
import { AuthService } from "@auth0/auth0-angular"
import { map, Observable, of, switchMap } from "rxjs";



@Injectable()
export class AuthGuard
  implements CanActivate, CanActivateChild {
  constructor(
    @Inject(AuthService) private auth: AuthService
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.auth.isAuthenticated$.pipe(switchMap((isAuthenticated)=>{
      if(isAuthenticated){
        return of(true);
      }
      else{
        return this.auth.loginWithRedirect().pipe(map(()=>false));
      }
    }));
  }

  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.canActivate(next, state)
  }
}
