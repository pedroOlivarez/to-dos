import { useCallback, useMemo } from 'react';
import { getToDos, type PaginatedResponse } from '../actions/ToDo';
import { useQuery } from './useQuery';
import { type UseQueryArgs } from './useQueryArgs';

function useToDos({
   lastUpdated,
   searchTerm,
   page = '1',
   pageSize = '100',
}: {
   lastUpdated: string;
   searchTerm?: string;
   page?: string;
   pageSize?: string;
}) {
   const args = useMemo(() => {
      const newArgs: Record<string, string> = {
         page,
         page_size: pageSize,
      };
      if (searchTerm) {
         newArgs['q'] = searchTerm;
      }
      return newArgs;
   }, [page, pageSize, searchTerm]);

   const queryFn = useCallback((args?: Record<string, string>) => {
      return getToDos(args);
   }, []);

   const toDoQueryArgs: UseQueryArgs<PaginatedResponse> = useMemo(
      () => ({
         queryKey: `${page}-${searchTerm}-${lastUpdated}`,
         args,
         queryFn,
      }),
      [page, searchTerm, lastUpdated, args, queryFn],
   );
   return useQuery(toDoQueryArgs);
}

export { useToDos };
