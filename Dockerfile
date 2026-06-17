# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the csproj files and restore dependencies
# This is done before copying the rest of the code to cache the restored layers
COPY ["src/TierLab.Api/TierLab.Api.csproj", "src/TierLab.Api/"]
COPY ["src/TierLab.Application/TierLab.Application.csproj", "src/TierLab.Application/"]
COPY ["src/TierLab.Domain/TierLab.Domain.csproj", "src/TierLab.Domain/"]
COPY ["src/TierLab.Infrastructure/TierLab.Infrastructure.csproj", "src/TierLab.Infrastructure/"]
RUN dotnet restore "./src/TierLab.Api/TierLab.Api.csproj"

# Copy the remaining source code
COPY . .

# Build the project
WORKDIR "/src/src/TierLab.Api"
RUN dotnet build "./TierLab.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TierLab.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TierLab.Api.dll"]
