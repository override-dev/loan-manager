import { BehaviorSubject, combineLatest, Observable } from "rxjs";
import { map, debounceTime, distinctUntilChanged } from "rxjs/operators";
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
  
  // Map all the specifications
  private propertySpecs: Map<string, ISpecification<BankInformation>[]>;
  
  public data$: Observable<BankInformation>;
  public errors$: Observable<IValidationError[]>;
  public isValid$: Observable<boolean>;

  constructor() {
    this.model = new BankInformation();
    this._errors = new BehaviorSubject<IValidationError[]>([]);

    this.propertySpecs = new Map([
      ['bankName', [new BankNameSpecification()]],
      ['accountType', [new AccountTypeSpecification()]],
      ['accountNumber', [
        new AccountNumberSpecification(),
        new AccountNumberFormatByTypeSpecification()
      ]]
    ]);

    this.data$ = combineLatest([
      this.model.bankName$,
      this.model.accountType$,
      this.model.accountNumber$,
    ]).pipe(
      map(([bankName, accountType, accountNumber]) => {
        const newData = new BankInformation();
        if (bankName) newData.setBankName(bankName);
        if (accountType) newData.setAccountType(accountType);
        if (accountNumber) newData.setAccountNumber(accountNumber);
        return newData;
      }),
      distinctUntilChanged((prev, curr) =>
        prev.bankName === curr.bankName &&
        prev.accountType === curr.accountType &&
        prev.accountNumber === curr.accountNumber
      )
    );

    // we set the viewModel isValid = true by default
    this.errors$ = this._errors.asObservable();
    this.isValid$ = this.errors$.pipe(
      map(errors => errors.length === 0)
    );

    this.model.propertyChanged$.pipe(
      debounceTime(300),
      distinctUntilChanged((prev, curr) => 
        prev.propertyName === curr.propertyName && prev.newValue === curr.newValue
      )
    ).subscribe(change => {
      // validate the property when there is a change
      this.validateProperty(change.propertyName);
    });
  }

  private validateProperty(propertyName: string): void {
    const currentErrors = this._errors.value;
    const errorsWithoutCurrentProperty = currentErrors.filter(error => error.field !== propertyName);
    
    const specs = this.propertySpecs.get(propertyName) || [];
    const newErrors: IValidationError[] = [];
    
    specs.forEach(spec => {
      const result = spec.check(this.model);
      newErrors.push(...result.errors.map(error => ({
        field: error.field,
        message: error.message
      })));
    });
    
    const updatedErrors = [...errorsWithoutCurrentProperty, ...newErrors];
    this._errors.next(updatedErrors);
  }

  
  validate(): void {
    this.validateAll();
  }

  private validateAll(): void {
    const allErrors: IValidationError[] = [];
    
    this.propertySpecs.forEach((specs, propertyName) => {
      specs.forEach(spec => {
        const result = spec.check(this.model);
        allErrors.push(...result.errors.map(error => ({
          field: error.field,
          message: error.message
        })));
      });
    });
    
    this._errors.next(allErrors);
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

  getCurrentData(): BankInformation {
    return this.model;
  }

  destroy(): void {
    this.model.destroy();
    this._errors.complete();
  }
}