import { getToDos, type PaginatedResponse } from "../actions/ToDo";
import { useQuery, type UseQueryArgs } from "./useQuery";

function useToDos(lastUpdated: string, page = "1", pageSize = "20") {
  const toDoQueryArgs: UseQueryArgs<PaginatedResponse> = {
    queryKey: lastUpdated,
    args: { page, page_size: pageSize },
    queryFn: getToDos,
  };
  return useQuery(toDoQueryArgs);
}

export { useToDos };
