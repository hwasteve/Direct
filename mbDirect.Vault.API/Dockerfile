#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["mbDirect.Vault.API/mbDirect.Vault.API.csproj", "mbDirect.Vault.API/"]
COPY ["mbDirect.Vault.Models/mbDirect.Vault.Models.csproj", "mbDirect.Vault.Models/"]
COPY ["mbDirect.Vault.Repo/mbDirect.Vault.Repo.csproj", "mbDirect.Vault.Repo/"]
RUN dotnet restore "mbDirect.Vault.API/mbDirect.Vault.API.csproj"
COPY . .
WORKDIR "/src/mbDirect.Vault.API"
RUN dotnet build "mbDirect.Vault.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mbDirect.Vault.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mbDirect.Vault.API.dll"]