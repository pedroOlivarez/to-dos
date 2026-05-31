import {
  useEffect,
  useState,
  type ComponentProps,
  type SubmitEvent,
} from "react";
import { Field, FieldGroup } from "../../components/ui/Field";
import { Label } from "../../components/ui/Label";
import { Input } from "../../components/ui/Input";
import { Button } from "../../components/ui/Button/Button";
import { useLogin } from "./hooks";
import { cn } from "../../libs/utils/classNames";
import { useAuth } from "../../hooks/useAuth";
import { useNavigate } from "react-router";

export function Login(props: ComponentProps<"div">) {
  const [type, setType] = useState<"authenticate" | "register">("authenticate");
  const navigate = useNavigate();
  const {
    errors,
    formIsTouched,
    formIsValid,
    isSubmitting,
    handleEmailBlur,
    handlePasswordBlur,
    handleFormSubmit,
  } = useLogin(type);

  const { isAuthenticated } = useAuth();

  useEffect(() => {
    if (isAuthenticated === true) {
      navigate("/to-dos");
    }
  }, [isAuthenticated, navigate]);

  // To-Do (medium): this can get moved to utils, we'll use this on all forms probably
  const handleSubmit = (event?: SubmitEvent<HTMLFormElement>) => {
    event?.preventDefault();
    const formData = new FormData(event?.target);

    handleFormSubmit(formData);
  };

  return (
    <div
      className={cn(
        "flex flex-col h-full sm:w-1/5 w-3/4 justify-center items-center",
        props.className,
      )}
    >
      <form className="flex flex-col w-full gap-4" onSubmit={handleSubmit}>
        <FieldGroup>
          <Field>
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              name="user_email"
              onBlur={handleEmailBlur}
              className={errors.email ? "border border-red-400" : ""}
              placeholder="email"
              maxLength={500}
            />
            {errors.email && <p>{errors.email}</p>}
          </Field>
          <Field>
            <Label htmlFor="password">Password</Label>
            <Input
              id="password"
              name="user_password"
              type="password"
              onBlur={handlePasswordBlur}
              className={errors.password ? "border border-red-400" : ""}
              placeholder="password"
              maxLength={500}
            />
            {errors.password && <p>{errors.password}</p>}
          </Field>
        </FieldGroup>
        <Button
          onClick={() => setType("authenticate")}
          variant="default"
          type="submit"
          disabled={!formIsTouched || !formIsValid || isSubmitting}
        >
          Log in
        </Button>
        <Button
          onClick={() => setType("register")}
          type="submit"
          disabled={!formIsTouched || !formIsValid || isSubmitting}
          variant="ghost"
        >
          Create account
        </Button>
      </form>
    </div>
  );
}
