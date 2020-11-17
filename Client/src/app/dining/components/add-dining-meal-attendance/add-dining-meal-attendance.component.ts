import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { DiningBoarderType, ResponseStatusCodeEnum } from 'src/app/enums/Enums';
import { CommonMethod } from 'src/app/merp-common/common-method';
import { ResponseMessage } from 'src/app/model/response-message';
import { DataService } from 'src/app/services/data.service';
import { DiningMealAttendance } from '../../models/dining-meal-manage.model';
import { DiningMealAttendanceService } from '../../services/dining-meal-attendance.service';

@Component({
  selector: 'app-add-dining-meal-attendance',
  templateUrl: './add-dining-meal-attendance.component.html',
  styleUrls: ['./add-dining-meal-attendance.component.css']
})
export class AddDiningMealAttendanceComponent implements OnInit {

  mealTakenForm: FormGroup;
  mealDateForm: FormGroup;
  mealManageAddData: any;
  response: ResponseMessage = new ResponseMessage();
  diningMealManage: DiningMealAttendance = new DiningMealAttendance();
  diningBoarderType = DiningBoarderType;
  reset: any[] = [];
  diningMealManageId: number = 0;
  displayedColumns = ["sl", "boarderId", "boarderName", "takenMeal"];
  dataSource = new MatTableDataSource();
  initializeChoice: boolean = false;
  dataInitializeOption = ["All Field"];

