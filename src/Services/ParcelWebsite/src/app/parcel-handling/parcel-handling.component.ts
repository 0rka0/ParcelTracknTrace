import { Component, OnInit } from '@angular/core';
import { ParcelService } from '../shared/parcel.service';

@Component({
  selector: 'app-parcel-handling',
  templateUrl: './parcel-handling.component.html',
  styles: [
  ]
})
export class ParcelHandlingComponent implements OnInit {

  constructor(public service: ParcelService) { }

  ngOnInit(): void {
  }

}
