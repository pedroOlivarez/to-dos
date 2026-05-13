import { getMany, create, update, archive } from "../http";

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
  return await getMany<ToDo>();
};

const createToDo = async (toDo: InsertToDo): Promise<ToDo> => {
  const created = await create<InsertToDo, ToDo>(toDo);
  return {
    ...created,
    updatedAt: new Date(created.updatedAt),
  };
};

const updateToDo = async (id: number, toDo: UpdateToDo): Promise<ToDo> => {
  const updated = await update<UpdateToDo, ToDo>(id, toDo);
  return {
    ...updated,
    updatedAt: new Date(updated.updatedAt),
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
