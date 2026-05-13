import type { ComponentProps } from "react";
import { type ToDo } from "../../actions/ToDo";

interface ToDoProps extends ComponentProps<"div"> {
  toDo: ToDo;
}

export function ToDo({ toDo }: ToDoProps) {
  return (
    <div className="flex flex-col bg-fuchsia-400 p-4 min-h-48 max-h-48 min-w-40 max-w-40 gap-2 justify-between shadow-md shadow-black">
      <div className="text-black/85 truncate min-h-5 max-h-5">{toDo.title}</div>
      <div className="text-black/70 text-wrap">{toDo.body}</div>
      <div className="text-black/60 text-xs">
        updatedAt: {toDo.updatedAt.toLocaleDateString()}{" "}
        {toDo.updatedAt.toLocaleTimeString("en-US", {
          hour: "2-digit",
          minute: "2-digit",
          hour12: true,
        })}
      </div>
    </div>
  );
}
