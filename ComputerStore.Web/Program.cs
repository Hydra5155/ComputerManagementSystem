namespace ComputerStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Lấy URL của API từ biến môi trường/appsettings, nếu không có thì mặc định gọi cổng nội bộ 5000
            var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000/api/";
            if (!apiBaseUrl.EndsWith("/"))
            {
                apiBaseUrl += "/";
            }

            // Đăng ký HttpClient duy nhất gọi sang API
            builder.Services.AddHttpClient("StoreAPI", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                // Bỏ qua kiểm tra chứng chỉ SSL (hữu ích cho môi trường dev và container)
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });

            // Thêm hỗ trợ Session và Cache
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Đăng ký HttpContextAccessor để hỗ trợ inject trong Razor Views
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseSession();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Product}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}