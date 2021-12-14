import { newArray } from "@angular/compiler/src/util";
import { HopArrival } from "./hop-arrival.model";

export class TrackingInf {
  state: string = '';

  visitedHops: HopArrival[] = [];
  futureHops: HopArrival[] = [];
}
