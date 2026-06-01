import { Outlet, useNavigate } from "react-router";
import { Layout } from "./components/ui/Layout";
import { NuqsAdapter } from "nuqs/adapters/react";
import { useAuth } from "./hooks/useAuth";
import { useEffect } from "react";

function App() {
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    if (!isAuthenticated) {
      navigate("login");
    }
  }, [isAuthenticated, navigate]);

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
