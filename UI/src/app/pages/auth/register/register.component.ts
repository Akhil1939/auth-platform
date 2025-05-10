import { Component, inject, signal } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { IMAGES } from '../../../shared/models/images';
import { SystemConstant } from '../../../shared/models/system.constant';
import { InputComponent } from '../../../shared/components/input/input.component';
import { ButtonComponent } from '../../../shared/components/button/button.component';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
   InputComponent,
   ButtonComponent,
   RouterLink,
    FormsModule,
    CommonModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  standalone: true,
  providers: [],
})
export class RegisterComponent {
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);

  hide = true;
  imagePaths = IMAGES;
  isButtonDisabled = signal<boolean>(false);

  formGroup: FormGroup<{
    email: FormControl<string>;
    fullName: FormControl<string>;
    password: FormControl<string>;
  }> = this.formBuilder.nonNullable.group({
    fullName: ['', [Validators.maxLength(100), Validators.required]],
    email: [
      '',
      [
        Validators.required,
        Validators.pattern(SystemConstant.EMAIL_REGEX),
        Validators.maxLength(250),
      ],
    ],
    password: [
      '',
      [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(20),
        Validators.pattern(SystemConstant.PASSWORD_REGEX),
      ],
    ],
  });

  submitForm() {
    this.isButtonDisabled.set(true);
    if (this.formGroup.valid) {
      console.info(this.formGroup.value);
    }
  }
}
