import { useMemo, useState, type InputEvent } from "react";
import type { InsertToDo } from "../../../../actions/ToDo";

export function useAddToDoDialog({
  onSubmit,
}: {
  onSubmit: (data: InsertToDo) => Promise<void>;
}) {
  const [errors, setErrors] = useState<Record<string, boolean>>({
    title: false,
  });
  const [currentState, setCurrentState] = useState<InsertToDo>({
    title: "",
    body: "",
  });
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const formIsTouched = useMemo(() => {
    return currentState.title || currentState.body;
  }, [currentState]);

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

  const handleFormSubmit = async (formData: FormData) => {
    try {
      setIsSubmitting(true);
      const title = formData.get("todo_title")?.toString().trim();
      const body = formData.get("todo_body")?.toString().trim();
      if (!title) {
        console.error("Attempted to submit without required fields");
        return;
      }
      const data: InsertToDo = {
        title,
        body,
      };

      await onSubmit(data);
      setIsSubmitting(false);
      reset();
    } catch {
      setIsSubmitting(false);
    }
  };

  const reset = () => {
    setErrors({
      title: false,
    });
    setCurrentState({
      title: "",
      body: "",
    });
  };

  return {
    errors,
    formIsTouched,
    formIsValid,
    isSubmitting,
    validateTitle,
    validateBody,
    handleFormSubmit,
    reset,
  };
}
