import  { ValidationResult } from '~/helpers/validationResult';
import type  { ISpecification } from './ISpecification';

export abstract class Specification<T> implements ISpecification<T> {
  abstract isSatisfiedBy(candidate: T): boolean;
  abstract check(candidate: T): ValidationResult;

  and(other: ISpecification<T>): ISpecification<T> {
    return new AndSpecification<T>(this, other);
  }

  or(other: ISpecification<T>): ISpecification<T> {
    return new OrSpecification<T>(this, other);
  }

  not(): ISpecification<T> {
    return new NotSpecification<T>(this);
  }
}

class OrSpecification<T> extends Specification<T> {
    constructor(private left: ISpecification<T>, private right: ISpecification<T>) {
      super();
    }
  
    isSatisfiedBy(candidate: T): boolean {
      return this.left.isSatisfiedBy(candidate) || this.right.isSatisfiedBy(candidate);
    }
  
    check(candidate: T): ValidationResult {

      if (this.left.isSatisfiedBy(candidate) || this.right.isSatisfiedBy(candidate)) {
        return ValidationResult.valid();
      }
      
      const leftResult = this.left.check(candidate);
      const rightResult = this.right.check(candidate);
      
      return leftResult.combine(rightResult);
    }
  }
  
  class NotSpecification<T> extends Specification<T> {
    constructor(private specification: ISpecification<T>) {
      super();
    }
  
    isSatisfiedBy(candidate: T): boolean {
      return !this.specification.isSatisfiedBy(candidate);
    }
  
    check(candidate: T): ValidationResult {
      if (this.isSatisfiedBy(candidate)) {
        return ValidationResult.valid();
      }
      
      return ValidationResult.invalid("validation", "The condition should not be satisfied");
    }
  }

  class AndSpecification<T> extends Specification<T> {
    constructor(private left: ISpecification<T>, private right: ISpecification<T>) {
      super();
    }
  
    isSatisfiedBy(candidate: T): boolean {
      return this.left.isSatisfiedBy(candidate) && this.right.isSatisfiedBy(candidate);
    }
  
    check(candidate: T): ValidationResult {
      const leftResult = this.left.check(candidate);
      const rightResult = this.right.check(candidate);
      
      return leftResult.combine(rightResult);
    }
  }