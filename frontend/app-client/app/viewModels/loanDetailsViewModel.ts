import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { map, debounceTime } from 'rxjs/operators';
import type { IStepViewModel } from '~/interfaces/IStepViewModel';
import type { IValidationError } from '~/interfaces/IValidationError';
import { LoanDetails } from '~/models/loanDetails';
import type { ISpecification } from '~/specifications/ISpecification';
import { LoanAmountByPurposeSpecification } from '~/specifications/LoanDetailsSpecifications/LoanAmountByPurposeSpecification';
import { LoanAmountSpecification } from '~/specifications/LoanDetailsSpecifications/LoanAmountSpecification';
import { LoanPurposeSpecification } from '~/specifications/LoanDetailsSpecifications/LoanPurposeSpecification';
import { LoanTermSpecification } from '~/specifications/LoanDetailsSpecifications/LoanTermSpecification';

export class LoanDetailsViewModel implements IStepViewModel<LoanDetails> {
  private model: LoanDetails;
  private _errors: BehaviorSubject<IValidationError[]>;
  private formSpecification: ISpecification<LoanDetails>;

  public data$: Observable<LoanDetails>;
  public errors$: Observable<IValidationError[]>;
  public isValid$: Observable<boolean>;

  constructor() {
    this.model = new LoanDetails();
    this._errors = new BehaviorSubject<IValidationError[]>([]);

    // Create individual specifications
    const loanAmountSpec = new LoanAmountSpecification();
    const loanPurposeSpec = new LoanPurposeSpecification();
    const loanTermSpec = new LoanTermSpecification();
    const loanAmountByPurposeSpec = new LoanAmountByPurposeSpecification();

    // Combine specifications for the whole form
    this.formSpecification = loanAmountSpec
      .and(loanPurposeSpec)
      .and(loanTermSpec)
      .and(loanAmountByPurposeSpec);

    // Set up data observable
    this.data$ = combineLatest([
      this.model.loanAmount$,
      this.model.loanPurpose$,
      this.model.loanTerm$,
    ]).pipe(
      map(([loanAmount, loanPurpose, loanTerm]) => {
        return new LoanDetails({ loanAmount, loanPurpose, loanTerm });
      })
    );

    // Set up errors observable
    this.errors$ = this._errors.asObservable();

    // Set up validity observable
    this.isValid$ = this.errors$.pipe(
      map(errors => errors.length === 0)
    );

    // Set up automatic validation when data changes
    this.data$.pipe(
      debounceTime(300) // Wait 300ms after the last change before validating
    ).subscribe(() => {
      this.validate();
    });
    
    // Run initial validation
    this.validate();
  }

  updateData(data: Partial<LoanDetails>): void {
    if (data.loanAmount !== undefined) this.model.setLoanAmount(data.loanAmount);
    if (data.loanPurpose !== undefined) this.model.setLoanPurpose(data.loanPurpose);
    if (data.loanTerm !== undefined) this.model.setLoanTerm(data.loanTerm);
  }

  updateLoanAmount(loanAmount: number): void {
    this.model.setLoanAmount(loanAmount);
  }

  updateLoanPurpose(loanPurpose: number): void {
    this.model.setLoanPurpose(loanPurpose);
  }

  updateLoanTerm(loanTerm: number): void {
    this.model.setLoanTerm(loanTerm);
  }

  validate(): void {
    // Use the check method from the combined specification
    const validationResult = this.formSpecification.check(this.model);
    
    // Map validation errors to the expected format
    const errors: IValidationError[] = validationResult.errors.map(error => ({
      field: error.field,
      message: error.message
    }));
    
    // Update the errors subject
    this._errors.next(errors);
  }
}