﻿## Base image
#FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
## Build stage
#FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#ARG TARGETARCH
#WORKDIR /src
#
## Copy and restore dependencies
#RUN echo ls
#RUN ls
#RUN ls -la
#RUN ls -la /src
#
#COPY ["Domain/Domain.csproj", "Domain/"]
#COPY ["Shared/Shared.csproj", "Shared/"]
#COPY ["Persistance/Persistance.csproj", "Persistance/"]
#COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
#COPY ["Application/Application.csproj", "Application/"]
#COPY ["Presentations/Presentations.csproj", "Presentations/"]
#
## Restore dependencies
#RUN dotnet restore -a $TARGETARCH -r linux-$TARGETARCH "Presentations/Presentations.csproj" 
#
## Copy entire source code
##COPY ["src/Presentations/certificate.pfx", "Presentations/"]
##COPY ./src .
#COPY . .
#
## Build and publish
#WORKDIR "/src/Presentations"
#
#RUN dotnet publish "Presentations.csproj" \
#    -c $BUILD_CONFIGURATION \
#    -o /app/publish \
#    -r linux-$TARGETARCH \
#    --self-contained true \
#    --no-restore \
#    /p:PublishTrimmed=true \
#    /p:PublishSingleFile=false \
#    /p:DebugSymbols=false \
#    /p:DebugType=None
#    
## Final image
#FROM base AS final
#ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true \
#    DOTNET_RUNNING_IN_CONTAINER=true \
#    ASPNETCORE_URLS=http://+:8080
#
#WORKDIR /app
#
## Copy published artifacts
#COPY --from=build /app/publish .
#
#RUN adduser --disabled-password --gecos "" appuser && \
#    chown -R appuser:appuser /app
#USER appuser
#
#HEALTHCHECK --interval=30s --timeout=5s --start-period=5s --retries=3 \
#    CMD wget -q --spider http://localhost:8080/health || exit 1
##    CMD curl -f http://localhost:8080/health || exit 1
#    
#ENTRYPOINT ["dotnet", "Presentations.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG APP_UID=1000
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM base AS final

WORKDIR /app
COPY . .

ENTRYPOINT ["dotnet", "Presentations.dll"]