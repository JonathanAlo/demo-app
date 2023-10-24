import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class DataService {

    httpOPtions = new HttpHeaders({ 'Contenet-Type': 'application/json ' })

    constructor(private http: HttpClient) {}

    UserService() {
        return this.http.get('assets/json/users.json');
    }
   
}