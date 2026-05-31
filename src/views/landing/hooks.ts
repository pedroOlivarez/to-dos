import { useMemo, useState, type FocusEvent } from "react";
import {
  authenticate,
  type AuthenticationRequest,
} from "../../actions/Authenticate";
import { register } from "../../actions/User";
import { containsNumber, isValidEmail } from "../../libs/utils/stringUtils";
import { useNavigate } from "react-router";

export function useLogin(type: "authenticate" | "register") {
  const navigate = useNavigate();
  const [currentState, setCurrentState] = useState<AuthenticationRequest>({
    email: "",
    password: "",
  });
  const [errors, setErrors] = useState<Record<string, string>>({
    email: "",
    password: "",
  });
  const formIsTouched = useMemo(() => {
    return currentState.email || currentState.password;
  }, [currentState]);

  const formIsValid = useMemo(() => {
    for (const value of Object.values(errors)) {
      if (value) return false;
    }
    return currentState.email && currentState.password;
  }, [currentState, errors]);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const validateEmail = (e: FocusEvent<HTMLInputElement, Element>) => {
    const email = e.currentTarget.value.trim();
    const isBlank = !email;
    const isValid = isValidEmail(email);
    if (isValid) {
      setCurrentState((prev) => ({
        ...prev,
        email,
      }));
    }
    setErrors((prev) => ({
      ...prev,
      email: isBlank
        ? "Email is required"
        : !isValid
          ? "Not a valid email"
          : "",
    }));
  };

  const validatePassword = (e: FocusEvent<HTMLInputElement, Element>) => {
    const password = e.currentTarget.value;
    const isValidLength = password.length >= 8;
    const isValid = containsNumber(password);
    if (isValid) {
      setCurrentState((prev) => ({
        ...prev,
        password,
      }));
    }
    setErrors((prev) => ({
      ...prev,
      password: !isValidLength
        ? "Password must be at least 8 characters"
        : !isValid
          ? "Password must contain a number"
          : "",
    }));
  };

  const resetFormState = () => {
    setIsSubmitting(false);
    setCurrentState({
      email: "",
      password: "",
    });
  };

  const handleFormSubmit = async (formData: FormData): Promise<void> => {
    setIsSubmitting(true);
    const email = formData.get("user_email")?.toString().trim();
    const password = formData.get("user_password")?.toString();
    if (!email) {
      console.error("Attempted to submit login form without email");
      resetFormState();
      return;
    }
    if (!password) {
      console.error("Attempted to submit login form without password");
      resetFormState();
      return;
    }

    const data: AuthenticationRequest = {
      email,
      password,
    };
    if (type === "authenticate") {
      await authenticateUser(data);
    } else {
      await registerUser(data);
    }

    resetFormState();
  };

  const authenticateUser = async (data: AuthenticationRequest) => {
    const response = await authenticate(data);
    if (response) {
      navigate("/to-dos");
    } else {
      setErrors({
        email: "Invalid email or password",
        password: "Invalid email or password",
      });
    }
  };

  const registerUser = async (data: AuthenticationRequest) => {
    const response = await register(data);
    if (response.success) {
      navigate("/to-dos");
    } else if (response.statusCode === 400) {
      setErrors({
        email: "Email is already in use",
        password: "",
      });
    }
  };

  return {
    errors,
    formIsTouched,
    formIsValid,
    isSubmitting,
    validateEmail,
    validatePassword,
    handleFormSubmit,
  };
}
