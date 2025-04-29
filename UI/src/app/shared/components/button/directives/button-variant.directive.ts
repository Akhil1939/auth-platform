import { Directive, ElementRef, input, Renderer2 } from "@angular/core";
import { ButtonVariant } from "../models";

@Directive({
  selector: "[ButtonVariant]",
})
export class ButtonVariantDirective {
  buttonVariant = input<ButtonVariant>();

  constructor(private elementRef: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit(): void {
    this.setButtonAttributes();
  }

  private setButtonAttributes() {
    const buttonElement = this.elementRef.nativeElement;

    buttonElement.removeAttribute("mat-button");

    switch (this.buttonVariant()) {
      case "raised":
        this.renderer.setAttribute(buttonElement, "mat-raised-button", "");
        this.renderer.addClass(buttonElement, "mdc-button--raised");
        this.renderer.addClass(buttonElement, "mat-mdc-raised-button");
        break;
      case "stroked":
        this.renderer.setAttribute(buttonElement, "mat-stroked-button", "");
        this.renderer.addClass(buttonElement, "mdc-button--outlined");
        this.renderer.addClass(buttonElement, "mat-mdc-outlined-button");
        break;
      case "flat":
        this.renderer.setAttribute(buttonElement, "mat-flat-button", "");
        this.renderer.addClass(buttonElement, "mdc-button--unelevated");
        this.renderer.addClass(buttonElement, "mat-mdc-unelevated-button");
        break;
      case "icon":
        this.renderer.setAttribute(buttonElement, "mat-icon-button", "");
        this.renderer.removeClass(buttonElement, "mdc-button");
        this.renderer.removeClass(buttonElement, "mat-mdc-button");
        this.renderer.addClass(buttonElement, "mdc-icon-button");
        this.renderer.addClass(buttonElement, "mat-mdc-icon-button");
        break;
      case "menu":
        this.renderer.setAttribute(buttonElement, "mat-menu-item", "");
        this.renderer.removeClass(buttonElement, "mdc-button");
        this.renderer.removeClass(buttonElement, "mat-mdc-button");
        this.renderer.addClass(buttonElement, "mat-mdc-menu-item");
        this.renderer.addClass(buttonElement, "mat-focus-indicator");
        break;
      default:
        this.renderer.setAttribute(buttonElement, "mat-button", ""); // Default to mat-button
        break;
    }
  }
}
