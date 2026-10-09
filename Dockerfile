# Stage 1: Build cả 2 ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy toàn bộ solution và source code
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish ComputerStore.API
RUN dotnet publish "ComputerStore.API/ComputerStore.API.csproj" -c Release -o /app/publish/api

# Publish ComputerStore.Web
RUN dotnet publish "ComputerStore.Web/ComputerStore.Web.csproj" -c Release -o /app/publish/web

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy kết quả build vào các thư mục tương ứng
COPY --from=build /app/publish/api ./api
COPY --from=build /app/publish/web ./web

# Copy file script khởi động
COPY run.sh ./run.sh
RUN chmod +x ./run.sh

# Cổng Render thường map
EXPOSE 8080

ENTRYPOINT ["./run.sh"]