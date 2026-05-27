import { useState } from "react";
import { checkAuthentication } from "../../actions/Authenticate";

export function useAuth() {
  // Default to null. Defaulting to false unmounts protected route component before response can be set
  // Defaulting to true could work but feels not great and would render a quick error (hopefully) while auth check comes back
  const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);

  checkAuthentication()
    .then(setIsAuthenticated)
    .catch(() => {
      setIsAuthenticated(false);
    });

  return { isAuthenticated };
}
