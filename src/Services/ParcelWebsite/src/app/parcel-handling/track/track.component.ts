import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ParcelService } from '../../shared/parcel.service';
import { TrackingInf } from '../../shared/tracking-inf.model';

@Component({
  selector: 'app-track',
  templateUrl: './track.component.html',
  styles: [
  ]
})
export class TrackComponent implements OnInit {

  constructor(public service: ParcelService) { }

  ngOnInit(): void {
  }

  onTrack(form: NgForm) {
    this.service.getTrack().toPromise().then(
      (res: any) => {
        this.service.trackingInf = res as TrackingInf
        this.service.response = "Success";
        console.log(this.service.trackingInf)
      },
      err =>
      {
        this.service.response = err
        console.log(err)
      }
    );
  }
}

