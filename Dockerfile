# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy solution file
COPY ["ECommerce.sln", "./"]

# Copy project files with CORRECT paths based on your folder structure
COPY ["Presentation/API/API.csproj", "Presentation/API/"]
COPY ["Presentation/DTO/DTO.csproj", "Presentation/DTO/"]
COPY ["Presentation/Middlewares/Middlewares.csproj", "Presentation/Middlewares/"]
COPY ["Common/Helper/Helper.csproj", "Common/Helper/"]
COPY ["Common/Settings/Settings.csproj", "Common/Settings/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Application/Application.csproj", "Application/"]

# Restore dependencies
RUN dotnet restore "Presentation/API/API.csproj"

# Copy everything
COPY . .

# Build
WORKDIR "/src/Presentation/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 80

ENTRYPOINT ["dotnet", "API.dll"]