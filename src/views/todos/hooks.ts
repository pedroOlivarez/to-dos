import { useState } from "react";
import { useToDos } from "../../hooks/useToDos";
import {
  archiveToDo,
  createToDo,
  updateToDo,
  type InsertToDo,
  type ToDo,
  type UpdateToDo,
} from "../../actions/ToDo";

type Modal = "ADD_TODO" | "EDIT_TODO";

const TODO_MODALS: Record<string, Modal> = {
  ADD: "ADD_TODO",
  EDIT: "EDIT_TODO",
};

export function useToDosView({ page }: { page: string }) {
  const [modal, setModal] = useState<Modal | null>(null);
  const [lastUpdated, setLastUpdated] = useState<string>(
    new Date().toISOString(),
  );
  const [selectedToDo, setSelectedToDo] = useState<ToDo | null>(null);
  const { data, isLoading, isError } = useToDos(lastUpdated, page);

  const handleAddToDoButtonClick = () => setModal(TODO_MODALS.ADD);

  const handleSelectToDo = (id: number) => {
    const toDo = data?.data.find((t) => t.id === id);
    if (toDo) {
      setSelectedToDo(toDo);
      setModal(TODO_MODALS.EDIT);
    }
  };

  const handleAddToDoDialogOpenChange = (newOpen: boolean) => {
    if (!newOpen) {
      setModal(null);
    }
  };

  const handleEditToDoDialogOpenChange = (newOpen: boolean) => {
    if (!newOpen) {
      setModal(null);
      setSelectedToDo(null);
    }
  };

  const handleUpdate = async (id: number, data: UpdateToDo) => {
    const updated = await updateToDo(id, data);
    setLastUpdated(updated.updatedAt.toISOString());
    setModal(null);
    setSelectedToDo(null);
  };

  const handleCreate = async (data: InsertToDo) => {
    const created = await createToDo(data);
    setModal(null);
    setLastUpdated(created.updatedAt.toISOString());
  };

  const handleArchive = async (id: number) => {
    await archiveToDo(id);
    setModal(null);
    setSelectedToDo(null);
    setLastUpdated(new Date().toISOString());
  };

  return {
    modal,
    selectedToDo,
    data,
    isLoading,
    isError,
    handleAddToDoButtonClick,
    handleSelectToDo,
    handleAddToDoDialogOpenChange,
    handleEditToDoDialogOpenChange,
    handleUpdate,
    handleCreate,
    handleArchive,
  };
}
