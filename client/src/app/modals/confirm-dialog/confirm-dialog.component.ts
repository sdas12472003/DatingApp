import { Component, inject } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  imports: [],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.css'
})
export class ConfirmDialogComponent {
  bsModalRef=inject(BsModalRef);
  title='';
  message='';
  btnOkTest='';
  btnCancelTest='';
  result=false;
  confirm()
  {
    this.result=true;
    this.bsModalRef.hide();
  }
  decline()
  {
    this.bsModalRef.hide();
  }
}
