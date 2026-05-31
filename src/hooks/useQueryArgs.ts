type UseQueryArgs<T> = {
   queryKey: string;
   args: Record<string, string>;
   queryFn: (args?: Record<string, string>) => Promise<T>;
};

export { type UseQueryArgs };
