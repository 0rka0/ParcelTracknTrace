import { Injectable } from '@angular/core';
import { Parcel } from './parcel.model';
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject } from 'rxjs';
import { TrackingInf } from './tracking-inf.model';
import { environment } from 'src/environments/environment'

@Injectable({
  providedIn: 'root'
})
export class ParcelService {
  private showNav$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) {
    this.baseUrl = environment.appUrl;
  }

  response: string;
  parcelData: Parcel = new Parcel();
  trackingInf: TrackingInf = new TrackingInf();
  returnedTrackingId: string;
  searchTrackingId: string;
  inputTrackingId1: string;
  inputTrackingId2: string;
  inputCode: string;
  baseUrl = 'API Url';

  postSubmit()
  {
    return this.http.post(this.baseUrl, this.parcelData)
  }

  getTrack()
  {
    return this.http.get(this.baseUrl + this.searchTrackingId)
  }

  postReport()
  {
    return this.http.post(this.baseUrl + this.inputTrackingId1 + '/reportHop/' + this.inputCode, null)
  }

  postDelivery()
  {
    return this.http.post(this.baseUrl + this.inputTrackingId2 + '/reportDelivery', null)
  }
}
