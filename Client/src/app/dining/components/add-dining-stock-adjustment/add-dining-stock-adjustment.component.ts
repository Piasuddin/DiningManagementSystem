import { Component, OnInit, ViewChild } from '@angular/core';
import { ResponseMessage } from 'src/app/model/response-message';
import { DatePipe } from '@angular/common';
import { DiningStockManageAddData } from '../../models/dining-stock-manage.model';
import { ResponseStatusCodeEnum } from 'src/app/enums/Enums';
import { DataService } from 'src/app/services/data.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DiningStockAdjustmentService } from '../../services/dining-stock-adjustment.service';
import { monthNameYYYYDateFormat } from 'src/app/formater/formater';
import { DateAdapter } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonMethod } from 'src/app/merp-common/common-method';
import moment from 'moment';
import { DiningStockAdjustmentAddData } from '../../models/dining-stock-adjustment.model';

@Component({
  selector: 'app-add-dining-stock-adjustment',
  templateUrl: './add-dining-stock-adjustment.component.html',
  styleUrls: ['./add-dining-stock-adjustment.component.css'],
  providers: [
    {
      provide: DateAdapter, useClass: monthNameYYYYDateFormat
    }
  ]
})
export class AddDiningStockAdjustmentComponent implements OnInit {
  dates: any;
  isShow: boolean;
  response: ResponseMessage = new ResponseMessage();
  reset: any[] = [];
  campusId: number;
  adjustmentId: number;
  lstStockManageAddData: Array<DiningStockAdjustmentAddData> = [];
  @ViewChild(MatDatepicker, { static: false }) picker;

  constructor(private diningStockAdjustmentService: DiningStockAdjustmentService, private datePipe: DatePipe,
    private dataService: DataService, private router: Router, private matSnackBar: MatSnackBar,
    public commonMethod: CommonMethod, private activatedRoute: ActivatedRoute) { }
  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(e => {
      this.adjustmentId = +e.get("id");
      if (this.adjustmentId > 0) {
        this.diningStockAdjustmentService.SearchDiningStockAdjustmentUpdateData(JSON.stringify(this.adjustmentId))
          .subscribe((res: ResponseMessage) => {
            this.isShow = false;
            if (res.responseObj != null) {
              this.lstStockManageAddData = <DiningStockAdjustmentAddData[]>res.responseObj;
            }
            else {
              this.matSnackBar.open(res.message, "OK", { duration: 10000 })
            }
          })
      }
    })
  }
  getManageProduct(date: Date) {
    this.lstStockManageAddData = [];
    if (date) {
      let dateToSubmit = moment(date).format('MM/DD/YYYY')
      if ((this.commonMethod.showForInstituteAdmin() && this.campusId > 0)
        || !this.commonMethod.showForInstituteAdmin()) {
        this.isShow = true;
        let data = { date: dateToSubmit, campusId: this.campusId };
        this.diningStockAdjustmentService.SearchDiningStockAdjustmentAddData(JSON.stringify(data))
          .subscribe((res: ResponseMessage) => {
            this.isShow = false;
            if (res.responseObj != null) {
              this.lstStockManageAddData = <DiningStockAdjustmentAddData[]>res.responseObj;
            }
            else {
              this.matSnackBar.open(res.message, "OK", { duration: 10000 })
            }
          })
      }
      else {
        this.matSnackBar.open("Please Select a Campus!", "OK", { duration: 10000 })
      }
    }
  }
  monthSelected(params) {
    this.dates = params;
    this.picker.close();
  }
  calculateAvailableStock() {
    for (var i = 0; i < this.lstStockManageAddData.length; i++) {
      this.lstStockManageAddData[i].usedQyt =
        Number((this.lstStockManageAddData[i].totalQty - this.lstStockManageAddData[i].inStock).toFixed(3));
    }
  }
  calculateUsedQty() {
    for (var i = 0; i < this.lstStockManageAddData.length; i++) {
      this.lstStockManageAddData[i].inStock =
        Number((this.lstStockManageAddData[i].totalQty - this.lstStockManageAddData[i].usedQyt).toFixed(3));
    }
  }
  onCloseAccounts() {
    if (this.lstStockManageAddData.length > 0) {
      if(this.adjustmentId > 0){
        this.diningStockAdjustmentService.UpdateDiningStockAdjustment(JSON.stringify(this.lstStockManageAddData))
        .subscribe((res: ResponseMessage) => {
          this.workWithResponse(res);
        })
      }
      else{
      this.diningStockAdjustmentService.SaveDiningStockAdjustment(JSON.stringify(this.lstStockManageAddData))
        .subscribe((res: ResponseMessage) => {
          this.workWithResponse(res);
        })
      }
    }
    else {
      this.matSnackBar.open("Don't have any product to submit for stock adjustment.", "OK", { duration: 5000 })
    }
  }
  workWithResponse(res: ResponseMessage) {
    this.response.message = res.message;
    this.response.statusCode = res.statusCode;
    if (res.statusCode == ResponseStatusCodeEnum.Success) {
      this.dataService.setValueToResponseMessageProperty(this.response);
      this.router.navigateByUrl("/finance/diningStockAdjustment");
    }
    else {
      this.matSnackBar.open(res.message, "OK", { duration: 5000 })
    }
  }
}
