import { ValidationResult } from "~/helpers/validationResult";
import type { BankInformation } from "~/models/bankInformation";
import { Specification } from "../Specification";

export class BankNameSpecification extends Specification<BankInformation> {
    constructor(private minLength: number = 2) {
      super();
    }
  
    isSatisfiedBy(candidate: BankInformation): boolean {
      return candidate.bankName.length >= this.minLength;
    }
  
    check(candidate: BankInformation): ValidationResult {
      if (!candidate.bankName || candidate.bankName.trim().length === 0) {
        return ValidationResult.invalid('bankName', 'Bank name is required');
      }
      
      if (candidate.bankName.length < this.minLength) {
        return ValidationResult.invalid('bankName', `Bank name must be at least ${this.minLength} characters`);
      }
      
      return ValidationResult.valid();
    }
  }