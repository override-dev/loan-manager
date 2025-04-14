import { BehaviorSubject } from "rxjs";

export class LoanDetails {

   private loanAmountSubject = new BehaviorSubject<number>(0);
   private loanTermSubject = new BehaviorSubject<number>(0);
   private loanPurposeSubject = new BehaviorSubject<number>(0);

   public loanAmount$ = this.loanAmountSubject.asObservable();
   public loanTerm$ = this.loanTermSubject.asObservable();
   public loanPurpose$ = this.loanPurposeSubject.asObservable();

  
  
    constructor(data?: { loanAmount?: number; loanTerm?: number; loanPurpose?: number  }) {
     if (data?.loanAmount) this.loanAmountSubject.next(data.loanAmount);
     if (data?.loanTerm) this.loanTermSubject.next(data.loanTerm);
     if (data?.loanPurpose) this.loanPurposeSubject.next(data.loanPurpose);
    }
  

    setLoanAmount(loanAmount: number): void {
      this.loanAmountSubject.next(loanAmount);
    }

    setLoanTerm(loanTerm: number): void {
      this.loanTermSubject.next(loanTerm);
    }

    setLoanPurpose(loanPurpose: number): void {
      this.loanPurposeSubject.next(loanPurpose); 
    }

    get loanAmount(): number {
      return this.loanAmountSubject.value;
    }

    get loanTerm(): number {
      return this.loanTermSubject.value;
    }

    get loanPurpose(): number {
      return this.loanPurposeSubject.value; 
    }
  }