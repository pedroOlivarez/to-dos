import { useMemo, useState, type FocusEvent } from "react";
import type { ToDo, UpdateToDo } from "../../../actions/ToDo";

export default function useEditToDoDialog({
  defaultValues,
  onSubmit,
}: {
  defaultValues: ToDo;
  onSubmit: (id: number, data: UpdateToDo) => void;
}) {
  const [errors, setErrors] = useState<Record<string, boolean>>({
    title: false,
  });
  const [currentState, setCurrentState] = useState<UpdateToDo>({
    body: defaultValues.body,
    completed: defaultValues.completed,
    title: defaultValues.title,
  });
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
  const validateCompleted = (e: FocusEvent<HTMLInputElement, Element>) => {
    const completed = Boolean(e.currentTarget.value.trim());
    setCurrentState((prev) => ({
      ...prev,
      completed,
    }));
  };

  const handleFormSubmit = async (formData: FormData) => {
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
    if (!updated) return;

    onSubmit(defaultValues.id, data);
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
    validateTitle,
    validateBody,
    validateCompleted,
    handleFormSubmit,
    reset,
  };
}
