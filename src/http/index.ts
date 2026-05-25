const url = `${import.meta.env['VITE_API_URL']}/toDos`;

async function get<T>(): Promise<T[]> {
   try {
      const response = await fetch(url, {
         headers: {
            'Content-Type': 'application/json',
         },
      });

      if (response.ok) {
         const responseBody = await response.json();
         return responseBody.data as T[];
      } else {
         return Promise.reject();
      }
   } catch (err) {
      console.error(err);
      throw new Error('Error fetching data', { cause: err });
   }
}

async function post<Dto, T>(insert: Dto): Promise<T> {
   try {
      const response = await fetch(url, {
         method: 'post',
         headers: {
            'Content-Type': 'application/json',
         },
         body: JSON.stringify(insert),
      });

      if (response.ok) {
         const model: T = await response.json();
         return model;
      } else {
         return Promise.reject();
      }
   } catch (err) {
      console.error(err);
      throw new Error('Error creating entity', { cause: err });
   }
}

async function patch<Dto, T>(id: number, update: Dto): Promise<T> {
   try {
      const response = await fetch(`${url}/${id}`, {
         method: 'patch',
         headers: {
            'Content-Type': 'application/json',
         },
         body: JSON.stringify(update),
      });

      if (response.ok) {
         const model: T = await response.json();
         return model;
      } else {
         return Promise.reject();
      }
   } catch (err) {
      console.error(err);
      throw new Error('Error updating entity', { cause: err });
   }
}

async function del(id: number): Promise<void> {
   try {
      const response = await fetch(`${url}/${id}`, {
         method: 'delete',
      });

      if (response.ok) {
         return Promise.resolve();
      } else {
         return Promise.reject();
      }
   } catch (err) {
      console.error(err);
      throw new Error('Error updating entity', { cause: err });
   }
}

export { post, get, patch, del };
