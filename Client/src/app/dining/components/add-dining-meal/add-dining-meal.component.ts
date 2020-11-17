import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningMeal } from '../../models/dining-meal.model';
import { DiningMealService } from '../../services/dining-meal.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { ResponseStatusCodeEnum } from 'src/app/enums/Enums';
import { checkIfAlreadyExist, MaxLengthPrevent } from 'src/app/validators/merp-validators';
import { DataSortingHelperService } from 'src/app/services/data-sorting-helper.service';
import { CommonMethod } from 'src/app/merp-common/common-method';

@Component({
  selector: 'app-add-dining-meal',
  templateUrl: './add-dining-meal.component.html',
  styleUrls: ['./add-dining-meal.component.css']
})
export class AddDiningMealComponent implements OnInit {
  diningMealForm: FormGroup;
  response: ResponseMessage = new ResponseMessage();
  diningMeal: DiningMeal = new DiningMeal();
  diningMeals: Array<DiningMeal> = [];
  reset: any = [{}];
  diningMealId: number;
  lstMealSequence: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
  constructor(private _formBuilder: FormBuilder, private diningMealService: DiningMealService,
    private dataService: DataService, private router: Router, public commonMethod: CommonMethod,
    private activatedRoute: ActivatedRoute, private dataSortingHelperService: DataSortingHelperService) {
    this.diningMealForm = this._formBuilder.group({
      id: [""],
      campusId: [''],
      mealName: ["", Validators.required],
      mealSize: ["", Validators.required],
      mealTime: ["", Validators.required],
      mealKey: ["", [Validators.required, MaxLengthPrevent("maelKey", 3)]],
      mealSequence: ["", Validators.required]
    })
  }
  ngOnInit() {
    if (this.commonMethod.showForInstituteAdmin()) {
      this.diningMealForm.get('campusId').setValidators(Validators.required);
    }
    this.diningMealService.SearchDiningMeal("").subscribe((res: ResponseMessage) => {
      this.diningMeals = <DiningMeal[]>res.responseObj;
      this.resetMealSequence();
      this.diningMealForm.get("mealKey").valueChanges.subscribe(e => {
        checkIfAlreadyExist(this.diningMealForm, "mealKey", this.diningMeals, e, "mealKey", this.diningMealId);
      })
      this.activatedRoute.paramMap.subscribe(param => {
        this.diningMealId = +param.get("id");
        if (this.diningMealId > 0) {
          this.diningMeal = this.diningMeals.find(e => e.id == this.diningMealId);
          this.lstMealSequence.push(this.diningMeal.mealSequence);
          this.lstMealSequence.sort((a, b) => {
            return this.dataSortingHelperService.sortDateType(a.toString(),
              b.toString(), true);
          });
          this.diningMealForm.patchValue(this.diningMeal);
        }
      })
    })
  }
  resetMealSequence() {
    let sequence: number[] = [];
    for (let index in this.lstMealSequence) {
      let i = Number(index);
      if (!this.diningMeals.find(e => e.mealSequence == this.lstMealSequence[i])) {
        sequence.push(Number(this.lstMealSequence[i]));
      };
    }
    this.lstMealSequence = sequence;
  }
  onSubmit() {
    this.diningMealForm.markAllAsTouched();
    if (this.diningMealForm.valid) {
      let data = JSON.stringify(this.diningMeal);
      this.diningMeal = this.diningMealForm.value;
      if (this.diningMealId > 0) {
        this.diningMealService.UpdateDiningMeal(this.diningMeal).subscribe((res: ResponseMessage) => {
          this.workWithResponse(res);
        })
      }
      else {
        this.diningMealService.SaveDiningMeal(this.diningMeal).subscribe((res: ResponseMessage) => {
          this.workWithResponse(res);
        })
      }
    }
  }
  workWithResponse(res: ResponseMessage) {
    this.response.message = res.message;
    this.response.statusCode = res.statusCode;
    if (this.response.statusCode == ResponseStatusCodeEnum.Success) {
      this.dataService.setValueToResponseMessageProperty(this.response);
      this.router.navigateByUrl("/finance/diningMeal");
    }
    else {
      this.reset.push({});
    }
  }
}
