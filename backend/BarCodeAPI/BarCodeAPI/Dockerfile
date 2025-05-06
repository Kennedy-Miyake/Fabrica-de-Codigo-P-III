# syntax=docker/dockerfile:1

# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia somente o .csproj e restaura
COPY ["BarCodeAPI/BarCodeAPI.csproj", "BarCodeAPI/"]
RUN dotnet restore "BarCodeAPI/BarCodeAPI.csproj"

# Copia somente o código da API
COPY ["BarCodeAPI/", "BarCodeAPI/"]
WORKDIR /src/BarCodeAPI
RUN dotnet build "BarCodeAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# 2. Publish Stage
FROM build AS publish
WORKDIR /src/BarCodeAPI
RUN dotnet publish "BarCodeAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 3. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
# Define porta unica e ambiente
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Cria usuário não-root
ARG APP_UID=1000
ARG APP_GID=1000
RUN addgroup --gid $APP_GID appgroup \
     && adduser --uid $APP_UID --ingroup appgroup --disabled-password appuser \
     && mkdir -p /app \
     && chown appuser:appgroup /app 

USER appuser
WORKDIR /app

# Copia os artefatos publicados
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BarCodeAPI.dll"]
