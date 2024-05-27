#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

#WORKDIR /app

#COPY ["/src/API/API.csproj", "API/"]
#COPY ["/src/Application/Application.csproj", "Application/"]
#COPY ["/src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
#COPY ["/src/Domain/Domain.csproj", "Domain/"]

#RUN dotnet restore "API/API.csproj"

#COPY . .
#WORKDIR /app/API

#RUN dotnet build "API.csproj" -c Release -o /app/build
#RUN dotnet publish "API.csproj" -c Release -o /app/publish

#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
#WORKDIR /app
#EXPOSE 8080

#COPY --from=build /app/publish .

#ENTRYPOINT ["dotnet", "API.dll"]


# Use the official .NET 8.0 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the .csproj files and restore dependencies
COPY ["src/API/API.csproj", "API/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]

RUN dotnet restore "API/API.csproj"

# Copy the remaining project files
COPY . .

# Set the working directory to the API project folder
WORKDIR /app/API

# Build the API project in release mode
RUN dotnet build "API.csproj" -c Release -o /app/build

# Publish the API project to the publish directory
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# Use the official ASP.NET Core runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Expose port 8080 to the outside world
EXPOSE 8080

# Copy the published files from the build stage
COPY --from=build /app/publish .

# Set the entry point for the container to run the API project
ENTRYPOINT ["dotnet", "API.dll"]
