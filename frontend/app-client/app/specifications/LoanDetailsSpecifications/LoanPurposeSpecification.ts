import { ValidationResult } from "~/helpers/validationResult";
import type { LoanDetails } from "~/models/loanDetails";
import { Specification } from "../Specification";

/**
 * Specification for validating loan purpose
 */
export class LoanPurposeSpecification extends Specification<LoanDetails> {
    constructor(private validPurposes: number[] = [1, 2, 3]) {
      super();
    }
  
    isSatisfiedBy(candidate: LoanDetails): boolean {
      return this.validPurposes.includes(candidate.loanPurpose);
    }
  
    check(candidate: LoanDetails): ValidationResult {
      if (!candidate.loanPurpose || candidate.loanPurpose === 0) {
        return ValidationResult.invalid('loanPurpose', 'Please select a loan purpose');
      }
      
      if (!this.validPurposes.includes(candidate.loanPurpose)) {
        return ValidationResult.invalid('loanPurpose', 'Please select a valid loan purpose');
      }
      
      return ValidationResult.valid();
    }
  }