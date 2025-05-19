# Usa la imagen base para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solo el .csproj primero para restaurar dependencias
COPY ["APITaqueriaIguala.csproj", "."]
RUN dotnet restore "APITaqueriaIguala.csproj"

# Copia el resto del proyecto
COPY . .

# Compila en modo release
RUN dotnet publish "APITaqueriaIguala.csproj" -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "APITaqueriaIguala.dll"]
