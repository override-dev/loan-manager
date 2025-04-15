import { type LoaderFunctionArgs } from "react-router";
import ParentView from "~/views/parentView";

export async function loader(args: LoaderFunctionArgs) {
}


interface FormData {
  step: number;
  personalInfo: {
    fullName: string;
    email: string;
    phone: string;
  };
  loanInfo: {
    amount: string;
    purpose: string;
    term: string;
  };
  bankInfo: {
    bankName: string;
    accountType: string;
    accountNumber: string;
  };
}

const Overview = () => {
  return (
    <div className="min-h-screen bg-base-100">
      <div className="max-w-2xl mx-auto p-4 md:p-8">
        <div className="card bg-base-200 shadow-xl">
          <div className="card-body">
            <h1 className="text-2xl font-bold mb-6">Loan Application</h1>
            <ParentView />
          </div>
        </div>
      </div>
    </div>
  );
};

export default Overview;