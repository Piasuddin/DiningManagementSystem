import { Injectable } from '@angular/core';
import { AppHost } from 'src/app/model/app-host.model';
import { RequestMessage } from 'src/app/model/request-message';
import { ResponseMessage } from 'src/app/model/response-message';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class DiningMealManageService {
    constructor(private http: HttpClient, private appHost: AppHost) { }
    request = new RequestMessage();

    url = this.appHost.hostName + "api/DiningMealManage/";

    public searchDiningMealManage(id: string): Observable<ResponseMessage> {
        this.request.content = id;
        return this.http.post<ResponseMessage>(this.url + "search", this.request);
    }
    public searchDiningMealManageForTable(obj : any): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "searchTableData", this.request);
    }
    public deleteDiningMealManage(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "delete", this.request);
    }
    public saveDiningMealManage(obj: any): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "save", this.request);
    }
    public updateDiningMealManage(obj: any): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "update", this.request);
    }
    public searchDiningMealManageDetails(obj: string): any {
        this.request.content = obj;
        return this.http.post(this.url + "searchDetails", this.request);
    }
    public SearchDiningMealManageAddData(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "addData", this.request);
    }
    public SearchDiningMealManageEditData(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "editData", this.request);
    }
}