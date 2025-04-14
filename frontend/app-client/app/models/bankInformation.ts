import { BehaviorSubject } from "rxjs";

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
    this.bankNameSubject.next(bankName);
  }

  setAccountType(accountType: string): void {
    this.accountTypeSubject.next(accountType);
  }

  setAccountNumber(accountNumber: string): void {
    this.accountNumberSubject.next(accountNumber);
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
}