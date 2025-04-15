import { useEffect, useState, type FC } from "react";
import type { IStepViewModel } from "~/interfaces/IStepViewModel";
import type { IValidationError } from "~/interfaces/IValidationError";
import { LoanDetails } from "~/models/loanDetails";

interface LoanDetailsViewProps {
  viewModel: IStepViewModel<LoanDetails>;
}

const LoanDetailsView: FC<LoanDetailsViewProps> = ({ viewModel }) => {
  const [loanDetails, setLoanDetails] = useState<LoanDetails>(new LoanDetails());
  const [errors, setErrors] = useState<IValidationError[]>([]);

  useEffect(() => {
    // Subscribe to data changes
    const dataSubscription = viewModel.data$.subscribe((details) => {
      setLoanDetails(details);
    });
    
    // Subscribe to validation errors
    const errorsSubscription = viewModel.errors$.subscribe((validationErrors) => {
      setErrors(validationErrors);
    });

    // Force an initial validation
    if (typeof viewModel.validate === 'function') {
      viewModel.validate();
    }

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

  const handleLoanAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const loanAmount = parseFloat(e.target.value);
    viewModel.updateData({ loanAmount: isNaN(loanAmount) ? 0 : loanAmount });
  };

  const handleLoanPurposeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const loanPurpose = parseInt(e.target.value, 10);
    viewModel.updateData({ loanPurpose: isNaN(loanPurpose) ? 0 : loanPurpose });
  };

  const handleLoanTermChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const loanTerm = parseInt(e.target.value, 10);
    viewModel.updateData({ loanTerm: isNaN(loanTerm) ? 0 : loanTerm });
  };

  // Get the max loan amount based on the selected purpose
  const getMaxLoanAmount = (): string => {
    switch (loanDetails.loanPurpose) {
      case 1: return '$50,000';
      case 2: return '$100,000';
      case 3: return '$30,000';
      default: return '$100,000';
    }
  };

  return (
    <div className="space-y-4">
      <h2 className="text-xl font-semibold">Loan Details</h2>
      
      {/* Loan Amount Field */}
      <div className="form-control">
        <label className="label">
          <span className="label-text">Loan Amount</span>
          {loanDetails.loanPurpose > 0 && (
            <span className="label-text-alt">Max: {getMaxLoanAmount()}</span>
          )}
        </label>
        <input
          type="number"
          placeholder="Loan Amount"
          value={loanDetails.loanAmount || ''}
          onChange={handleLoanAmountChange}
          className={`input input-bordered w-full ${getFieldError('loanAmount') ? 'border-red-500' : ''}`}
        />
        {getFieldError('loanAmount') && (
          <div className="text-red-500 text-sm mt-1">{getFieldError('loanAmount')}</div>
        )}
      </div>
      
      {/* Loan Purpose Field */}
      <div className="form-control">
        <label className="label">
          <span className="label-text">Loan Purpose</span>
        </label>
        <select
          className={`select select-bordered w-full ${getFieldError('loanPurpose') ? 'border-red-500' : ''}`}
          value={loanDetails.loanPurpose || ''}
          onChange={handleLoanPurposeChange}
        >
          <option value="0">Select Loan Purpose</option>
          <option value="1">Personal</option>
          <option value="2">Business</option>
          <option value="3">Education</option>
        </select>
        {getFieldError('loanPurpose') && (
          <div className="text-red-500 text-sm mt-1">{getFieldError('loanPurpose')}</div>
        )}
      </div>
      
      {/* Loan Term Field */}
      <div className="form-control">
        <label className="label">
          <span className="label-text">Loan Term</span>
        </label>
        <select
          className={`select select-bordered w-full ${getFieldError('loanTerm') ? 'border-red-500' : ''}`}
          value={loanDetails.loanTerm || ''}
          onChange={handleLoanTermChange}
        >
          <option value="0">Select Loan Term</option>
          <option value="12">12 months</option>
          <option value="24">24 months</option>
          <option value="36">36 months</option>
        </select>
        {getFieldError('loanTerm') && (
          <div className="text-red-500 text-sm mt-1">{getFieldError('loanTerm')}</div>
        )}
      </div>
      
      {/* Display monthly payment estimate if all fields are valid */}
      {!getFieldError('loanAmount') && !getFieldError('loanTerm') && loanDetails.loanAmount > 0 && loanDetails.loanTerm > 0 && (
        <div className="p-4 bg-blue-50 rounded-lg mt-4">
          <h3 className="font-semibold text-blue-800">Estimated Monthly Payment</h3>
          <p className="text-lg font-bold text-blue-900">
            ${calculateMonthlyPayment(loanDetails.loanAmount, loanDetails.loanTerm, 0.05).toFixed(2)}/month
          </p>
          <p className="text-xs text-blue-700">Estimated at 5% annual interest rate</p>
        </div>
      )}
    </div>
  );
};

// Helper function to calculate monthly payment
function calculateMonthlyPayment(principal: number, termMonths: number, annualRate: number): number {
  const monthlyRate = annualRate / 12;
  
  // Use the standard loan payment formula
  return (principal * monthlyRate) / (1 - Math.pow(1 + monthlyRate, -termMonths));
}

export default LoanDetailsView;