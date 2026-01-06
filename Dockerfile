# --- Base image (runtime) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend.Api/backend.Api.csproj", "src/backend.Api/"]
COPY ["src/backend.Core/backend.Core.csproj", "src/backend.Core/"]
COPY ["src/backend.DataAccess/backend.DataAccess.csproj", "src/backend.DataAccess/"]
RUN dotnet restore "src/backend.Api/backend.Api.csproj"
COPY . .
WORKDIR "/src/src/backend.Api"
RUN dotnet build "backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --- Publish stage ---
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --- Final stage ---
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "backend.Api.dll"]
