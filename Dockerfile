# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy solution file
COPY ["ECommerce.sln", "./"]

# Copy all project files
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DTO/DTO.csproj", "DTO/"]
COPY ["Middlewares/Middlewares.csproj", "Middlewares/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Common/Common.csproj", "Common/"]

# Restore all dependencies
RUN dotnet restore "API/API.csproj"

# Copy everything
COPY . .

# Build
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Important environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 80

# Your main DLL
ENTRYPOINT ["dotnet", "API.dll"]