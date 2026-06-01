import { type ComponentProps } from "react";
import { AddToDoDialog } from "../../components/todos/dialogs/AddToDoDialog/AddToDoDialog";
import { EditToDoDialog } from "../../components/todos/dialogs/EditToDoDialog/EditToDoDialog";
import { ResultDisplay } from "../../components/todos/ResultDisplay";
import { cn } from "../../libs/utils/classNames";
import { useToDosView } from "./hooks";
import { Pagination } from "../../components/ui/Pagination";
import { Search } from "../../components/ui/Search";
import { parseAsStringEnum, useQueryState } from "nuqs";
import { TODO_MODALS } from "../../libs/types/index.ts";

export function ToDos(props: ComponentProps<"div">) {
  const [modal, setModal] = useQueryState(
    "modal",
    parseAsStringEnum(Object.values(TODO_MODALS)),
  );
  const {
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
    handleUpdate,
    handleCreate,
    handleArchive,
  } = useToDosView();

  return (
    <>
      <div
        className={cn(
          "flex flex-col min-h-full w-full p-2 sm:items-start  items-center gap-2",
          props.className,
        )}
      >
        <Search
          onInput={handleSearch}
          debounceDelay={500}
          className="bg-black/65"
        />
        {showPagination ? (
          <Pagination {...toDos!.meta} onPageSelect={handlePageSelect} />
        ) : null}
        <ResultDisplay
          onSelectToDo={handleSelectToDo}
          toDos={toDos?.data ?? null}
          isLoading={toDosLoading}
          isError={toDosError}
          onAddToDoClick={() => setModal(TODO_MODALS.ADD)}
        />
      </div>
      <AddToDoDialog
        open={modal === TODO_MODALS.ADD}
        onOpenChange={handleAddToDoDialogOpenChange}
        onSubmit={handleCreate}
      />
      {!!toDo && !toDoLoading && !toDoError ? (
        <EditToDoDialog
          open={modal === TODO_MODALS.EDIT}
          onOpenChange={handleEditToDoDialogOpenChange}
          defaultValues={toDo}
          onSubmit={handleUpdate}
          onArchive={handleArchive}
        />
      ) : null}
    </>
  );
}
