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
import type { InsertToDo } from "../../../../actions/ToDo";
import { useAddToDoDialog } from "./hooks";

interface AddToDoDialog extends ComponentProps<typeof Dialog> {
  onSubmit: (data: InsertToDo) => Promise<void>;
}

export function AddToDoDialog({ onSubmit, ...rest }: AddToDoDialog) {
  const {
    errors,
    formIsTouched,
    formIsValid,
    isSubmitting,
    validateTitle,
    validateBody,
    handleFormSubmit,
    reset,
  } = useAddToDoDialog({ onSubmit });

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
            <DialogTitle>Create To-Do</DialogTitle>
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
              />
            </Field>
            <Field>
              <Label htmlFor="body">Body</Label>
              <Input id="body" name="todo_body" onBlur={validateBody} />
            </Field>
          </FieldGroup>
          <DialogFooter>
            <DialogClose render={<Button variant="outline">Cancel</Button>} />
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
