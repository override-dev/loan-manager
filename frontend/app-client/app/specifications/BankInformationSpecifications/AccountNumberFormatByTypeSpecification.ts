import { ValidationResult } from "~/helpers/validationResult";
import type { BankInformation } from "~/models/bankInformation";
import { Specification } from "../Specification";

/**
 * Special specification that checks if the account number format is valid 
 * based on the account type selected
 */
export class AccountNumberFormatByTypeSpecification extends Specification<BankInformation> {
    isSatisfiedBy(candidate: BankInformation): boolean {
      // Different formats for different account types
      switch(candidate.accountType.toLowerCase()) {
        case 'checking':
          // Example: Checking accounts must start with 4 or 5
          return /^[45]\d+$/.test(candidate.accountNumber);
        case 'savings':
          // Example: Savings accounts must start with 1 or 2
          return /^[12]\d+$/.test(candidate.accountNumber);
        case 'credit':
          // Example: Credit accounts must start with 9
          return /^9\d+$/.test(candidate.accountNumber);
        default:
          return true; // No specific validation for unknown types
      }
    }
  
    check(candidate: BankInformation): ValidationResult {
      if (!candidate.accountNumber || !candidate.accountType) {
        return ValidationResult.valid(); // Skip this validation if either field is empty
      }
      
      switch(candidate.accountType.toLowerCase()) {
        case 'checking':
          if (!/^[45]\d+$/.test(candidate.accountNumber)) {
            return ValidationResult.invalid(
              'accountNumber', 
              'Checking account numbers must start with 4 or 5'
            );
          }
          break;
        case 'savings':
          if (!/^[12]\d+$/.test(candidate.accountNumber)) {
            return ValidationResult.invalid(
              'accountNumber', 
              'Savings account numbers must start with 1 or 2'
            );
          }
          break;
        case 'credit':
          if (!/^9\d+$/.test(candidate.accountNumber)) {
            return ValidationResult.invalid(
              'accountNumber', 
              'Credit account numbers must start with 9'
            );
          }
          break;
      }
      
      return ValidationResult.valid();
    }
  }