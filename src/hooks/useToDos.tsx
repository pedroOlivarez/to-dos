import { getToDos, type PaginatedResponse } from "../actions/ToDo";
import { useQuery, type UseQueryArgs } from "./useQuery";

function useToDos({
  lastUpdated,
  searchTerm,
  page = "1",
  pageSize = "20",
}: {
  lastUpdated: string;
  searchTerm?: string;
  page?: string;
  pageSize?: string;
}) {
  const args: Record<string, string> = {
    page,
    page_size: pageSize,
  };
  if (searchTerm) {
    args["q"] = searchTerm;
  }
  const toDoQueryArgs: UseQueryArgs<PaginatedResponse> = {
    queryKey: `${page}-${searchTerm}-${lastUpdated}`,
    args,
    queryFn: getToDos,
  };
  return useQuery(toDoQueryArgs);
}

export { useToDos };
