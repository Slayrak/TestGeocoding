# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY TestGeocoding/*.csproj ./TestGeocoding/
COPY TestGeocoding.Tests/*.csproj ./TestGeocoding.Tests/
RUN dotnet restore

# copy everything else and build app
COPY TestGeocoding/. ./TestGeocoding/
COPY TestGeocoding.Tests/. ./TestGeocoding.Tests/
WORKDIR /source
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "TestGeocoding.dll"]