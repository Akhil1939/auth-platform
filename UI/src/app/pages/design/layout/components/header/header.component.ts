import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatBadgeModule } from "@angular/material/badge";
import { MatButtonModule } from "@angular/material/button";
import { MatDialog } from "@angular/material/dialog";
import { MatIconModule } from "@angular/material/icon";
import { MatMenuModule } from "@angular/material/menu";
import { MatToolbarModule } from "@angular/material/toolbar";
import { Router, RouterModule } from "@angular/router";

@Component({
  selector: 'app-header',
  imports: [ MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatBadgeModule,
    MatMenuModule,
    RouterModule,
    CommonModule],
  templateUrl: './header.component.html',
  standalone: true,
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  constructor(private router: Router) {}

  readonly dialog = inject(MatDialog);

  ngOnInit(): void {
    document.body.classList.add("sidebar-full");
    this.setActiveModule();
  }

  // Toggle sidebar
  public toggleSidebar(): void {
    const body = document.body;
    const windowWidth = window.innerWidth;
    if (windowWidth > 1199) {
      if (body.classList.contains("sidebar-full")) {
        body.classList.remove("sidebar-full");
        body.classList.add("sidebar-collapsed");
      } else {
        body.classList.remove("sidebar-collapsed");
        body.classList.add("sidebar-full");
      }
    } else {
      body.classList.add("show-sidebar");
    }
  }

  // Dialog Code
  openChangePasswordDialog() {
    
  }
  openHelpDialog() {
    // const dialogRef = this.dialog.open(NeedHelpDialogComponent, {
    //   autoFocus: false,
    //   panelClass: ["dialog-xsm", "need-help-dialog"],
    // });
  }

  currentModule: string = "";

  setActiveModule() {
    // Check the first part of the URL to determine the module
    const url = this.router.url;
    if (url.includes("/super-admin")) {
      this.currentModule = "Super Admin";
    } else if (url.includes("/company")) {
      this.currentModule = "Company Owner";
    } else if (url.includes("/project-manager")) {
      this.currentModule = "Project Manager";
    }
  }
}
