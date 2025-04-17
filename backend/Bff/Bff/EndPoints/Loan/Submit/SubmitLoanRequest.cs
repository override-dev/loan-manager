namespace Bff.EndPoints.Loan.Submit;

internal record SubmitLoanRequest(int LoanAmount,
                                  int LoanTerm,
                                  int LoanPurpose,
                                  BankInfo BankInformation,
                                  PersonalInfo PersonalInformation);
