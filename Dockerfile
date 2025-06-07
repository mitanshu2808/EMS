
# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as separate layers
COPY *.csproj ./
RUN dotnet restore

# Copy the full source and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Serve the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Replace EMS.Web.dll with your actual dll name (same as your .csproj file)
ENTRYPOINT ["dotnet", "EMS.Web.dll"]
