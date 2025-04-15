import { ValidationResult } from "~/helpers/validationResult";
import type { BankInformation } from "~/models/bankInformation";
import { Specification } from "../Specification";

/**
 * Specification for validating account number format
 */
export class AccountNumberSpecification extends Specification<BankInformation> {
    constructor(private minLength: number = 5, private maxLength: number = 20) {
      super();
    }
  
    isSatisfiedBy(candidate: BankInformation): boolean {
      const accountNumber = candidate.accountNumber;
      
      // Basic validation: must be a non-empty string of appropriate length containing only digits
      return (
        accountNumber.length >= this.minLength && 
        accountNumber.length <= this.maxLength &&
        /^\d+$/.test(accountNumber)
      );
    }
  
    check(candidate: BankInformation): ValidationResult {
      if (!candidate.accountNumber || candidate.accountNumber.trim().length === 0) {
        return ValidationResult.invalid('accountNumber', 'Account number is required');
      }
      
      if (candidate.accountNumber.length < this.minLength) {
        return ValidationResult.invalid(
          'accountNumber', 
          `Account number must be at least ${this.minLength} digits`
        );
      }
      
      if (candidate.accountNumber.length > this.maxLength) {
        return ValidationResult.invalid(
          'accountNumber', 
          `Account number must not exceed ${this.maxLength} digits`
        );
      }
      
      if (!/^\d+$/.test(candidate.accountNumber)) {
        return ValidationResult.invalid(
          'accountNumber', 
          'Account number must contain only digits'
        );
      }
      
      return ValidationResult.valid();
    }
  }