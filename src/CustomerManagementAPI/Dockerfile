FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build-env
WORKDIR /app

# Copy necessary files
COPY Infrastructure.Messaging/Infrastructure.Messaging.csproj ./Infrastructure.Messaging
RUN dotnet restore Infrastructure.Messaging

# Copy and restore explicit project
COPY CustomerManagementAPI/CustomerManagementAPI.csproj ./CustomerManagementAPI

RUN dotnet restore CustomerManagementAPI

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out CustomerManagementAPI

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 5100/tcp

ENV ASPNETCORE_URLS http://*:5100
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5100/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "Pitstop.CustomerManagementAPI.dll"]
