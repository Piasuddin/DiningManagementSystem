import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Campus } from 'src/app/administration/models/campus';
import { Institute } from 'src/app/administration/models/Institute.model';
import { AppDeleteConfirmDialogComponent } from 'src/app/components/app-delete-confirm-dialog/app-delete-confirm-dialog.component';
import { CommonEnum, DiningMealStatus, MonthsOfYear, ResponseStatusCodeEnum } from 'src/app/enums/Enums';
import { CommonMethod, getSearchBoxStyle } from 'src/app/merp-common/common-method';
import { ResponseMessage } from 'src/app/model/response-message';
import { DataSortingHelperService } from 'src/app/services/data-sorting-helper.service';
import { DataService } from 'src/app/services/data.service';
import { MatTableService } from 'src/app/services/table-search-box.service';
import { DiningMealAttendanceTableData } from '../../models/dining-meal-manage.model';
import { DiningMealAttendanceService } from '../../services/dining-meal-attendance.service';

@Component({
  selector: 'app-dining-meal-attendance',
  templateUrl: './dining-meal-attendance.component.html',
  styleUrls: ['./dining-meal-attendance.component.css'],
  styles: [getSearchBoxStyle()]
})
export class DiningMealAttendanceComponent implements OnInit {
  othersData: any;
  filterForm: FormGroup;
  monthsOfYear = MonthsOfYear;
  response: ResponseMessage = new ResponseMessage();
  diningMealManageTableData: DiningMealAttendanceTableData = new DiningMealAttendanceTableData();
  diningMealManageDetails: any;
  dataSource = new MatTableDataSource();
  isShow: boolean = false;
  reset: any = [{}];
  commonEnum = CommonEnum;
  lstOfCampus: Campus[] = [];
  lstOfInstitute: Institute[] = [];
  diningMealStatus = DiningMealStatus;
  obj = { month: new Date().getMonth() + 1, year: new Date().getFullYear() };
  displayedColumns = ["mealDate", "numberOfBoarder", "takenMeal", "statusName", "Action"];
  constructor(private dataService: DataService, private diningMealAttendanceService: DiningMealAttendanceService,
    private dataSortingHelperService: DataSortingHelperService, private matTableService: MatTableService,
    private dialog: MatDialog, public commonMethod: CommonMethod, private formBuilder: FormBuilder) {
    this.filterForm = this.formBuilder.group({
      month: [''],
      year: [''],
      isNext: [false]
    });
    let obj = { month: new Date().getMonth() + 1, year: new Date().getFullYear() }
    this.filterForm.patchValue(obj);
  }
  ngOnInit() {
    this.commonMethod.columnIndex = 3;
    if (this.dataService.responseMessageData != null) {
      this.response = this.dataService.responseMessageData;
      this.dataService.setValueToResponseMessageProperty(null);
    }
    this.getDiningMealAttendanceTableData(this.obj);
  }
  getDiningMealAttendanceTableData(obj: any): void {
    this.diningMealAttendanceService.searchDiningMealAttendanceForTable(JSON.stringify(obj)).subscribe((res: any) => {
      if (res.responseObj != null) {
        this.diningMealManageDetails = res.responseObj;
        this.othersData = res.responseObj.others;
        if (this.commonMethod.showForSuperAdmin()) {
          this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "instituteName");
          this.lstOfInstitute = this.diningMealManageDetails.diningMealManage.filter((v, i, a) =>
            a.indexOf(this.diningMealManageDetails.diningMealManage.find(e => e.instituteId == v.instituteId)) === i);
          this.commonMethod.columnIndex += 1;
        }
        if (this.commonMethod.showForInstituteAdmin() || this.commonMethod.showForSuperAdmin()) {
          this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "campusName");
          this.lstOfCampus = this.diningMealManageDetails.diningMealManage.filter((v, i, a) =>
            a.indexOf(this.diningMealManageDetails.diningMealManage.find(e => e.campusId == v.campusId)) === i);
        }
        this.dataSource.data = this.diningMealManageDetails.diningMealManage;
      }
      if (this.prevYearChange) {
        this.prevYearChange = false;
        this.filterForm.get('month').setValue(this.othersData.months[this.othersData.months.length - 1]);
      }
      if (this.nextYearChange) {
        this.nextYearChange = false;
        this.filterForm.get('month').setValue(this.othersData.months[0]);
      }
    })
  }
  // resizeData(diningMealManageTableData: DiningMealManageTableData) {
  //   // diningMealManageTableData.diningMealManage = diningMealManageTableData.diningMealManage.sort((x , y)=>{
  //   //   return x < y ? -1 : x > y ? 1 : 0;
  //   // })
  //   let totalStudent;
  //   let diningMealManageDetails = [];
  //   for (let i = 0; i < diningMealManageTableData.diningMealManage.length; i++) {
  //     let lstTakenMeal = [];
  //     let details: DiningMealManageDetails = new DiningMealManageDetails();
  //     var deserializeData: any[] = <any[]>JSON.parse(this.diningMealManageTableData.diningMealManage[i].boarderWithTakenMeal);
  //     details.mealDate = diningMealManageTableData.diningMealManage[i].mealDate;
  //     details.id = diningMealManageTableData.diningMealManage[i].id;
  //     details.status = diningMealManageTableData.diningMealManage[i].status;
  //     details.numberOfStudent = deserializeData.length;
  //     diningMealManageDetails.push(details);
  //     let keys = Object.keys(deserializeData[0]);
  //     keys.forEach(e => {
  //       let takenMeal = new TakenMeal();
  //       if (!isNaN(+e)) {
  //         let count = 0;
  //         deserializeData.forEach(e2 => {
  //           if (e2[e] > 0) {
  //             count++;
  //           }
  //         })
  //         takenMeal.id = +e;
  //         takenMeal.num = count;
  //         lstTakenMeal.push(takenMeal);
  //       }
  //     })
  //     details.takenMeal = lstTakenMeal;
  //   }
  //   this.diningMealManageDetails = diningMealManageDetails;
  //   this.dataSource.data = this.diningMealManageDetails;
  // }
  onChange() {
    const obj = this.filterForm.value;
    this.getDiningMealAttendanceTableData(obj);
  }
  prevYearChange = false;
  previousClick() {
    let isLast = false;
    let obj = this.filterForm.value;
    this.othersData.months.filter((x, i) => {
      if (obj.month == x) {
        if (i == 0) {
          this.filterForm.get('month').setValue(this.othersData.months[0]);
          this.othersData.years.filter((x2, i2) => {
            if (obj.year == x2) {
              if (i2 == 0) {
                isLast = true;
                this.filterForm.get('year').setValue(this.othersData.years[0]);
                this.filterForm.get('month').setValue(this.othersData.months[0]);
              }
              else {
                this.filterForm.get('year').setValue(this.othersData.years[i2 - 1]);
                this.filterForm.get('month').setValue(0);
                this.filterForm.get('isNext').setValue(false);
                this.prevYearChange = true;
                isLast = false;
              }
            }
          })
        }
        else {
          this.filterForm.get('month').setValue(this.othersData.months[i - 1]);
        }
      }
    });
    if (!isLast) {
      this.getDiningMealAttendanceTableData(this.filterForm.value);
    }
  }
  nextYearChange = false;
  nextClick() {
    let isLast = false;
    let obj = this.filterForm.value;
    this.othersData.months.filter((x, i) => {
      if (obj.month == x) {
        if (i + 1 == this.othersData.months.length) {
          this.filterForm.get('month').setValue(this.othersData.months[0]);
          this.othersData.years.filter((x2, i2) => {
            if (obj.year == x2) {
              if (i2 + 1 == this.othersData.years.length) {
                isLast = true;
                this.filterForm.get('year').setValue(this.othersData.years[i2]);
                this.filterForm.get('month').setValue(this.othersData.months[i]);
              }
              else {
                this.filterForm.get('year').setValue(this.othersData.years[i2 + 1]);
                this.filterForm.get('month').setValue(0);
                this.filterForm.get('isNext').setValue(true);
                this.nextYearChange = true;
                isLast = false;
              }
            }
          })
        }
        else {
          this.filterForm.get('month').setValue(this.othersData.months[i + 1]);
        }
      }
    });
    if (!isLast) {
      this.getDiningMealAttendanceTableData(this.filterForm.value);
    }
  }
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  isAssending = false;
  sortTableData(event, columnHead) {
    const columnHeadName = columnHead.displayedColumns[event];

    if (columnHeadName == "mealDate") {
      this.diningMealManageDetails.diningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortDateType(a.mealDate.toString(),
          b.mealDate.toString(), this.isAssending);
      });
    }
    else if (columnHeadName == "numberOfBoarder") {
      this.diningMealManageDetails.diningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.totalBorder, b.totalBorder, this.isAssending);
      });
    }
    else if (columnHeadName == "instituteName") {
      this.diningMealManageDetails.diningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.instituteName,
          b.instituteName, this.isAssending);
      });
    }
    else if (columnHeadName == "campusName") {
      this.diningMealManageDetails.diningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.campusName,
          b.campusName, this.isAssending);
      });
    }
    else if (columnHeadName == "statusName") {
      this.diningMealManageDetails.diningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.statusName, b.statusName, this.isAssending);
      });
    }
    this.matTableService.showHideUpDowmIcon(event);
    this.isAssending = !this.isAssending;
    this.dataSource.data = this.diningMealManageDetails.diningMealManage;
  }
  showSearchBox(id: string) {
    this.isShow = !this.isShow;
    this.matTableService.showSearchBox(id, this.isShow);
  }
  closeSearchBox() {
    this.isShow = !this.isShow;
    this.matTableService.closeSearchBox();
  }
  getCurrentMonth(e: number) {
    let month = this.filterForm.get('month').value;
    if (e == +month) {
      return true;
    }
    return false;
  }
  getCurrentYear(e: number) {
    let year = this.filterForm.get('year').value;
    if (e == +year) {
      return true;
    }
    return false;
  }
  onDelete(id: number) {
    let dialogRef = this.dialog.open(AppDeleteConfirmDialogComponent, {
      width: '350px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        const data = JSON.stringify(id);
        this.diningMealAttendanceService.deleteDiningMealAttendance(data).subscribe((res: ResponseMessage) => {
          this.workWithResponse(res);
        })
      }
    });
  }
  workWithResponse(res: ResponseMessage) {
    this.response.message = res.message;
    this.response.statusCode = res.statusCode;
    if (this.response.statusCode == ResponseStatusCodeEnum.Success) {
      this.getDiningMealAttendanceTableData(JSON.stringify(this.obj));
    }
    this.reset.push({});
  }
  changeStatus(id: number, status: number, operationName: string) {
    this.commonMethod.onStatusChange(id, status, operationName, "DiningMealManage").then(e => {
      if (e) {
        this.getDiningMealAttendanceTableData(JSON.stringify(this.obj));
      }
    })
  }

}
