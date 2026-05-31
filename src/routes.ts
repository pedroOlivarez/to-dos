import { createBrowserRouter } from "react-router";
import { ToDos } from "./views/todos/ToDos";
import { Login } from "./views/landing/Login";
import { NotFound } from "./views/boundaries/NotFound";
import { ErrorBoundary } from "./views/boundaries/ErrorBoundary";
import App from "./App";

const router = createBrowserRouter([
  {
    path: "/",
    Component: App,
    children: [
      {
        path: "login",
        Component: Login,
      },
      {
        path: "to-dos",
        Component: ToDos,
        ErrorBoundary,
      },
    ],
  },
  {
    path: "*",
    Component: NotFound,
  },
]);

export { router };
