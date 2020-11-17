import { Component, OnInit, ViewChild } from '@angular/core';
import { ResponseMessage } from 'src/app/model/response-message';
import { ResponseStatusCodeEnum, ExpenseVoucherStatus } from 'src/app/enums/Enums';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { DataService } from 'src/app/services/data.service';
import { DiningExpenseService } from '../../services/dining-expense.service';
import { DataSortingHelperService } from 'src/app/services/data-sorting-helper.service';
import { MatTableService } from 'src/app/services/table-search-box.service';
import { DiningExpenseDetails } from '../../models/dining-expense.model';
import { getSearchBoxStyle, CommonMethod } from 'src/app/merp-common/common-method';
import { Campus } from 'src/app/administration/models/campus';
import { Institute } from 'src/app/administration/models/Institute.model';
import { AppDeleteConfirmDialogComponent } from 'src/app/components/app-delete-confirm-dialog/app-delete-confirm-dialog.component';
import { StockProduct } from '../../models/stock-product.model';
import { TableDataInfo } from 'src/app/model/table-data-info.model';
import { CachedTableDataService } from 'src/app/services/cached-table-data.Service';

@Component({
  selector: 'app-dining-expense',
  templateUrl: './dining-expense.component.html',
  styleUrls: ['./dining-expense.component.css'],
  styles: [getSearchBoxStyle()]
})
export class DiningExpenseComponent implements OnInit {
  response: ResponseMessage = new ResponseMessage();
  dataSource = new MatTableDataSource();
  diningExpense: any[] = [];
  isShow: boolean = false;
  expenseVoucherStatus = ExpenseVoucherStatus;
  reset: any = [{}];
  lstOfCampus: Campus[] = [];
  lstOfInstitute: Institute[] = [];
  displayedColumns = ["expenseId", "expenseDate", "voucherID", "expenseHead", "productName", "qty", "rate", "amount",
    "responsible", "statusName", "Action"];
  constructor(private dataService: DataService, private diningExpenseService: DiningExpenseService,
    private dataSortingHelperService: DataSortingHelperService, private matTableService: MatTableService,
    private dialog: MatDialog, public commonMethod: CommonMethod, public cachedTableDataService: CachedTableDataService) { }
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  ngOnInit() {
    this.cachedTableDataService.selectedCampus(31);
    if (this.dataService.responseMessageData != null) {
      this.response = this.dataService.responseMessageData;
      this.dataService.setValueToResponseMessageProperty(null);
    }
    this.getDiningExpenseTableData(this.cachedTableDataService.campusId);
    // this.cachedTableDataService.campusChangeEvent.subscribe(e => {
    //   this.getDiningExpenseTableData(e);
    // })
  }
  getDiningExpenseTableData(e: number): void {
    // this.dataSource.data = [];
    // if ((this.commonMethod.showForInstituteAdmin() && e > 0) || this.commonMethod.showForCampusAdmin()) {
    //   let data = new TableDataInfo();
    //   data.campusId = e;
    //   this.diningExpenseService.SearchDiningExpenseForTable(data).subscribe((res: ResponseMessage) => {
    //     this.diningExpense = <DiningExpenseDetails[]>res.responseObj;
    //     if (this.diningExpense != null) {
    //       this.dataSource.data = this.diningExpense;
    //     }
    //   })
    // }
    this.diningExpenseService.SearchDiningExpenseForTable(JSON.stringify(false)).subscribe((res: ResponseMessage) => {
      if (res.responseObj != null) {
        this.diningExpense = <DiningExpenseDetails[]>res.responseObj;
        // if (this.commonMethod.showForSuperAdmin()) {
        //   this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "instituteName");
        //   this.lstOfInstitute = this.diningExpense.filter((v, i, a) =>
        //     a.indexOf(this.diningExpense.find(e => e.instituteId == v.instituteId)) === i);
        //   this.commonMethod.columnIndex += 1;
        // }
        // if (this.commonMethod.showForInstituteAdmin() || this.commonMethod.showForSuperAdmin()) {
        //   this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "campusName");
        //   this.lstOfCampus = this.diningExpense.filter((v, i, a) =>
        //     a.indexOf(this.diningExpense.find(e => e.campusId == v.campusId)) === i);
        // }
        this.dataSource.data = this.diningExpense;
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
    if (columnHeadName == "expenseId") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(Number(a.expenseNo),
          Number(b.expenseNo), this.isAssending);
      });
    }
    else if (columnHeadName == "expenseDate") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortDateType(a.expenseDate.toString(), b.expenseDate.toString(), this.isAssending);
      });
    }
    else if (columnHeadName == "voucherID") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(Number(a.voucherId),
          Number(b.voucherId), this.isAssending);
      });
    }
    else if (columnHeadName == "expenseHead") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.expenseHeadName, b.expenseHeadName, this.isAssending);
      });
    }
    else if (columnHeadName == "productName") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.productName, b.productName, this.isAssending);
      });
    }
    else if (columnHeadName == "amount") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.amount, b.amount, this.isAssending);
      });
    }
    else if (columnHeadName == "qty") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.qty, b.qty, this.isAssending);
      });
    }
    else if (columnHeadName == "rate") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.rate, b.rate, this.isAssending);
      });
    }
    else if (columnHeadName == "responsible") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.responsible, b.responsible, this.isAssending);
      });
    }
    else if (columnHeadName == "statusName") {
      this.diningExpense.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.statusName, b.statusName, this.isAssending);
      });
    }
    this.matTableService.showHideUpDowmIcon(event);
    this.isAssending = !this.isAssending;
    this.dataSource.data = this.diningExpense;
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
      if (result == true) {
        const data = JSON.stringify(id);
        this.diningExpenseService.deleteDiningExpense(data).subscribe((res: ResponseMessage) => {
          this.workWithResponse(res);
        })
      }
    });
  }
  workWithResponse(res: ResponseMessage) {
    this.response.message = res.message;
    this.response.statusCode = res.statusCode;
    if (this.response.statusCode == ResponseStatusCodeEnum.Success) {
      this.getDiningExpenseTableData(this.cachedTableDataService.campusId);
    }
    this.reset.push({});
  }
}
