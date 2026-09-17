import { Routes } from '@angular/router';
import { Auth } from './components/auth/auth';
import { authGuard } from './guards/auth-guard';
import { Dashboard } from './components/dashboard/dashboard';
import { Profile } from './components/profile/profile';

export const routes: Routes = [
    {
        path:"auth",
        component:Auth
    },
    {
        path:"",
        component:Dashboard,
        canActivate:[authGuard],
    },
    // {
    //     path:"/",
    //     component:Profile,
    //     canActivate:[authGuard],

    // }

];
