import { useEffect, type ReactNode } from "react";
import { useNavigate } from "react-router";
import { useAuth } from "./hooks";

export function ProtectedRoute({ children }: { children: ReactNode }) {
  const navigate = useNavigate();

  const { isAuthenticated } = useAuth();

  useEffect(() => {
    // null is falsey. This would immediately navigate away and never allow children to render because component would dismount before auth result comes back
    if (isAuthenticated === false) {
      navigate("/login");
    }
  }, [isAuthenticated, navigate]);

  // null remains falsey. This handles not rendering anything before auth result comes back
  if (!isAuthenticated) {
    return null;
  }

  return <>{children}</>;
}
