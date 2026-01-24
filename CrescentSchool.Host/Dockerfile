# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY CrescentSchool.API/CrescentSchool.API.csproj CrescentSchool.API/
COPY CrescentSchool.BLL/CrescentSchool.BLL.csproj CrescentSchool.BLL/
COPY CrescentSchool.DAL/CrescentSchool.DAL.csproj CrescentSchool.DAL/
COPY CrescentSchool.Models/CrescentSchool.Models.csproj CrescentSchool.Models/
COPY CrescentSchool.Host/CrescentSchool.Host.csproj CrescentSchool.Host/

# Restore
RUN dotnet restore CrescentSchool.Host/CrescentSchool.Host.csproj

# Copy everything
COPY . .

# Publish
WORKDIR /src/CrescentSchool.Host
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "CrescentSchool.Host.dll"]
