import { Injectable } from '@angular/core'; import { AppHost } from 'src/app/model/app-host.model';
import { HttpClient } from '@angular/common/http';
import { RequestMessage } from 'src/app/model/request-message';
import { Observable } from 'rxjs';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBoarder } from '../models/dining-boarder.model';

@Injectable({
    providedIn: 'root'
})
export class DiningBoarderService {
    constructor(private http: HttpClient, private appHost: AppHost) {

    }
    request = new RequestMessage();
    url = this.appHost.hostName + "api/DiningBoarder/";

    public SearchDiningBoarder(id: string): Observable<ResponseMessage> {
        this.request.content = id;
        return this.http.post<ResponseMessage>(this.url + "search", this.request);
    }
    public SearchDiningBoarderForTable(obj: any): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "searchTableData", this.request);
    }
    public DeleteDiningBoarder(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "delete", this.request);
    }
    public SaveDiningBoarder(obj: DiningBoarder): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "save", this.request);
    }
    public UpdateDiningBoarder(obj: DiningBoarder): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "update", this.request);
    }
    public DiningBoarderDetails(obj: string): any {
        this.request.content = obj;
        return this.http.post(this.url + "searchDetails", this.request);
    }
    public SearchDiningBoarderAddData(searchKey: string): Observable<ResponseMessage> {
        this.request.content = searchKey;
        return this.http.post<ResponseMessage>(this.url + "diningBoarderAddData", this.request);
    }
    SearchBoarderBySearchKey(searchKey: string): Observable<ResponseMessage> {
        this.request.content = searchKey;
        return this.http.post<ResponseMessage>(this.url + "searchBySearchKey", this.request);
    }
}