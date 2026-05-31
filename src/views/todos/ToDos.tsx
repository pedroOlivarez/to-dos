import { useMemo, type ComponentProps } from 'react';
import { AddToDoDialog } from '../../components/todos/dialogs/AddToDoDialog/AddToDoDialog';
import { EditToDoDialog } from '../../components/todos/dialogs/EditToDoDialog/EditToDoDialog';
import { AddToDoButton } from '../../components/todos/AddToDoButton';
import { ResultDisplay } from '../../components/todos/ResultDisplay';
import { cn } from '../../libs/utils/classNames';
import { useToDosView } from './hooks';
import { useSearchParams } from 'react-router';
import { Pagination } from '../../components/ui/Pagination';
import { Search } from '../../components/ui/Search';

export function ToDos(props: ComponentProps<'div'>) {
   const [searchParams, setSearchParams] = useSearchParams();

   const page = useMemo(() => {
      return searchParams.get('page') ?? '1';
   }, [searchParams]);

   const searchTerm = useMemo(() => {
      return searchParams.get('q') ?? undefined;
   }, [searchParams]);

   const {
      modal,
      selectedToDo,
      data,
      isLoading,
      isError,
      handleAddToDoButtonClick,
      handleSelectToDo,
      handleAddToDoDialogOpenChange,
      handleEditToDoDialogOpenChange,
      handleUpdate,
      handleCreate,
      handleArchive,
   } = useToDosView({ page, searchTerm });

   const showPagination = useMemo(() => data && data.meta && data.meta.totalPages > data.data.length, [data]);

   const handleSearch = (term: string | null) => {
      if (term) {
         searchParams.set('q', term);
      } else {
         searchParams.delete('q');
      }
      setSearchParams(searchParams);
   };

   return (
      <>
         <div
            className={cn(
               'flex flex-col min-h-full w-full p-2 pr-18 sm:items-start  items-center gap-2',
               props.className,
            )}
         >
            <Search onInput={handleSearch} debounceDelay={500} className="bg-black/65" />
            {showPagination ? <Pagination {...data!.meta} /> : null}

            <ResultDisplay
               onSelectToDo={handleSelectToDo}
               toDos={data?.data ?? null}
               isLoading={isLoading}
               isError={isError}
            />
         </div>
         {/* this placement doesn't stick to bottom on mobile after scroll :anguish: */}
         <AddToDoButton
            onClick={handleAddToDoButtonClick}
            // replace this with cool custom animation to help guide user on how to create a to-do on fresh register
            className={!data?.data.length && !modal ? 'animate-bounce' : undefined}
         />
         <AddToDoDialog
            open={modal === 'ADD_TODO'}
            onOpenChange={handleAddToDoDialogOpenChange}
            onSubmit={handleCreate}
         />
         <EditToDoDialog
            open={modal === 'EDIT_TODO'}
            onOpenChange={handleEditToDoDialogOpenChange}
            defaultValues={selectedToDo}
            onSubmit={handleUpdate}
            onArchive={handleArchive}
         />
      </>
   );
}
