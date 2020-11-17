import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { Campus } from 'src/app/administration/models/campus';
import { Institute } from 'src/app/administration/models/Institute.model';
import { AppDeleteConfirmDialogComponent } from 'src/app/components/app-delete-confirm-dialog/app-delete-confirm-dialog.component';
import { AppMatDialogComponent } from 'src/app/components/app-mat-dialog/app-mat-dialog.component';
import { CommonEnum, ResponseStatusCodeEnum } from 'src/app/enums/Enums';
import { CommonMethod, getSearchBoxStyle } from 'src/app/merp-common/common-method';
import { ResponseMessage } from 'src/app/model/response-message';
import { TableDataInfo } from 'src/app/model/table-data-info.model';
import { DataSortingHelperService } from 'src/app/services/data-sorting-helper.service';
import { DataService } from 'src/app/services/data.service';
import { CachedTableDataService } from 'src/app/services/cached-table-data.Service';
import { MatTableService } from 'src/app/services/table-search-box.service';
import { DiningMealManage } from '../../models/dining-meal-manage.model';
import { DiningMealManageService } from '../../services/dining-meal-manage.service';

@Component({
  selector: 'app-dining-meal-manage',
  templateUrl: './dining-meal-manage.component.html',
  styleUrls: ['./dining-meal-manage.component.css'],
  styles: [getSearchBoxStyle()]
})
export class DiningMealManageComponent implements OnInit {
  response: ResponseMessage = new ResponseMessage();
  lstDiningMealManage: any[] = [];
  dataSource = new MatTableDataSource();
  isShow: boolean = false;
  reset: any = [{}];
  commonEnum = CommonEnum;
  // lstOfCampus: Campus[] = [];
  // lstOfInstitute: Institute[] = [];
  displayedColumns = ["diningMealManageNo", "fromDateString", "toDateString", "totalBoarder", "takenMeals", "status", "Action"];
  constructor(private dataService: DataService, private diningMealManageService: DiningMealManageService,
    private dataSortingHelperService: DataSortingHelperService, private matTableService: MatTableService,
    private dialog: MatDialog, public commonMethod: CommonMethod, private matSnackBar: MatSnackBar,
    public cachedTableDataService: CachedTableDataService) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  ngOnInit() {
    this.cachedTableDataService.selectedCampus(53);
    if (this.dataService.responseMessageData != null) {
      this.response = this.dataService.responseMessageData;
      this.dataService.setValueToResponseMessageProperty(null);
    }
    this.getDiningMealManageTableData(this.cachedTableDataService.campusId);
    this.cachedTableDataService.campusChangeEvent.subscribe(e => {
      this.getDiningMealManageTableData(e);
    })
  }
  getDiningMealManageTableData(e: number): void {
    this.dataSource.data = [];
    if ((this.commonMethod.showForInstituteAdmin() && e > 0) || this.commonMethod.showForCampusAdmin()) {
      let data = new TableDataInfo();
      data.campusId = e;
      this.diningMealManageService.searchDiningMealManageForTable(data).subscribe((res: ResponseMessage) => {
        this.lstDiningMealManage = <DiningMealManage[]>res.responseObj;
        if (this.lstDiningMealManage != null) {
          this.dataSource.data = this.lstDiningMealManage;
        }
      })
    }
    // if (this.commonMethod.showForCampusAdmin()) {
    //   this.diningMealManageService.searchDiningMealManageForTable("").subscribe((res: ResponseMessage) => {
    //     this.lstDiningMealManage = <DiningMealManage[]>res.responseObj;
    //     if (this.lstDiningMealManage != null) {
    //       this.dataSource.data = this.lstDiningMealManage;
    //     }
    //   })
    // }
    // this.diningMealManageService.searchDiningMealManageForTable().subscribe((res: ResponseMessage) => {
    //   if (res.responseObj != null) {
    //     this.lstDiningMealManage = <DiningMealManage[]>res.responseObj;
    //     if (this.commonMethod.showForSuperAdmin()) {
    //       this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "instituteName");
    //       this.lstOfInstitute = this.lstDiningMealManage.filter((v, i, a) =>
    //         a.indexOf(this.lstDiningMealManage.find(e => e.instituteId == v.instituteId)) === i);
    //       this.commonMethod.columnIndex += 1;
    //     }
    //     if (this.commonMethod.showForInstituteAdmin() || this.commonMethod.showForSuperAdmin()) {
    //       this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "campusName");
    //       this.lstOfCampus = this.lstDiningMealManage.filter((v, i, a) =>
    //         a.indexOf(this.lstDiningMealManage.find(e => e.campusId == v.campusId)) === i);
    //     }
    //     this.dataSource.data = this.lstDiningMealManage;
    //   }
    // })
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
    if (columnHeadName == "diningMealManageNo") {
      this.lstDiningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.diningMealManageNo,
          b.diningMealManageNo, this.isAssending);
      });
    }
    else if (columnHeadName == "fromDateString") {
      this.lstDiningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortDateType(a.fromDate, b.fromDate, this.isAssending);
      });
    }
    else if (columnHeadName == "toDateString") {
      this.lstDiningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortDateType(a.toDate, b.toDate, this.isAssending);
      });
    }
    else if (columnHeadName == "totalBoarder") {
      this.lstDiningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.totalBoarder, b.totalBoarder, this.isAssending);
      });
    }
    else if (columnHeadName == "takenMeals") {
      this.lstDiningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.takenMeals, b.takenMeals, this.isAssending);
      });
    }
    // else if (columnHeadName == "instituteName") {
    //   this.lstDiningMealManage.sort((a, b) => {
    //     return this.dataSortingHelperService.sortStringType(a.instituteName,
    //       b.instituteName, this.isAssending);
    //   });
    // }
    // else if (columnHeadName == "campusName") {
    //   this.lstDiningMealManage.sort((a, b) => {
    //     return this.dataSortingHelperService.sortStringType(a.campusName,
    //       b.campusName, this.isAssending);
    //   });
    // }
    else if (columnHeadName == "status") {
      this.lstDiningMealManage.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.statusName, b.statusName, this.isAssending);
      });
    }
    this.matTableService.showHideUpDowmIcon(event);
    this.isAssending = !this.isAssending;
    this.dataSource.data = this.lstDiningMealManage;
  }
  showSearchBox(id: string) {
    this.isShow = !this.isShow;
    this.matTableService.showSearchBox(id, this.isShow);
  }
  closeSearchBox() {
    this.isShow = !this.isShow;
    this.matTableService.closeSearchBox();
  }
  workWithResponse(res: ResponseMessage) {
    this.response.message = res.message;
    this.response.statusCode = res.statusCode;
    this.reset.push({})
  }
  onDelete(id: number) {
    let dialogRef = this.dialog.open(AppDeleteConfirmDialogComponent, {
      width: '350px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        const data = JSON.stringify(id);
        this.diningMealManageService.deleteDiningMealManage(data).subscribe((res: ResponseMessage) => {
          this.response.message = res.message;
          this.response.statusCode = res.statusCode;
          this.getDiningMealManageTableData(this.cachedTableDataService.campusId);
          this.reset.push({});
        })
      }
    });
  }
  onStatusChange(id: number, status: number, operationName: string) {
    let dialogRef = this.dialog.open(AppMatDialogComponent, {
      width: '350px',
      data: {
        action: operationName
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.commonMethod.changeStatus("DiningMealManage", id, status, operationName).subscribe((res: ResponseMessage) => {
          if (res.statusCode == ResponseStatusCodeEnum.Success) {
            this.getDiningMealManageTableData(this.cachedTableDataService.campusId);
          }
          this.matSnackBar.open(res.message, "OK", {
            duration: 10000
          })
        });
      }
    })
  };
}
