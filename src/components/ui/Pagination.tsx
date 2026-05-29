import {
  Pagination as PaginationBase,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "./Pagination.Base";
import type { Meta } from "../../http";

export function Pagination({
  page,
  totalPages,
  hasNext,
  hasPrevious,
}: Pick<Meta, "page" | "totalPages" | "hasNext" | "hasPrevious">) {
  const range = Array.from({ length: totalPages }, (_, i) => i + 1);
  return (
    <PaginationBase>
      <PaginationContent>
        {hasPrevious ? (
          <PaginationItem>
            <PaginationPrevious href={`?page=${page - 1}`} />
          </PaginationItem>
        ) : null}
        {range.map((pageNumber) => (
          <PaginationItem>
            <PaginationLink
              href={`?page=${pageNumber}`}
              isActive={pageNumber === page}
            >
              {pageNumber}
            </PaginationLink>
          </PaginationItem>
        ))}
        {hasNext ? (
          <PaginationItem>
            <PaginationNext href={`?page=${page + 1}`} />
          </PaginationItem>
        ) : null}
      </PaginationContent>
    </PaginationBase>
  );
}
