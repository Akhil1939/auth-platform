import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  effect,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ErrorMessage, InputType } from './models';
import { ArrowEnum } from './enums/input.enum';
import { CommonModule } from '@angular/common';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-input',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatIcon,
    MatIconButton,
  ],
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InputComponent {
  private destroy$ = inject(DestroyRef);

  label = input<string>('');
  type = input<InputType>('text');
  placeholder = input<string>('');
  control = input<FormControl | null>(null);
  customClass = input<string>('w-full');
  prefix = input<string>('');
  prefixIcon = input<string>('');
  prefixImgPath = input<string>('');
  suffix = input<string>('');
  suffixIcon = input<string>('');
  suffixImgPath = input<string>('');
  suffixButtonIcon = input<string>('');
  suffixButtonImgPath = input<string>('');
  customErrorMessage = input<ErrorMessage[]>([]);
  prefixClass = input<string>('');
  suffixClass = input<string>('');
  required = input<boolean>(false);
  disabled = input<boolean>(false);
  readonly = input<boolean>(false);
  suffixText = input<string>('');
  prefixText = input<string>('');
  hideLabel = input<boolean>(false);
  value = input<string>('');
  inputClasses = input<string[]>([]);
  // Number-only Props
  min = input<number>(0);
  max = input<number>(Infinity);
  step = input<number>(1);
  showArrowsForNumber = input<boolean>(false);
  allowDecimal = input<boolean>(true);

  suffixButtonClick = output<void>();
  valueChange = output<string>();

  activeArrow = signal<'up' | 'down' | null>(null);

  internalControl = new FormControl(
    { value: '', disabled: this.disabled() },
    { nonNullable: true }
  );

  get activeControl(): FormControl {
    return this.control() ?? this.internalControl;
  }

  constructor() {
    this.setupControl();
    this.setupValueChanges();
  }

  writeValue(value: string): void {
    this.activeControl.setValue(value ?? '', { emitEvent: false });
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    isDisabled
      ? this.activeControl.disable({ emitEvent: false })
      : this.activeControl.enable({ emitEvent: false });
  }

  adjustValue(action: 'increment' | 'decrement'): void {
    if (this.readonly() || this.disabled()) return;

    const currentValue = Number(this.activeControl.value) || 0;
    const step = this.step();
    const min = this.min();
    const max = this.max();
    const isDecimalAllowed = this.allowDecimal();

    let newValue =
      action === 'increment' ? currentValue + step : currentValue - step;

    newValue = Math.min(Math.max(newValue, min), max);

    this.activeControl.setValue(
      isDecimalAllowed ? newValue : Math.floor(newValue)
    );
  }

  onKeyDown(event: KeyboardEvent): void {
    if (this.type() !== 'number' || this.readonly() || this.disabled()) return;

    switch (event.key) {
      case ArrowEnum.ArrowUp:
        this.adjustValue('increment');
        this.activeArrow.set('up');
        event.preventDefault();
        break;

      case ArrowEnum.ArrowDown:
        this.adjustValue('decrement');
        this.activeArrow.set('down');
        event.preventDefault();
        break;

      case '.':
        if (!this.allowDecimal()) event.preventDefault();
        break;
    }
  }

  onKeyUp(event: KeyboardEvent): void {
    if (this.readonly() || this.disabled()) return;
    if (event.key === ArrowEnum.ArrowUp || event.key === ArrowEnum.ArrowDown) {
      this.activeArrow.set(null);
    }
  }

  errorMessage(): string {
    if (!this.activeControl.errors) return '';

    const errors = this.activeControl.errors;
    const customError = this.customErrorMessage().find((e) => errors[e.key]);
    if (customError) return customError.error;

    const label = this.label() || 'This field';
    if (errors['required']) return `${label} is required`;
    if (errors['minlength'])
      return `${label} must be at least ${errors['minlength'].requiredLength} characters`;
    if (errors['maxlength'])
      return `${label} must not exceed ${errors['maxlength'].requiredLength} characters`;
    if (errors['min']) return `${label} should be minimum ${errors['min'].min}`;
    if (errors['max']) return `${label} can be max ${errors['max'].max}`;
    if (errors['pattern']) return `${label} format is invalid`;
    return 'Invalid input';
  }

  onTouched: () => void = () => {
    let value = this.activeControl.value;

    if (this.type() === 'number') {
      const numericValue = Number(value);
      value = this.allowDecimal()
        ? numericValue
        : Math.round(numericValue || 0);
    } else if (typeof value === 'string') {
      value = value.trim();
    }

    const isEmpty =
      value === '' ||
      value === null ||
      value === undefined ||
      Number.isNaN(value);
    this.activeControl.setValue(isEmpty ? null : value);
  };

  private setupControl(): void {
    if (!this.control()) {
      if (this.required()) {
        this.internalControl.addValidators(Validators.required);
      }

      effect(() => {
        this.internalControl.setValue(this.value(), { emitEvent: false });
        this.disabled()
          ? this.internalControl.disable({ emitEvent: false })
          : this.internalControl.enable({ emitEvent: false });
      });
    }
  }

  private setupValueChanges(): void {
    this.activeControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntilDestroyed(this.destroy$)
      )
      .subscribe((value) => {
        this.onChange(value);

        const trimmedValue = typeof value === 'string' ? value.trim() : value;
        switch (this.type()) {
          case 'search':
            if (!trimmedValue || trimmedValue.length > 2) {
              this.valueChange.emit(trimmedValue);
            }
            break;
          case 'email':
            this.valueChange.emit(trimmedValue?.toLowerCase?.() ?? '');
            break;
          default:
            this.valueChange.emit(value);
        }
      });
  }

  private onChange: (value: string) => void = () => {};
}
