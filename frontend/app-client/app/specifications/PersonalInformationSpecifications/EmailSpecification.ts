import type { PersonalInformation } from "~/models/perfonalInformation";
import { Specification } from "../Specification";
import { ValidationResult } from "~/helpers/validationResult";

export class EmailSpecification extends Specification<PersonalInformation> {
    isSatisfiedBy(candidate: PersonalInformation): boolean {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return candidate.email.length > 0 && emailRegex.test(candidate.email);
    }
  
    check(candidate: PersonalInformation): ValidationResult {
      if (!candidate.email || candidate.email.length === 0) {
        return ValidationResult.invalid('email', 'Email is required');
      }
      
      if (!candidate.email.includes('@')) {
        return ValidationResult.invalid('email', 'Email must include @ symbol');
      }
      
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(candidate.email)) {
        return ValidationResult.invalid('email', 'Please enter a valid email address');
      }
      
      return ValidationResult.valid();
    }
  }