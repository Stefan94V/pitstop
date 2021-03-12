FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build-env
WORKDIR /app

# Copy necessary files
COPY Infrastructure.Messaging/Infrastructure.Messaging.csproj ./Infrastructure.Messaging
RUN dotnet restore Infrastructure.Messaging

# Copy and restore explicit project
COPY InvoiceService/InvoiceService.csproj ./InvoiceService

RUN dotnet restore InvoiceService

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out InvoiceService

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 5260/tcp
ENV ASPNETCORE_URLS http://*:5260
#HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5250/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "Pitstop.InvoiceService.dll"]
