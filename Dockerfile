#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["./src/Selah.WebAPI/Selah.WebAPI.csproj", "src/Selah.WebAPI/"]
COPY ["./src/Selah.Domain/Selah.Domain.csproj", "src/Selah.Domain/"]
COPY ["./src/Selah.Infrastructure/Selah.Infrastructure.csproj", "src/Selah.Infrastructure/"]
COPY ["./src/Selah.Application/Selah.Application.csproj", "src/Selah.Application/"]


RUN dotnet restore "src/Selah.WebAPI/Selah.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Selah.WebAPI"
RUN dotnet build "Selah.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Selah.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app


COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Selah.WebAPI.dll"]