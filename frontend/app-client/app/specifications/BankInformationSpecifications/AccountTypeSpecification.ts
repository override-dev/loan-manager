import { ValidationResult } from "~/helpers/validationResult";
import type { BankInformation } from "~/models/bankInformation";
import { Specification } from "../Specification";

/**
 * Specification for validating account type
 */
export class AccountTypeSpecification extends Specification<BankInformation> {
    constructor(private validTypes: string[] = ['checking', 'savings', 'credit']) {
      super();
    }
  
    isSatisfiedBy(candidate: BankInformation): boolean {
      return this.validTypes.includes(candidate.accountType.toLowerCase());
    }
  
    check(candidate: BankInformation): ValidationResult {
      if (!candidate.accountType || candidate.accountType.trim().length === 0) {
        return ValidationResult.invalid('accountType', 'Account type is required');
      }
      
      if (!this.validTypes.includes(candidate.accountType.toLowerCase())) {
        return ValidationResult.invalid(
          'accountType', 
          `Account type must be one of: ${this.validTypes.join(', ')}`
        );
      }
      
      return ValidationResult.valid();
    }
  }