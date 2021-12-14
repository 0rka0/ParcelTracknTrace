import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Parcel } from '../../shared/parcel.model';
import { ParcelService } from '../../shared/parcel.service';

@Component({
  selector: 'app-submit',
  templateUrl: './submit.component.html',
  styles: [
  ]
})
export class SubmitComponent implements OnInit {

  constructor(public service: ParcelService) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {
    this.service.postSubmit().subscribe(
      (res : any) =>
      {
        this.resetForm(form);
        this.service.returnedTrackingId = res['trackingId'];
        this.service.response = "Success";
      },
      err =>
      {
        this.service.response = err
        console.log(err)
      }
    );
  }

  resetForm(form: NgForm)
  {
    form.form.reset();
    this.service.parcelData = new Parcel();
  }

}
