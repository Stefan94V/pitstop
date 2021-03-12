FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build-env
WORKDIR /app

# Copy necessary files
COPY Infrastructure.Messaging/Infrastructure.Messaging.csproj ./Infrastructure.Messaging
RUN dotnet restore Infrastructure.Messaging

# Copy and restore explicit project
COPY TimeService/TimeService.csproj ./TimeService

RUN dotnet restore TimeService

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out TimeService

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:5.0-focal
COPY --from=build-env /app/out .

#EXPOSE 8800

# Start
ENTRYPOINT ["dotnet", "Pitstop.TimeService.dll"]
