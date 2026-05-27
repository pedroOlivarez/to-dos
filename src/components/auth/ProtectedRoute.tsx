import { useEffect, type ReactNode } from "react";
import { useNavigate } from "react-router";
import { useAuth } from "./hooks";

export function ProtectedRoute({ children }: { children: ReactNode }) {
  const navigate = useNavigate();

  const { isAuthenticated } = useAuth();

  useEffect(() => {
    if (isAuthenticated === false) {
      navigate("/login");
    }
  }, [isAuthenticated]);

  // null is falsey. This handles not rendering anything before auth result comes back
  if (!isAuthenticated) {
    return null;
  }

  return <>{children}</>;
}
