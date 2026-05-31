import { Button } from "./Button/Button";
import type { Meta } from "../../http";
import type { ComponentProps } from "react";
import { cn } from "../../libs/utils/classNames";

type PaginationProps = Pick<
  Meta,
  "page" | "totalPages" | "hasNext" | "hasPrevious"
> &
  ComponentProps<"div"> & {
    onPageSelect: (pageNumber: number) => void;
  };

export function Pagination({
  page,
  totalPages,
  hasNext,
  hasPrevious,
  onPageSelect,
  ...rest
}: PaginationProps) {
  const range = Array.from({ length: totalPages }, (_, i) => i + 1);
  const handleClick = (pageNumber: number) => onPageSelect(pageNumber);
  return (
    <div className={cn("flex w-full justify-center gap-1")} {...rest}>
      <Button
        variant="outline"
        onClick={() => handleClick(page - 1)}
        disabled={!hasPrevious}
      >
        prev
      </Button>
      {range.map((pageNumber) => (
        <Button
          variant="ghost"
          key={`pagination-item-${pageNumber}`}
          onClick={() => handleClick(pageNumber)}
          disabled={pageNumber === page}
        >
          {pageNumber}
        </Button>
      ))}
      <Button
        variant="outline"
        onClick={() => handleClick(page + 1)}
        disabled={!hasNext}
      >
        next
      </Button>
    </div>
  );
}
