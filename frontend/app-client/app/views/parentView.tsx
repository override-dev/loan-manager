import React, { useEffect, useRef, useState } from "react";
import { Subscription } from "rxjs";
import { stepRegistry } from "~/helpers/stepRegistry";
import { ParentViewModel } from "~/viewModels/parentViewModel";


const ParentView = () => {
  // Crear una única instancia del ParentViewModel
  const parentViewModelRef = useRef<ParentViewModel | null>(null);
  if (!parentViewModelRef.current) {
    parentViewModelRef.current = new ParentViewModel();
  }
  const parentViewModel = parentViewModelRef.current;

  // Estado local para el paso actual
  const [currentStep, setCurrentStep] = useState(0);

  let subscription: Subscription | null = null;

  useEffect(() => {
    // Suscribirse al observable del ParentViewModel para obtener el paso actual
    subscription = parentViewModel.currentStep$.subscribe((step) => {
      setCurrentStep(step);
    });

    return () => {
      if (subscription) {
        subscription.unsubscribe(); // Limpiar la suscripción al desmontar el componente
      }
    };
  }, []);

  // Obtener la vista y el ViewModel correspondientes al paso actual
  const currentEntry = Object.values(stepRegistry)[currentStep];
  const CurrentComponent = currentEntry.component;
  const currentViewModel = parentViewModel.getViewModel(currentStep);

  if (!currentViewModel) {
    return <div>Loading...</div>;
  }

  return (
    <div className="min-h-screen bg-base-100">
      <div className="max-w-2xl mx-auto p-4 md:p-8">
        <div className="card bg-base-200 shadow-xl">
          <div className="card-body">
            <h1 className="text-2xl font-bold mb-6">Loan Application Wizard</h1>

            {/* Barra de progreso */}
            <ul className="steps steps-horizontal w-full mb-8">
              {[0, 1, 2].map((step) => (
                <li
                  key={step}
                  className={`step ${currentStep >= step ? "step-primary" : ""}`}
                >
                  {["Personal", "Loan", "Bank"][step]}
                </li>
              ))}
            </ul>

            {/* Contenido del paso actual */}
            <CurrentComponent viewModel={currentViewModel} />

            {/* Botones de navegación */}
            <div className="flex justify-between mt-6">
              {currentStep > 0 && (
                <button
                  type="button"
                  onClick={() => parentViewModel.previousStep()}
                  className="btn btn-outline"
                >
                  Previous
                </button>
              )}
              {currentStep < 2 ? (
                <button
                  type="button"
                  onClick={() => parentViewModel.nextStep()}
                  className="btn btn-primary ml-auto"
                >
                  Next
                </button>
              ) : (
                <button
                  type="submit"
                  className="btn btn-primary ml-auto"
                >
                  Submit Application
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ParentView;