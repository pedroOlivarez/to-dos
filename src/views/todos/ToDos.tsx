import { type ComponentProps } from "react";
import { AddToDoDialog } from "../../components/todos/dialogs/AddToDoDialog/AddToDoDialog";
import { EditToDoDialog } from "../../components/todos/dialogs/EditToDoDialog/EditToDoDialog";
import { AddToDoButton } from "../../components/todos/AddToDoButton";
import { ResultDisplay } from "../../components/todos/ResultDisplay";
import { cn } from "../../libs/utils/classNames";
import { useToDosView } from "./hooks";

export function ToDos(props: ComponentProps<"div">) {
  const {
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
  } = useToDosView();

  return (
    <>
      <div
        className={cn(
          "flex flex-col min-h-full w-full p-2 pr-18 sm:items-start  items-center",
          props.className,
        )}
      >
        <ResultDisplay
          onSelectToDo={handleSelectToDo}
          toDos={data}
          isLoading={isLoading}
          isError={isError}
        />
      </div>
      <AddToDoButton onClick={handleAddToDoButtonClick} />
      <AddToDoDialog
        open={modal === "ADD_TODO"}
        onOpenChange={handleAddToDoDialogOpenChange}
        onSubmit={handleCreate}
      />
      <EditToDoDialog
        open={modal === "EDIT_TODO"}
        onOpenChange={handleEditToDoDialogOpenChange}
        defaultValues={selectedToDo}
        onSubmit={handleUpdate}
        onArchive={handleArchive}
      />
    </>
  );
}
