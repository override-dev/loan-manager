import { ValidationResult } from "~/helpers/validationResult";
import type { LoanDetails } from "~/models/loanDetails";
import { Specification } from "../Specification";

/**
 * Specification for validating loan amount
 */
export class LoanAmountSpecification extends Specification<LoanDetails> {
    constructor(private minAmount: number = 1000, private maxAmount: number = 100000) {
      super();
    }
  
    isSatisfiedBy(candidate: LoanDetails): boolean {
      return (
        candidate.loanAmount >= this.minAmount && 
        candidate.loanAmount <= this.maxAmount
      );
    }
  
    check(candidate: LoanDetails): ValidationResult {
      if (!candidate.loanAmount || candidate.loanAmount <= 0) {
        return ValidationResult.invalid('loanAmount', 'Loan amount is required');
      }
      
      if (candidate.loanAmount < this.minAmount) {
        return ValidationResult.invalid(
          'loanAmount', 
          `Loan amount must be at least $${this.minAmount.toLocaleString()}`
        );
      }
      
      if (candidate.loanAmount > this.maxAmount) {
        return ValidationResult.invalid(
          'loanAmount', 
          `Loan amount cannot exceed $${this.maxAmount.toLocaleString()}`
        );
      }
      
      return ValidationResult.valid();
    }
  }