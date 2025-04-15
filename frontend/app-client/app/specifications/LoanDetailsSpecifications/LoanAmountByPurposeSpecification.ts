import { ValidationResult } from "~/helpers/validationResult";
import type { LoanDetails } from "~/models/loanDetails";
import { Specification } from "../Specification";

/**
 * Specification for validating loan amount based on the purpose
 * Different purposes may have different maximum amounts
 */
export class LoanAmountByPurposeSpecification extends Specification<LoanDetails> {
    constructor() {
      super();
    }
  
    isSatisfiedBy(candidate: LoanDetails): boolean {
      switch (candidate.loanPurpose) {
        case 1: // Personal
          return candidate.loanAmount <= 50000;
        case 2: // Business
          return candidate.loanAmount <= 100000;
        case 3: // Education
          return candidate.loanAmount <= 30000;
        default:
          return true;
      }
    }
  
    check(candidate: LoanDetails): ValidationResult {
      if (!candidate.loanPurpose || !candidate.loanAmount) {
        return ValidationResult.valid(); // Skip this validation if either field is not set
      }
      
      switch (candidate.loanPurpose) {
        case 1: // Personal
          if (candidate.loanAmount > 50000) {
            return ValidationResult.invalid(
              'loanAmount', 
              'Personal loans cannot exceed $50,000'
            );
          }
          break;
        case 2: // Business
          if (candidate.loanAmount > 100000) {
            return ValidationResult.invalid(
              'loanAmount', 
              'Business loans cannot exceed $100,000'
            );
          }
          break;
        case 3: // Education
          if (candidate.loanAmount > 30000) {
            return ValidationResult.invalid(
              'loanAmount', 
              'Education loans cannot exceed $30,000'
            );
          }
          break;
      }
      
      return ValidationResult.valid();
    }
  }