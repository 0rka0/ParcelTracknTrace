import { Receipient } from "./receipient.model";

export class Parcel {
  weight: number = 0;

  receipient: Receipient = new Receipient();
  sender: Receipient = new Receipient();
}
