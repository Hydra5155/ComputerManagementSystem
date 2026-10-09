# Stage 1: Build Web và API
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy toàn bộ mã nguồn vào
COPY . .

# Chỉ restore và publish riêng project API
RUN dotnet publish "ComputerStore.API/ComputerStore.API.csproj" -c Release -o /app/publish/api

# Chỉ restore và publish riêng project Web
RUN dotnet publish "ComputerStore.Web/ComputerStore.Web.csproj" -c Release -o /app/publish/web

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy sản phẩm đã build sang môi trường runtime
COPY --from=build /app/publish/api ./api
COPY --from=build /app/publish/web ./web

# Copy file run.sh và cấp quyền thực thi
COPY run.sh ./run.sh
RUN chmod +x ./run.sh

EXPOSE 8080

ENTRYPOINT ["./run.sh"]