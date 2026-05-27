import { useState } from "react";
import { checkAuthentication } from "../../actions/Authenticate";

export function useAuth() {
  // default to true. Data will not render regardless and if we default to false,
  // hook gets called and immediately navigates away. By the time response comes back, protected route has dismounted and user is back on landing page infinitely
  const [isAuthenticated, setIsAuthenticated] = useState(true);

  checkAuthentication()
    .then(setIsAuthenticated)
    .catch(() => {
      setIsAuthenticated(false);
    });

  return { isAuthenticated };
}
