#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["backend.Recycle/backend.Recycle.csproj", "backend.Recycle/"]
RUN dotnet restore "backend.Recycle/backend.Recycle.csproj"
COPY . .
WORKDIR "/src/backend.Recycle"
RUN dotnet build "backend.Recycle.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "backend.Recycle.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "backend.Recycle.dll"]
