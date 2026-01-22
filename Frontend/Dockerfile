# ===== build stage =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj first for better caching
COPY Frontend/TaskManager_Client/TaskManager_Client.csproj Frontend/TaskManager_Client/
COPY Backend/Contracts/Contracts.csproj Backend/Contracts/

RUN dotnet restore Frontend/TaskManager_Client/TaskManager_Client.csproj

# copy everything
COPY . .

RUN dotnet publish Frontend/TaskManager_Client/TaskManager_Client.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

# ===== runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskManager_Client.dll"]
