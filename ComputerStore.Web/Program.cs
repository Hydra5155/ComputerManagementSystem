namespace ComputerStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Đăng ký HttpClient trỏ đến cổng API đang chạy
            builder.Services.AddHttpClient("StoreAPI", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5293/");
            });

            // Thêm hỗ trợ Session và Cache
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // Đăng ký HttpClient gọi sang API (chú ý có dấu gạch chéo ở cuối)
            builder.Services.AddHttpClient("StoreAPI", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7275/api/");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });
            // Đăng ký HttpContextAccessor để hỗ trợ inject trong Razor Views
            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Product}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
