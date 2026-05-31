import { useRouteError, Link } from "react-router";
import blueberry from "../../public/blueberry.svg";

export function ErrorBoundary() {
  console.log("In the error boundary");
  const error = useRouteError() as { message: string };

  console.log({ error });
  return (
    <div className="flex flex-col justify-center items-center w-full h-full">
      <img
        src={blueberry}
        alt="blueberry"
        className="sm:min-h-60 min-h-40 sm:min-w-60 min-w-60 sm:max-h-60 max-h-60 sm:max-w-60 max-w-60"
      />
      <p>This is embarrassing. Something went wrong</p>
      <p>{error.message}</p>
      <Link to="/">Back to homepage</Link>
    </div>
  );
}
