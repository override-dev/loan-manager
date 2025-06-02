import type { Observable } from "rxjs";

export interface PropertyChangedEventArgs {
  propertyName: string;
  oldValue: any;
  newValue: any;
}
