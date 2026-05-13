import { Button } from "../ui/Button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "../ui/Dialog";
import { Field, FieldGroup } from "../ui/Field";
import { Input } from "../ui/Input";
import { Label } from "../ui/Label";
import type { ComponentProps } from "react";
import type { InsertToDo, ToDo } from "../../actions/ToDo";

interface AddEditToDoDialogProps extends ComponentProps<typeof Dialog> {
  defaultValues?: ToDo;
  onSubmit: (data: InsertToDo) => void;
}

export function AddEditToDoDialog({
  defaultValues,
  onSubmit,
  ...rest
}: AddEditToDoDialogProps) {
  const isEdit = !!defaultValues?.id;

  return (
    <Dialog {...rest}>
      <DialogContent className="sm:max-w-sm">
        <form
          action={async (formData: FormData) => {
            const title = formData.get("title")?.toString();
            const body = formData.get("body")?.toString();
            if (!title) {
              // error handling
              return;
            }

            const data: InsertToDo = {
              title,
              body,
            };

            onSubmit(data);
          }}
          className="flex flex-col gap-6"
        >
          <DialogHeader>
            <DialogTitle>{isEdit ? "Edit" : "Create"} To-Do</DialogTitle>
          </DialogHeader>
          <FieldGroup>
            <Field>
              <Label htmlFor="title">Title</Label>
              <Input
                name="title"
                defaultValue={isEdit ? defaultValues.title : undefined}
              />
            </Field>
            <Field>
              <Label htmlFor="body">Body</Label>
              <Input
                name="body"
                defaultValue={isEdit ? defaultValues.body : undefined}
              />
            </Field>
          </FieldGroup>
          <DialogFooter>
            <DialogClose render={<Button variant="outline">Cancel</Button>} />
            <DialogClose
              render={
                <Button variant="default" type="submit">
                  Save changes
                </Button>
              }
            />
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
