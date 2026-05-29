import { getToDos, type ToDo } from "../actions/ToDo";
import { useQuery, type UseQueryArgs } from "./useQuery";

function useToDos(lastUpdated: string, page = 1, pageSize = 100) {
  const toDoQueryArgs: UseQueryArgs<ToDo[]> = {
    queryKey: lastUpdated,
    args: { page: page.toString(), page_size: pageSize.toString() },
    queryFn: getToDos,
  };
  return useQuery(toDoQueryArgs);
}

export { useToDos };
