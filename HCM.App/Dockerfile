FROM nginx:1 AS base
WORKDIR /usr/share/nginx/html
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HCM.App/HCM.App.csproj", "HCM.App/"]
RUN dotnet restore "HCM.App/HCM.App.csproj"
COPY . .
WORKDIR "/src/HCM.App"
RUN dotnet build "HCM.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HCM.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
