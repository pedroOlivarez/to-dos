import { useMemo, useState, type FocusEvent } from "react";
import type { ToDo, UpdateToDo } from "../../../../actions/ToDo";

export default function useEditToDoDialog({
  defaultValues,
  onSubmit,
}: {
  defaultValues: ToDo;
  onSubmit: (id: number, data: UpdateToDo) => Promise<void>;
}) {
  const [errors, setErrors] = useState<Record<string, boolean>>({
    title: false,
  });
  const [currentState, setCurrentState] = useState<UpdateToDo>({
    title: defaultValues.title,
    body: defaultValues.body,
    completed: defaultValues.completed,
  });
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const formIsTouched = useMemo(() => {
    return (
      currentState.title !== defaultValues.title ||
      currentState.body !== defaultValues.body ||
      currentState.completed !== defaultValues.completed
    );
  }, [currentState, defaultValues]);
  const formIsValid = useMemo(() => {
    for (const value of Object.values(errors)) {
      if (value) return false;
    }
    return true;
  }, [errors]);

  const validateTitle = (e: FocusEvent<HTMLInputElement, Element>) => {
    const title = e.currentTarget.value.trim();
    setCurrentState((prev) => ({
      ...prev,
      title,
    }));
    setErrors((prev) => ({
      ...prev,
      title: !title,
    }));
  };

  const validateBody = (e: FocusEvent<HTMLInputElement, Element>) => {
    const body = e.currentTarget.value.trim();
    setCurrentState((prev) => ({
      ...prev,
      body,
    }));
  };

  const validateCompleted = () => {
    setCurrentState((prev) => ({
      ...prev,
      completed: !prev.completed,
    }));
  };

  const handleFormSubmit = async (formData: FormData) => {
    setIsSubmitting(true);
    const title = formData.get("todo_title")?.toString().trim();
    const body = formData.get("todo_body")?.toString().trim();
    const completed = Boolean(formData.get("todo_completed")?.toString());
    if (!title) {
      console.error("Attempted to submit without required fields");
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
    if (!updated) {
      setIsSubmitting(false);
      return;
    }

    await onSubmit(defaultValues.id, data);
    setIsSubmitting(false);
  };

  const reset = () => {
    setErrors({
      title: false,
    });
    setCurrentState({});
  };

  return {
    errors,
    formIsTouched,
    formIsValid,
    isSubmitting,
    validateTitle,
    validateBody,
    validateCompleted,
    handleFormSubmit,
    reset,
  };
}
