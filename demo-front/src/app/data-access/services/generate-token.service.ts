import { Injectable } from '@angular/core';
import { environment } from 'src/environment/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TokenItems } from '../interfaces/token-items';

@Injectable({
    providedIn: 'root'
})
export class GenerateTokenService {
    private rootUrl: string;
    private myApiUrls: string;
    httpOPtions = new HttpHeaders({ 'Contenet-Type': 'application/json ' });

    constructor(private http: HttpClient) {
        this.rootUrl = environment.endpoint;
        this.myApiUrls = 'security/createToken';
    }

    createToken(data: TokenItems) {
        return this.http.post(`${this.rootUrl}${this.myApiUrls}`, data)
    }

}