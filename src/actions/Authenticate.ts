import { post } from "../http";

type AuthenticationRequest = {
  email: string;
  password: string;
};

const route = "auth";

const authenticate = async (
  authRequest: AuthenticationRequest,
): Promise<boolean> => {
  const response = await post<AuthenticationRequest>(
    `${route}/authenticate`,
    authRequest,
  );

  return response.success;
};

export { authenticate, type AuthenticationRequest };
