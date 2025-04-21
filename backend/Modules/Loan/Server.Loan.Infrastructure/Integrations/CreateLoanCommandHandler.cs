using Ardalis.Result;
using FastEndpoints;
using Loan.StorageProvider.Models;
using Server.Loan.Application.Features.Loan.CreateLoan;
using Server.Loan.Infrastructure.Enums;
using Server.Loan.Infrastructure.Interfaces;

namespace Server.Loan.Infrastructure.Integrations
{
    internal class CreateLoanCommandHandler(ILoanRepositoryFactory loanRepositoryFactory): CommandHandler<CreateLoanCommand, Result<CreateLoanCommandResponse>>
    {
        public override async Task<Result<CreateLoanCommandResponse>> ExecuteAsync(CreateLoanCommand command, CancellationToken ct = default)
        {
            var loanDraftRepository = loanRepositoryFactory.Create(Enums.StorageType.Draft);

            var loanDraft = await loanDraftRepository.GetLoanByIdAsync(command.DraftLoanId);

            if (loanDraft is null)
            {
                return Result<CreateLoanCommandResponse>.NotFound($"Loan draft with ID {command.DraftLoanId} not found.");
            }
            // Validate personal information
            var personalInformationResult = Domain.Aggregates.Loan.Entities.PersonalInformation.Create(
                fullName: loanDraft.PersonalInformation.FullName,
                email: loanDraft.PersonalInformation.Email,
                dateOfBirth: loanDraft.PersonalInformation.DateOfBirth
            );

            // Validate bank information
            var bankInformationResult = Domain.Aggregates.Loan.Entities.BankInformation.Create(
                accountNumber: loanDraft.BankInformation.AccountNumber,
                accountType: loanDraft.BankInformation.AccountType,
                bankName: loanDraft.BankInformation.BankName
            );

            // STEP 2: Return early if validation fails
            if (!personalInformationResult.IsSuccess)
            {
                return personalInformationResult.Map();
            }

            if (!bankInformationResult.IsSuccess)
            {
                return bankInformationResult.Map();
            }

            // Extract validated entities
            var personalInformation = personalInformationResult.Value;
            var bankInformation = bankInformationResult.Value;

            // STEP 3: Create loan aggregate with validated entities
            var loanResult = Domain.Aggregates.Loan.Loan.Create(
                Id: new Domain.Aggregates.Loan.ValueObjects.LoanId(Guid.NewGuid()),
                LoanAmount: loanDraft.LoanAmount,
                LoanTerm: loanDraft.LoanTerm,
                LoanPurpose: loanDraft.LoanPurpose,
                loanStatus: Domain.Aggregates.Loan.Enums.LoanStatus.Pending,
                personalInformation: personalInformation,
                bankInformation: bankInformation);

            // STEP 4: Return early if loan creation fails
            if (!loanResult.IsSuccess)
            {
                return loanResult.Map();
            }

            // STEP 5: Persist loan to database
            var loan = loanResult.Value;

            // Trigger domain logic for loan creation
            var loanCreationResult = loan.CreateNewLoan();
            if (!loanCreationResult.IsSuccess)
            {
                return loanCreationResult.Map();
            }

            // Map domain model to persistence entity
            var loanEntity = new LoanEntity
            {
                LoanId = loan.Id.Value.ToString(),
                LoanAmount = loan.LoanAmount,
                LoanTerm = loan.LoanTerm,
                LoanPurpose = loan.LoanPurpose,
                PersonalInformation = new PersonalInformationEntity(
                    loan.PersonalInformation.FullName,
                    loan.PersonalInformation.Email,
                    loan.PersonalInformation.DateOfBirth),
                BankInformation = new BankInformationEntity(
                    loan.BankInformation.BankName,
                    loan.BankInformation.AccountType,
                    loan.BankInformation.AccountNumber),
                LoanStatus = (int)loan.LoanStatus
            };

            // Save to repository
            var loanRepository = loanRepositoryFactory.Create(StorageType.Database);
            var createdLoan = await loanRepository.CreateLoanAsync(loanEntity);

            if(createdLoan is null)
            {
                return Result<CreateLoanCommandResponse>.Error($"Failed to create loan with ID {loan.Id}");
            }
            // after the loan is created, the status is set to pending by reseting the loan status
            var resetLoanStatus = loan.Reset();

            if (!resetLoanStatus.IsSuccess)
            {
                return resetLoanStatus.Map();
            }

            return Result<CreateLoanCommandResponse>.Success(new CreateLoanCommandResponse(loanEntity.LoanId, loan.LoanStatus));
        }
    }
}
