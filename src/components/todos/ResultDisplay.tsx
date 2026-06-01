import type { ToDo as ToDoType } from "../../actions/ToDo";
import { AddToDoButton } from "./AddToDoButton.tsx";
import { ToDo } from "./ToDo";

export function ResultDisplay({
  toDos,
  isLoading,
  isError,
  onSelectToDo,
  onAddToDoClick,
}: {
  toDos: ToDoType[] | null;
  isLoading: boolean;
  isError: boolean;
  onSelectToDo: (id: number) => void;
  onAddToDoClick: () => void;
}) {
  return isLoading ? (
    "Fetching to-dos..."
  ) : isError ? (
    "Oops, an error occurred"
  ) : toDos && toDos.length ? (
    <div className="flex sm:flex-row flex-col gap-2 flex-wrap">
      {toDos.map((td) => (
        <ToDo onClick={onSelectToDo} key={td.id} toDo={td} />
      ))}
      <AddToDoButton
        onClick={() => onAddToDoClick()}
        className={!toDos.length ? "animate-bounce" : undefined}
      />
    </div>
  ) : (
    <div>Nothing to show yet</div>
  );
}
