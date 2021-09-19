# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY GraphQL/*.csproj ./GraphQL/
COPY IntegrationTests/*.csproj ./IntegrationTests/
RUN dotnet restore

# copy everything else and build app
COPY GraphQL/. ./GraphQL/
WORKDIR /source/GraphQL
RUN dotnet publish -c release -o /app --no-restore --no-cache

# This is the port that the app listens on - and what ECS will direct traffic towards 
EXPOSE 80

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "GraphQL.dll", "--verbosity detailed"]
