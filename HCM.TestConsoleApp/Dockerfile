#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HCM.TestConsoleApp/HCM.TestConsoleApp.csproj", "HCM.TestConsoleApp/"]
RUN dotnet restore "HCM.TestConsoleApp/HCM.TestConsoleApp.csproj"
COPY . .
WORKDIR "/src/HCM.TestConsoleApp"
RUN dotnet build "HCM.TestConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HCM.TestConsoleApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HCM.TestConsoleApp.dll"]