import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { stepRegistry } from "~/helpers/stepRegistry";
import type { IStepViewModel } from "~/interfaces/IStepViewModel";

export class ParentViewModel {
  private viewModels: Array<IStepViewModel<any>> = [];
  private currentStepSubject = new BehaviorSubject<number>(0);

  public currentStep$ = this.currentStepSubject.asObservable();
  public currentViewModel$: Observable<IStepViewModel<any>>;

  constructor() {
    // we could use DI to inject the view models, but for now we'll just use the registry
    this.viewModels = Object.values(stepRegistry).map((entry) => new entry.viewModelClass());
    this.currentViewModel$ = this.currentStep$.pipe(
      map((step) => this.viewModels[step])
    );
  }

  nextStep(): void {
    const currentStep = this.currentStepSubject.value;
    if (currentStep < this.viewModels.length - 1) {
      this.currentStepSubject.next(currentStep + 1);
    }
  }

  previousStep(): void {
    const currentStep = this.currentStepSubject.value;
    if (currentStep > 0) {
      this.currentStepSubject.next(currentStep - 1);
    }
  }

  getCurrentStep(): number {
    return this.currentStepSubject.value;
  }

  getViewModel(step: number): IStepViewModel<any> | undefined {
    return this.viewModels[step];
  }
}