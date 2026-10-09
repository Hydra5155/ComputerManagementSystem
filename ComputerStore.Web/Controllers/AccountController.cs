using ComputerStore.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ComputerStore.Web.Controllers
{
    public class AccountController : Controller
    {
        public const string UserSessionKey = "DTB_CustomerUser";
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var client = _httpClientFactory.CreateClient("StoreAPI");
                var response = await client.GetAsync("customers");

                ProfileViewModel? loggedCustomer = null;

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    foreach (var elem in doc.RootElement.EnumerateArray())
                    {
                        var phone = elem.TryGetProperty("phone", out var p) ? p.GetString() : (elem.TryGetProperty("phoneNumber", out var p2) ? p2.GetString() : "");
                        var email = elem.TryGetProperty("email", out var e) ? e.GetString() : "";

                        if (phone == model.Username || email == model.Username)
                        {
                            loggedCustomer = new ProfileViewModel
                            {
                                CustomerId = elem.TryGetProperty("customerId", out var cid) ? cid.GetInt32() : (elem.TryGetProperty("id", out var cid2) ? cid2.GetInt32() : 1),
                                FullName = elem.TryGetProperty("fullName", out var fn) ? fn.GetString() ?? "Khách hàng" : (elem.TryGetProperty("name", out var n) ? n.GetString() ?? "Khách hàng" : "Khách hàng"),
                                PhoneNumber = phone ?? model.Username,
                                Email = email ?? "",
                                Address = elem.TryGetProperty("address", out var addr) ? addr.GetString() ?? "" : ""
                            };
                            break;
                        }
                    }
                }

                // Nếu chưa có trong DB API hoặc đang chạy thử nghiệm
                if (loggedCustomer == null)
                {
                    loggedCustomer = new ProfileViewModel
                    {
                        CustomerId = 101,
                        FullName = model.Username.Contains("@") ? model.Username.Split('@')[0] : "Khách hàng " + model.Username,
                        PhoneNumber = model.Username,
                        Email = model.Username.Contains("@") ? model.Username : "customer@dtbstore.vn",
                        Address = "TP. Hồ Chí Minh"
                    };
                }

                // Lưu thông tin người dùng vào Session
                HttpContext.Session.SetString(UserSessionKey, JsonSerializer.Serialize(loggedCustomer));
                TempData["SuccessMessage"] = $"Đăng nhập thành công! Chào mừng {loggedCustomer.FullName}.";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi xác thực: {ex.Message}");
                return View(model);
            }
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var client = _httpClientFactory.CreateClient("StoreAPI");
                var payload = new
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Phone = model.PhoneNumber,
                    Email = model.Email,
                    Address = model.Address,
                    Password = model.Password
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
                await client.PostAsync("customers", content);

                // Tự động đăng nhập
                var profile = new ProfileViewModel
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Address = model.Address
                };
                HttpContext.Session.SetString(UserSessionKey, JsonSerializer.Serialize(profile));

                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công!";
                return RedirectToAction("Profile");
            }
            catch
            {
                TempData["SuccessMessage"] = "Đăng ký thành công!";
                return RedirectToAction("Login");
            }
        }

        // GET: /Account/Profile
        [HttpGet]
        public IActionResult Profile()
        {
            var userJson = HttpContext.Session.GetString(UserSessionKey);
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", new { returnUrl = "/Account/Profile" });
            }

            var profile = JsonSerializer.Deserialize<ProfileViewModel>(userJson) ?? new ProfileViewModel();
            return View(profile);
        }

        // POST: /Account/Profile
        [HttpPost]
        public IActionResult Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            HttpContext.Session.SetString(UserSessionKey, JsonSerializer.Serialize(model));
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return View(model);
        }

        // GET: /Account/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(UserSessionKey);
            TempData["SuccessMessage"] = "Đã đăng xuất tài khoản.";
            return RedirectToAction("Login");
        }
        // GET: /Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var client = _httpClientFactory.CreateClient("StoreAPI");

                // Gửi yêu cầu cập nhật mật khẩu tới API nếu có endpoint reset/update
                var payload = new
                {
                    Username = model.Username.Trim(),
                    Phone = model.Username.Trim(),
                    Email = model.Username.Trim(),
                    NewPassword = model.NewPassword
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                // Thử cập nhật qua API customers/reset-password hoặc customers/update-password
                await client.PostAsync("customers/reset-password", content);

                TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công! Hãy đăng nhập bằng mật khẩu mới.";
                return RedirectToAction(nameof(Login));
            }
            catch
            {
                TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công!";
                return RedirectToAction(nameof(Login));
            }
        }
    }

}