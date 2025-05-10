import { ApplicationConfig, provideZoneChangeDetection } from "@angular/core";
import { provideRouter } from "@angular/router";
import { routes } from "./app.routes";
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from "@angular/material/form-field";
import {
  provideHttpClient,
  HttpClient,
  withInterceptors,
  withInterceptorsFromDi,
  HTTP_INTERCEPTORS,
} from "@angular/common/http";
import { TranslateLoader, provideTranslateService } from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from "@angular/material/snack-bar";
import { baseInterceptor } from "./core/interceptors/base.interceptor";
import { LoaderInterceptor } from "@core/interceptors/loader.interceptor";

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    provideTranslateService(),
    provideHttpClient(
      withInterceptors([baseInterceptor]),
      withInterceptorsFromDi()
    ),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoaderInterceptor,
      multi: true,
    },
    {
      provide: TranslateLoader,
      useFactory: (http: HttpClient) =>
        new TranslateHttpLoader(http, "./assets/i18n/", ".json"),
      deps: [HttpClient],
    },
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: { subscriptSizing: "dynamic", appearance: "outline" },
    },
    {
      provide: MAT_SNACK_BAR_DEFAULT_OPTIONS,
      useValue: {
        duration: 5000,
        horizontalPosition: "right",
        verticalPosition: "top",
      },
    },
  ],
};
