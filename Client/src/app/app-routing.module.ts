import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadingStrategy, PreloadAllModules } from '@angular/router';
import { LoginGuardService } from './services/login-guard.service';
import { LogInComponent } from './user/components/log-in/log-in.component';
import { ForgotPasswordComponent } from './user/components/forgot-password/forgot-password.component';
import { DashboardComponent } from './user/components/dashboard/dashboard.component';
import { UserAccessDeniedComponent } from './merp-common/components/user-access-denied/user-access-denied.component';
import { CreateNewUserComponent } from './administration/components/create-new-user/create-new-user.component';

const routes: Routes = [
  { path: "", component: DashboardComponent },
  { path: "dashboard", component: DashboardComponent },
  { path: "login", component: LogInComponent },
  { path: "forgetPassword", component: ForgotPasswordComponent },
  { path: "accessDenied", component: UserAccessDeniedComponent },
  { path: "createNewUser", component: CreateNewUserComponent },
  { path: "createNewUser/:id", component: CreateNewUserComponent },
  // {path: 'home',
  // loadChildren: "./home/home.module#HomeModule", canActivate: [LoginGuardService]},
  {
    path: 'addmission',
    loadChildren: "./addmission/addmission.module#AddmissionModule", canActivate: [LoginGuardService]
  },
  {
    path: 'education',
    loadChildren: "./education/education.module#EducationModule", canActivate: [LoginGuardService]
  },
  {
    path: "examination",
    loadChildren: "./examination/examination.module#ExaminationModule", canActivate: [LoginGuardService]
  },
  {
    path: "finance",
    loadChildren: "./finance/finance.module#FinanceModule", canActivate: [LoginGuardService]
  },
  {
    path: "administration",
    loadChildren: "./administration/administration.module#AdministrationModule", canActivate: [LoginGuardService]
  },
  {
    path: "human-resource",
    loadChildren: "./human-resource/human-resource.module#HumanResourceModule", canActivate: [LoginGuardService]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
