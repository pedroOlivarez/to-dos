import { useEffect, useState } from "react";

type UseQueryArgs<T> = {
  queryKey: string;
  queryFn: () => Promise<T>;
  initialData: T;
};

function useQuery<T>({ queryFn, queryKey, initialData }: UseQueryArgs<T>) {
  const [data, setData] = useState<T>(initialData);
  const [isLoading, setIsLoading] = useState(false);
  const [isError, setIsError] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      setIsError(false);
      setIsLoading(true);

      try {
        const result = await queryFn();

        setData(result);
      } catch (error) {
        setIsError(true);
      }

      setIsLoading(false);
    };

    fetchData();
  }, [queryKey]);

  return { data, isLoading, isError };
}

export { useQuery, type UseQueryArgs };
