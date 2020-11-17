import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBillService } from '../../services/dining-bill.service';

@Component({
  selector: 'app-dining-bill-details',
  templateUrl: './dining-bill-details.component.html',
  styleUrls: ['./dining-bill-details.component.css']
})
export class DiningBillDetailsComponent implements OnInit {
  date: string;
  diningBillDetails: any;
  constructor(private activatedRoute: ActivatedRoute, private diningBillService: DiningBillService) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(e => {
      this.date = e.get('id');
      if (this.date != null) {
        this.diningBillService.DiningBillDetails('"' + this.date + '"').subscribe((res: ResponseMessage) => {
          if (res.responseObj != null) {
            this.diningBillDetails = res.responseObj;
          }
        })
      }
    })
  }
}