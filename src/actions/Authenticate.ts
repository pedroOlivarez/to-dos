import { post } from "../http";

type AuthenticationRequest = {
  email: string;
  password: string;
};

type AuthenticationResponse = {
  token: string;
};

const route = "auth";

const authenticate = async (
  authRequest: AuthenticationRequest,
): Promise<AuthenticationResponse> => {
  return await post<AuthenticationRequest, AuthenticationResponse>(
    `${route}/authenticate`,
    authRequest,
  );
};

export { authenticate, type AuthenticationRequest };
