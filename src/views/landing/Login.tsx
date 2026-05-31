import { useEffect, useState, type ComponentProps } from 'react';
import { Field, FieldGroup } from '../../components/ui/Field';
import { Label } from '../../components/ui/Label';
import { Input } from '../../components/ui/Input';
import { Button } from '../../components/ui/Button/Button';
import { useLogin } from './hooks';
import { cn } from '../../libs/utils/classNames';
import { useAuth } from '../../components/auth/hooks';
import { useNavigate } from 'react-router';

export function Login(props: ComponentProps<'div'>) {
   const [type, setType] = useState<'authenticate' | 'register'>('authenticate');
   const navigate = useNavigate();
   const { errors, formIsTouched, formIsValid, isSubmitting, validateEmail, validatePassword, handleFormSubmit } =
      useLogin(type);

   const { isAuthenticated } = useAuth();

   useEffect(() => {
      if (isAuthenticated === true) {
         navigate('/');
      }
   }, [isAuthenticated, navigate]);

   return (
      <div className={cn('flex flex-col h-full sm:w-1/5 w-3/4 justify-center items-center', props.className)}>
         <form className="flex flex-col w-full gap-4" action={handleFormSubmit}>
            <FieldGroup>
               <Field>
                  <Label htmlFor="email">Email</Label>
                  <Input
                     id="email"
                     name="user_email"
                     onBlur={validateEmail}
                     className={errors.email ? 'border border-red-400' : ''}
                     placeholder="email"
                  />
                  {errors.email && <p>{errors.email}</p>}
               </Field>
               <Field>
                  <Label htmlFor="password">Password</Label>
                  <Input
                     id="password"
                     name="user_password"
                     type="password"
                     onBlur={validatePassword}
                     className={errors.password ? 'border border-red-400' : ''}
                     placeholder="password"
                  />
                  {errors.password && <p>{errors.password}</p>}
               </Field>
            </FieldGroup>
            <Button
               onClick={() => setType('authenticate')}
               variant="default"
               type="submit"
               disabled={!formIsTouched || !formIsValid || isSubmitting}
            >
               Log in
            </Button>
            <Button
               onClick={() => setType('register')}
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
