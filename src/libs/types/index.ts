type ToDoModal = "ADD_TODO" | "EDIT_TODO";

const TODO_MODALS: Record<string, ToDoModal> = {
  ADD: "ADD_TODO",
  EDIT: "EDIT_TODO",
};

export { TODO_MODALS, type ToDoModal };
