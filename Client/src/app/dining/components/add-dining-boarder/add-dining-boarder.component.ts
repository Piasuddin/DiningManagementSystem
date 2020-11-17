import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBoarderType, Religion } from 'src/app/enums/Enums';
import { DiningBoarder, DiningBoarderAddData } from '../../models/dining-boarder.model';
import { DiningBoarderService } from '../../services/dining-boarder.service';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonMethod } from 'src/app/merp-common/common-method';
import { CommonService } from 'src/app/services/common.service';
import { CommonDataList } from 'src/app/model/common-data-list.model';
import { DiningBoarderExternalService } from '../../services/dining-boarder-external.service';
import { DiningBoarderExternal } from '../../models/dining-boarder-external.model';

@Component({
  selector: 'app-add-dining-boarder',
  templateUrl: './add-dining-boarder.component.html',
  styleUrls: ['./add-dining-boarder.component.css']
})
export class AddDiningBoarderComponent implements OnInit {
  diningBoarderForm: FormGroup;
  response: ResponseMessage = new ResponseMessage();
  diningBoarder: DiningBoarder = new DiningBoarder();
  diningBoarderAddData: DiningBoarderAddData;
  diningBoardertype = DiningBoarderType;
  boarderType: Object;
  reset: any = [{}];
  isDisable: boolean = false;
  diningBoarderId: number;
  empOrStuId: number;
  diningBoarderNo: string;
  diningBoarderTypeId: number;
  campusId: number;
  borderId: string;
  diningBoarderExternalForm: FormGroup;
  keysOfReligion: any = new Object();
  religion = Religion;
  commonDataList: CommonDataList = new CommonDataList();

