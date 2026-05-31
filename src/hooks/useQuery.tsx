import { useEffect, useState } from 'react';
import { type UseQueryArgs } from './useQueryArgs';

function useQuery<T>({ queryFn, queryKey, args }: UseQueryArgs<T>) {
   const [data, setData] = useState<T | null>(null);
   const [isLoading, setIsLoading] = useState(false);
   const [isError, setIsError] = useState(false);

   useEffect(() => {
      let didCancel = false;
      const fetchData = async () => {
         setIsError(false);
         setIsLoading(true);

         try {
            const result = await queryFn(args);
            if (!didCancel) setData(result);
         } catch (error) {
            if (!didCancel) {
               console.error(error);
               setIsError(true);
            }
         }

         setIsLoading(false);
      };

      fetchData();

      return () => {
         didCancel = true;
      };
   }, [args, queryFn, queryKey]);

   return { data, isLoading, isError };
}

export { useQuery };
