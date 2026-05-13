const url = `${import.meta.env["VITE_API_URL"]}/toDos/`;

type ToDo = {
  id: number;
  title: string;
  body: string;
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
      throw new Error("Server error");
    }
  } catch (err) {
    console.error(err);
    throw new Error("Error fetching data");
  }
};

export { getMany, type ToDo };
