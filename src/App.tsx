import { Layout } from "./components/ui/Layout";
import { ToDos } from "./components/todos/views/ToDos";
import { Landing } from "./components/landing/Landing";
import { HashRouter, Routes, Route } from "react-router";

function App() {
  return (
    <section className="flex h-full flex-col bg-gray-500">
      <Layout>
        <HashRouter>
          <Routes>
            <Route path="/" element={<Landing />} />
            <Route path="/to-dos" element={<ToDos />} />
          </Routes>
        </HashRouter>
      </Layout>
    </section>
  );
}

export default App;
