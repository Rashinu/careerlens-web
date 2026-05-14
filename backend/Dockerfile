FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CareerLens.slnx ./
COPY CareerLens.API/CareerLens.API.csproj CareerLens.API/
COPY CareerLens.Application/CareerLens.Application.csproj CareerLens.Application/
COPY CareerLens.Domain/CareerLens.Domain.csproj CareerLens.Domain/
COPY CareerLens.Infrastructure/CareerLens.Infrastructure.csproj CareerLens.Infrastructure/
COPY CareerLens.Shared/CareerLens.Shared.csproj CareerLens.Shared/

RUN dotnet restore CareerLens.API/CareerLens.API.csproj

COPY . .

RUN dotnet publish CareerLens.API/CareerLens.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CareerLens.API.dll"]
