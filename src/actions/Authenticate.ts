import { post } from "../http";

type AuthenticationRequest = {
  email: string;
  password: string;
};

const route = "auth";

const authenticate = async (
  authRequest: AuthenticationRequest,
): Promise<boolean> => {
  const response = await post<AuthenticationRequest, string>(
    `${route}/authenticate`,
    authRequest,
    false,
  );

  return response === "true";
};

export { authenticate, type AuthenticationRequest };
