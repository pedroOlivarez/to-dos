import { Button } from "../../ui/Button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "../../ui/Dialog";
import { Field, FieldGroup } from "../../ui/Field";
import { Input } from "../../ui/Input";
import { Label } from "../../ui/Label";
import { type ComponentProps } from "react";
import type { UpdateToDo, ToDo } from "../../../actions/ToDo";
import useEditToDoDialog from "./hooks";

interface EditToDoDialog extends ComponentProps<typeof Dialog> {
  defaultValues: ToDo | null;
  onSubmit: (id: number, data: UpdateToDo) => void;
  onArchive: (id: number) => void;
}

export function EditToDoDialog({
  defaultValues,
  onSubmit,
  onArchive,
  ...rest
}: EditToDoDialog) {
  if (!defaultValues) {
    return null;
  }

  const {
    errors,
    formIsTouched,
    formIsValid,
    validateTitle,
    validateBody,
    validateCompleted,
    handleFormSubmit,
    reset,
  } = useEditToDoDialog({ defaultValues, onSubmit });

  const handleArchive = () => {
    onArchive(defaultValues.id);
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
        <form action={handleFormSubmit} className="flex flex-col gap-6">
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
              disabled={!formIsTouched || !formIsValid}
            >
              Save changes
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
