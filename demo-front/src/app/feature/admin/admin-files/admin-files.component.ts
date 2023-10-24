import { Component } from '@angular/core';
import { FileManagerService } from 'src/app/data-access/services/upload-files.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
declare var $: any;

@Component({
  selector: 'app-admin-files',
  templateUrl: './admin-files.component.html',
  styles: [
  ]
})
export class AdminFilesComponent {

  constructor(
    private _FileService: FileManagerService,
    private _sanitizer: DomSanitizer,
    private _toast: ToastrService,
  ) { }

  ngOnInit() {
    this.getAllFiles();
  }

  listItems: any[] = [];
  getAllFiles() {
    this._FileService.getAll().subscribe((res: any) => {
      this.listItems = res;
      console.log(res)
    })
  }

  fileUrluser!: SafeResourceUrl;
  string64!: string;
  getFile(docName: string) {
    this._FileService.getFile(docName).subscribe((res: any) => {
      console.log(res);
      this.string64 = res;
      this.fileUrluser = this._sanitizer.bypassSecurityTrustResourceUrl(
        'data:image/png;base64,' + this.string64
      );
      this.fileUrluser = this.fileUrluser;
    })
  }

  fileName!: string;
  viewString!: string;
  itemFile: File | null = null;
  onFileChange(event: any) {
    const newFile = event.target.files[0];
    this.itemFile = newFile;
    this.fileName = event.srcElement.files[0]['name'];
    this.viewString = event.target.value;
  }

  uploading = false;
  uploadFile(item: File){
    this.uploading = true;
    this._FileService.uploadFile(item).subscribe((res: any) => {
      this.uploading = false;
      this._toast.success('File upload successfuly', 'Complete', {
        timeOut: 3000,
      });
      this.getAllFiles();
      $('#upload').modal('hide');
    })
  }
}
