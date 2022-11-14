FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BudgetData.csproj", "BudgetData/"]
RUN dotnet restore "BudgetData/BudgetData.csproj"
COPY . .
WORKDIR "/src/BudgetData"
RUN dotnet build "BudgetData.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetData.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetData.dll"]
