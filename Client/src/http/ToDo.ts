const url = `${import.meta.env["VITE_API_URL"]}/toDos/`;

type InsertToDo = {
  title: string;
  body?: string;
};

type ToDo = InsertToDo & {
  id: number;
  updatedAt: Date;
};

const getMany = async (): Promise<ToDo[]> => {
  try {
    const response = await fetch(url, {
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (response.ok) {
      const responseBody = await response.json();
      return responseBody.data as ToDo[];
    } else {
      return Promise.reject();
    }
  } catch (err) {
    console.error(err);
    throw new Error("Error fetching data");
  }
};

const create = async (toDo: InsertToDo): Promise<ToDo> => {
  try {
    const response = await fetch(url, {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(toDo),
    });

    if (response.ok) {
      const todo: ToDo = await response.json();
      return {
        ...todo,
        updatedAt: new Date(todo.updatedAt),
      };
    } else {
      return Promise.reject();
    }
  } catch (err) {
    console.error(err);
    throw new Error("Error creating ToDo");
  }
};

export { create, getMany, type InsertToDo, type ToDo };
