import { useState, type ComponentProps } from "react";
import { PlusIcon } from "lucide-react";
import { useToDos } from "../hooks/useToDos";

export function ToDos(props: ComponentProps<"div">) {
  const [lastUpdated, setLastUpdated] = useState<string>(
    new Date().toDateString(),
  );
  const { data, isLoading, isError } = useToDos(lastUpdated, []);
  console.log({ data });
  return (
    <>
      <div className="flex flex-col min-h-full w-full pl-2 pr-18">
        {isLoading
          ? "Fetching to-dos..."
          : isError
            ? "Oops, an error occurred"
            : data && data.length
              ? data.map((d) => <div key={d.id}>{d.title}</div>)
              : null}
      </div>
      <button
        className="absolute flex items-center justify-center rounded-2xl p-7 bg-black/85 bottom-2 right-2 min-h-6 max-h-6 min-w-6 max-w-6"
        onClick={() => {
          setLastUpdated(new Date().toISOString());
        }}
      >
        <span>
          <PlusIcon size={32} />
        </span>
      </button>
    </>
  );
}
