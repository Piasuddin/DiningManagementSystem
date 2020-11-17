import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DiningBillService } from '../../services/dining-bill.service';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBoarderStatement, DiningBoarderStatementSearch } from '../../models/dining-boarder-statement.model';

@Component({
  selector: 'app-dining-boarder-statement',
  templateUrl: './dining-boarder-statement.component.html',
  styleUrls: ['./dining-boarder-statement.component.css']
})
export class DiningBoarderStatementComponent implements OnInit {

  diningBoarderStatementForm: FormGroup;
  hide: boolean = false;
  searchData: DiningBoarderStatementSearch = new DiningBoarderStatementSearch();
  diningBoarderStatement: Array<DiningBoarderStatement>;
  message: string;

  constructor(private formBuilder: FormBuilder, private diningBillService: DiningBillService) { 
    this.diningBoarderStatementForm = this.formBuilder.group({
      boarderNo: ['', [Validators.required]],
      formDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]],
      outputFormat: ['', [Validators.required]]
    })
  }
  ngOnInit() {
  }
  onSubmit(){
    if(this.diningBoarderStatementForm.valid){
      this.searchData = this.diningBoarderStatementForm.value;
      this.diningBillService.GetDiningBoarderStatement(JSON.stringify(this.diningBoarderStatementForm.value))
      .subscribe((res: ResponseMessage)=>{
        if(res.responseObj != null){
          this.hide = true;
          this.diningBoarderStatement  = <Array<DiningBoarderStatement>>res.responseObj;
        }
        else{
          console.log(res.message)
          this.message = res.message;
        }
      })
    }
  }
}
