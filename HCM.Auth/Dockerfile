#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HCM.Auth/HCM.Auth.csproj", "HCM.Auth/"]
RUN dotnet restore "HCM.Auth/HCM.Auth.csproj"
COPY . .
WORKDIR "/src/HCM.Auth"
RUN dotnet build "HCM.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HCM.Auth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HCM.Auth.dll"]