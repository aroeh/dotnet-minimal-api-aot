#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.


# TODO: Creating the image and container with compose works....but
# Something is wrong when trying to call the MongoDb container from the api container
# Was able to call the MongoDB container when running the http profile locally

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Install clang/zlib1g-dev dependencies for publishing to native
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MinimalApiAotAot.csproj", "MinimalApiAotAot/"]
RUN dotnet restore "./MinimalApiAotAot/MinimalApiAotAot.csproj"
COPY . MinimalApiAotAot/
WORKDIR "/src/MinimalApiAotAot"
RUN dotnet build "./MinimalApiAotAot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MinimalApiAotAot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["./MinimalApiAotAot"]