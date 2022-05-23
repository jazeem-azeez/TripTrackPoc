#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TripTrackerApi/TripTrackerApi.csproj", "TripTrackerApi/"]
COPY ["PersistenceServices/PersistenceServices.csproj", "PersistenceServices/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["DomainServices/DomainServices.csproj", "DomainServices/"]
COPY ["ExternalInfrastructure/ExternalInfrastructure.csproj", "ExternalInfrastructure/"]
RUN dotnet restore "TripTrackerApi/TripTrackerApi.csproj"
COPY . .
WORKDIR "/src/TripTrackerApi"
RUN dotnet build "TripTrackerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TripTrackerApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TripTrackerApi.dll"]