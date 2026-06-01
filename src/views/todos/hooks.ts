import { useMemo, useState } from "react";
import { useToDos } from "../../hooks/useToDos";
import {
  archiveToDo,
  createToDo,
  updateToDo,
  type InsertToDo,
  type UpdateToDo,
} from "../../actions/ToDo";
import { useToDo } from "../../hooks/useToDo";
import {
  parseAsInteger,
  parseAsString,
  parseAsStringEnum,
  useQueryState,
} from "nuqs";
import { TODO_MODALS } from "../../libs/types.ts";

export function useToDosView() {
  const [searchTerm, setSearchTerm] = useQueryState("q", parseAsString);
  const [page, setPage] = useQueryState("page", parseAsInteger);
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const [_, setModal] = useQueryState(
    "modal",
    parseAsStringEnum(Object.values(TODO_MODALS)),
  );
  const [toDoId, setToDoId] = useQueryState("id", parseAsInteger);
  const [lastUpdated, setLastUpdated] = useState<string>(
    new Date().toISOString(),
  );
  const {
    data: toDos,
    isLoading: toDosLoading,
    isError: toDosError,
  } = useToDos({
    lastUpdated,
    page: page ? page.toString() : undefined,
    searchTerm: searchTerm ?? undefined,
  });
  const {
    data: toDo,
    isLoading: toDoLoading,
    isError: toDoError,
  } = useToDo(toDoId);

  const showPagination = useMemo(
    () => toDos && toDos.meta && toDos.meta.totalPages > 1,
    [toDos],
  );

  const handleSearch = (term: string | null) => {
    setSearchTerm(term, {
      shallow: true,
    });
  };
  const handlePageSelect = (page: number) => {
    setPage(page, {
      shallow: true,
    });
  };
  const handleAddToDoDialogOpenChange = (newOpen: boolean) => {
    if (!newOpen) {
      setModal(null);
    }
  };
  const handleEditToDoDialogOpenChange = (newOpen: boolean) => {
    if (!newOpen) {
      setToDoId(null);
      setModal(null);
    }
  };
  const handleSelectToDo = (id: number) => {
    setToDoId(id);
    setModal(TODO_MODALS.EDIT);
  };

  const handleUpdate = async (id: number, data: UpdateToDo) => {
    const updated = await updateToDo(id, data);
    setLastUpdated(updated.updatedAt.toISOString());
    setToDoId(null);
    setModal(null);
  };

  const handleCreate = async (data: InsertToDo) => {
    const created = await createToDo(data);
    setLastUpdated(created.updatedAt.toISOString());
    setModal(null);
  };

  const handleArchive = async (id: number) => {
    await archiveToDo(id);
    setLastUpdated(new Date().toISOString());
    setModal(null);
  };

  return {
    toDos,
    toDosLoading,
    toDosError,
    toDo,
    toDoLoading,
    toDoError,
    showPagination,
    handleSearch,
    handlePageSelect,
    handleAddToDoDialogOpenChange,
    handleEditToDoDialogOpenChange,
    handleSelectToDo,
    handleCreate,
    handleUpdate,
    handleArchive,
  };
}
