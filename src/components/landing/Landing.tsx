import {
  GoogleLogin,
  googleLogout,
  type CredentialResponse,
} from "@react-oauth/google";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router";

export function Landing() {
  const navigate = useNavigate();

  const handleSuccess = (response: CredentialResponse) => {
    if (response && response.credential) {
      const credential = jwtDecode(response.credential);
      console.log({ credential });
      navigate("/to-dos");
    }
  };

  const handleError = () => {
    console.error("Error logging in");
  };

  // gotta add this somewhere
  const handleLogOut = () => {
    googleLogout();
  };
  return (
    <GoogleLogin onSuccess={handleSuccess} onError={handleError} auto_select />
  );
}
