const url = `${import.meta.env['VITE_API_URL']}`;

const headers = {
   'Content-Type': 'application/json',
};
const credentials = 'include';

type BaseResponse = {
   statusCode: number;
   success: boolean;
};

type Meta = {
   page: number;
   pageSize: number;
   count: number;
   total: number;
   totalPages: number;
   offSet: number;
   hasNext: boolean;
   hasPrevious: boolean;
};

type Response<T> =
   | (BaseResponse & {
        success: true;
        data: T;
        meta?: Meta;
     })
   | (BaseResponse & {
        success: false;
        error: string;
     });

async function get(route: string): Promise<BaseResponse> {
   try {
      const response = await fetch(`${url}/${route}`, {
         method: 'get',
         credentials,
      });
      return {
         statusCode: response.status,
         success: response.ok,
      };
   } catch (err) {
      console.error(err);
      throw new Error('Error on get request', { cause: err });
   }
}

async function getWithResult<T>(route: string, params?: URLSearchParams): Promise<Response<T[]>> {
   try {
      const response = await fetch(`${url}/${route}${params ? `?${params}` : ''}`, {
         headers,
         credentials,
      });

      if (response.ok) {
         const responseBody = await response.json();
         return {
            statusCode: response.status,
            success: true,
            data: responseBody.data as T[],
            meta: responseBody.meta ? (responseBody.meta as Meta) : undefined,
         };
      }
      return {
         statusCode: response.status,
         success: false,
         error: 'Error fetching data',
      };
   } catch (err) {
      console.error(err);
      throw new Error('Error fetching data', { cause: err });
   }
}

async function post<Dto = null>(route: string, body?: Dto): Promise<BaseResponse> {
   try {
      const response = await fetch(`${url}/${route}`, {
         method: 'post',
         headers,
         credentials,
         body: body ? JSON.stringify(body) : undefined,
      });
      return {
         statusCode: response.status,
         success: response.ok,
      };
   } catch (err) {
      console.error(err);
      throw new Error('Error making post request', { cause: err });
   }
}

async function postWithResult<Dto, T>(route: string, body: Dto): Promise<Response<T>> {
   try {
      const response = await fetch(`${url}/${route}`, {
         method: 'post',
         headers,
         credentials,
         body: JSON.stringify(body),
      });

      if (response.ok) {
         const data: T = await response.json();
         return {
            statusCode: response.status,
            success: true,
            data,
         };
      } else {
         return {
            statusCode: response.status,
            success: false,
            error: 'Error fetching data',
         };
      }
   } catch (err) {
      console.error(err);
      throw new Error('Error creating entity', { cause: err });
   }
}

async function patch<Dto, T>(route: string, id: number, update: Dto): Promise<Response<T>> {
   try {
      const response = await fetch(`${url}/${route}/${id}`, {
         method: 'patch',
         headers,
         credentials,
         body: JSON.stringify(update),
      });

      if (response.ok) {
         const data: T = await response.json();
         return {
            statusCode: response.status,
            success: true,
            data,
         };
      } else {
         return {
            statusCode: response.status,
            success: false,
            error: 'Error fetching data',
         };
      }
   } catch (err) {
      console.error(err);
      throw new Error('Error updating entity', { cause: err });
   }
}

async function del(route: string, id: number): Promise<BaseResponse> {
   try {
      const response = await fetch(`${url}/${route}/${id}`, {
         method: 'delete',
         credentials,
      });

      console.log(response);
      return {
         statusCode: response.status,
         success: response.ok,
      };
   } catch (err) {
      console.error(err);
      throw new Error('Error updating entity', { cause: err });
   }
}

export { del, get, getWithResult, post, postWithResult, patch, type Meta, type Response };
