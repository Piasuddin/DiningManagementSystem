import { Component, OnInit, ViewChild } from '@angular/core';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBoarderDetails } from '../../models/dining-boarder.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatPaginator } from '@angular/material/paginator';
import { DataService } from 'src/app/services/data.service';
import { DataSortingHelperService } from 'src/app/services/data-sorting-helper.service';
import { MatTableService } from 'src/app/services/table-search-box.service';
import { DiningBoarderService } from '../../services/dining-boarder.service';
import { CommonEnum, DiningBoarderType } from 'src/app/enums/Enums';
import { CommonMethod, getSearchBoxStyle } from 'src/app/merp-common/common-method';
import { Campus } from 'src/app/administration/models/campus';
import { Institute } from 'src/app/administration/models/Institute.model';
import { AppDeleteConfirmDialogComponent } from 'src/app/components/app-delete-confirm-dialog/app-delete-confirm-dialog.component';
import { CachedTableDataService } from 'src/app/services/cached-table-data.Service';
import { TableDataInfo } from 'src/app/model/table-data-info.model';

@Component({
  selector: 'app-dining-boarder',
  templateUrl: './dining-boarder.component.html',
  styleUrls: ['./dining-boarder.component.css'],
  styles: [getSearchBoxStyle()]
})
export class DiningBoarderComponent implements OnInit {
  response: ResponseMessage = new ResponseMessage();
  lstDiningBoarder: any[] = [];
  dataSource = new MatTableDataSource();
  commonEnum = CommonEnum;
  isShow: boolean = false;
  reset: any = [{}];
  lstOfCampus: Campus[] = [];
  lstOfInstitute: Institute[] = [];
  diningBorder = DiningBoarderType;
  displayedColumns = ["boarderNo", "boarderName", "boarderTypeName", "enrollmentDateString", "statusName", "Action"];
  constructor(private dataService: DataService, private matTableService: MatTableService, private dialog: MatDialog,
    private dataSortingHelperService: DataSortingHelperService, private diningBoarderService: DiningBoarderService,
    public commonMethod: CommonMethod, private matSnackBar: MatSnackBar, public cachedTableDataService: CachedTableDataService) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  ngOnInit() {
    this.cachedTableDataService.selectedCampus(52);
    if (this.dataService.responseMessageData != null) {
      this.response = this.dataService.responseMessageData;
      this.matSnackBar.open(this.response.message, "OK", { duration: 10000 })
      this.dataService.setValueToResponseMessageProperty(null);
    }
    this.getDiningBoarderTableData(this.cachedTableDataService.campusId);
    this.cachedTableDataService.campusChangeEvent.subscribe(e => {
      this.getDiningBoarderTableData(e);
    })
  }
  getDiningBoarderTableData(e): void {
    this.dataSource.data = [];
    if ((this.commonMethod.showForInstituteAdmin() && e > 0) || this.commonMethod.showForCampusAdmin()) {
      let data = new TableDataInfo();
      data.campusId = e;
      this.diningBoarderService.SearchDiningBoarderForTable(data).subscribe((res: ResponseMessage) => {
        this.lstDiningBoarder = <DiningBoarderDetails[]>res.responseObj;
        if (this.lstDiningBoarder != null) {
          this.dataSource.data = this.lstDiningBoarder;
        }
      })
    }
    // this.diningBoarderService.SearchDiningBoarderForTable().subscribe((res: ResponseMessage) => {
    //   if (res.responseObj != null) {
    //     this.lstDiningBoarder = <DiningBoarderDetails[]>res.responseObj;
    //     if (this.commonMethod.showForSuperAdmin()) {
    //       this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "instituteName");
    //       this.lstOfInstitute = this.lstDiningBoarder.filter((v, i, a) =>
    //         a.indexOf(this.lstDiningBoarder.find(e => e.instituteId == v.instituteId)) === i);
    //       this.commonMethod.columnIndex += 1;
    //     }
    //     if (this.commonMethod.showForInstituteAdmin() || this.commonMethod.showForSuperAdmin()) {
    //       this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "campusName");
    //       this.lstOfCampus = this.lstDiningBoarder.filter((v, i, a) =>
    //         a.indexOf(this.lstDiningBoarder.find(e => e.campusId == v.campusId)) === i);
    //     }
    //     this.dataSource.data = this.lstDiningBoarder;
    //   }
    //   if (res.statusCode != ResponseStatusCodeEnum.Success) {
    //     this.workWithResponse(res)
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

    if (columnHeadName == "boarderNo") {
      this.lstDiningBoarder.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.boarderNo, b.boarderNo, this.isAssending);
      });
    }
    else if (columnHeadName == "boarderName") {
      this.lstDiningBoarder.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.boarderName, b.boarderName, this.isAssending);
      });
    }
    else if (columnHeadName == "boarderTypeName") {
      this.lstDiningBoarder.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.boarderTypeName, b.boarderTypeName, this.isAssending);
      });
    }
    else if (columnHeadName == "enrollmentDateString") {
      this.lstDiningBoarder.sort((a, b) => {
        return this.dataSortingHelperService.sortDateType(a.enrollmentDate, b.enrollmentDate, this.isAssending);
      });
    }
    else if (columnHeadName == "statusName") {
      this.lstDiningBoarder.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.statusName, b.statusName, this.isAssending);
      });
    }
    this.matTableService.showHideUpDowmIcon(event);
    this.isAssending = !this.isAssending;
    this.dataSource.data = this.lstDiningBoarder;
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
  onStatusChange(id: number, status: number, operationName: string) {
    this.commonMethod.onStatusChange(id, status, operationName, "DiningBoarder").then(e => {
      if (e) {
        this.getDiningBoarderTableData(this.cachedTableDataService.campusId);
      }
    })
  }
  onDelete(id: number) {
    let dialogRef = this.dialog.open(AppDeleteConfirmDialogComponent, {
      width: '350px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        const data = JSON.stringify(id);
        this.diningBoarderService.DeleteDiningBoarder(data).subscribe((res: ResponseMessage) => {
          this.response.message = res.message;
          this.response.statusCode = res.statusCode;
          this.getDiningBoarderTableData(this.cachedTableDataService.campusId);
          this.reset.push({});
        })
      }
    });
  }
}
