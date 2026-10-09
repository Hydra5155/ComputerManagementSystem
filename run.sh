#!/bin/sh

# Chạy Backend API ngầm ở cổng nội bộ 5000
ASPNETCORE_URLS=http://+:5000 dotnet /app/api/ComputerStore.API.dll &

# Đợi 3 giây để API kịp khởi động và kết nối database
sleep 3

# Chạy Frontend MVC lắng nghe cổng do Render cấp qua biến $PORT (mặc định 8080 nếu chạy local)
export ASPNETCORE_URLS="http://+:${PORT:-8080}"
exec dotnet /app/web/ComputerStore.Web.dll