import { get, post, patch, del } from "../http";
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

const getToDos = async (): Promise<ToDo[]> => {
  const response = await get<ToDo>(route);
  return response.map((r) => ({
    ...r,
    updatedAt: adjustedDate(new Date(r.updatedAt)),
  }));
};

const createToDo = async (toDo: InsertToDo): Promise<ToDo> => {
  const created = await post<InsertToDo, ToDo>(route, toDo);
  return {
    ...created,
    updatedAt: adjustedDate(new Date(created.updatedAt)),
  };
};

const updateToDo = async (id: number, toDo: UpdateToDo): Promise<ToDo> => {
  const updated = await patch<UpdateToDo, ToDo>(route, id, toDo);
  return {
    ...updated,
    updatedAt: adjustedDate(new Date(updated.updatedAt)),
  };
};

const archiveToDo = async (id: number): Promise<void> => {
  await del(route, id);
};

export {
  getToDos,
  createToDo,
  updateToDo,
  archiveToDo,
  type InsertToDo,
  type ToDo,
  type UpdateToDo,
};
