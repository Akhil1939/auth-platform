import { ChangeDetectionStrategy, Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { IMAGES } from "../../../../shared/models/images";
import { SliderComponent } from "../../../../shared/components";

@Component({
  selector: "app-auth-layout",
  imports: [RouterOutlet, SliderComponent],
  templateUrl: "./auth-layout.component.html",
  styleUrls: ["./auth-layout.component.scss"],
  standalone: true,
  changeDetection:ChangeDetectionStrategy.OnPush
})
export class AuthLayoutComponent {
  imagePaths = IMAGES;
}
