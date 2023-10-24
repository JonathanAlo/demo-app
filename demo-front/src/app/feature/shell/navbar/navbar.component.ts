import { Component } from '@angular/core';
import { GenerateTokenService } from 'src/app/data-access/services/generate-token.service';
import { TokenItems } from 'src/app/data-access/interfaces/token-items';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MESSAGE_TYPES } from 'src/app/data-access/messages-types';
declare var $: any;//use jQuery

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styles: [
  ]
})
export class NavbarComponent {
  constructor(
    private _fb: FormBuilder,
    private _toast: ToastrService,
    private _token: GenerateTokenService,
  ){
    this.tokenForm = this._fb.group({
      email: [],
      password: []
    });
  }

  tokenForm: FormGroup;
  signIn(){
    const newItem: TokenItems = {
      email: this.tokenForm.value.email,
      password: this.tokenForm.value.password
    }
    console.log(newItem);
    this._token.createToken(newItem).subscribe((res: any) => {
      if (res !== MESSAGE_TYPES.RES_TOKEN) {
        console.log(res);
        this._toast.success('Now you are logged with a token', 'Success', {
          timeOut: 3000,
        });
        this. modalMannager('signgIn', 'hide');
      } else {
        console.log(res);
        this._toast.error('Something goes wrong with your email or password', 'Error', {
          timeOut: 3000,
        });
        this. modalMannager('signgIn', 'hide');
      }
    });
  }

  modalMannager(nModa: string, action: string){
    $('#'+nModa).modal(action);
  }
}
