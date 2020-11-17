import { Component, OnInit, ChangeDetectorRef, AfterContentChecked, Inject, EventEmitter } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, Event } from '@angular/router';
import { SecuirityService } from './services/secuirity.service';
import { ResponseMessage } from './model/response-message';
import { BnNgIdleService } from 'bn-ng-idle';
import { LoginEventService } from './services/Login-event.service';
import { UserService } from './administration/services/user.service';
import { UserDetails } from './administration/models/users';
import { AppHost } from './model/app-host.model';
import { UserAuthorization } from './administration/models/user-authorization.model';
import { UserTypes } from './enums/Enums';
import { CommonMethod } from './merp-common/common-method';
import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { DOCUMENT } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { MatDrawer } from '@angular/material/sidenav';
import { TranslateService } from '@ngx-translate/core';
import { AppLanguageService } from './services/app-language.service';
declare const removeHeaderShadow: any;
declare const onMenuClick: any;
declare const updateTabTitle: any;
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  styles: [`
  ::ng-deep .mat-menu-panel{
    height: 400px!important;
    width: 312px!important;
}`]
})
export class AppComponent implements OnInit, AfterContentChecked {

  public IsLoggedIn: boolean = false;
  user: UserDetails = new UserDetails();
  appHost: AppHost = new AppHost();
  info: UserAuthorization = new UserAuthorization();
  userType: string;
  userTypes = UserTypes;
  logoSize = "21%";
  smallScreen: boolean = false;
  test: EventEmitter<boolean> = new EventEmitter<boolean>(false);
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  navigationObject = [
    {
      "id": 0,
      "module": "Home",
      "subModule": [
        {
          "id": 0,
          "name": "Dashboard",
          "action": [],
          "link": "/"
        }
      ]
    },
    {
      "id": 1,
      "module": "Admission",
      "subModule": [
        {
          "id": 2,
          "name": "Application",
          "action": [],
          "link": "/addmission/applicationlist"
        },
        {
          "id": 0,
          "name": "Application Test",
          "action": [],
          "link": "/"
        }
      ]
    },
    {
      "id": 3,
      "module": "Education",
      "subModule": [
        {
          "id": 56,
          "name": "Student",
          "action": [],
          "link": "/education/students"
        },
        {
          "name": "Class Setting",
          "action": [
            { "id": 43, "actionName": "Class Name", "link": "/education/className" },
            { "id": 58, "actionName": "Class Subject", "link": "/education/classSubject" },
            { "id": 41, "actionName": "Class Group", "link": "/education/classGroup" },
            { "id": 52, "actionName": "Shift/Section", "link": "/education/ShiftSection" },
            { "id": 49, "actionName": "Class Period", "link": "/education/classPeriod" }
          ]
        },
        {
          "id": 46,
          "name": "Class Routine",
          "action": [],
          "link": "/education/classRoutine"
        },
        {
          "id": 53,
          "name": "Class Attendance",
          "action": [],
          "link": "/education/attendence"
        }
      ]
    },
    {
      "id": 4,
      "module": "Examination",
      "subModule": [
        {
          "name": "Exam",
          "action": [
            { "id": 68, "actionName": "Exam Name", "link": "/examination/examName" },
            { "id": 72, "actionName": "Exam Setting", "link": "/examination/examSetting" },
          ]
        },
        {
          "id": 76,
          "name": "Examiner",
          "action": [],
          "link": "/examination/examiner"
        },
        {
          "name": "Examine",
          "action": [
            { "id": 74, "actionName": "Examine Setting", "link": "/examination/examineSetting" },
            { "id": 70, "actionName": "Exam Obtained Mark", "link": "/examination/examObtainedMark" },
          ]
        }
      ]
    },
    {
      "id": 5,
      "module": "Finance",
      "subModule": [
        {
          "name": "Initialization",
          "action": [
            { "id": 171, "actionName": "Initial Value", "link": "/finance/initialValue" },
          ]
        },
        {
          "name": "Fees",
          "action": [
            { "id": 162, "actionName": "Fees", "link": "/finance" },
            { "id": 163, "actionName": "Fee Setting", "link": "/finance/feeSetting" },
            { "id": 159, "actionName": "Fee Collection", "link": "/finance/feeCollection" },
            { "id": 183, "actionName": "Student Ledger", "link": "/finance/studentLedger" },
            { "id": 248, "actionName": "Fee Deduction", "link": "/finance/feeDeduction" },
            { "id": 251, "actionName": "Fee Addition", "link": "/finance/feeAddition" }
          ]
        },
        {
          "name": "Donation",
          "action": [
            { "id": 150, "actionName": "Donor", "link": "/finance/donor" },
            { "id": 147, "actionName": "Donation Setting", "link": "/finance/donationSetting" },
            { "id": 146, "actionName": "Donation Head", "link": "/finance/donationHead" },
            { "id": 145, "actionName": "Donation Collection", "link": "/finance/donationCollection" }
          ]
        },
        {
          "name": "Expanse",
          "action": [
            { "id": 169, "actionName": "General Expense", "link": "/finance/generalExpanse" },
            { "id": 165, "actionName": "Fixed Expense", "link": "/finance/fixedExpanse" },
            { "id": 180, "actionName": "Purchase Goods", "link": "/finance/purchaseGoods" },
            { "id": 179, "actionName": "Project Expense", "link": "/finance/projectExpanse" },
            { "id": 132, "actionName": "Dining Expense", "link": "/finance/diningExpense" },
            { "id": 229, "actionName": "Salary Payment", "link": "/finance/salaryPayment" },
            { "id": 187, "actionName": "Donation Distribution", "link": "/finance/swDonationDistribution" },
            { "id": 190, "actionName": "Voucher", "link": "/finance/voucher" },
            { "id": 182, "actionName": "Show Expense", "link": "/finance/showExpanses" },
            { "id": 152, "actionName": "Expense Head", "link": "/finance/expanseHead" },
            { "id": 167, "actionName": "Fixed Expense Setting", "link": "/finance/fixedExpanseSetting" },
            { "id": 241, "actionName": "Purchased Products", "link": "/finance/purchasedProduct" }
          ]
        },
        {
          "name": "Balance Transfer",
          "action": [
            { "id": 173, "actionName": "Internal Transfer", "link": "/finance/internalTransfer" },
            { "id": 157, "actionName": "External Transfer", "link": "/finance/externalTransfer" },
            { "id": 121, "actionName": "Bank Transfer", "link": "/finance/bankTransfer" },
            { "id": 176, "actionName": "Mobile Bank Transfer", "link": "/finance/mobileBankTransfer" },
            { "id": 155, "actionName": "External Person", "link": "/finance/externalPerson" },
            { "id": 261, "actionName": "Suplier Company", "link": "/finance/supplierCompany" },
            { "id": 118, "actionName": "Bank Account", "link": "/finance/bankAccount" },
            { "id": 254, "actionName": "Mobile Banking Account", "link": "/finance/mobileBankingAccount" }
          ]
        },
        {
          "name": "Student Welfare",
          "action": [
            { "id": 143, "actionName": "Donation Candidate", "link": "/finance/donationCandidate" },
            { "id": 185, "actionName": "Donation Allocation", "link": "/finance/swDonationAllocation" },
            { "id": 150, "actionName": "Donor", "link": "/finance/donor" },
            { "id": 186, "actionName": "Donation Collection", "link": "/finance/swDonationCollection" },
            { "id": 189, "actionName": "Donation Setting", "link": "/finance/swDonationSetting" },
            { "id": 188, "actionName": "Donation Head", "link": "/finance/swDonationHead" }
          ]
        },
        {
          "name": "Dining",
          "action": [
            { "id": 128, "actionName": "Dining Boarder", "link": "/finance/diningBoarder" },
            { "id": 136, "actionName": "Meal Attendance", "link": "/finance/diningMealManage" },
            { "id": 124, "actionName": "Dining Bill", "link": "/finance/diningBill" },
            { "id": 125, "actionName": "Bill Collection", "link": "/finance/diningBillCollection" },
            { "id": 139, "actionName": "Stock Management", "link": "/finance/diningStockManage" },
            { "id": 137, "actionName": "Stock Adjustment", "link": "/finance/diningStockAdjustment" },
            { "id": 134, "actionName": "Dining Meal", "link": "/finance/diningMeal" },
            { "id": 141, "actionName": "Stock Product", "link": "/finance/diningStockProduct" }
          ]
        },
        {
          "name": "Report",
          "action": [
            { "id": 175, "actionName": "Journal", "link": "/finance/journal" },
            { "id": 122, "actionName": "Cash Book", "link": "/finance/cashBook" },
            { "id": 178, "actionName": "Personal Ledger", "link": "/finance/personalLedger" }
          ]
        }
      ]
    },
    {
      "id": 6,
      "module": "Human Resource",
      "subModule": [
        {
          "id": 21,
          "name": "Employee",
          "action": [],
          "link": "/human-resource/employee"
        },
        {
          "name": "Attendance",
          "action": [
            { "id": 233, "actionName": "Attendance", "link": "/human-resource/employeesAttendence" },
            { "id": 218, "actionName": "Attendance Summery", "link": "/human-resource/employeeAttendenceSummery" }
          ]
        },
        {
          "name": "Leave",
          "action": [
            { "id": 206, "actionName": "Aquired Leave", "link": "/human-resource/employeeAcquiredLeave" },
            { "id": 220, "actionName": "Availed Leave", "link": "/human-resource/employeeAvailedLeave" },
            { "id": 238, "actionName": "Leave Application", "link": "/human-resource/leaveApplication" },
            { "id": 208, "actionName": "Acquired Leave Name", "link": "/human-resource/employeeAcquiredLeaveName" },
            { "id": 210, "actionName": "Acquired Leave Setting", "link": "/human-resource/employeeAcquiredLeaveSetting" }
          ]
        },
        {
          "name": "Salary",
          "action": [
            { "id": 231, "actionName": "Salary Setting", "link": "/human-resource/employeeSalarySetting" },
            { "id": 243, "actionName": "Salary Payable", "link": "/human-resource/salaryPayable" },
            { "id": 228, "actionName": "Salary Statement", "link": "/human-resource/salaryStatement" }
          ]
        },
        {
          "name": "Allowance",
          "action": [
            { "id": 212, "actionName": "Allowance Name", "link": "/human-resource/employeeAllowanceName" },
            { "id": 214, "actionName": "Allowance Setting", "link": "/human-resource/employeeAllowanceSetting" },
          ]
        },
        {
          "name": "Provident Fund",
          "action": [
            { "id": 222, "actionName": "Provident Fund", "link": "/human-resource/empProvidentFund" },
            { "id": 224, "actionName": "Provident Fund Name", "link": "/human-resource/empProvidentFundName" },
            { "id": 226, "actionName": "Provident Fund Setting", "link": "/human-resource/empProvidentFundSetting" }
          ]
        }
      ]
    },
    {
      "id": 2,
      "module": "Admin",
      "subModule": [
        {
          "name": "Institute",
          "action": [
            { "id": 0, "actionName": "Institute Information", "link": "/administration/institute" },
            { "id": 0, "actionName": "Campus Information", "link": "/administration/campus" },
          ]
        },
        {
          "name": "User Management",
          "action": [
            { "id": 29, "actionName": "User", "link": "/administration/user" },
            { "id": 0, "actionName": "User Type", "link": "/administration/userType" },
            { "id": 0, "actionName": "Create Request", "link": "/administration/userCreateRequestList" }
          ]
        },
        {
          "name": "Holiday",
          "action": [
            { "id": 234, "actionName": "Holiday Name", "link": "/administration/holidayName" },
            { "id": 236, "actionName": "Holiday Setting", "link": "/administration/holidaySetting" },
          ]
        },
        {
          "name": "App Setting",
          "action": [
            { "id": 7, "actionName": "AcademicYear", "link": "/administration/academicYear" },
            { "id": 28, "actionName": "subject", "link": "/administration/subject" },
            { "id": 17, "actionName": "Designation", "link": "/administration/designation" },
            { "id": 25, "actionName": "Service Provider", "link": "/administration/serviceProvider" },
            { "id": 18, "actionName": "Education Board", "link": "/administration/educationBoard" },
            { "id": 5, "actionName": "Academic Institute", "link": "/administration/academicInstitute" },
            { "id": 24, "actionName": "Monthly Income", "link": "/administration/monthlyIncome" },
            { "id": 244, "actionName": "Id Generater", "link": "/administration/codeGenerate" },
            { "id": 246, "actionName": "Report Template", "link": "/administration/reportTemplate" }
          ]
        },
        {
          "id": 264,
          "name": "Exceptions",
          "action": [],
          "link": "/administration/exceptions"
        }
      ]
    },
  ];
  constructor(private router: Router, private securityService: SecuirityService, private bnIdle: BnNgIdleService,
    private loginEventService: LoginEventService, private userService: UserService,
    private changeDetector: ChangeDetectorRef, public commonMethod: CommonMethod,
    @Inject(DOCUMENT) private _document: HTMLDocument, private breakpointObserver: BreakpointObserver,
    public translate: TranslateService, private appLanguageService: AppLanguageService) {
    this.breakpointObserver.observe('(max-width: 692px)').subscribe((state: BreakpointState) => {
      if (state.matches) {
        this.smallScreen = true;
        this.logoSize = "50%";
        this.appHeaderSetUp();
      }
      else {
        this.smallScreen = false;
        this.logoSize = "21%";
        this.appHeaderSetUp();
      }
    });
  }
  ngOnInit(): void {
    if (localStorage.getItem('lan')) {
      const val = localStorage.getItem('lan');
      this.appLanguageService.setValue(val);
    }
    else {
      this.appLanguageService.setValue('en');
    }
    this.translate.use(this.appLanguageService.language);
    this.appLanguageService.event.subscribe(e => {
      this.translate.use(e);
    })
    let token = localStorage.getItem("token");
    this.router.events.subscribe((e: Event) => {
      if (e instanceof NavigationEnd) {
        updateTabTitle();
      }
    });
    if (token != null && token != undefined) {
      this.IsLoggedIn = true;
      this.info = this.userService.getUserType();
      this.getUserInfo();
    }
    this.loginEventService.loggedinValueChange.subscribe(e => {
      this.IsLoggedIn = e;
      if (this.IsLoggedIn) {
        const token2 = localStorage.getItem("token");
        if (token2 != null && token2 != undefined) {
          this.info = this.userService.getUserType();
          this.getUserInfo();
          // this.appHeaderSetUp();
        }
      }
    });
    // this.bnIdle.startWatching(600).subscribe((res) => {
    //   if(res) {
    //       this.logout();
    //       console.log("session expired");
    //   }
    // })
  }
  onMenu(drawer: MatDrawer) {
    if (this.smallScreen) {
      drawer.toggle();
    }
    else {
      if (this.info.userTypes[0]) {
        onMenuClick(true);
      }
      else {
        onMenuClick(false);
      }
    }
  }
  getUserInfo() {
    this.userService.SearchProfile("").subscribe((res: ResponseMessage) => {
      if (res.responseObj != null) {
        this.user = <UserDetails>res.responseObj;
        this.appHeaderSetUp();
        localStorage.setItem("userInfo", JSON.stringify(this.user));

        if (!this.commonMethod.showForSuperAdmin()) {
          this._document.getElementById('appFavicon').setAttribute('href', this.appHost.hostName + "images/InstituteImages/"
            + this.user.institueLogo);
        }
        if (this.commonMethod.showForInstituteAdmin()) {
          this.commonMethod.getCampuses();
        }
        if (this.info.userTypes[0] && !this.smallScreen) {
          removeHeaderShadow(true);
        }
        else {
          removeHeaderShadow(false);
        }
      }
    })
  }

  appHeaderSetUp() {
    if (this.IsLoggedIn) {
      if (this.smallScreen) {
        document.getElementById("header").classList.add('header-shadow', 'small-screen-header');
        document.getElementById("userNotifySms").classList.remove('margin-auto');
      }
      else {
        document.getElementById("header").classList.remove('header-shadow', 'small-screen-header');
        document.getElementById("userNotifySms").classList.add('margin-auto');
      }
    }
  }
  ngAfterContentChecked(): void {
    this.changeDetector.detectChanges();
  }
  title = 'merp';
  onLogout() {
    this.securityService.logout().subscribe((res: ResponseMessage) => {
      if (res.statusCode == 1) {
        removeHeaderShadow(false);
        localStorage.removeItem("token");
        localStorage.clear();
        this.router.navigateByUrl("/login");
        this.loginEventService.onLoggedinValueChange(false);
      } else {
        alert(res.message);
      }
    })

  }
  onLanSelect(e) {
    if (localStorage.getItem('lan') && localStorage.getItem('lan') != e) {
      localStorage.removeItem('lan');
      localStorage.setItem('lan', e);
    }
    else {
      localStorage.setItem('lan', e);
    }
    this.appLanguageService.setValue(e);
  }
}
