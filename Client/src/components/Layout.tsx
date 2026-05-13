import type { ComponentProps } from "react";
import { cn } from "../libs/utils/classNames";

export function Layout(props: ComponentProps<"div">) {
  return (
    <>
      <div className="flex items-center justify-center sticky min-h-20 max-h-20 w-full top-0 border-b border-b-fuchsia-400 bg-black/80">
        <h1>Pedro's To-Do Submission</h1>
      </div>
      <div className="flex flex-row items-center justify-center overflow-y-auto w-full min-h-[calc(100vh-var(--spacing)*20)] bg-black/65">
        <div
          className={cn(
            "w-full flex flex-col items-center justify-center",
            props.className,
          )}
        >
          {props.children}
        </div>
      </div>
    </>
  );
}
