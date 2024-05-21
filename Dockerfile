FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build


WORKDIR /app

COPY ["/src/Api/Api.csproj", "Api/"]
COPY ["/src/Api/Program.cs", "Api/"]
COPY ["/src/Domain/Domain.csproj", "Domain/"]
COPY ["/src/Application/Application.csproj", "Application/"]
COPY ["/src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "Api/Api.csproj"

COPY . .
WORKDIR /app/Api

RUN dotnet build "Api.csproj" -c Release -o /app/build
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
ENV TZ Africa/Lagos
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Api.dll"]
