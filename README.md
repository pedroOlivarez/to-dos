install dotnet sdk
https://dotnet.microsoft.com/en-us/download/dotnet/10.0
in Api directory
create a copy of appsettings.json and name it appsettings.Development.json
request invite to neon.tech workspace to get access to connectionstring and paste value under "Neon": {"ConnectionString"}

## terminal 1

`dotnet run --project ./Api/api.csproj`
if working on the api
`dotnet watch --project ./Api/api.csproj`

## terminal 2

`cd Client`
`npm run dev`
