using Bff.EndPoints.Loan.Submit;

namespace Bff.Models;

internal record LoanSubmissionRequest(
                              string LoanId,
                              int LoanAmount,
                              int LoanTerm,
                              int LoanPurpose,
                              BankInfo BankInformation,
                              PersonalInfo PersonalInformation);
