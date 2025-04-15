import { useEffect, useState, type FC } from "react";
import type { IStepViewModel } from "~/interfaces/IStepViewModel";
import type { IValidationError } from "~/interfaces/IValidationError";
import { PersonalInformation } from "~/models/perfonalInformation";

interface PersonalInformationViewProps {
  viewModel: IStepViewModel<PersonalInformation>;
}

const PersonalInformationView: FC<PersonalInformationViewProps> = ({ viewModel }) => {
  const [personalInfo, setPersonalInfo] = useState<PersonalInformation>(new PersonalInformation());
  const [errors, setErrors] = useState<IValidationError[]>([]);

  useEffect(() => {
    console.log("Setting up subscriptions");
    
    // Subscribe to data changes
    const dataSubscription = viewModel.data$.subscribe((info) => {
      console.log("Data updated from observable:", info);
      setPersonalInfo(info);
    });
    
    // Subscribe to validation errors
    const errorsSubscription = viewModel.errors$.subscribe((validationErrors) => {
      console.log("Errors received from observable:", validationErrors);
      setErrors(validationErrors);
    });

    // Force an initial validation
    if (typeof viewModel.validate === 'function') {
      console.log("Calling initial validation");
      viewModel.validate();
    }

    return () => {
      console.log("Cleaning up subscriptions");
      dataSubscription.unsubscribe();
      errorsSubscription.unsubscribe();
    };
  }, [viewModel]);

  // Helper function to get error for a specific field
  const getFieldError = (fieldName: string): string | null => {
    console.log(`Checking errors for ${fieldName}:`, errors);
    const error = errors.find(e => e.field === fieldName);
    return error ? error.message : null;
  };

  const handleFullNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    console.log("Updating fullName:", e.target.value);
    viewModel.updateData({ fullName: e.target.value });
  };

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    console.log("Updating email:", e.target.value);
    viewModel.updateData({ email: e.target.value });
  };

  const handleDateOfBirthChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    console.log("Updating dateOfBirth:", e.target.value);
    viewModel.updateData({ dateOfBirth: e.target.value });
  };

  console.log("Rendering with errors:", errors);

  return (
    <div className="space-y-4">
      <h2 className="text-xl font-semibold">Personal Information</h2>
      <div className="form-control">
        <input
          type="text"
          placeholder="Full Name"
          value={personalInfo.fullName}
          onChange={handleFullNameChange}
          className={`input input-bordered w-full ${getFieldError('fullName') ? 'border-red-500' : ''}`}
        />
        {getFieldError('fullName') && (
          <div className="text-red-500 text-sm mt-1">{getFieldError('fullName')}</div>
        )}
      </div>
      <div className="form-control">
        <input
          type="email"
          placeholder="Email"
          value={personalInfo.email}
          onChange={handleEmailChange}
          className={`input input-bordered w-full ${getFieldError('email') ? 'border-red-500' : ''}`}
        />
        {getFieldError('email') && (
          <div className="text-red-500 text-sm mt-1">{getFieldError('email')}</div>
        )}
      </div>
      <div className="form-control">
        <input
          type="date"
          placeholder="Date of Birth"
          value={personalInfo.dateOfBirth}
          onChange={handleDateOfBirthChange}
          className={`input input-bordered w-full ${getFieldError('dateOfBirth') ? 'border-red-500' : ''}`}
        />
        {getFieldError('dateOfBirth') && (
          <div className="text-red-500 text-sm mt-1">{getFieldError('dateOfBirth')}</div>
        )}
      </div>
      
    </div>
  );
};

export default PersonalInformationView;