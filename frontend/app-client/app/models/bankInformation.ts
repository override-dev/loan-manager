import { BehaviorSubject, Observable, Subject } from "rxjs";
import type { PropertyChangedEventArgs } from "~/interfaces/INotifyPropertyChanged";

export class BankInformation {
  private bankNameSubject = new BehaviorSubject<string>("");
  private accountTypeSubject = new BehaviorSubject<string>("");
  private accountNumberSubject = new BehaviorSubject<string>("");

  public bankName$ = this.bankNameSubject.asObservable();
  public accountType$ = this.accountTypeSubject.asObservable();
  public accountNumber$ = this.accountNumberSubject.asObservable();

  constructor(data?: { bankName?: string; accountType?: string; accountNumber?: string }) {
    if (data?.bankName) this.bankNameSubject.next(data.bankName);
    if (data?.accountType) this.accountTypeSubject.next(data.accountType);
    if (data?.accountNumber) this.accountNumberSubject.next(data.accountNumber);
  }



  setBankName(bankName: string): void {
    this.setProperty(this.bankNameSubject, 'bankName', bankName);
  }

  setAccountType(accountType: string): void {
    this.setProperty(this.accountTypeSubject, 'accountType', accountType);
  }

  setAccountNumber(accountNumber: string): void {
    this.setProperty(this.accountNumberSubject, 'accountNumber', accountNumber);
  }

  get bankName(): string {
    return this.bankNameSubject.value;
  }

  get accountType(): string {
    return this.accountTypeSubject.value;
  }

  get accountNumber(): string {
    return this.accountNumberSubject.value;
  }

  // this can be moved to an abstract class 
  private _propertyChanged = new Subject<PropertyChangedEventArgs>();

  get propertyChanged$(): Observable<PropertyChangedEventArgs> {
    return this._propertyChanged.asObservable();
  }

  protected notifyPropertyChanged(propertyName: string, oldValue: any, newValue: any): void {
    this._propertyChanged.next({ propertyName, oldValue, newValue });
  }

  destroy(): void {
    this._propertyChanged.complete();
  }

  private setProperty<T>(subject: BehaviorSubject<T>, propertyName: string, value: T): void {
    const oldValue = subject.value;
    if (oldValue === value) return;

    subject.next(value);
    this.notifyPropertyChanged(propertyName, oldValue, value);
  }
}