# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY TemperatureAlertSystem.csproj .
RUN dotnet restore

# Copy the remaining source code and build the application
COPY . .
RUN dotnet publish -c Release -o /app

# Install Altair package
RUN dotnet add package GraphQL.Server.Ui.Altair

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Set the environment variables (if required)
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

# Expose the HTTP port
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "TemperatureAlertSystem.dll"]

COPY . /app

