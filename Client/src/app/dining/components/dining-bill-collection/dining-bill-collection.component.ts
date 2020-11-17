import { Component, OnInit, ViewChild } from '@angular/core';
import { ResponseMessage } from 'src/app/model/response-message';
import { MatTableDataSource } from '@angular/material/table';import { MatDialog } from '@angular/material/dialog';import { MatPaginator } from '@angular/material/paginator';
import { CommonEnum, TransferStatus, CollectionMode, ResponseStatusCodeEnum } from 'src/app/enums/Enums';
import { DataService } from 'src/app/services/data.service';
import { DiningBillCollectionService } from '../../services/dining-bill-collection.service';
import { DataSortingHelperService } from 'src/app/services/data-sorting-helper.service';
import { MatTableService } from 'src/app/services/table-search-box.service';
import { DiningBillCollectionTalbeData } from '../../models/dining-bill-collection.model';
import { getSearchBoxStyle, CommonMethod } from 'src/app/merp-common/common-method';
import { Campus } from 'src/app/administration/models/campus';
import { Institute } from 'src/app/administration/models/Institute.model';
import { AppDeleteConfirmDialogComponent } from 'src/app/components/app-delete-confirm-dialog/app-delete-confirm-dialog.component';

@Component({
  selector: 'app-dining-bill-collection',
  templateUrl: './dining-bill-collection.component.html',
  styleUrls: ['./dining-bill-collection.component.css'],
  styles: [getSearchBoxStyle()]
})
export class DiningBillCollectionComponent implements OnInit {
  response: ResponseMessage = new ResponseMessage();
  lstDiningBillCollection: any[] = [];
  dataSource = new MatTableDataSource();
  isShow: boolean = false;
  reset: any = [{}];
  commonEnum = CommonEnum;
  transferStatus = TransferStatus;
  collectionMode = CollectionMode;
  lstOfCampus: Campus[] = [];
  lstOfInstitute: Institute[] = [];
  displayedColumns = ["diningBillCollectionId", "collectionDate", "boarderName", "description", "amount", "statusName", "Action"];
  constructor(private dataService: DataService, private diningBillCollectionService: DiningBillCollectionService,
    private dataSortingHelperService: DataSortingHelperService, private matTableService: MatTableService,
    private dialog: MatDialog, public commonMethod: CommonMethod) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
  ngOnInit() {
    this.commonMethod.columnIndex = 5;
    if (this.dataService.responseMessageData != null) {
      this.response = this.dataService.responseMessageData;
      this.dataService.setValueToResponseMessageProperty(null);
    }
    this.getDonationCollectionTableData();
  }
  getDonationCollectionTableData(): void {
    this.diningBillCollectionService.SearchDiningBillCollectionForTable()
      .subscribe((res: ResponseMessage) => {
        if (res.responseObj != null) {
          this.lstDiningBillCollection = <DiningBillCollectionTalbeData[]>res.responseObj;
          if (this.commonMethod.showForSuperAdmin()) {
            this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "instituteName");
            this.lstOfInstitute = this.lstDiningBillCollection.filter((v, i, a) =>
              a.indexOf(this.lstDiningBillCollection.find(e => e.instituteId == v.instituteId)) === i);
            this.commonMethod.columnIndex += 1;
          }
          if (this.commonMethod.showForInstituteAdmin() || this.commonMethod.showForSuperAdmin()) {
            this.displayedColumns.splice(this.commonMethod.columnIndex, 0, "campusName");
            this.lstOfCampus = this.lstDiningBillCollection.filter((v, i, a) =>
              a.indexOf(this.lstDiningBillCollection.find(e => e.campusId == v.campusId)) === i);
          }
          this.dataSource.data = this.lstDiningBillCollection;
        }
      })
  }
  isAssending = false;
  sortTableData(event, columnHead) {
    const columnHeadName = columnHead.displayedColumns[event];

    if (columnHeadName == "diningBillCollectionId") {
      this.lstDiningBillCollection.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(Number(a.diningBillCollectionNo),
          Number(b.diningBillCollectionNo), this.isAssending);
      });
    }
    else if (columnHeadName == "boarderName") {
      this.lstDiningBillCollection.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.boarderName, b.boarderName, this.isAssending);
      });
    }
    else if (columnHeadName == "collectionDate") {
      this.lstDiningBillCollection.sort((a, b) => {
        return this.dataSortingHelperService.sortDateType(a.collectionDate.toString(),
          b.collectionDate.toString(), this.isAssending);
      });
    }
    else if (columnHeadName == "description") {
      this.lstDiningBillCollection.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.note, b.note, this.isAssending);
      });
    }
    else if (columnHeadName == "amount") {
      this.lstDiningBillCollection.sort((a, b) => {
        return this.dataSortingHelperService.sortNumericType(a.amount, b.amount, this.isAssending);
      });
    }
    else if (columnHeadName == "instituteName") {
      this.lstDiningBillCollection.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.instituteName,
          b.instituteName, this.isAssending);
      });
    }
    else if (columnHeadName == "campusName") {
      this.lstDiningBillCollection.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.campusName,
          b.campusName, this.isAssending);
      });
    }
    else if (columnHeadName == "statusName") {
      this.lstDiningBillCollection.sort((a, b) => {
        return this.dataSortingHelperService.sortStringType(a.statusName, b.statusName, this.isAssending);
      });
    }
    this.matTableService.showHideUpDowmIcon(event);
    this.isAssending = !this.isAssending;
    this.dataSource.data = this.lstDiningBillCollection;
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
    if (this.response.statusCode == ResponseStatusCodeEnum.Success) {
      this.getDonationCollectionTableData();
    }
    this.reset.push({});
    window.scroll(0, 0);
  }
  isCashChaque(id: number) {
    if (id == this.collectionMode["Cash Cheque"] || id == this.collectionMode.Cash) {
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
        this.diningBillCollectionService.DeleteDiningBillCollection(data).subscribe((res: ResponseMessage) => {
          this.workWithResponse(res);
        })
      }
    });
  }
}
