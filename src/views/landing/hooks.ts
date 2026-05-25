import { useMemo, useState, type FocusEvent } from 'react';
import { authenticate, type AuthenticationRequest } from '../../actions/Authenticate';

export function useLogin() {
   const [currentState, setCurrentState] = useState<AuthenticationRequest>({
      email: '',
      password: '',
   });
   const [errors, setErrors] = useState<Record<string, boolean>>({
      email: false,
      password: false,
   });
   const formIsTouched = useMemo(() => {
      return currentState.email || currentState.password;
   }, [currentState]);

   const formIsValid = useMemo(() => {
      for (const value of Object.values(errors)) {
         if (value) return false;
      }
      return true;
   }, [errors]);
   const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

   const validateEmail = (e: FocusEvent<HTMLInputElement, Element>) => {
      const email = e.currentTarget.value.trim();
      setCurrentState(prev => ({
         ...prev,
         email,
      }));
      setErrors(prev => ({
         ...prev,
         email: !email,
      }));
   };

   const validatePassword = (e: FocusEvent<HTMLInputElement, Element>) => {
      const password = e.currentTarget.value.trim();
      setCurrentState(prev => ({
         ...prev,
         password,
      }));
      setErrors(prev => ({
         ...prev,
         password: !password,
      }));
   };
   const handleFormSubmit = async (formData: FormData) => {
      setIsSubmitting(true);
      const email = formData.get('user_email')?.toString().trim();
      const password = formData.get('user_password')?.toString();
      if (!email) {
         console.error('Attempted to submit login form without email');
         return;
      }
      if (!password) {
         console.error('Attempted to submit login form without password');
         return;
      }
      const data: AuthenticationRequest = {
         email,
         password,
      };
      console.log('authenticating');

      const tokenmaybe = await authenticate(data);
      console.log({
         tokenmaybe,
      });
      setIsSubmitting(false);
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
