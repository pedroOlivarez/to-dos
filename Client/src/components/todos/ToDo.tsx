import type { ComponentProps } from "react";
import { type ToDo } from "../../actions/ToDo";
import { cn } from "../../libs/utils/classNames";

interface ToDoProps extends Omit<ComponentProps<"div">, "onClick"> {
  toDo: ToDo;
  onClick: (id: number) => void;
}

export function ToDo({ toDo, onClick, ...rest }: ToDoProps) {
  return (
    <div
      {...rest}
      className={cn(
        "flex flex-col p-4 min-h-48 max-h-48 min-w-40 max-w-40 gap-2 justify-between shadow-md shadow-black",
        toDo.completed ? "bg-teal-400/80" : "bg-fuchsia-400",
        rest.className,
      )}
      onClick={() => {
        onClick(toDo.id);
      }}
    >
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
