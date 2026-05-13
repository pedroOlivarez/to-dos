import type { ComponentProps } from "react";
import { cn } from "../libs/utils/classNames";

export function Layout(props: ComponentProps<"div">) {
  return (
    <div className="flex flex-col items-center justify-center h-full w-full bg-black/65">
      <div className="flex flex-row items-center justify-center min-h-20 max-h-20 w-full sticky top-0 border-b border-b-fuchsia-400 bg-black/80">
        <h1>Pedro's To-Do Submission</h1>
      </div>
      <div className="flex flex-row items-center justify-center h-full w-full overflow-y-auto">
        <div
          className={cn(
            "w-full h-full flex flex-col items-center",
            props.className,
          )}
        >
          {props.children}
        </div>
      </div>
    </div>
  );
}
