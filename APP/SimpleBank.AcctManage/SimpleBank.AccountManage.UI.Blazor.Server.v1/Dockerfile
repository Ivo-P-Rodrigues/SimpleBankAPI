#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimpleBank.AccountManage.UI.Blazor.Server.v1/SimpleBank.AccountManage.UI.Blazor.Server.v1.csproj", "SimpleBank.AccountManage.UI.Blazor.Server.v1/"]
RUN dotnet restore "SimpleBank.AccountManage.UI.Blazor.Server.v1/SimpleBank.AccountManage.UI.Blazor.Server.v1.csproj"
COPY . .
WORKDIR "/src/SimpleBank.AccountManage.UI.Blazor.Server.v1"
RUN dotnet build "SimpleBank.AccountManage.UI.Blazor.Server.v1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleBank.AccountManage.UI.Blazor.Server.v1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleBank.AccountManage.UI.Blazor.Server.v1.dll"]