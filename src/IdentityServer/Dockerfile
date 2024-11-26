# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG APP_UID=1000
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and restore dependencies
#COPY ./src/IdentityServer/IdentityServer.csproj ./IdentityServer/
COPY ./src ./
RUN dotnet restore "./IdentityServer/IdentityServer.csproj"

# Copy entire source code
#COPY . .

# Build the project
WORKDIR IdentityServer
RUN dotnet build "IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "IdentityServer.dll"]
