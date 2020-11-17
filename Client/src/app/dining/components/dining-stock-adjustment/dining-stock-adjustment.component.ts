import { Component, OnInit, ViewChild } from '@angular/core';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningStockAdjustmentTableData } from '../../models/dining-stock-adjustment.model';
import { MatTableDataSource } from '@angular/material/table';import { MatDialog } from '@angular/material/dialog';import { MatPaginator } from '@angular/material/paginator';
import { ResponseStatusCodeEnum, DiningStockAdjustmentStatus } from 'src/app/enums/Enums';
import { DataService } from 'src/app/services/data.service';
import { DiningStockAdjustmentService } from '../../services/dining-stock-adjustment.service';
import { DataSortingHelperService } from 'src/app/services/data-sorting-helper.service';
import { MatTableService } from 'src/app/services/table-search-box.service';
import { getSearchBoxStyle, CommonMethod } from 'src/app/merp-common/common-method';
import { Campus } from 'src/app/administration/models/campus';
import { Institute } from 'src/app/administration/models/Institute.model';
import { AppDeleteConfirmDialogComponent } from 'src/app/components/app-delete-confirm-dialog/app-delete-confirm-dialog.component';
import { DiningBillService } from '../../services/dining-bill.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-dining-stock-adjustment',
  templateUrl: './dining-stock-adjustment.component.html',
  styleUrls: ['./dining-stock-adjustment.component.css'],
  styles: [getSearchBoxStyle()]
})
export class DiningStockAdjustmentComponent implements OnInit {

  response: ResponseMessage = new ResponseMessage();
  diningStockAdjustmentTableData: any[] = [];
  dataSource = new MatTableDataSource();
  isShow: boolean = false;
  reset: any = [{}];
  diningStockManageStatus = DiningStockAdjustmentStatus;
  lstOfCampus: Campus[] = [];
  lstOfInstitute: Institute[] = [];
  displayedColumns = ["adjustmentNo", "adjustmentForMonth", "noOfAdjustmentProduct", "statusName", "Action"];
  constructor(private dataService: DataService, private diningStockAdjustmentService: DiningStockAdjustmentService,
    private dataSortingHelperService: DataSortingHelperService, private matTableService: MatTableService,
    private dialog: MatDialog, public commonMethod: CommonMethod, private diningBillService: DiningBillService,
    private matSnackBar: MatSnackBar) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  ngOnInit() {
    this.commonMethod.columnIndex = 3;
    if (this.dataService.responseMessageData != null) {
      this.response = this.dataService.responseMessageData;
      this.dataService.setValueToResponseMessageProperty(null);
    }
    this.getDiningStockAdjustmentTableData();
  }
  getDiningStockAdjustmentTableData(): void {
    this.diningStockAdjustmentService.SearchDiningStockAdjustmentForTable().subscribe((res: ResponseMessage) => {
      console.log(res)
      if (res.responseObj != null) {
        this.diningStockAdjustmentTableData = <DiningStockAdjustmentTableData[]>res.responseObj;
        if (this.commonMethod.showForSuperAdmin()) {
          this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "instituteName");
          this.lstOfInstitute = this.diningStockAdjustmentTableData.filter((v, i, a) =>
            a.indexOf(this.diningStockAdjustmentTableData.find(e => e.instituteId == v.instituteId)) === i);
          this.commonMethod.columnIndex += 1;
        }
        if (this.commonMethod.showForInstituteAdmin() || this.commonMethod.showForSuperAdmin()) {
          this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "campusName");
          this.lstOfCampus = this.diningStockAdjustmentTableData.filter((v, i, a) =>
            a.indexOf(this.diningStockAdjustmentTableData.find(e => e.campusId == v.campusId)) === i);
        }
        this.dataSource.data = this.diningStockAdjustmentTableData;
      }
      else{
        this.dataSource.data = [];
      }
    })
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

    if (columnHeadName == "adjustmentNo") {
      this.diningStockAdjustmentTableData.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(Number(a.adjustmentNo),
          Number(b.adjustmentNo), this.isAssending);
      });
    }
    else if (columnHeadName == "adjustmentForMonth") {
      this.diningStockAdjustmentTableData.sort((a, b) => {
        return this.dataSortingHelperService.sortDateType(a.adjustmentForMonth.toString(),
          b.adjustmentForMonth.toString(), this.isAssending);
      });
    }
    else if (columnHeadName == "noOfAdjustmentProduct") {
      this.diningStockAdjustmentTableData.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.noOfAdjustmentProduct, b.noOfAdjustmentProduct, this.isAssending);
      });
    }
    else if (columnHeadName == "instituteName") {
      this.diningStockAdjustmentTableData.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.instituteName,
          b.instituteName, this.isAssending);
      });
    }
    else if (columnHeadName == "campusName") {
      this.diningStockAdjustmentTableData.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.campusName,
          b.campusName, this.isAssending);
      });
    }
    else if (columnHeadName == "statusName") {
      this.diningStockAdjustmentTableData.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.statusName, b.statusName, this.isAssending);
      });
    }
    this.matTableService.showHideUpDowmIcon(event);
    this.isAssending = !this.isAssending;
    this.dataSource.data = this.diningStockAdjustmentTableData;
  }
  showSearchBox(id: string) {
    this.isShow = !this.isShow;
    this.matTableService.showSearchBox(id, this.isShow);
  }
  closeSearchBox() {
    this.isShow = !this.isShow;
    this.matTableService.closeSearchBox();
  }
  onDelete(id: number) {
    let dialogRef = this.dialog.open(AppDeleteConfirmDialogComponent, {
      width: '350px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const data = JSON.stringify(id);
        this.diningStockAdjustmentService.DeleteDiningStockAdjustment(data).subscribe((res: ResponseMessage) => {
          this.workWithResponse(res);
        })
      }
    });
  }
  workWithResponse(res: ResponseMessage) {
    if (this.response.statusCode == ResponseStatusCodeEnum.Success) {
      this.matSnackBar.open(res.message, "OK", {duration: 5000});
      this.getDiningStockAdjustmentTableData();
    }
  }
  onCloseAccounts(id: number){
    this.commonMethod.openDialog(" close accounts ").then(e =>{
      if(e){
        this.diningBillService.SaveDiningBill(JSON.stringify(id)).subscribe((res: ResponseMessage)=>{
          if(res.statusCode == ResponseStatusCodeEnum.Success){
            this.getDiningStockAdjustmentTableData();
          }
          this.matSnackBar.open(res.message, "OK", {duration: 5000});
        })
      }
    })
  }
}
