import { Injectable } from '@angular/core';
import { AppHost } from 'src/app/model/app-host.model';
import { HttpClient } from '@angular/common/http';
import { RequestMessage } from 'src/app/model/request-message';
import { Observable } from 'rxjs';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBillCollection } from '../models/dining-bill-collection.model';

@Injectable({
    providedIn: 'root'
})
export class DiningBillCollectionService {
    constructor(private http: HttpClient, private appHost: AppHost) {
    }
    request = new RequestMessage();
    url = this.appHost.hostName + "api/DiningBillCollection/";

    public SearchDiningBillCollection(id: string): Observable<ResponseMessage> {
        this.request.content = id;
        return this.http.post<ResponseMessage>(this.url + "search", this.request);
    }
    public SearchDiningBillCollectionForTable(): Observable<ResponseMessage> {
        return this.http.post<ResponseMessage>(this.url + "searchTableData", this.request);
    }
    public DeleteDiningBillCollection(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "delete", this.request);
    }
    public SaveDiningBillCollection(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "save", this.request);
    }
    public UpdateDiningBillCollection(obj: DiningBillCollection): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "update", this.request);
    }
    public DiningBillCollectionDetails(obj: string): any {
        this.request.content = obj;
        return this.http.post(this.url + "searchDetails", this.request);
    }
    public SearchDiningBillCollectionAddData(searchKey: string): Observable<ResponseMessage> {
        this.request.content = searchKey;
        return this.http.post<ResponseMessage>(this.url + "addData", this.request);
    }
}