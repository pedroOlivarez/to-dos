import { useMemo, useState, type InputEvent } from "react";
import type { ToDo, UpdateToDo } from "../../../../actions/ToDo";

export default function useEditToDoDialog({
  defaultValues,
  onSubmit,
}: {
  defaultValues: ToDo | null;
  onSubmit: (id: number, data: UpdateToDo) => Promise<void>;
}) {
  const [errors, setErrors] = useState<Record<string, boolean>>({
    title: false,
  });
  const [currentState, setCurrentState] = useState<UpdateToDo>({
    title: defaultValues?.title,
    body: defaultValues?.body,
    completed: defaultValues?.completed,
  });
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const formIsTouched = useMemo(() => {
    return (
      currentState.title !== defaultValues?.title ||
      currentState.body !== defaultValues?.body ||
      currentState.completed !== defaultValues?.completed
    );
  }, [currentState, defaultValues]);
  const formIsValid = useMemo(() => {
    for (const value of Object.values(errors)) {
      if (value) return false;
    }
    return true;
  }, [errors]);

  const validateTitle = (e: InputEvent<HTMLInputElement>) => {
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

  const validateBody = (e: InputEvent<HTMLInputElement>) => {
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
    try {
      if (!defaultValues) {
        console.error(
          "Attempted to update a to-do without passing in initial values",
        );
        return;
      }
      setIsSubmitting(true);
      const title = formData.get("todo_title")?.toString().trim();
      const body = formData.get("todo_body")?.toString().trim();
      const completed = Boolean(formData.get("todo_completed")?.toString());
      if (!title) {
        setErrors((prev) => ({
          ...prev,
          title: true,
        }));
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
      reset();
    } catch {
      setIsSubmitting(false);
    }
  };

  const reset = () => {
    setIsSubmitting(false);
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
