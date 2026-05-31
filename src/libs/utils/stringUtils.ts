export function isValidEmail(s: string) {
   const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
   return regex.test(s);
}

export function containsNumber(s: string) {
   const regex = /\d/;
   return regex.test(s);
}
