const url = `${import.meta.env["VITE_API_URL"]}`;

const headers = {
  "Content-Type": "application/json",
};

async function get<T>(route: string): Promise<T[]> {
  try {
    const response = await fetch(`${url}/${route}`, {
      headers,
    });

    if (response.ok) {
      const responseBody = await response.json();
      return responseBody.data as T[];
    } else {
      return Promise.reject();
    }
  } catch (err) {
    console.error(err);
    throw new Error("Error fetching data", { cause: err });
  }
}

async function post<Dto, T>(
  route: string,
  insert: Dto,
  returnsObj = true,
): Promise<T> {
  try {
    const response = await fetch(`${url}/${route}`, {
      method: "post",
      headers,
      body: JSON.stringify(insert),
    });

    if (response.ok) {
      if (returnsObj) {
        const model: T = await response.json();
        return model;
      } else return response.body as T;
    } else {
      return Promise.reject();
    }
  } catch (err) {
    console.error(err);
    throw new Error("Error creating entity", { cause: err });
  }
}

async function patch<Dto, T>(
  route: string,
  id: number,
  update: Dto,
  returnsObj = true,
): Promise<T> {
  try {
    const response = await fetch(`${url}/${route}/${id}`, {
      method: "patch",
      headers,
      body: JSON.stringify(update),
    });

    if (response.ok) {
      if (returnsObj) {
        const model: T = await response.json();
        return model;
      } else return response.body as T;
    } else {
      return Promise.reject();
    }
  } catch (err) {
    console.error(err);
    throw new Error("Error updating entity", { cause: err });
  }
}

async function del(route: string, id: number): Promise<void> {
  try {
    const response = await fetch(`${url}/${route}/${id}`, {
      method: "delete",
    });

    if (response.ok) {
      return Promise.resolve();
    } else {
      return Promise.reject();
    }
  } catch (err) {
    console.error(err);
    throw new Error("Error updating entity", { cause: err });
  }
}

export { post, get, patch, del };
