import { Injectable } from '@angular/core'; import { AppHost } from 'src/app/model/app-host.model';
import { HttpClient } from '@angular/common/http';
import { RequestMessage } from 'src/app/model/request-message';
import { Observable } from 'rxjs';
import { ResponseMessage } from 'src/app/model/response-message';
import { DiningBoarder } from '../models/dining-boarder.model';
import { DiningBoarderExternal } from '../models/dining-boarder-external.model';

@Injectable({
    providedIn: 'root'
})
export class DiningBoarderExternalService {
    constructor(private http: HttpClient, private appHost: AppHost) {

    }
    request = new RequestMessage();
    url= this.appHost.hostName + "api/DiningBoarderExternal/";

    public SearchExternalDiningBoarder(id: string): Observable<ResponseMessage> {
        this.request.content = id;
        return this.http.post<ResponseMessage>(this.url + "search", this.request);
    }
    public DeleteDiningBoarder(obj: string): Observable<ResponseMessage> {
        this.request.content = obj;
        return this.http.post<ResponseMessage>(this.url + "delete", this.request);
    }
    public SaveExternalDiningBoarder(obj: DiningBoarderExternal): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "save", this.request);
    }
    public UpdateExternalDiningBoarder(obj: DiningBoarderExternal): Observable<ResponseMessage> {
        this.request.content = JSON.stringify(obj);
        return this.http.post<ResponseMessage>(this.url + "update", this.request);
    }
}