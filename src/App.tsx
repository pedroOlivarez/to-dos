import { Outlet } from "react-router";
import { Layout } from "./components/ui/Layout";
import { NuqsAdapter } from "nuqs/adapters/react";

function App() {
  return (
    <section className="flex h-full flex-col bg-gray-500">
      <Layout>
        <NuqsAdapter>
          <Outlet />
        </NuqsAdapter>
      </Layout>
    </section>
  );
}

export default App;
