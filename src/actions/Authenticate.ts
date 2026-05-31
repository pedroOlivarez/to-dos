import { post, get } from '../http';

type AuthenticationRequest = {
   email: string;
   password: string;
};

const route = 'auth';

const authenticate = async (authRequest: AuthenticationRequest): Promise<boolean> => {
   const response = await post<AuthenticationRequest>(`${route}/authenticate`, authRequest);

   return response.success;
};

const checkAuthentication = async (): Promise<boolean> => {
   const response = await get(`${route}/check`);

   return response.success;
};

export { authenticate, checkAuthentication, type AuthenticationRequest };
