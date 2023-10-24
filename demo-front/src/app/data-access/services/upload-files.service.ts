import { Injectable } from '@angular/core';
import { environment } from 'src/environment/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class FileManagerService {
  private rootUrl: string;
  private myApiUrls: string;
  private getFiles: string;
  private getItemFile: string;

  httpOPtions = new HttpHeaders({ 'Contenet-Type': 'application/json ' });

  constructor(private http: HttpClient) {
    this.rootUrl = environment.endpoint;
    this.myApiUrls = 'uploadlocal/';
    this.getFiles = 'getFilesfolder'
    this.getItemFile = 'getfilelocal/';
  }

  getAll(){
    return this.http.get(`${this.rootUrl}${this.getFiles}`)
  }

  uploadFile(fileupload: File) {
    const formData = new FormData();
    formData.append('file', fileupload);
    return this.http.post(`${this.rootUrl}${this.myApiUrls}`, formData);
  }

  getFile(fileName: string) {
    return this.http.get(`${this.rootUrl}${this.getItemFile}${fileName}`, {
      responseType: 'text',
    });
  }
}
