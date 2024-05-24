FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build


WORKDIR /app

COPY ["/src/API/API.csproj", "API/"]
COPY ["/src/Application/Application.csproj", "Application/"]
COPY ["/src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["/src/Domain/Domain.csproj", "Domain/"]

RUN dotnet restore "API/API.csproj"

COPY . .
WORKDIR /app/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
