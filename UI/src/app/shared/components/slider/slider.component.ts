import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  ViewChild,
} from "@angular/core";
import KeenSlider, { KeenSliderInstance } from "keen-slider";
import { IMAGES } from "../../models/images";

@Component({
  selector: "app-slider",
  imports: [],
  templateUrl: "./slider.component.html",
  styleUrls: ["./slider.component.scss"],
  standalone: true,
})
export class SliderComponent implements AfterViewInit {
  imagePaths = IMAGES;
  @ViewChild("sliderRef") sliderRef!: ElementRef<HTMLElement>;

  currentSlide: number = 0;
  dotHelper: Array<number> = [];
  slider!: KeenSliderInstance;

  ngAfterViewInit() {
    this.initializeSlider();
  }

  @HostListener("window:resize")
  onResize() {
    this.updateSlider();
  }

  initializeSlider() {
    setTimeout(() => {
      this.slider = new KeenSlider(
        this.sliderRef.nativeElement,
        {
          initial: this.currentSlide,
          loop: true,
          slideChanged: (s) => {
            this.currentSlide = s.track.details.rel;
          },
        },
        [
          (slider) => {
            let timeout: ReturnType<typeof setTimeout>;
            let mouseOver = false;

            const clearNextTimeout = () => clearTimeout(timeout);

            const nextTimeout = () => {
              clearNextTimeout();
              if (mouseOver) return;
              timeout = setTimeout(() => slider.next(), 2000);
            };

            slider.on("created", () => {
              slider.container.addEventListener("mouseover", () => {
                mouseOver = true;
                clearNextTimeout();
              });

              slider.container.addEventListener("mouseout", () => {
                mouseOver = false;
                nextTimeout();
              });

              nextTimeout();
            });

            slider.on("dragStarted", clearNextTimeout);
            slider.on("animationEnded", nextTimeout);
            slider.on("updated", nextTimeout);
          },
        ]
      );

      this.dotHelper = [
        ...Array(this.slider.track.details.slides.length).keys(),
      ];
    }, 10);
  }

  updateSlider() {
    if (this.slider) {
      this.slider.update();
    }
  }

  ngOnDestroy() {
    if (this.slider) {
      // this.slider.destroy();
    }
  }
}
