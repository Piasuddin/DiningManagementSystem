import { Component, OnInit, ViewChild } from '@angular/core';
import { DiningBillService } from '../../services/dining-bill.service';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBill } from '../../models/dining-bill.model';
import { MatTableDataSource } from '@angular/material/table';import { MatDialog } from '@angular/material/dialog';import { MatPaginator } from '@angular/material/paginator';
import { DataService } from 'src/app/services/data.service';
import { MatTableService } from 'src/app/services/table-search-box.service';
import { DataSortingHelperService } from 'src/app/services/data-sorting-helper.service';
import { getSearchBoxStyle, CommonMethod } from 'src/app/merp-common/common-method';
import { Campus } from 'src/app/administration/models/campus';
import { Institute } from 'src/app/administration/models/Institute.model';
import { AppDeleteConfirmDialogComponent } from 'src/app/components/app-delete-confirm-dialog/app-delete-confirm-dialog.component';

@Component({
  selector: 'app-dining-bill',
  templateUrl: './dining-bill.component.html',
  styleUrls: ['./dining-bill.component.css'],
  styles: [getSearchBoxStyle()]
})
export class DiningBillComponent implements OnInit {
  response: ResponseMessage = new ResponseMessage();
  lstDiningBill: any[] = [];
  dataSource = new MatTableDataSource();
  isShow: boolean = false;
  reset: any = [{}];
  lstOfCampus: Campus[] = [];
  lstOfInstitute: Institute[] = [];
  displayedColumns = ["billformonth", "totalMeal", "totalExpense", "mealRate", "statusName", "Action"];
  constructor(private dataService: DataService, private matTableService: MatTableService, private dialog: MatDialog,
    private dataSortingHelperService: DataSortingHelperService, private diningBillService: DiningBillService,
    public commonMethod: CommonMethod) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  ngOnInit() {
    this.commonMethod.columnIndex = 4;
    if (this.dataService.responseMessageData != null) {
      this.response = this.dataService.responseMessageData;
      this.dataService.setValueToResponseMessageProperty(null);
    }
    this.getTableData();
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

    if (columnHeadName == "billformonth") {
      this.lstDiningBill.sort((a, b) => {
        return this.dataSortingHelperService.sortDateType(a.billMonth.toString(), b.billMonth.toString(), this.isAssending);
      });
    }
    else if (columnHeadName == "totalMeal") {
      this.lstDiningBill.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.totalMeal, b.totalMeal, this.isAssending);
      });
    }
    else if (columnHeadName == "totalExpense") {
      this.lstDiningBill.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.totalBill, b.totalBill, this.isAssending);
      });
    }
    else if (columnHeadName == "mealRate") {
      this.lstDiningBill.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.mealRate, b.mealRate, this.isAssending);
      });
    }
    else if (columnHeadName == "instituteName") {
      this.lstDiningBill.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.instituteName,
          b.instituteName, this.isAssending);
      });
    }
    else if (columnHeadName == "campusName") {
      this.lstDiningBill.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.campusName,
          b.campusName, this.isAssending);
      });
    }
    else if (columnHeadName == "statusName") {
      this.lstDiningBill.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.statusName, b.statusName, this.isAssending);
      });
    }
    this.matTableService.showHideUpDowmIcon(event);
    this.isAssending = !this.isAssending;
    this.dataSource.data = this.lstDiningBill;
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
  getTableData(): void {
    this.diningBillService.SearchDiningBillForTable().subscribe((res: ResponseMessage) => {
      if (res.responseObj != null) {
        this.lstDiningBill = <DiningBill[]>res.responseObj;
        if (this.commonMethod.showForSuperAdmin()) {
          this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "instituteName");
          this.lstOfInstitute = this.lstDiningBill.filter((v, i, a) =>
            a.indexOf(this.lstDiningBill.find(e => e.instituteId == v.instituteId)) === i);
          this.commonMethod.columnIndex += 1;
        }
        if (this.commonMethod.showForInstituteAdmin() || this.commonMethod.showForSuperAdmin()) {
          this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "campusName");
          this.lstOfCampus = this.lstDiningBill.filter((v, i, a) =>
            a.indexOf(this.lstDiningBill.find(e => e.campusId == v.campusId)) === i);
        }
        this.dataSource.data = this.lstDiningBill;
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
        this.diningBillService.deleteDiningBill(data).subscribe((res: ResponseMessage) => {
          this.response.message = res.message;
          this.response.statusCode = res.statusCode;
          this.getTableData();
          this.reset.push({});
        })
      }
    });
  }
}
