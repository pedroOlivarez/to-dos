import { getToDos, type ToDo } from "../actions/ToDo";
import { useQuery, type UseQueryArgs } from "./useQuery";

function useToDos(lastUpdated: string, data: ToDo[]) {
  const toDoQueryArgs: UseQueryArgs<ToDo[]> = {
    queryKey: lastUpdated,
    queryFn: getToDos,
    initialData: data,
  };
  return useQuery(toDoQueryArgs);
}

export { useToDos };
