import type { ComponentProps } from "react";
import blueberry from "../../public/blueberry.svg";
import { cn } from "../../libs/utils/classNames";

export function NotFound(props: ComponentProps<"div">) {
  return (
    <div
      className={cn(
        "flex flex-col h-full w-full items-center justify-center gap-4",
      )}
      {...props}
    >
      <img
        src={blueberry}
        alt="blueberry"
        className="sm:min-h-60 min-h-40 sm:min-w-60 min-w-60 sm:max-h-60 max-h-60 sm:max-w-60 max-w-60"
      />
      <p>Nothing to see here</p>
    </div>
  );
}
