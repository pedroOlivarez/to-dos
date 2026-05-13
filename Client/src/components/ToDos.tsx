import { useState, type ComponentProps } from "react";
import { PlusIcon } from "lucide-react";
import { useToDos } from "../hooks/useToDos";
import { AddEditToDoDialog } from "./todos/AddEditToDoDialog";
import { createToDo, type InsertToDo } from "../actions/ToDo";
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
}: {
  toDos: ToDoType[];
  isLoading: boolean;
  isError: boolean;
}) {
  return isLoading ? (
    "Fetching to-dos..."
  ) : isError ? (
    "Oops, an error occurred"
  ) : toDos && toDos.length ? (
    <div className="flex flex-row gap-2 flex-wrap">
      {toDos.map((td) => (
        <ToDo key={td.id} toDo={td} />
      ))}
    </div>
  ) : null;
}

export function ToDos(props: ComponentProps<"div">) {
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const [lastUpdated, setLastUpdated] = useState<string>(
    new Date().toDateString(),
  );
  const { data, isLoading, isError } = useToDos(lastUpdated, []);

  const handleSubmit = async (data: InsertToDo) => {
    const created = await createToDo(data);
    setLastUpdated(created.updatedAt.toISOString());
  };
  return (
    <>
      <div
        className={cn(
          "flex flex-col min-h-full w-full p-2 pr-18",
          props.className,
        )}
      >
        <ResultDisplay toDos={data} isLoading={isLoading} isError={isError} />
      </div>
      <AddToDoButton onClick={() => setIsOpen(true)} />
      <AddEditToDoDialog
        open={isOpen}
        onOpenChange={setIsOpen}
        onSubmit={handleSubmit}
      />
    </>
  );
}
