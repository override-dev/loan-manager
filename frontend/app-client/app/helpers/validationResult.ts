import type { IValidationError } from "~/interfaces/IValidationError";

export class ValidationResult {
    private _errors: IValidationError[];
  
    constructor(errors: IValidationError[] = []) {
      this._errors = errors;
    }
  
    get isValid(): boolean {
      return this._errors.length === 0;
    }
  
    get errors(): IValidationError[] {
      return [...this._errors];
    }
  
    static valid(): ValidationResult {
      return new ValidationResult();
    }
  
    static invalid(field: string, message: string): ValidationResult {
      return new ValidationResult([{ field, message }]);
    }
  
    combine(result: ValidationResult): ValidationResult {
      if (this.isValid && result.isValid) {
        return ValidationResult.valid();
      }
      
      return new ValidationResult([...this._errors, ...result.errors]);
    }
  }