import type { PersonalInformation } from "~/models/perfonalInformation";
import { Specification } from "../Specification";
import { ValidationResult } from "~/helpers/validationResult";

export class DateOfBirthSpecification extends Specification<PersonalInformation> {
    constructor(private minAge: number = 18, private maxAge: number = 120) {
      super();
    }
  
    isSatisfiedBy(candidate: PersonalInformation): boolean {
      if (!candidate.dateOfBirth || candidate.dateOfBirth.length === 0) {
        return false;
      }
  
      const birthDate = new Date(candidate.dateOfBirth);
      if (isNaN(birthDate.getTime())) {
        return false;
      }
  
      const today = new Date();
      if (birthDate >= today) {
        return false;
      }
  
      let age = today.getFullYear() - birthDate.getFullYear();
      const monthDiff = today.getMonth() - birthDate.getMonth();
      
      if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }
  
      return age >= this.minAge && age <= this.maxAge;
    }
  
    check(candidate: PersonalInformation): ValidationResult {
      if (!candidate.dateOfBirth || candidate.dateOfBirth.length === 0) {
        return ValidationResult.invalid('dateOfBirth', 'Date of birth is required');
      }
    
      const birthDate = new Date(candidate.dateOfBirth);
      if (isNaN(birthDate.getTime())) {
        return ValidationResult.invalid('dateOfBirth', 'Please enter a valid date');
      }
  
      const today = new Date();
      if (birthDate >= today) {
        return ValidationResult.invalid('dateOfBirth', 'Date of birth must be in the past');
      }
  
      let age = today.getFullYear() - birthDate.getFullYear();
      const monthDiff = today.getMonth() - birthDate.getMonth();

      if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }
  
      if (age < this.minAge) {
        return ValidationResult.invalid('dateOfBirth', `You must be at least ${this.minAge} years old`);
      }
      
      if (age > this.maxAge) {
        return ValidationResult.invalid('dateOfBirth', 'Please enter a valid date of birth');
      }
      
      return ValidationResult.valid();
    }
  }