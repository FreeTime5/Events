FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .
COPY Events.Api/*.csproj ./Events.Api/
COPY Events.Infrastructure/*.csproj ./Events.Infrastructure/
COPY Events.Application/*.csproj ./Events.Application/
COPY Events.Domain/*.csproj ./Events.Domain/
COPY Events.DataAccess/*.csproj ./Events.DataAccess/
COPY Events.Tests/*.csproj ./Events.Tests/
RUN dotnet restore

COPY . .
WORKDIR /source/Events.Api
RUN dotnet publish -c release -o /app /p:EnvironmentName=Development

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Events.Api.dll"]