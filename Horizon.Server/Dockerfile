FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Horizon.Server.csproj .
RUN dotnet restore Horizon.Server.csproj

COPY . .
RUN dotnet publish Horizon.Server.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app .
EXPOSE 5000

ENTRYPOINT ["dotnet", "Horizon.Server.dll"]