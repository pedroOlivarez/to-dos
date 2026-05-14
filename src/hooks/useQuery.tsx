import { useEffect, useState } from "react";

type UseQueryArgs<T> = {
  queryKey: string;
  queryFn: () => Promise<T>;
};

function useQuery<T>({ queryFn, queryKey }: UseQueryArgs<T>) {
  const [data, setData] = useState<T | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isError, setIsError] = useState(false);

  useEffect(() => {
    let didCancel = false;
    const fetchData = async () => {
      setIsError(false);
      setIsLoading(true);

      try {
        const result = await queryFn();
        if (!didCancel) setData(result);
      } catch (error) {
        if (!didCancel) setIsError(true);
      }

      setIsLoading(false);
    };

    fetchData();

    return () => {
      didCancel = true;
    };
  }, [queryKey]);

  return { data, isLoading, isError };
}

export { useQuery, type UseQueryArgs };
