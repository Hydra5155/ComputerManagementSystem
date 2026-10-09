# 1. Dùng đúng SDK 9.0
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Sao chép mã nguồn
COPY . .

# Chỉ restore riêng project Web (kèm theo các project core phụ thuộc)
RUN dotnet restore "ComputerStore.Web/ComputerStore.Web.csproj"

# Build và Publish ra thư mục publish
WORKDIR "/src/ComputerStore.Web"
RUN dotnet publish "ComputerStore.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Dùng Runtime ASP.NET 9.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "ComputerStore.Web.dll"]
