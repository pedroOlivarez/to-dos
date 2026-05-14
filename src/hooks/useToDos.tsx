import { getToDos, type ToDo } from "../actions/ToDo";
import { useQuery, type UseQueryArgs } from "./useQuery";

function useToDos(lastUpdated: string) {
  const toDoQueryArgs: UseQueryArgs<ToDo[]> = {
    queryKey: lastUpdated,
    queryFn: getToDos,
  };
  return useQuery(toDoQueryArgs);
}

export { useToDos };
