import { Injectable } from '@angular/core';
import { AppHost } from 'src/app/model/app-host.model';
import { RequestMessage } from 'src/app/model/request-message';
import { ResponseMessage } from 'src/app/model/response-message';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class DiningMealAttendanceService {
    constructor(private http: HttpClient, private appHost: AppHost) { }
    request = new RequestMessage();

    url = this.appHost.hostName + "api/DiningMealAttendance/";

    searchDiningMealAttendance(id: string): Observable<ResponseMessage> {
        this.request.content = id;
        return this.http.post<ResponseMessage>(this.url + "search", this.request);
    }
    searchDiningMealAttendanceForTable(data: string): Observable<ResponseMessage> {
        this.request.content = data;
        return this.http.post<ResponseMessage>(this.url + "searchTableData", this.request);
    }
    deleteDiningMealAttendance(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "delete", this.request);
    }
    saveDiningMealAttendance(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "save", this.request);
    }
    createAttendanceByMealManage(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "createByMealManage", this.request);
    }
    updateDiningMealAttendance(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "update", this.request);
    }
    searchDiningMealAttendanceDetails(obj: string): any {
        this.request.content = obj;
        return this.http.post(this.url + "searchDetails", this.request);
    }
    public SearchDiningMealAttendanceAddData(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "addData", this.request);
    }
    public SearchDiningMealAttendanceEditData(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "editData", this.request);
    }
}


