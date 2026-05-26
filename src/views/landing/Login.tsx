import { type ComponentProps } from "react";
import { Field, FieldGroup } from "../../components/ui/Field";
import { Label } from "../../components/ui/Label";
import { Input } from "../../components/ui/Input";
import { Button } from "../../components/ui/Button";
import { useLogin } from "./hooks";
import { cn } from "../../libs/utils/classNames";

export function Login(props: ComponentProps<"div">) {
  const {
    errors,
    formIsTouched,
    formIsValid,
    isSubmitting,
    validateEmail,
    validatePassword,
    handleFormSubmit,
  } = useLogin();
  return (
    <div
      className={cn(
        "flex flex-col h-full sm:w-1/5 w-3/4 justify-center items-center",
        props.className,
      )}
    >
      <form className="flex flex-col w-full gap-4" action={handleFormSubmit}>
        <FieldGroup>
          <Field>
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              name="user_email"
              onBlur={validateEmail}
              className={errors.email ? "border border-red-400" : ""}
              placeholder={errors.email ? "Email is required" : undefined}
            />
          </Field>
          <Field>
            <Label htmlFor="password">Password</Label>
            <Input
              id="password"
              name="user_password"
              type="password"
              onBlur={validatePassword}
              className={errors.password ? "border border-red-400" : ""}
              placeholder={errors.password ? "Password is required" : undefined}
            />
          </Field>
        </FieldGroup>
        <Button
          variant="default"
          type="submit"
          disabled={!formIsTouched || !formIsValid || isSubmitting}
        >
          Log in
        </Button>
      </form>
    </div>
  );
}
