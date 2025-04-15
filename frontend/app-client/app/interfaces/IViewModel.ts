import { Observable } from "rxjs";
import type { IValidationError } from "./IValidationError";

export interface IViewModel<T> {

  data$: Observable<T>;
  errors$: Observable<IValidationError[]>;
  updateData(data: Partial<T>): void;
  validate(): void;
}