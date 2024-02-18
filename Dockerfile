FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-step
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app
COPY --from=build-step /app .
ENV ASPNETCORE_URLS http://+:8081
ENTRYPOINT ["dotnet", "Venta.Api.dll"]
