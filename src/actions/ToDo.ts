import { patch, del, postWithResult, getWithResult } from "../http";
import { adjustedDate } from "../libs/utils/dateHelpers";

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

const getToDos = async (args?: Record<string, string>): Promise<ToDo[]> => {
  const params = args ? new URLSearchParams() : undefined;
  if (params && args) {
    for (const key in args) {
      params.append(key, args[key]);
    }
  }
  const response = await getWithResult<ToDo>(route, params);
  if (!response.success) {
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
