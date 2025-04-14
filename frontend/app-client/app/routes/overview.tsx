import type { LoaderFunctionArgs } from "react-router";

export async function loader(args: LoaderFunctionArgs) {
 
  /* const authProvider = new AnonymousAuthenticationProvider(
    
  );
  const adapter = new FetchRequestAdapter(authProvider);
  adapter.baseUrl = process.env.BACKEND_URL ?? "";

  const apiClient = createMyTsClient(adapter); */

}


const Overveiw = () => {
  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">Overview</h1>
      <p>
        Welcome to the Overview page! This page provides an overview of the application.
      </p>
    </div>
  );

}
export default Overveiw;