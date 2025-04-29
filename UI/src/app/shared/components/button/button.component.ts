import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';

import {MatButton} from '@angular/material/button';
import {MatTooltip} from '@angular/material/tooltip';
import { ActionButtonType, ButtonVariant } from './models';
import { ThemePalette } from "@angular/material/core";
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { ButtonVariantDirective } from './directives';
@Component({
  selector: 'app-button',
  imports: [MatButton, MatTooltip, CommonModule, RouterModule, MatIcon, ButtonVariantDirective],
  standalone: true,
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ButtonComponent {
  label = input<string>("");
  matIcon = input<string>("");
  imageUrl = input<string>("");
  iconUrl = input<string>("");
  altText = input<string>("");
  buttonVariant = input<ButtonVariant>();
  disabled = input<boolean>(false);
  isVisible = input<boolean>(true);
  classes = input<string[]>([""]);
  type = input<ActionButtonType>("button");
  routerLink = input<string>("");
  color = input<ThemePalette>("primary");
  onClick = output<MouseEvent>();
}