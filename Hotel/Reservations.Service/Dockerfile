FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Reservations.Service.csproj", "./"]
RUN dotnet restore "Reservations.Service.csproj"
COPY . .
RUN dotnet publish "Reservations.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Reservations.Service.dll"]