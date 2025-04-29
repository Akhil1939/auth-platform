import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatDrawer, MatSidenavModule } from "@angular/material/sidenav";
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-design-layout',
  imports: [RouterModule, MatSidenavModule, HeaderComponent],
  standalone: true,
  templateUrl: './design-layout.component.html',
  styleUrl: './design-layout.component.scss'
})
export class DesignLayoutComponent {

}
