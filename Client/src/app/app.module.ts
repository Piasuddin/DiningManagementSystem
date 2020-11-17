import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { DatePipe } from '@angular/common';
import { HttpConfigInterceptor } from './services/http-config-interceptor'
import { AppRoutingModule } from './app-routing.module';
import { BnNgIdleService } from 'bn-ng-idle';
import { NavmenuComponent } from './home/components/navmenu/navmenu.component';
import { LogInComponent } from './user/components/log-in/log-in.component';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { LoginEventService } from './services/Login-event.service';
import { LoginGuardService } from './services/login-guard.service';
import { DataService } from './services/data.service';
import { AppCommonModule } from './components/app-common.module';
import { LayoutModule } from '@angular/cdk/layout';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { PdfPreviewComponent } from './components/pdf-preview/pdf-preview.component';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { ForgotPasswordComponent } from './user/components/forgot-password/forgot-password.component';
import { DashboardComponent } from './user/components/dashboard/dashboard.component';
import { UserAccessDeniedComponent } from './merp-common/components/user-access-denied/user-access-denied.component';
import { ReportService } from './merp-common/services/report.service';
import { getTemplate } from './merp-common/common-method';
import { MomentUtcDateAdapter } from './merp-common/services/moment-utc-date-adapter.service';
import { CreateNewUserComponent } from './administration/components/create-new-user/create-new-user.component';
import { AppDeleteConfirmDialogComponent } from './components/app-delete-confirm-dialog/app-delete-confirm-dialog.component';
import { AppSideNavComponent } from './components/app-side-nav/app-side-nav.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { DateAdapter } from '@angular/material/core';
import { AppMatDialogComponent } from './components/app-mat-dialog/app-mat-dialog.component';
import {TranslateModule, TranslateLoader} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    NavmenuComponent,
    LogInComponent,
    ForgotPasswordComponent,
    DashboardComponent,
    UserAccessDeniedComponent,
    CreateNewUserComponent,
    AppSideNavComponent,
    AppMatDialogComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule,
    AppRoutingModule,
    MaterialModule,
    BrowserAnimationsModule,
    AppCommonModule,
    LayoutModule,
    PdfViewerModule,
    MatMomentDateModule,
    MatToolbarModule,
    MatMenuModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
      },
      defaultLanguage: 'en'
  })
  ],

  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true },
    { provide: DateAdapter, useClass: MomentUtcDateAdapter },
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    DatePipe, BnNgIdleService, LoginEventService, LoginGuardService,
    DataService, ReportService, { provide: '', useFactory: getTemplate, deps: [HttpClient] }
  ],
  bootstrap: [AppComponent],
  exports: [
    BrowserModule,
    BrowserAnimationsModule
  ],
  entryComponents: [
    PdfPreviewComponent, AppDeleteConfirmDialogComponent,
    AppMatDialogComponent
  ]
})
export class AppModule { }
