import type { IStepViewModel } from "~/interfaces/IStepViewModel";
import { PersonalInformationViewModel } from "~/viewModels/personalInformationViewModel";
import { LoanDetailsViewModel } from "~/viewModels/loanDetailsViewModel";
import { BankInformationViewModel } from "~/viewModels/bankInformationViewModel";
import PersonalInformationView from "~/views/personalInformationView";
import LoanDetailsView from "~/views/loanDetailsView";
import BankInformationView from "~/views/bankInformationView";

type StepRegistry = {
  [key: string]: {
    viewModelClass: new () => IStepViewModel<any>;
    component: React.ComponentType<{ viewModel: IStepViewModel<any> }>;
  };
};

export const stepRegistry: StepRegistry = {
  personalInfo: {
    viewModelClass: PersonalInformationViewModel,
    component: PersonalInformationView,
  },
  loanDetails: {
    viewModelClass: LoanDetailsViewModel,
    component: LoanDetailsView,
  },
  bankInfo: {
    viewModelClass: BankInformationViewModel,
    component: BankInformationView,
  },
};