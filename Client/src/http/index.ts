const url = `${import.meta.env["VITE_API_URL"]}/toDos`;

async function getMany<T>(): Promise<T[]> {
  try {
    const response = await fetch(url, {
      headers: {
        "Content-Type": "application/json",
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
    throw new Error("Error fetching data");
  }
}

async function create<Dto, T>(insert: Dto): Promise<T> {
  try {
    const response = await fetch(url, {
      method: "post",
      headers: {
        "Content-Type": "application/json",
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
    throw new Error("Error creating entity");
  }
}

async function update<Dto, T>(id: number, update: Dto): Promise<T> {
  try {
    const response = await fetch(`${url}/${id}`, {
      method: "patch",
      headers: {
        "Content-Type": "application/json",
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
    throw new Error("Error updating entity");
  }
}

async function archive(id: number): Promise<void> {
  try {
    const response = await fetch(`${url}/${id}`, {
      method: "delete",
    });

    if (response.ok) {
      return Promise.resolve();
    } else {
      return Promise.reject();
    }
  } catch (err) {
    console.error(err);
    throw new Error("Error updating entity");
  }
}

export { create, getMany, update, archive };
