import { Observable } from "rxjs";

export interface IViewModel<T> {

  data$: Observable<T>;
  updateData(data: Partial<T>): void;
}