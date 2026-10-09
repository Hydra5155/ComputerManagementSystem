# 1. Base SDK để biên dịch
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép toàn bộ mã nguồn vào container
COPY . .

# Khôi phục dependencies
RUN dotnet restore

# Biên dịch ra thư mục publish
WORKDIR "/src/ComputerStore.Web"
RUN dotnet publish "ComputerStore.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime môi trường chạy sản phẩm
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Cấu hình cổng cho Render (Render dùng cổng 10000 mặc định)
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "ComputerStore.Web.dll"]