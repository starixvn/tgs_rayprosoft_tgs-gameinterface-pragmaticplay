#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 9090

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.PragmaticPlay/Tgs.GameInterface.PragmaticPlay.csproj", "Tgs.GameInterface.PragmaticPlay/"]
COPY ["Tgs.GameInterface.PragmaticPlay.Core/Tgs.GameInterface.PragmaticPlay.Core.csproj", "Tgs.GameInterface.PragmaticPlay.Core/"]
RUN dotnet restore "./Tgs.GameInterface.PragmaticPlay/./Tgs.GameInterface.PragmaticPlay.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.PragmaticPlay"
RUN dotnet build "./Tgs.GameInterface.PragmaticPlay.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameInterface.PragmaticPlay.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.PragmaticPlay.dll"]