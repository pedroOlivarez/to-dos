import { post, get, postWithResult } from "../http";

type AuthenticationRequest = {
  email: string;
  password: string;
};

const route = "auth";

const authenticate = async (
  authRequest: AuthenticationRequest,
): Promise<boolean> => {
  const response = await postWithResult<AuthenticationRequest, boolean>(
    `${route}/authenticate`,
    authRequest,
  );
  if (!response.success) {
    throw new Error(response.error);
  }
  return response.data;
};

const logOut = async (): Promise<void> => {
  await post(`${route}/log-out`);
};

const checkAuthentication = async (): Promise<boolean> => {
  const response = await get(`${route}/check`);

  return response.success;
};

export {
  authenticate,
  checkAuthentication,
  logOut,
  type AuthenticationRequest,
};
