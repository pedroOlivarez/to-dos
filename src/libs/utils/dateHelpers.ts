function adjustedDate(date: Date): Date {
   const offSetMinutes = date.getTimezoneOffset();

   date.setMinutes(date.getMinutes() - offSetMinutes);

   return date;
}

export { adjustedDate };
