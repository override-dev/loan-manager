import { combineLatest, Observable } from "rxjs";
import { map } from "rxjs/operators";
import type { IStepViewModel } from "~/interfaces/IStepViewModel";
import { BankInformation } from "~/models/bankInformation";

export class BankInformationViewModel implements IStepViewModel<BankInformation> {
    private model: BankInformation;
    
    public data$: Observable<BankInformation>;
    constructor() {
        this.model = new BankInformation();

        this.data$ = combineLatest([
            this.model.bankName$,
            this.model.accountType$,
            this.model.accountNumber$,
        ]).pipe(
            map(([bankName, accountType, accountNumber]) => {
                return new BankInformation({ bankName, accountType, accountNumber });
            })
        );
    }

    updateData(data: Partial<BankInformation>): void {
        if (data.bankName) this.model.setBankName(data.bankName);
        if (data.accountType) this.model.setAccountType(data.accountType);
        if (data.accountNumber) this.model.setAccountNumber(data.accountNumber);
    }

    updateBankName(bankName: string): void {
        this.model.setBankName(bankName);
        console.log(bankName)
    }

    updateAccountType(accountType: string): void {
        this.model.setAccountType(accountType);
        console.log(accountType)
    }

    updateAccountNumber(accountNumber: string): void {
        this.model.setAccountNumber(accountNumber);
        console.log(accountNumber)
    }
}