import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { ResponseMessage } from 'src/app/model/response-message';
import { DataSortingHelperService } from 'src/app/services/data-sorting-helper.service';
import { MatTableService } from 'src/app/services/table-search-box.service';
import { DiningBillService } from '../../services/dining-bill.service';

@Component({
  selector: 'app-dining-expense-statement',
  templateUrl: './dining-expense-statement.component.html',
  styleUrls: ['./dining-expense-statement.component.css']
})
export class DiningExpenseStatementComponent implements OnInit {
  displayedColumns = ["sl", "productName", "productType", "qty", "rate", "amount"];
  isShow: boolean = false;
  dataSource = new MatTableDataSource();
  data: any[] = [];
  constructor(private diningBillService: DiningBillService, private activatedRoute: ActivatedRoute,
    private matTableService: MatTableService, private dataSortingHelperService: DataSortingHelperService) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(e => {
      let id = +e.get('id');
      if (id > 0) {
        this.diningBillService.GetDiningExpenseStatement(JSON.stringify(id)).subscribe((res: ResponseMessage) => {
          if(res.responseObj != null){
            this.data = <any[]>res.responseObj;
            this.dataSource.data = this.data;
          }
          console.log(res.responseObj);
        })
      }
    })
  }
  showSearchBox(id: string) {
    this.isShow = !this.isShow;
    this.matTableService.showSearchBox(id, this.isShow);
  }
  closeSearchBox() {
    this.isShow = !this.isShow;
    this.matTableService.closeSearchBox();
  }
  getTotla(){
    return this.data.reduce((prev, cur, index)=> cur.amount + prev, 0);
  }
  // isAssending = false;
  // sortTableData(event, columnHead) {
  //   const columnHeadName = columnHead.displayedColumns[event];
  //   if (columnHeadName == "productType") {
  //     this.data.sort((a, b) => {
  //       return this.dataSortingHelperService.sortStringType(a.productTypeName,
  //         b.productTypeName, this.isAssending);
  //     });
  //   }
  //   else if (columnHeadName == "productName") {
  //     this.data.sort((a, b) => {
  //       return this.dataSortingHelperService.sortStringType(a.productName, b.productName, this.isAssending);
  //     });
  //   }
  //   else if (columnHeadName == "qty") {
  //     this.data.sort((a, b) => {
  //       return this.dataSortingHelperService.sortNumericType(Number(a.qty),
  //         Number(b.qty), this.isAssending);
  //     });
  //   }
  //   else if (columnHeadName == "rate") {
  //     this.data.sort((a, b) => {
  //       return this.dataSortingHelperService.sortNumericType(a.rate, b.rate, this.isAssending);
  //     });
  //   }
  //   else if (columnHeadName == "amount") {
  //     this.data.sort((a, b) => {
  //       return this.dataSortingHelperService.sortStringType(a.amount, b.amount, this.isAssending);
  //     });
  //   }
  //   this.matTableService.showHideUpDowmIcon(event);
  //   this.isAssending = !this.isAssending;
  //   this.dataSource.data = this.data;
  // }
}
