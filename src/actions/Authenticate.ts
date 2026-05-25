import { post } from '../http';

type AuthenticationRequest = {
   email: string;
   password: string;
};

const authenticate = async (authRequest: AuthenticationRequest): Promise<string> => {
   return await post<AuthenticationRequest, string>(authRequest);
};

export { authenticate, type AuthenticationRequest };
