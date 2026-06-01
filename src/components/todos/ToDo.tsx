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
        "flex flex-col p-4 sm:min-h-48 sm:max-h-48 min-h-64 max-h-64 sm:min-w-40 sm:max-w-40 min-w-60 max-w-60 gap-2 justify-between shadow-md shadow-black",
        toDo.completed ? "bg-fuchsia-400/70" : "bg-fuchsia-400",
        rest.className,
      )}
      onClick={() => {
        onClick(toDo.id);
      }}
    >
      <div className="text-black/85 truncate min-h-5 max-h-5">{toDo.title}</div>
      <div className="text-black/70 text-wrap sm:line-clamp-4 line-clamp-3 truncate">
        {toDo.body}
      </div>
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
