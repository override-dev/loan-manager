import { useEffect, useState, type FC } from "react";
import type { IStepViewModel } from "~/interfaces/IStepViewModel";
import type { IValidationError } from "~/interfaces/IValidationError";
import { BankInformation } from "~/models/bankInformation";

interface BankInformationViewProps {
  viewModel: IStepViewModel<BankInformation>;
}

const BankInformationView: FC<BankInformationViewProps> = ({ viewModel }) => {
  const [bankInfo, setBankInfo] = useState<BankInformation>(new BankInformation());
  const [errors, setErrors] = useState<IValidationError[]>([]);

  useEffect(() => {
    // Subscribe to data changes
    const dataSubscription = viewModel.data$.subscribe((info) => {
      setBankInfo(info);
    });
    
    // Subscribe to validation errors
    const errorsSubscription = viewModel.errors$.subscribe((validationErrors) => {
      setErrors(validationErrors);
    });

  

    return () => {
      dataSubscription.unsubscribe();
      errorsSubscription.unsubscribe();
    };
  }, [viewModel]);

  // Helper function to get error for a specific field
  const getFieldError = (fieldName: string): string | null => {
    const error = errors.find(e => e.field === fieldName);
    return error ? error.message : null;
  };

  const handleBankNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    viewModel.updateData({ bankName: e.target.value });
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
      
      {/* Bank Name Field */}
      <div className="form-control">
        <label className="label">
          <span className="label-text">Bank Name</span>
        </label>
        <input
          type="text"
          placeholder="Bank Name"
          value={bankInfo.bankName}
          onChange={handleBankNameChange}
          className={`input input-bordered w-full ${getFieldError('bankName') ? 'border-red-500' : ''}`}
        />
        {getFieldError('bankName') && (
          <div className="text-red-500 text-sm mt-1">{getFieldError('bankName')}</div>
        )}
      </div>
      
      {/* Account Type Field */}
      <div className="form-control">
        <label className="label">
          <span className="label-text">Account Type</span>
        </label>
        <select
          value={bankInfo.accountType}
          onChange={handleAccountTypeChange}
          className={`select select-bordered w-full ${getFieldError('accountType') ? 'border-red-500' : ''}`}
        >
          <option value="">Select account type</option>
          <option value="checking">Checking</option>
          <option value="savings">Savings</option>
          <option value="credit">Credit</option>
        </select>
        {getFieldError('accountType') && (
          <div className="text-red-500 text-sm mt-1">{getFieldError('accountType')}</div>
        )}
      </div>
      
      {/* Account Number Field */}
      <div className="form-control">
        <label className="label">
          <span className="label-text">Account Number</span>
        </label>
        <input
          type="text"
          placeholder="Account Number"
          value={bankInfo.accountNumber}
          onChange={handleAccountNumberChange}
          className={`input input-bordered w-full ${getFieldError('accountNumber') ? 'border-red-500' : ''}`}
        />
        {getFieldError('accountNumber') && (
          <div className="text-red-500 text-sm mt-1">{getFieldError('accountNumber')}</div>
        )}
        <div className="text-gray-500 text-xs mt-1">
          {bankInfo.accountType === 'checking' && 'Checking accounts must start with 4 or 5'}
          {bankInfo.accountType === 'savings' && 'Savings accounts must start with 1 or 2'}
          {bankInfo.accountType === 'credit' && 'Credit accounts must start with 9'}
        </div>
      </div>
    </div>
  );
};

export default BankInformationView;