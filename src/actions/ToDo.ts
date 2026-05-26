import { get, patch, del, postWithResult } from "../http";
import { adjustedDate } from "../libs/utils/dateHelpers";
import { UNAUTHENTICATED_USER } from "../libs/utils/consts";

type InsertToDo = {
  title: string;
  body?: string;
};

type ToDo = InsertToDo & {
  id: number;
  updatedAt: Date;
  completed: boolean;
};

type UpdateToDo = {
  title?: string;
  body?: string;
  completed?: boolean;
};

const route = "toDos";

const getToDos = async (): Promise<ToDo[]> => {
  const response = await get<ToDo>(route);
  if (!response.success) {
    if (response.statusCode === 401) {
      throw new Error(UNAUTHENTICATED_USER);
    }
    throw new Error(response.error);
  }
  return response.data.map((r) => ({
    ...r,
    updatedAt: adjustedDate(new Date(r.updatedAt)),
  }));
};

const createToDo = async (toDo: InsertToDo): Promise<ToDo> => {
  const response = await postWithResult<InsertToDo, ToDo>(route, toDo);
  if (!response.success) {
    if (response.statusCode === 401) {
      throw new Error(UNAUTHENTICATED_USER);
    }
    throw new Error(response.error);
  }
  return {
    ...response.data,
    updatedAt: adjustedDate(new Date(response.data.updatedAt)),
  };
};

const updateToDo = async (id: number, toDo: UpdateToDo): Promise<ToDo> => {
  const response = await patch<UpdateToDo, ToDo>(route, id, toDo);
  if (!response.success) {
    if (response.statusCode === 401) {
      throw new Error(UNAUTHENTICATED_USER);
    }
    throw new Error(response.error);
  }
  return {
    ...response.data,
    updatedAt: adjustedDate(new Date(response.data.updatedAt)),
  };
};

const archiveToDo = async (id: number): Promise<void> => {
  const response = await del(route, id);
  if (!response.success) {
    if (response.statusCode === 401) {
      throw new Error(UNAUTHENTICATED_USER);
    }
    throw new Error("Error archiving To-Do");
  }
};

export {
  archiveToDo,
  createToDo,
  getToDos,
  updateToDo,
  type InsertToDo,
  type ToDo,
  type UpdateToDo,
};
