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

interface EditToDoDialog extends ComponentProps<typeof Dialog> {
  defaultValues: ToDo | null;
  onSubmit: (id: number, data: UpdateToDo) => void;
}

export function EditToDoDialog({
  defaultValues,
  onSubmit,
  ...rest
}: EditToDoDialog) {
  if (!defaultValues) {
    return null;
  }
  return (
    <Dialog {...rest}>
      <DialogContent className="sm:max-w-sm bg-black shadow-xl shadow-black">
        <form
          action={async (formData: FormData) => {
            const title = formData.get("todo_title")?.toString().trim();
            const body = formData.get("todo_body")?.toString().trim();
            const completed = Boolean(
              formData.get("todo_completed")?.toString(),
            );
            if (!title) {
              // error handling
              return;
            }
            let updated = false;
            const data: UpdateToDo = {};

            if (title !== defaultValues.title) {
              data.title = title;
              updated = true;
            }
            if (body !== defaultValues.body) {
              data.body = body;
              updated = true;
            }
            if (completed !== defaultValues.completed) {
              data.completed = completed;
              updated = true;
            }
            if (!updated) return;

            onSubmit(defaultValues.id, data);
          }}
          className="flex flex-col gap-6"
        >
          <DialogHeader>
            <DialogTitle>Update To-Do</DialogTitle>
          </DialogHeader>
          <FieldGroup>
            <Field>
              <Label htmlFor="title">Title</Label>
              <Input
                id="title"
                name="todo_title"
                defaultValue={defaultValues.title}
              />
            </Field>
            <Field>
              <Label htmlFor="body">Body</Label>
              <Input
                id="body"
                name="todo_body"
                defaultValue={defaultValues.body}
              />
            </Field>
            <Field>
              <Label htmlFor="completed">Completed</Label>
              <Input
                type="checkbox"
                id="completed"
                name="todo_completed"
                defaultChecked={defaultValues.completed}
              />
            </Field>
          </FieldGroup>
          <DialogFooter>
            <DialogClose render={<Button variant="outline">Cancel</Button>} />
            <Button variant="default" type="submit">
              Save changes
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
