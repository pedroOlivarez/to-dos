import { redirect } from "react-router";
import { checkAuthentication } from "../actions/Authenticate";

async function authMiddleware() {
  const isAuthenticated = await checkAuthentication();
  if (!isAuthenticated) throw redirect("/login");
}

export { authMiddleware };
