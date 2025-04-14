import { useEffect, useState, type FC } from "react";
import { Subscription } from "rxjs";
import type { IStepViewModel } from "~/interfaces/IStepViewModel";
import { PersonalInformation } from "~/models/perfonalInformation";

interface PersonalInformationViewProps {
  viewModel: IStepViewModel<PersonalInformation>;
}

const PersonalInformationView: FC<PersonalInformationViewProps> = ({ viewModel }) => {
  const [personalInfo, setPersonalInfo] = useState<PersonalInformation>(new PersonalInformation());

  let subscription: Subscription | null = null;

  useEffect(() => {
    subscription = viewModel.data$.subscribe((info) => {
      setPersonalInfo(info);
    });

    return () => {
      if (subscription) {
        subscription.unsubscribe();
      }
    };
  }, [viewModel]);

  const handleFullNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    viewModel.updateData({ fullName: e.target.value });
  };

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    viewModel.updateData({ email: e.target.value });
  };

  const handleDateOfBirthChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    viewModel.updateData({ dateOfBirth: e.target.value });
  };

  return (
    <div className="space-y-4">
      <h2 className="text-xl font-semibold">Personal Information</h2>
      <div className="form-control">
        <input
          type="text"
          placeholder="Full Name"
          value={personalInfo.fullName}
          onChange={handleFullNameChange}
          className="input input-bordered w-full"
        />
      </div>
      <div className="form-control">
        <input
          type="email"
          placeholder="Email"
          value={personalInfo.email}
          onChange={handleEmailChange}
          className="input input-bordered w-full"
        />
      </div>
      <div className="form-control">
        <input
          type="date"
          placeholder="Date of Birth"
          value={personalInfo.dateOfBirth}
          onChange={handleDateOfBirthChange}
          className="input input-bordered w-full"
        />
      </div>
    </div>
  );
};

export default PersonalInformationView;