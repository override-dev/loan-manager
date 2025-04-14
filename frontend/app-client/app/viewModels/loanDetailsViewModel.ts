import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LoanDetails } from '~/models/loanDetails';

export class LoanDetailsViewModel {
    private model: LoanDetails;
    public data$: Observable<LoanDetails>;

    constructor() {
        this.model = new LoanDetails();

       this.data$ = combineLatest([
            this.model.loanAmount$,
            this.model.loanPurpose$,
            this.model.loanTerm$,
        ]).pipe(
            map(([loanAmount, loanPurpose, loanTerm]) => {
                return new LoanDetails({ loanAmount, loanPurpose, loanTerm });
            })
        );
    }

    updateData(data: Partial<LoanDetails>): void {
        if (data.loanAmount) this.model.setLoanAmount(data.loanAmount);
        if (data.loanPurpose) this.model.setLoanPurpose(data.loanPurpose);
        if (data.loanTerm) this.model.setLoanTerm(data.loanTerm);
    }

    updateLoanAmount(loanAmount: number): void {
        this.model.setLoanAmount(loanAmount);
        console.log(loanAmount) // TODO: remove this log, it's only for testing purp 
    }

    updateLoanPurpose(loanPurpose: number): void {
        this.model.setLoanPurpose(loanPurpose);
        console.log(loanPurpose) // TODO: remove this log, it's only for testing purp 
    }

    updateLoanTerm(loanTerm: number): void {
        this.model.setLoanTerm(loanTerm);
        console.log(loanTerm) // TODO: remove this log, it's only for testing purp 
    }
}