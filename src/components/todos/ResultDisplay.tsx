import type { ToDo as ToDoType } from "../../actions/ToDo";
import { ToDo } from "./ToDo";

export function ResultDisplay({
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
    <div className="flex sm:flex-row flex-col gap-2 flex-wrap">
      {toDos.map((td) => (
        <ToDo onClick={onSelectToDo} key={td.id} toDo={td} />
      ))}
    </div>
  ) : (
    <div>Nothing to show yet</div>
  );
}
