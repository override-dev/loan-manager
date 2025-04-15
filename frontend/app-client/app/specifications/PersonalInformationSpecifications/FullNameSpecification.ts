import type { PersonalInformation } from "~/models/perfonalInformation";
import { Specification } from "../Specification";
import { ValidationResult } from "~/helpers/validationResult";

export class FullNameSpecification extends Specification<PersonalInformation> {
    constructor(private minLength: number = 3) {
      super();
    }
  
    isSatisfiedBy(candidate: PersonalInformation): boolean {
      return candidate.fullName.length > 0 && candidate.fullName.length >= this.minLength;
    }
  
    check(candidate: PersonalInformation): ValidationResult {
      if (!candidate.fullName || candidate.fullName.length === 0) {
        return ValidationResult.invalid('fullName', 'Full name is required');
      }
      
      if (candidate.fullName.length < this.minLength) {
        return ValidationResult.invalid('fullName', `Full name must be at least ${this.minLength} characters`);
      }
      
      return ValidationResult.valid();
    }
  }
  