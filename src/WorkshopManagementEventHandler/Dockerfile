FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build-env
WORKDIR /app

# Copy necessary files
COPY Infrastructure.Messaging/Infrastructure.Messaging.csproj ./Infrastructure.Messaging
RUN dotnet restore Infrastructure.Messaging

# Copy and restore explicit project
COPY WorkshopManagementEventHandler/WorkshopManagementEventHandler.csproj ./WorkshopManagementEventHandler

RUN dotnet restore WorkshopManagementEventHandler

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out WorkshopManagementEventHandler

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 5250/tcp
ENV ASPNETCORE_URLS http://*:5250
#HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5250/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "Pitstop.WorkshopManagementEventHandler.dll"]
