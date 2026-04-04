# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy solution file
COPY ["ECommerce.sln", "./"]

# Copy all project files - ALL at the same level (sibling folders)
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Middlewares/Middlewares.csproj", "Middlewares/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Helper/Helper.csproj", "Helper/"]
COPY ["Domain/Domain.csproj", "Domain/"]

# Restore
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

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 80

ENTRYPOINT ["dotnet", "API.dll"]