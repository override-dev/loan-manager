import { useEffect, useState, type FC } from "react";
import { Subscription } from "rxjs";
import type { IStepViewModel } from "~/interfaces/IStepViewModel";
import { BankInformation } from "~/models/bankInformation";


interface BankInformationViewProps {
    viewModel: IStepViewModel<BankInformation>;
  }
  
const BankInformationView:FC<BankInformationViewProps> = ({viewModel}) => {
  
  const [bankInfo, setBankInfo] = useState<BankInformation>(new BankInformation());

  let subscription: Subscription | null = null;

  useEffect(() => {
 
    subscription = viewModel.data$.subscribe((info) => {
      setBankInfo(info);
    });

  
    return () => {
      if (subscription) {
        subscription.unsubscribe();
      }
    };
  }, []);

 
  const handleBankNameChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    viewModel.updateData({ bankName: e.target.value});
  };

 
  const handleAccountTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    viewModel.updateData({ accountType: e.target.value });
  };

  const handleAccountNumberChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    viewModel.updateData({ accountNumber: e.target.value });
  };

  return (
    <div className="space-y-4">
      <h2 className="text-xl font-semibold">Bank Information</h2>
      <div className="form-control">
        <select
          className="select select-bordered w-full"
          value={bankInfo.bankName}
          onChange={handleBankNameChange}
        >
          <option disabled>Select Bank</option>
          <option>Bank A</option>
          <option>Bank B</option>
          <option>Bank C</option>
        </select>
      </div>
      <div className="form-control">
        <select
          className="select select-bordered w-full"
          value={bankInfo.accountType}
          onChange={handleAccountTypeChange}
        >
          <option disabled>Account Type</option>
          <option>Savings</option>
          <option>Checking</option>
        </select>
      </div>
      <div className="form-control">
        <input
          type="text"
          placeholder="Account Number"
          value={bankInfo.accountNumber}
          onChange={handleAccountNumberChange}
          className="input input-bordered w-full"
        />
      </div>
    </div>
  );
};

export default BankInformationView;