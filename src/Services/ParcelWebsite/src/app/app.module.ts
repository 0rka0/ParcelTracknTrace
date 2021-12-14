import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from "@angular/forms"

import { AppComponent } from './app.component';
import { ParcelHandlingComponent } from './parcel-handling/parcel-handling.component';
import { SubmitComponent } from './parcel-handling/submit/submit.component';
import { HttpClientModule } from '@angular/common/http';
import { TrackComponent } from './parcel-handling/track/track.component';
import { ReportComponent } from './parcel-handling/report/report.component';

@NgModule({
  declarations: [
    AppComponent,
    ParcelHandlingComponent,
    SubmitComponent,
    TrackComponent,
    ReportComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
