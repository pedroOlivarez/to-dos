import { createBrowserRouter } from "react-router";
import { ToDos } from "./views/todos/ToDos";
import { Login } from "./views/login/Login";
import { NotFound } from "./views/boundaries/NotFound";
import { ErrorBoundary } from "./views/boundaries/ErrorBoundary";
import App from "./App";
import { authMiddleware } from "./middlewares/authMiddleware";

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
        middleware: [authMiddleware],
        ErrorBoundary,
      },
    ],
  },
  {
    path: "*",
    Component: NotFound,
    middleware: [authMiddleware],
  },
]);

export { router };
