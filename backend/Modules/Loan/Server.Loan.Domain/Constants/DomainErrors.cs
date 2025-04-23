namespace Server.Loan.Domain.Constants;

internal class DomainErrors
{
    public class Loan
    {
        public const string LOAN_NOT_FOUND = "LOAN_NOT_FOUND";
        public const string LOAN_ALREADY_EXISTS = "LOAN_ALREADY_EXISTS";
        public const string LOAN_AMOUNT_INVALID = "LOAN_AMOUNT_INVALID";
        public const string LOAN_TERM_INVALID = "LOAN_TERM_INVALID";
        public const string LOAN_PURPOSE_INVALID = "LOAN_PURPOSE_INVALID";
        public const string LOAN_STATUS_INVALID = "LOAN_STATUS_INVALID";
        public const string LOAN_DRAFT_ID_INVALID = "LOAN_DRAFT_ID_INVALID";
        public const string LOAN_DRAFT_ID_SAME_AS_LOAN_ID = "LOAN_DRAFT_ID_SAME_AS_LOAN_ID";
    }

    public class BankInformation
    {
        public const string BANK_NAME_REQUIRED = "BANK_NAME_REQUIRED";
        public const string ACCOUNT_TYPE_REQUIRED = "ACCOUNT_TYPE_REQUIRED";
        public const string ACCOUNT_NUMBER_REQUIRED = "ACCOUNT_NUMBER_REQUIRED";
    }

    public class PersonalInformation
    {
        public const string FULL_NAME_REQUIRED = "FULL_NAME_REQUIRED";
        public const string EMAIL_REQUIRED = "EMAIL_REQUIRED";
        public const string DATE_OF_BIRTH_REQUIRED = "DATE_OF_BIRTH_REQUIRED";
    }
}
