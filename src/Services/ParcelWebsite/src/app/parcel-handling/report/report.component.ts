import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ParcelService } from '../../shared/parcel.service';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styles: [
  ]
})
export class ReportComponent implements OnInit {

  constructor(public service: ParcelService) { }

  ngOnInit(): void {
  }

  onReport(form: NgForm) {
    this.service.postReport().subscribe(
      (res: any) =>
      {
        this.service.response = "Success";
      },
      err =>
      {
        this.service.response = err
        console.log(err)
      }
    )
  }

  onDelivery(form: NgForm) {
    this.service.postDelivery().subscribe(
      (res: any) => {
        this.service.response = "Success";
      },
      err => {
        this.service.response = err
        console.log(err)
      }
    )
  }
}
