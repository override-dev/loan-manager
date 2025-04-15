import type { ValidationResult } from "~/helpers/validationResult";

export interface ISpecification<T> {
    isSatisfiedBy(candidate: T): boolean;
    check(candidate: T): ValidationResult;
    and(other: ISpecification<T>): ISpecification<T>;
    or(other: ISpecification<T>): ISpecification<T>;
    not(): ISpecification<T>;
  }