# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy solution and project files
COPY *.sln .
COPY OrderManagementSystemApi/*.csproj ./OrderManagementSystemApi/
COPY OrderManagementSystemApplication/*.csproj ./OrderManagementSystemApplication/
COPY OrderManagementSystemDomain/*.csproj ./OrderManagementSystemDomain/
COPY OrderManagementSystemInfrastructure/*.csproj ./OrderManagementSystemInfrastructure/

# Restore dependencies
RUN dotnet restore

# Copy all source files
COPY OrderManagementSystemApi/. ./OrderManagementSystemApi/
COPY OrderManagementSystemApplication/. ./OrderManagementSystemApplication/
COPY OrderManagementSystemDomain/. ./OrderManagementSystemDomain/
COPY OrderManagementSystemInfrastructure/. ./OrderManagementSystemInfrastructure/

# Build and publish
WORKDIR /source/OrderManagementSystemApi
RUN dotnet publish -c Release -o /app 

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app ./

# Run the application
ENTRYPOINT ["dotnet", "OrderManagementSystemApi.dll"]