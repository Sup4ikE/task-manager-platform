# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj first (better caching)
COPY ./Api/Api.csproj ./Api/
COPY ./Core/Core.csproj ./Core/
COPY ./Infrastructure/Infrastructure.csproj ./Infrastructure/

RUN dotnet restore ./Api/Api.csproj

# copy everything
COPY . .

RUN dotnet publish ./Api/Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Api.dll"]