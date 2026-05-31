import { Button } from "../../../ui/Button/Button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "../../../ui/Dialog";
import { Field, FieldGroup } from "../../../ui/Field";
import { Input } from "../../../ui/Input";
import { Label } from "../../../ui/Label";
import { type ComponentProps, type SubmitEvent } from "react";
import type { UpdateToDo, ToDo } from "../../../../actions/ToDo";
import useEditToDoDialog from "./hooks";

interface EditToDoDialog extends ComponentProps<typeof Dialog> {
  defaultValues: ToDo | null;
  onSubmit: (id: number, data: UpdateToDo) => Promise<void>;
  onArchive: (id: number) => void;
}

export function EditToDoDialog({
  defaultValues,
  onSubmit,
  onArchive,
  ...rest
}: EditToDoDialog) {
  const {
    errors,
    formIsTouched,
    formIsValid,
    isSubmitting,
    validateTitle,
    validateBody,
    validateCompleted,
    handleFormSubmit,
    reset,
  } = useEditToDoDialog({ defaultValues, onSubmit });

  const handleArchive = () => {
    if (defaultValues) {
      onArchive(defaultValues.id);
    } else {
      throw new Error("Attempted to archive a to-do without passing in an id");
    }
  };

  if (!defaultValues) {
    return null;
  }

  // To-Do (medium): this can get moved to utils, we'll use this on all forms probably
  const handleSubmit = (event?: SubmitEvent<HTMLFormElement>) => {
    event?.preventDefault();
    const formData = new FormData(event?.target);

    handleFormSubmit(formData);
  };

  return (
    <Dialog
      onOpenChange={(newOpen, eventDetails) => {
        reset();
        rest.onOpenChange?.(newOpen, eventDetails);
      }}
      {...rest}
    >
      <DialogContent className="sm:max-w-sm bg-black shadow-xl shadow-black">
        <form onSubmit={handleSubmit} className="flex flex-col gap-6">
          <DialogHeader>
            <DialogTitle>Update To-Do</DialogTitle>
          </DialogHeader>
          <FieldGroup>
            <Field>
              <Label htmlFor="title">Title</Label>
              <Input
                id="title"
                name="todo_title"
                onBlur={validateTitle}
                className={errors.title ? "border border-red-400" : ""}
                placeholder={
                  errors.title ? "Title field is required" : undefined
                }
                defaultValue={defaultValues.title}
              />
            </Field>
            <Field>
              <Label htmlFor="body">Body</Label>
              <Input
                id="body"
                name="todo_body"
                onBlur={validateBody}
                defaultValue={defaultValues.body}
              />
            </Field>
            <Field className="flex flex-row">
              <Label htmlFor="completed">Completed</Label>
              <Input
                type="checkbox"
                id="completed"
                name="todo_completed"
                onClick={validateCompleted}
                defaultChecked={defaultValues.completed}
                className="min-h-6 max-h-6 min-w-6 max-w-6 "
              />
            </Field>
          </FieldGroup>
          <DialogFooter>
            <DialogClose render={<Button variant="outline">Cancel</Button>} />
            <Button variant="destructive" onClick={handleArchive}>
              Archive
            </Button>
            <Button
              variant="default"
              type="submit"
              disabled={!formIsTouched || !formIsValid || isSubmitting}
            >
              Save changes
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
