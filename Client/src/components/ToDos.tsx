import { useState, type ComponentProps } from "react";
import { PlusIcon } from "lucide-react";
import { useToDos } from "../hooks/useToDos";
import { AddToDoDialog } from "./todos/AddToDoDialog/AddToDoDialog";
import { EditToDoDialog } from "./todos/EditToDoDialog/EditToDoDialog";
import {
  archiveToDo,
  createToDo,
  updateToDo,
  type InsertToDo,
  type UpdateToDo,
} from "../actions/ToDo";
import { cn } from "../libs/utils/classNames";
import { ToDo } from "./todos/ToDo";
import { type ToDo as ToDoType } from "../actions/ToDo";

function AddToDoButton({ className, ...rest }: ComponentProps<"button">) {
  return (
    <button
      className={cn(
        "absolute flex items-center justify-center rounded-2xl p-7 bg-black/85 bottom-2 right-2 min-h-6 max-h-6 min-w-6 max-w-6",
        className,
      )}
      {...rest}
    >
      <span>
        <PlusIcon size={32} />
      </span>
    </button>
  );
}

function ResultDisplay({
  toDos,
  isLoading,
  isError,
  onSelectToDo,
}: {
  toDos: ToDoType[];
  isLoading: boolean;
  isError: boolean;
  onSelectToDo: (id: number) => void;
}) {
  return isLoading ? (
    "Fetching to-dos..."
  ) : isError ? (
    "Oops, an error occurred"
  ) : toDos && toDos.length ? (
    <div className="flex flex-row gap-2 flex-wrap">
      {toDos.map((td) => (
        <ToDo onClick={onSelectToDo} key={td.id} toDo={td} />
      ))}
    </div>
  ) : (
    <div>Nothing to show yet</div>
  );
}
type Modal = "ADD_TODO" | "EDIT_TODO";

const TODO_MODALS: Record<string, Modal> = {
  ADD: "ADD_TODO",
  EDIT: "EDIT_TODO",
};

export function ToDos(props: ComponentProps<"div">) {
  const [modal, setModal] = useState<Modal | null>(null);
  const [lastUpdated, setLastUpdated] = useState<string>(
    new Date().toISOString(),
  );
  const [selectedToDo, setSelectedToDo] = useState<ToDoType | null>(null);
  const { data, isLoading, isError } = useToDos(lastUpdated, []);

  const handleSelectToDo = (id: number) => {
    const toDo = data.find((t) => t.id === id);
    if (toDo) {
      setSelectedToDo(toDo);
      setModal(TODO_MODALS.EDIT);
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
  return (
    <>
      <div
        className={cn(
          "flex flex-col min-h-full w-full p-2 pr-18",
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
      <AddToDoButton onClick={() => setModal(TODO_MODALS.ADD)} />
      <AddToDoDialog
        open={modal === "ADD_TODO"}
        onOpenChange={(newOpen) => {
          if (!newOpen) {
            setModal(null);
          }
        }}
        onSubmit={handleCreate}
      />
      <EditToDoDialog
        open={modal === "EDIT_TODO"}
        onOpenChange={(newOpen) => {
          if (!newOpen) {
            setModal(null);
            setSelectedToDo(null);
          }
        }}
        defaultValues={selectedToDo}
        onSubmit={handleUpdate}
        onArchive={handleArchive}
      />
    </>
  );
}
