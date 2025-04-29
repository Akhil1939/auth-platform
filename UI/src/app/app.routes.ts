import { Routes } from '@angular/router';
import { DesignLayoutComponent } from './pages/design/layout/components/design-layout/design-layout.component';
import { ButtonComponent } from './shared/components/button/button.component';

export const routes: Routes = [
    {
        path:'',
        component:DesignLayoutComponent,
        children:[
            {
                path:'',
                component:ButtonComponent
            }
        ]
    }
];
