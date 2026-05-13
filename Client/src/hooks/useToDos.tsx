import { getMany, type ToDo } from "../http/ToDo";
import { useQuery, type UseQueryArgs } from "./useQuery";

function useToDos(lastUpdated: string, data: ToDo[]) {
  const toDoQueryArgs: UseQueryArgs<ToDo[]> = {
    queryKey: lastUpdated,
    queryFn: getMany,
    initialData: data,
  };
  return useQuery(toDoQueryArgs);
}

export { useToDos };
