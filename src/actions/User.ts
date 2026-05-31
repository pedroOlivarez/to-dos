import { postWithResult, type Response } from '../http';

import { type AuthenticationRequest as UserInsertDto } from './Authenticate';

type UserModel = {
   id: number;
   email: string;
};

const route = 'users';

const register = async (userRequest: UserInsertDto): Promise<Response<UserModel>> => {
   const response = await postWithResult<UserInsertDto, UserModel>(`${route}`, userRequest);

   return response;
};

export { register, type UserInsertDto };
