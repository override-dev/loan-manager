import { ValidationResult } from "~/helpers/validationResult";
import type { LoanDetails } from "~/models/loanDetails";
import { Specification } from "../Specification";

/**
 * Specification for validating loan term
 */
export class LoanTermSpecification extends Specification<LoanDetails> {
    constructor(private validTerms: number[] = [12, 24, 36]) {
      super();
    }
  
    isSatisfiedBy(candidate: LoanDetails): boolean {
      return this.validTerms.includes(candidate.loanTerm);
    }
  
    check(candidate: LoanDetails): ValidationResult {
      if (!candidate.loanTerm || candidate.loanTerm === 0) {
        return ValidationResult.invalid('loanTerm', 'Please select a loan term');
      }
      
      if (!this.validTerms.includes(candidate.loanTerm)) {
        return ValidationResult.invalid('loanTerm', 'Please select a valid loan term');
      }
      
      return ValidationResult.valid();
    }
  }