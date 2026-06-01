import { useCallback, useMemo } from "react";
import { getToDo, type ToDo } from "../actions/ToDo";
import { useQuery } from "./useQuery";
import { type UseQueryArgs } from "./useQueryArgs";

function useToDo(id?: number | null) {
  const args = useMemo(() => {
    const newArgs: Record<string, string> = {
      id: id?.toString() ?? "",
    };
    return newArgs;
  }, [id]);

  const queryFn = useCallback((args?: Record<string, string>) => {
    if (args?.["id"]) {
      const id = Number(args["id"]);
      return getToDo(id);
    }
    return Promise.resolve({} as ToDo);
  }, []);

  const toDoQueryArgs: UseQueryArgs<ToDo | null> = useMemo(
    () => ({
      queryKey: `${id}`,
      args,
      queryFn,
    }),
    [args, id, queryFn],
  );

  return useQuery(toDoQueryArgs);
}

export { useToDo };
