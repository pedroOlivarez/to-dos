import { Input } from "./Input";
import {
  useEffect,
  useState,
  type ComponentProps,
  type InputEvent,
} from "react";

interface SearchProps extends Omit<ComponentProps<typeof Input>, "onInput"> {
  onInput: (term: string | null) => void;
  debounceDelay?: number;
}

export function Search({ onInput, debounceDelay = 0, ...rest }: SearchProps) {
  const [searchTerm, setSearchTerm] = useState<string | null>(null);
  useEffect(() => {
    if (!debounceDelay) {
      onInput(searchTerm);
    }
    const timeoutId = setTimeout(() => {
      onInput(searchTerm);
    }, debounceDelay);

    return () => {
      clearTimeout(timeoutId);
    };
  }, [searchTerm, debounceDelay, onInput]);

  const handleInput = (e?: InputEvent<HTMLInputElement>) =>
    setSearchTerm(e?.currentTarget.value.trim() ?? null);

  return (
    <Input
      id="search"
      name="input_search"
      type="search"
      onInput={handleInput}
      placeholder="type to search.."
      {...rest}
    />
  );
}
