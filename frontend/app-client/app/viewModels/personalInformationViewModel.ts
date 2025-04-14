import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import type { IStepViewModel } from '~/interfaces/IStepViewModel';
import { PersonalInformation } from '~/models/perfonalInformation';

export class PersonalInformationViewModel implements IStepViewModel<PersonalInformation> {
  private model: PersonalInformation;

  public data$: Observable<PersonalInformation>;

  constructor() {
    this.model = new PersonalInformation();

    this.data$ = combineLatest([
      this.model.fullName$,
      this.model.dateOfBirth$,
      this.model.email$,
    ]).pipe(
      map(([fullName, dateOfBirth, email]) => {
        return new PersonalInformation({ fullName, dateOfBirth, email });
      })
    );
  }

  updateData(data: Partial<PersonalInformation>): void {
    if (data.fullName) this.model.setFullName(data.fullName);
    if (data.dateOfBirth) this.model.setDateOfBirth(data.dateOfBirth);
    if (data.email) this.model.setEmail(data.email);
  }

  updateFullName(fullName: string): void {
    this.model.setFullName(fullName);
    console.log(fullName); // TODO: remove this log, it's only for testing purposes
  }

  updateDateOfBirth(dateOfBirth: string): void {
    this.model.setDateOfBirth(dateOfBirth);
    console.log(dateOfBirth); // TODO: remove this log, it's only for testing purposes
  }

  updateEmail(email: string): void {
    this.model.setEmail(email);
    console.log(email); // TODO: remove this log, it's only for testing purposes
  }
}