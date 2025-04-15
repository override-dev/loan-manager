import { BehaviorSubject, combineLatest, Observable } from "rxjs";
import { map, debounceTime } from "rxjs/operators";
import type { IStepViewModel } from "~/interfaces/IStepViewModel";
import type { IValidationError } from "~/interfaces/IValidationError";
import { BankInformation } from "~/models/bankInformation";
import { AccountNumberFormatByTypeSpecification } from "~/specifications/BankInformationSpecifications/AccountNumberFormatByTypeSpecification";
import { AccountNumberSpecification } from "~/specifications/BankInformationSpecifications/AccountNumberSpecification";
import { AccountTypeSpecification } from "~/specifications/BankInformationSpecifications/AccountTypeSpecification";
import { BankNameSpecification } from "~/specifications/BankInformationSpecifications/BankNameSpecification";
import type { ISpecification } from "~/specifications/ISpecification";


export class BankInformationViewModel implements IStepViewModel<BankInformation> {
  private model: BankInformation;
  private _errors: BehaviorSubject<IValidationError[]>;
  private formSpecification: ISpecification<BankInformation>;

  public data$: Observable<BankInformation>;
  public errors$: Observable<IValidationError[]>;
  public isValid$: Observable<boolean>;
  
  constructor() {
    this.model = new BankInformation();
    this._errors = new BehaviorSubject<IValidationError[]>([]);

    // Create individual specifications
    const bankNameSpec = new BankNameSpecification();
    const accountTypeSpec = new AccountTypeSpecification();
    const accountNumberSpec = new AccountNumberSpecification();
    const accountNumberFormatSpec = new AccountNumberFormatByTypeSpecification();

    // Combine specifications for the whole form
    this.formSpecification = bankNameSpec
      .and(accountTypeSpec)
      .and(accountNumberSpec)
      .and(accountNumberFormatSpec);

    // Set up data observable
    this.data$ = combineLatest([
      this.model.bankName$,
      this.model.accountType$,
      this.model.accountNumber$,
    ]).pipe(
      map(([bankName, accountType, accountNumber]) => {
        return new BankInformation({ bankName, accountType, accountNumber });
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

  updateData(data: Partial<BankInformation>): void {
    if (data.bankName !== undefined) this.model.setBankName(data.bankName);
    if (data.accountType !== undefined) this.model.setAccountType(data.accountType);
    if (data.accountNumber !== undefined) this.model.setAccountNumber(data.accountNumber);
  }

  updateBankName(bankName: string): void {
    this.model.setBankName(bankName);
  }

  updateAccountType(accountType: string): void {
    this.model.setAccountType(accountType);
  }

  updateAccountNumber(accountNumber: string): void {
    this.model.setAccountNumber(accountNumber);
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