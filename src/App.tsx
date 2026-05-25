import { Layout } from './components/ui/Layout';
import { ToDos } from './views/todos/ToDos';
import { Login } from './views/landing/Login';
import { HashRouter, Routes, Route } from 'react-router';

function App() {
   return (
      <section className="flex h-full flex-col bg-gray-500">
         <Layout>
            <HashRouter>
               <Routes>
                  <Route path="/home" element={<ToDos />} />
                  <Route path="/" element={<Login />} />
               </Routes>
            </HashRouter>
         </Layout>
      </section>
   );
}

export default App;
