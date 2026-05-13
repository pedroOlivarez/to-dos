import { Layout } from "./components/ui/Layout";
import { ToDos } from "./components/ToDos";

function App() {
  return (
    <section className="flex h-full flex-col bg-gray-500">
      <Layout>
        <ToDos />
      </Layout>
    </section>
  );
}

export default App;
