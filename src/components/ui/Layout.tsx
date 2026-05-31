import type { ComponentProps } from 'react';
import { cn } from '../../libs/utils/classNames';
import { useAuth } from '../auth/hooks';
import { Button } from './Button/Button';
import { logOut } from '../../actions/Authenticate';
import { useNavigate } from 'react-router';

export function Layout(props: ComponentProps<'div'>) {
   const navigate = useNavigate();
   const { isAuthenticated } = useAuth();
   const handleLogOut = async () => {
      await logOut();
      navigate('/login');
   };
   return (
      <div className="flex flex-col items-center justify-center h-full w-full bg-black/65">
         <div className="flex flex-row items-center justify-center min-h-20 max-h-20 w-full sticky top-0 border-b border-b-fuchsia-400 bg-black/80">
            <h1>Pedro's To-Do Submission</h1>
            {isAuthenticated ? (
               <Button onClick={handleLogOut} variant="outline" className="absolute right-2">
                  Log Out
               </Button>
            ) : null}
         </div>
         <div className="flex flex-row items-center justify-center h-full w-full overflow-y-auto">
            <div className={cn('w-full h-full flex flex-col items-center', props.className)}>{props.children}</div>
         </div>
      </div>
   );
}
