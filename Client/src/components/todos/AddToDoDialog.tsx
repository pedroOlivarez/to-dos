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
import { type ComponentProps } from "react";
import type { InsertToDo } from "../../actions/ToDo";

interface AddToDoDialog extends ComponentProps<typeof Dialog> {
  onSubmit: (data: InsertToDo) => void;
}

export function AddToDoDialog({ onSubmit, ...rest }: AddToDoDialog) {
  return (
    <Dialog {...rest}>
      <DialogContent className="sm:max-w-sm bg-black shadow-xl shadow-black">
        <form
          action={async (formData: FormData) => {
            const title = formData.get("title")?.toString().trim();
            const body = formData.get("body")?.toString().trim();
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
            <DialogTitle>Create To-Do</DialogTitle>
          </DialogHeader>
          <FieldGroup>
            <Field>
              <Label htmlFor="title">Title</Label>
              <Input id="title" name="todo_title" />
            </Field>
            <Field>
              <Label htmlFor="body">Body</Label>
              <Input id="body" name="todo_body" />
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
