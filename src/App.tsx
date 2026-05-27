import { Layout } from "./components/ui/Layout";
import { ToDos } from "./views/todos/ToDos";
import { Login } from "./views/landing/Login";
import { Routes, Route } from "react-router";
import { ProtectedRoute } from "./components/auth/ProtectedRoute";

function App() {
  return (
    <section className="flex h-full flex-col bg-gray-500">
      <Layout>
        <Routes>
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <ToDos />
              </ProtectedRoute>
            }
          />
          <Route path="/login" element={<Login />} />
        </Routes>
      </Layout>
    </section>
  );
}

export default App;
