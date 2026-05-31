import { useState } from "react";
import { checkAuthentication } from "../actions/Authenticate";

export function useAuth() {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);

  checkAuthentication()
    .then(setIsAuthenticated)
    .catch(() => {
      setIsAuthenticated(false);
    });

  return { isAuthenticated };
}
