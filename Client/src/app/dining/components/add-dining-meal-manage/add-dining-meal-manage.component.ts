import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ResponseMessage } from 'src/app/model/response-message';
import { ResponseStatusCodeEnum } from 'src/app/enums/Enums';
import { DataService } from 'src/app/services/data.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { DiningMealManageService } from '../../services/dining-meal-manage.service';
import { DiningBoarderDetails } from '../../models/dining-boarder.model';
import { CommonMethod } from 'src/app/merp-common/common-method';
import { DiningMealManage } from '../../models/dining-meal-manage.model';
import { DiningMeal } from '../../models/dining-meal.model';
import { EndDate } from 'src/app/validators/merp-validators';

@Component({
  selector: 'app-add-dining-meal-manage',
  templateUrl: './add-dining-meal-manage.component.html',
  styleUrls: ['./add-dining-meal-manage.component.css']
})
export class AddDiningMealManageComponent implements OnInit {
  diningMealmanageForm: FormGroup;
  diningMealForm: FormGroup;
  mealManageAddData: any;
  response: ResponseMessage = new ResponseMessage();
  reset: any[] = [];
  diningMealManageId: number;
  dataSource = new MatTableDataSource();
  mealManage: DiningMealManage = new DiningMealManage();
  lstAddedDininBoarders: DiningBoarderDetails[] = [];
  lstBoarder: DiningBoarderDetails[] = [];
  diningMeals: DiningMeal[] = [];
  diningMealManage: DiningBoarderDetails[] = [];
  allChecked: boolean = false;
  constructor(private formBuilder: FormBuilder, private diningMealManageService: DiningMealManageService,
    private dataService: DataService, private router: Router, private activatedRoute: ActivatedRoute,
    private commonMethod: CommonMethod) {
    this.diningMealmanageForm = this.formBuilder.group({
      id: [''],
      campusId: [''],
      boarders: ['', Validators.required],
      meals: ['', Validators.required],
      fromDate: ['', Validators.required],
      toDate: ['']
    })
  }
  ngOnInit() {
    if (this.commonMethod.showForInstituteAdmin()) {
      this.diningMealmanageForm.get('campusId').setValidators(Validators.required);
    }
    // if (this.diningMealmanageForm.get("toDate").value != null) {
    //   console.log(this.diningMealmanageForm.get("toDate").value)
    //   this.diningMealmanageForm.get("fromDate").valueChanges.subscribe(e => {
    //     this.diningMealmanageForm.get("toDate").setValidators(EndDate(e));
    //     this.diningMealmanageForm.get("toDate").updateValueAndValidity();
    //   });
    // }
    this.diningMealManageService.SearchDiningMealManageAddData('').subscribe((res: any) => {
      if (res.responseObj != null) {
        this.diningMealManage = res.responseObj.diningBoarders;
        this.diningMeals = res.responseObj.diningMeals;
        this.AddCheckedProperty();
      }
      this.activatedRoute.paramMap.subscribe(e => {
        this.diningMealManageId = +e.get('id');
        if (this.diningMealManageId > 0) {
          this.diningMealManageService.searchDiningMealManage(JSON.stringify(this.diningMealManageId))
            .subscribe((res: ResponseMessage) => {
              this.mealManage = <DiningMealManage>res.responseObj;
              let meals = JSON.parse(this.mealManage.meals);
              for (let i = 0; i < meals.length; i++) {
                this.diningMeals.forEach(e => {
                  if (e.id == meals[i]) {
                    e.isChecked = true
                  }
                })
              }
              this.diningMealmanageForm.patchValue(this.mealManage);
              this.assignValueToGrid();
            })
        }
      })
    })
  }
  assignValueToGrid() {
    this.onAdd();
    let boarders = JSON.parse(this.mealManage.boarders);
    boarders.forEach(x1 => {
      this.lstBoarder.forEach(x2 => {
        if (x1 == x2.id) {
          x2.isChecked = true;
          this.lstAddedDininBoarders.push(x2);
        }
      });
    })
  }
  checkedAll() {
    if (this.allChecked) {
      this.diningMealManage.forEach(e => e.isChecked = false);
    }
    else
      this.diningMealManage.forEach(e => e.isChecked = true);
  }
  AddCheckedProperty() {
    this.diningMealManage.forEach(e => e.isChecked = false);
  }
  onAdd() {
    let id = this.diningMealmanageForm.get("campusId").value;
    if (id > 0) {
      this.lstBoarder = this.diningMealManage.filter(e => e.isChecked == false && e.campusId == id);
    }
    else if (this.commonMethod.showForCampusAdmin()) {
      let data = this.diningMealManage.filter(e => e.isChecked == false);
      this.lstBoarder = data;
    }
  }
  boarderId;
  errorMessage;
  onChange() {
    this.errorMessage = '';
    let boarderId = this.boarderId;
    let formFiltered = this.lstBoarder.find(e => e.boarderNo == boarderId);
    let formAll = this.lstBoarder.find(e => e.boarderNo == boarderId);
    let formAdded = this.lstAddedDininBoarders.find(e => e.boarderNo == boarderId);
    if (formAdded != undefined) {
      this.errorMessage = "The boarder with id " + boarderId + " has alredy been added.";
    }
    else if (formFiltered == undefined && formAll == undefined) {
      this.errorMessage = "Not found any boarder with id " + boarderId;
    }
    else {
      this.lstAddedDininBoarders.push(formAll);
      formAll.isChecked = true;
      this.boarderId = undefined;
    }
  }
  isCloseModal = false;
  onBoarderAdd() {
    this.lstAddedDininBoarders = [];
    this.diningMealManage.forEach((e, i) => {
      if (e.isChecked) {
        this.lstAddedDininBoarders.push(e);
        this.isCloseModal = true;
      }
    });
  }
  onClose() {
    this.diningMealManage.forEach((e, i) => {
      if (e.isChecked) {
        let index = this.lstAddedDininBoarders.findIndex(e2 => e2.id == e.id);
        if (index == -1) {
          e.isChecked = false;
        }
      }
    });
  }
  onDeleteBoarder(index: number) {
    this.lstAddedDininBoarders.forEach((e, i) => {
      if (index == i) {
        e.isChecked = false;
        this.lstAddedDininBoarders.splice(index, 1);
        this.diningMealManage.forEach(e2 => {
          if (e2.id == e.id) {
            e2.isChecked = false;
            this.allChecked = false;
          }
        })
      }
    })
  }
  onSubmit() {
    this.mealManage = this.diningMealmanageForm.value;
    this.diningMeals = this.diningMeals.filter(e => e.isChecked == true);
    this.mealManage.meals = JSON.stringify(this.diningMeals.map((x) => x.id));
    this.mealManage.boarders = JSON.stringify(this.lstAddedDininBoarders.map(x => x.id));
    if (this.diningMealManageId > 0) {
      this.mealManage.id = this.diningMealManageId;
      this.diningMealManageService.updateDiningMealManage(this.mealManage).subscribe((res: ResponseMessage) => {
        this.workWithResponse(res);
      });
    }
    else {
      this.diningMealManageService.saveDiningMealManage(this.mealManage).subscribe((res: ResponseMessage) => {
        this.workWithResponse(res);
      });
    }
  }
  workWithResponse(res: ResponseMessage) {
    this.response.message = res.message;
    this.response.statusCode = res.statusCode;
    if (this.response.statusCode == ResponseStatusCodeEnum.Success) {
      this.dataService.setValueToResponseMessageProperty(this.response);
      this.router.navigateByUrl("/finance/diningMealManage");
    }
    else {
      this.reset.push({});
    }
  }
}
