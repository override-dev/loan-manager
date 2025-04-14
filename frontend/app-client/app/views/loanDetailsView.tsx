import { useEffect, useState, type FC } from "react";
import { Subscription } from "rxjs";
import type { IStepViewModel } from "~/interfaces/IStepViewModel";
import { LoanDetails } from "~/models/loanDetails";


interface LoanDetailsViewProps {
    viewModel: IStepViewModel<LoanDetails>;
}
const LoanDetailsView: FC<LoanDetailsViewProps> = ({ viewModel }) => {

    const [loanDetails, setLoanDetails] = useState<LoanDetails>(new LoanDetails());

    let subscription: Subscription | null = null;

    useEffect(() => {

        subscription = viewModel.data$.subscribe((details) => {
            setLoanDetails(details);
            console.log("Updated details:", details);
        });

        return () => {
            if (subscription) {
                subscription.unsubscribe();
            }
        };
    }, []);


    const handleLoanAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const loanAmount = parseFloat(e.target.value);
        viewModel.updateData({ loanAmount: isNaN(loanAmount) ? 0 : loanAmount })
    };


    const handleLoanPurposeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const loanPurpose = parseInt(e.target.value, 10);
        viewModel.updateData({ loanPurpose: isNaN(loanPurpose) ? 0 : loanPurpose })

    };


    const handleLoanTermChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const loanTerm = parseInt(e.target.value, 10);
        viewModel.updateData({ loanTerm: isNaN(loanTerm) ? 0 : loanTerm })
    };

    return (
        <div className="space-y-4">
            <h2 className="text-xl font-semibold">Loan Details</h2>
            <div className="form-control">
                <input
                    type="number"
                    placeholder="Loan Amount"
                    value={loanDetails.loanAmount}
                    onChange={handleLoanAmountChange}
                    className="input input-bordered w-full"
                />
            </div>
            <div className="form-control">
                <select
                    className="select select-bordered w-full"
                    value={loanDetails.loanPurpose}
                    onChange={handleLoanPurposeChange}
                >
                    <option value="" disabled>Select Loan Purpose</option>
                    <option value="1">Personal</option>
                    <option value="2">Business</option>
                    <option value="3">Education</option>
                </select>
            </div>
            <div className="form-control">
                <select
                    className="select select-bordered w-full"
                    value={loanDetails.loanTerm}
                    onChange={handleLoanTermChange}
                >
                    <option value="" disabled>Loan Term</option>
                    <option value="12">12 months</option>
                    <option value="24">24 months</option>
                    <option value="36">36 months</option>
                </select>
            </div>
        </div>
    );
};

export default LoanDetailsView;