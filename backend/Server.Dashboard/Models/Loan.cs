namespace Server.Dashboard.Models;

internal record Loan(string Id,
                    int LoanAmount,
                    int LoanTerm,
                    int LoanPurpose,
                    string BankAccountNumber,
                    string BankAccountType,
                    string BankName,
                    string FullName,
                    string Email,
                    DateTime DateOfBirth,
                    int LoanStatus);
