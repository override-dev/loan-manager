import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { map, debounceTime } from 'rxjs/operators';
import type { IStepViewModel } from '~/interfaces/IStepViewModel';
import type { IValidationError } from '~/interfaces/IValidationError';
import  { PersonalInformation } from '~/models/perfonalInformation';
import type { ISpecification } from '~/specifications/ISpecification';
import { DateOfBirthSpecification } from '~/specifications/PersonalInformationSpecifications/DateOfBirthSpecification';
import { EmailSpecification } from '~/specifications/PersonalInformationSpecifications/EmailSpecification';
import { FullNameSpecification } from '~/specifications/PersonalInformationSpecifications/FullNameSpecification';

export class PersonalInformationViewModel implements IStepViewModel<PersonalInformation> {
  private model: PersonalInformation;
  private _errors: BehaviorSubject<IValidationError[]>;
  private formSpecification: ISpecification<PersonalInformation>;

  public data$: Observable<PersonalInformation>;
  public errors$: Observable<IValidationError[]>;
  public isValid$: Observable<boolean>;

  constructor() {
    this.model = new PersonalInformation();
    this._errors = new BehaviorSubject<IValidationError[]>([]);

    
    const fullNameSpec = new FullNameSpecification();
    const emailSpec = new EmailSpecification();
    const dateOfBirthSpec = new DateOfBirthSpecification();

    this.formSpecification = fullNameSpec
    .and(emailSpec)
    .and(dateOfBirthSpec);


    this.data$ = combineLatest([
      this.model.fullName$,
      this.model.dateOfBirth$,
      this.model.email$,
    ]).pipe(
      map(([fullName, dateOfBirth, email]) => {
        return new PersonalInformation({ fullName, dateOfBirth, email });
      })
    );

 
    this.errors$ = this._errors.asObservable();

   
    this.isValid$ = this.errors$.pipe(
      map(errors => errors.length === 0)
    );

    
    this.data$.pipe(
      debounceTime(300) 
    ).subscribe(() => {
      this.validate();
    });
  }

  updateData(data: Partial<PersonalInformation>): void {
    if (data.fullName !== undefined) this.model.setFullName(data.fullName);
    if (data.dateOfBirth !== undefined) this.model.setDateOfBirth(data.dateOfBirth);
    if (data.email !== undefined) this.model.setEmail(data.email);
  }

  updateFullName(fullName: string): void {
    this.model.setFullName(fullName);
  }

  updateDateOfBirth(dateOfBirth: string): void {
    this.model.setDateOfBirth(dateOfBirth);
  }

  updateEmail(email: string): void {
    this.model.setEmail(email);
  }

  validate(): void {
  
    const validationResult = this.formSpecification.check(this.model);
    
    
    const errors: IValidationError[] = validationResult.errors.map(error => ({
      field: error.field,
      message: error.message
    }));
    
    this._errors.next(errors);
  }
}