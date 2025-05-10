import { Routes } from '@angular/router';
import { DesignLayoutComponent } from './pages/design/layout/components/design-layout/design-layout.component';
import { ButtonComponent } from './shared/components/button/button.component';
import { AuthLayoutComponent } from './pages/design/layout/auth-layout/auth-layout.component';
import { RegisterComponent } from './pages/auth/register/register.component';

export const routes: Routes = [
    {
        path:'',
        component:AuthLayoutComponent,
        children:[
            {
                path:'',
                component:RegisterComponent
            }
        ]
    }
];
