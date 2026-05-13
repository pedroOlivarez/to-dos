import { getMany, create, update, archive } from "../http";
import { adjustedDate } from "../libs/utils/dateHelpers";

type InsertToDo = {
  title: string;
  body?: string;
};

type ToDo = InsertToDo & {
  id: number;
  updatedAt: Date;
};

type UpdateToDo = {
  title?: string;
  body?: string;
};

const getToDos = async (): Promise<ToDo[]> => {
  const response = await getMany<ToDo>();
  return response.map((r) => ({
    ...r,
    updatedAt: adjustedDate(new Date(r.updatedAt)),
  }));
};

const createToDo = async (toDo: InsertToDo): Promise<ToDo> => {
  const created = await create<InsertToDo, ToDo>(toDo);
  return {
    ...created,
    updatedAt: adjustedDate(new Date(created.updatedAt)),
  };
};

const updateToDo = async (id: number, toDo: UpdateToDo): Promise<ToDo> => {
  const updated = await update<UpdateToDo, ToDo>(id, toDo);
  return {
    ...updated,
    updatedAt: adjustedDate(new Date(updated.updatedAt)),
  };
};

const archiveToDo = async (id: number): Promise<void> => {
  await archive(id);
};

export {
  getToDos,
  createToDo,
  updateToDo,
  archiveToDo,
  type InsertToDo,
  type ToDo,
};
