import { BehaviorSubject } from "rxjs";

export class PersonalInformation {
    private fullNameSubject = new BehaviorSubject<string>('');
    private dateOfBirthSubject = new BehaviorSubject<string>('');
    private emailSubject = new BehaviorSubject<string>('');
  
    public fullName$ = this.fullNameSubject.asObservable();
    public dateOfBirth$ = this.dateOfBirthSubject.asObservable();
    public email$ = this.emailSubject.asObservable();
  
    constructor(data?: { fullName?: string; dateOfBirth?: string; email?: string }) {
      if (data?.fullName) this.fullNameSubject.next(data.fullName);
      if (data?.dateOfBirth) this.dateOfBirthSubject.next(data.dateOfBirth);
      if (data?.email) this.emailSubject.next(data.email);
    }
  
    get fullName(): string {
      return this.fullNameSubject.value; 
    }
  
    get dateOfBirth(): string {
      return this.dateOfBirthSubject.value; 
    }
  
    get email(): string {
      return this.emailSubject.value;
    }
  
    setFullName(fullName: string): void {
      this.fullNameSubject.next(fullName);
    }
  
    setDateOfBirth(dateOfBirth: string): void {
      this.dateOfBirthSubject.next(dateOfBirth);
    }
  
    setEmail(email: string): void {
      this.emailSubject.next(email);
    }
  }