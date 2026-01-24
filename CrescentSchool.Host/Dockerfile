# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY CrescentSchool.sln .

# Copy project files
COPY CrescentSchool.API/CrescentSchool.API.csproj CrescentSchool.API/
COPY CrescentSchool.BLL/CrescentSchool.BLL.csproj CrescentSchool.BLL/
COPY CrescentSchool.DAL/CrescentSchool.DAL.csproj CrescentSchool.DAL/
COPY CrescentSchool.Models/CrescentSchool.Models.csproj CrescentSchool.Models/
COPY CrescentSchool.Host/CrescentSchool.Host.csproj CrescentSchool.Host/

# Restore dependencies
RUN dotnet restore CrescentSchool.Host/CrescentSchool.Host.csproj

# Copy everything else
COPY . .

# Build & publish
WORKDIR /src/CrescentSchool.Host
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Run app
ENTRYPOINT ["dotnet", "CrescentSchool.Host.dll"]
