namespace Loan.StorageProvider.Models;

internal class LoanEntity
{
    public required string LoanId { get; set; }
    
    public int LoanAmount { get; set; }

    public int LoanTerm { get; set; }

    public int LoanPurpose { get; set; }

    public required PersonalInformationEntity PersonalInformation { get; set; }

    public required BankInformationEntity BankInformation { get; set; }

    public int LoanStatus { get; set; }

}

