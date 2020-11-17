import { Injectable } from '@angular/core'; import { AppHost } from 'src/app/model/app-host.model';
import { HttpClient } from '@angular/common/http';
import { RequestMessage } from 'src/app/model/request-message';
import { Observable } from 'rxjs';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningExpense } from '../models/dining-expense.model';

@Injectable({
    providedIn: 'root'
})
export class DiningExpenseService {
    constructor(private http: HttpClient, private appHost: AppHost) {

    }
    request = new RequestMessage();
    url= this.appHost.hostName + "api/DiningExpense/";

    public SearchDiningExpense(id: string): Observable<ResponseMessage> {
        this.request.content = id;
        return this.http.post<ResponseMessage>(this.url + "search", this.request);
    }
    public SearchDiningExpenseForTable(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "searchTableData", this.request);
    }
    public deleteDiningExpense(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "delete", this.request);
    }
    public SaveDiningExpense(obj: DiningExpense): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "save", this.request);
    }
    public UpdateDiningExpense(obj: DiningExpense): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "update", this.request);
    }
    public DiningExpenseDetails(obj: string): any {
        this.request.content = obj;
        return this.http.post(this.url + "searchDetails", this.request);
    }
    public SearchDiningExpenseAddData(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "addData", this.request);
    }
    public SearchDiningExpenseStatement(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "statement", this.request);
    }
}