  constructor(private _formBuilder: FormBuilder, private diningBoarderService: DiningBoarderService,
    private activatedRoute: ActivatedRoute, private matSnackBar: MatSnackBar, private commonService: 
    CommonService, private commonMethod: CommonMethod, private diningBoarderExternalService:
    DiningBoarderExternalService) {
    this.diningBoarderForm = this._formBuilder.group({
      id: [""],
      campusId: [''],
      boarderTypeId: ["", Validators.required],
      empOrStudentId: ["", Validators.required],
      enrollmentDate: ["", Validators.required]
    });
    this.diningBoarderExternalForm = this._formBuilder.group({
      id: [""],
      externalBorderId: [""],
      name: ["", Validators.required],
      fathersName: ["", Validators.required],
      mobileNo: ["", Validators.required],
      email: ["", [Validators.required, Validators.email]],
      aboutBorder: [""],
      dateOfBirth: ["", Validators.required],
      religionId: ["", Validators.required],
      nationalityId: ["", Validators.required],
      nationalIdNo: [""],
      presentVillageHouse: ["", Validators.required],
      presentRoadBlockSector: ["", Validators.required],
      presentPostOffice: ["", Validators.required],
      presentPostCode: ["", Validators.required],
      presentUpazila: ["", Validators.required],
      presentDistrict: ["", Validators.required],
      enrollmentDate: ["", Validators.required]
    });
    this.keysOfReligion = Object.keys(this.religion).filter(Number);
  }
  ngOnInit() {
    if (this.commonMethod.showForInstituteAdmin()) {
      this.diningBoarderForm.get('campusId').setValidators(Validators.required);
    }
    this.boarderType = Object.keys(DiningBoarderType).filter(Number);
    this.commonService.GetConmmonDataList('').subscribe((res: ResponseMessage) => {
      this.commonDataList = <CommonDataList>res.responseObj;
      this.activatedRoute.paramMap.subscribe(param => {
        this.diningBoarderId = +param.get("id");
        if (this.diningBoarderId > 0) {
          this.diningBoarderExternalService.SearchExternalDiningBoarder('"' + this.diningBoarderId + '"').subscribe((res: ResponseMessage) => {
            const data = <DiningBoarderExternal>res.responseObj;
            this.diningBoarderForm.controls['boarderTypeId'].setValue(data.boarderTypeId);
            this.diningBoarderForm.controls['enrollmentDate'].setValue(data.enrollmentDate);
            this.diningBoarderExternalForm.patchValue(data);
          })
        }
      })
    })
  }
  isShow = false;
  getDiningBoarder(searchKey: string) {
    const selectedType = +this.diningBoarderForm.get("boarderTypeId").value;
    if (selectedType > 0) {
      let enrollmentDate = this.diningBoarderForm.get("enrollmentDate").value;
      if (enrollmentDate) {
        if (searchKey != undefined) {
          this.diningBoarderNo = searchKey;
          let boarderTypeId = this.diningBoarderForm.get("boarderTypeId").value;
          this.isShow = true;
          this.diningBoarderAddData = new DiningBoarderAddData();
          let key = searchKey.trim();
          let data = { searchKey: key, boarderTypeId: boarderTypeId };
          this.diningBoarderService.SearchDiningBoarderAddData(JSON.stringify(data))
            .subscribe((res: ResponseMessage) => {
              if (res.responseObj != null) {
                this.diningBoarderAddData = <DiningBoarderAddData>res.responseObj;
                this.empOrStuId = this.diningBoarderAddData.diningBoarderDetails.diningBoarderId;
                this.diningBoarderTypeId = this.diningBoarderAddData.diningBoarderDetails.boarderTypeId;
                this.campusId = this.diningBoarderAddData.diningBoarderDetails.campusId;
              }
              else {
                this.matSnackBar.open(res.message, "OK", { duration: 10000 })
              }
              this.isShow = false;
            })
        }
        else {
          this.matSnackBar.open("Please enter search value!", "OK", { duration: 10000 })
        }
      }
      else {
        this.matSnackBar.open("Please select a enrollment date!", "GOT IT", { duration: 10000 })
      }
    }
    else {
      this.matSnackBar.open("Please select a boarder type!", "OK", { duration: 10000 })
    }
  }
  onSubmit() {
    this.diningBoarderForm.markAllAsTouched();
    this.diningBoarderExternalForm.markAllAsTouched();
    let boarderTypeId = this.diningBoarderForm.get("boarderTypeId").value;
    if (boarderTypeId > 0) {
      if (boarderTypeId == this.diningBoardertype.Outer) {
        this.diningBoarderExternalForm.get('enrollmentDate').setValue(this.diningBoarderForm.get("enrollmentDate").value);
        if (this.diningBoarderExternalForm.valid) {
          let data: DiningBoarderExternal = this.diningBoarderExternalForm.value;
          data.boarderTypeId = boarderTypeId;
          if (data.id > 0) {
            this.diningBoarderExternalService.UpdateExternalDiningBoarder(data).subscribe((res: ResponseMessage) => {
              this.commonMethod.workWithResponse(res, "/finance/diningBoarder");
            })
          }
          else {
            this.diningBoarderExternalService.SaveExternalDiningBoarder(data).subscribe((res: ResponseMessage) => {
              this.commonMethod.workWithResponse(res, "/finance/diningBoarder");
            })
          }
        }
      }
      else {
        if(this.commonMethod.showForCampusAdmin()){
          this.diningBoarder.campusId = this.campusId;
        }
        this.diningBoarder = new DiningBoarder();
        this.diningBoarder.boarderTypeId = this.diningBoarderTypeId;
        this.diningBoarder.diningBoarderId = this.empOrStuId;
        this.diningBoarder.enrollmentDate = this.diningBoarderForm.get("enrollmentDate").value
        if (this.empOrStuId > 0) {
          this.diningBoarderService.SaveDiningBoarder(this.diningBoarder).subscribe((res: ResponseMessage) => {
            this.commonMethod.workWithResponse(res, "/finance/diningBoarder");
          })
        }
        else {
          this.matSnackBar.open("Don't have any data to save", "GOT IT", { duration: 10000 })
        }
      }
    }
    else {
      this.matSnackBar.open("Please select a boarder type", "GOT IT", { duration: 10000 })
    }
  }
  getlstOfPresentUpazilas() {
    const districtId = +this.diningBoarderExternalForm.get('presentDistrict').value;
    if (districtId > 0) {
      return this.commonDataList.lstUpazilas.filter(e => e.districtId == districtId);
    }
    return [];
  }
  getPostOffice() {
    const upazilaId = +this.diningBoarderExternalForm.get('presentUpazila').value;
    if (upazilaId > 0) {
      return this.commonDataList.lstPostOffices.filter(e => e.upazilaId == upazilaId);
    }
    return [];
  }
  onPresentPostOfficeChange(event) {
    this.commonDataList.lstPostOffices.filter(e => {
      if (e.id == event.target.value) {
        this.diningBoarderExternalForm.controls['presentPostCode'].setValue(e.postCode);
      }
    })
  }
}