  constructor(private formBuilder: FormBuilder, private diningMealManageService: DiningMealAttendanceService,
    private dataService: DataService, private router: Router, private activatedRoute: ActivatedRoute,
    private matSnackBar: MatSnackBar, public commonMethod: CommonMethod) {
    this.mealDateForm = this.formBuilder.group({
      campusId: [''],
      date: ['', Validators.required]
    })
    this.mealTakenForm = this.formBuilder.group({
      borderId: ['', Validators.required],
      mealDate: ['', Validators.required],
      takenMeal: ['']
    });
  }
  ngOnInit() {
    this.diningMealManageService.createAttendanceByMealManage("").subscribe((res: ResponseMessage)=>{
      //
    })
    if (this.commonMethod.showForInstituteAdmin()) {
      this.mealDateForm.get('campusId').setValidators(Validators.required);
    }
    this.activatedRoute.paramMap.subscribe(e => {
      this.diningMealManageId = +e.get('id');
      if (this.diningMealManageId > 0) {
        this.diningMealManageService.SearchDiningMealAttendanceEditData(JSON.stringify(this.diningMealManageId))
          .subscribe((res: ResponseMessage) => {
            this.mealManageAddData = res.responseObj;
            this.dataSource.data = this.mealManageAddData.diningBoarders;
            this.initializeToDataInitializeOption();
          })
      }
      else {
        document.getElementById("datePickBtn").click();
      }
    })
  }
  onInitiaLizeChange(decision, mealName: string) {
    if (decision.checked) {
      this.mealManageAddData.diningBoarders.forEach(e => {
        if (!e.isGrouping) {
          e.diningMeals.forEach(e2 => {
            if (e2.mealKey == mealName || mealName == "All Field") {
              e2.mealNumber = 0;
            }
          });
        }
      });
    }
    else {
      this.mealManageAddData.diningBoarders.forEach(e => {
        if (!e.isGrouping) {
          e.diningMeals.forEach(e2 => {
            if (e2.mealKey == mealName || mealName == "All Field") {
              e2.mealNumber = 1;
            }
          });
        }
      });
    }
  }
  onDateSubmit() {
    this.mealDateForm.markAllAsTouched();
    if (this.mealDateForm.valid) {
      let id = this.mealDateForm.get('campusId').value
      let date = { date: this.mealDateForm.get('date').value, campusId:  id > 0 ? id : 0};
      this.diningMealManageService.SearchDiningMealAttendanceAddData(JSON.stringify(date)).subscribe((res: ResponseMessage) => {
        if (res.responseObj == null) {
          this.matSnackBar.open(res.message, "OK", { duration: 10000 });
        }
        else {
          this.mealManageAddData = res.responseObj;
          this.dataSource.data = this.mealManageAddData.diningBoarders;
          this.initializeToDataInitializeOption();
          this.onInitiaLizeChange(false, "All Field");
        }
      })
    }
  }
  initializeToDataInitializeOption() {
    this.mealManageAddData.diningMeals.forEach(element => {
      this.dataInitializeOption.push(element.mealKey);
    });
  }
  onSave() {
    let boarderWithTakenMeal: any[] = [];
    // let table = <HTMLTableElement>document.getElementById("diningMealManageTable");
    // let rowLength = table.rows.length;
    this.diningMealManage = new DiningMealAttendance();
    this.diningMealManage.mealDate = this.mealManageAddData.mealDate;
    for (let i = 0; i < this.mealManageAddData.diningBoarders.length; i++) {
      if (!this.mealManageAddData.diningBoarders[i].isGrouping) {
        let takenMeal = {};
        takenMeal['boarderId'] = +this.mealManageAddData.diningBoarders[i].id;

        for (let j = 0; j < this.mealManageAddData.diningBoarders[i].diningMeals.length; j++) {
          takenMeal[this.mealManageAddData.diningBoarders[i].diningMeals[j].diningMealNumberString] =
            Number(this.mealManageAddData.diningBoarders[i].diningMeals[j].mealNumber) > 0 ?
              (Number(this.mealManageAddData.diningBoarders[i].diningMeals[j].mealSize) *
                Number(this.mealManageAddData.diningBoarders[i].diningMeals[j].mealNumber)) : 0;
        }
        boarderWithTakenMeal.push(takenMeal);
      }
    }
    let boarderWithTakenMealJson = JSON.stringify(boarderWithTakenMeal);
    this.diningMealManage.boarderWithTakenMeal = boarderWithTakenMealJson;
    this.diningMealManage.campusId = this.mealDateForm.get('campusId').value;
    this.diningMealManage.id = this.diningMealManageId != undefined || !this.diningMealManageId ? this.diningMealManageId : 0;
    let data = JSON.stringify(this.diningMealManage);
    if (this.diningMealManageId > 0) {
      this.diningMealManageService.updateDiningMealAttendance(data).subscribe((res: ResponseMessage) => {
        this.workWithResponse(res);
      });
    }
    else {
      this.diningMealManageService.saveDiningMealAttendance(data).subscribe((res: ResponseMessage) => {
        this.workWithResponse(res);
      });
    }
    // else {
    //   this.response.message = (this.employeeAttendanceSheet.employees.length - this.employeeAttendence.length) +
    //     " Employee left. Please fill these employee attendence!";
    //   this.response.statusCode = ResponseStatusCodeEnum.Failed;
    //   this.reset.push({});
    //   window.scrollTo(0, 0);
    // }
  }
  // getTypeCount(boarderTypeId: number){
  //   return this.mealManageAddData.diningBoarders.filter(e => e.boarderTypeId == boarderTypeId).length;
  // }
  // isShowType(boarderTypeId: number, index: number){
  //   if(index != 0){
  //     let prevIndex = index -1;
  //     let prevRowValue = this.mealManageAddData.diningBoarders[prevIndex];
  //     if(boarderTypeId == prevRowValue.boarderTypeId){
  //       return false;
  //     }
  //   }
  //   return true;
  // }
  // getColor(boarderTypeId: number){
  //   const COLORS: string[] = ['orange', 'gray', 'blue', 'green', 'navy', 'black', 'purple', 'red', 'olive'];
  //   return COLORS[boarderTypeId - 1];
  // }
  isGroup(index, item): boolean {
    return item.isGrouping;
  }
  workWithResponse(res: ResponseMessage) {
    this.response.message = res.message;
    this.response.statusCode = res.statusCode;
    if (this.response.statusCode == ResponseStatusCodeEnum.Success) {
      this.dataService.setValueToResponseMessageProperty(this.response);
      this.router.navigateByUrl("/finance/diningMealAttendance");
    }
    else {
      this.matSnackBar.open(res.message, "OK", { duration: 5000 });
    }
  }
}
