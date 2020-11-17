import { Injectable } from '@angular/core';
import { AppHost } from 'src/app/model/app-host.model';
import { HttpClient } from '@angular/common/http';
import { RequestMessage } from 'src/app/model/request-message';
import { Observable } from 'rxjs';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBill } from '../models/dining-bill.model';

@Injectable({
    providedIn: 'root'
})
export class DiningBillService {
    constructor(private http: HttpClient, private appHost: AppHost) {
    }
    request = new RequestMessage();
    url = this.appHost.hostName + "api/DiningBill/";

    public SearchDiningBill(id: string): Observable<ResponseMessage> {
        this.request.content = id;
        return this.http.post<ResponseMessage>(this.url + "search", this.request);
    }
    public SearchDiningBillForTable(): Observable<ResponseMessage> {
        return this.http.post<ResponseMessage>(this.url + "searchTableData", this.request);
    }
    public deleteDiningBill(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "delete", this.request);
    }
    public SaveDiningBill(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "save", this.request);
    }
    public UpdateDiningBill(obj: DiningBill): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "update", this.request);
    }
    public DiningBillDetails(obj: string): any {
        this.request.content = obj;
        return this.http.post(this.url + "searchDetails", this.request);
    }
    public GetDiningBoarderStatement(obj: string): any {
        this.request.content = obj;
        return this.http.post(this.url + "statement", this.request);
    }
    public GetDiningExpenseStatement(obj: string): any {
        this.request.content = obj;
        return this.http.post(this.url + "expenseStatement", this.request);
    }
    public GetDiningBoarderBillDetails(obj: string): any {
        this.request.content = obj;
        return this.http.post(this.url + "diningBoarderBillDetails", this.request);
    }
}