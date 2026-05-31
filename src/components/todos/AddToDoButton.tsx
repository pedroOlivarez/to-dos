import type { ComponentProps } from 'react';
import { cn } from '../../libs/utils/classNames';
import { PlusIcon } from 'lucide-react';

export function AddToDoButton({ className, ...rest }: ComponentProps<'button'>) {
   return (
      <button
         className={cn(
            'absolute flex items-center justify-center rounded-2xl p-7 bg-black/85 bottom-2 right-2 min-h-6 max-h-6 min-w-6 max-w-6',
            className,
         )}
         {...rest}
      >
         <span>
            <PlusIcon size={32} />
         </span>
      </button>
   );
}
