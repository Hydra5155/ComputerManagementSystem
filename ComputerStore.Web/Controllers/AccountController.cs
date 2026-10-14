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
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return View(model);

            try
            {
                var client = _httpClientFactory.CreateClient("StoreAPI");

                // Gửi payload xác thực đăng nhập tới API Users
                var loginPayload = new
                {
                    Username = model.Username.Trim(),
                    Password = model.Password
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(loginPayload),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync("users/login", content);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Tài khoản chưa tồn tại hoặc mật khẩu không chính xác. Vui lòng đăng ký nếu chưa có tài khoản!");
                    return View(model);
                }

                var json = await response.Content.ReadAsStringAsync();
                var loggedCustomer = JsonSerializer.Deserialize<ProfileViewModel>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (loggedCustomer == null)
                {
                    ModelState.AddModelError("", "Không thể phân giải thông tin tài khoản. Vui lòng thử lại!");
                    return View(model);
                }

                // Lưu thông tin người dùng vào Session
                HttpContext.Session.SetString(UserSessionKey, JsonSerializer.Serialize(loggedCustomer));
                TempData["SuccessMessage"] = $"Đăng nhập thành công! Chào mừng {loggedCustomer.FullName}.";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi kết nối máy chủ xác thực: {ex.Message}");
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
                    Username = !string.IsNullOrWhiteSpace(model.PhoneNumber) ? model.PhoneNumber.Trim() : model.Email.Trim(),
                    FullName = model.FullName.Trim(),
                    PhoneNumber = model.PhoneNumber?.Trim() ?? "",
                    Email = model.Email?.Trim() ?? "",
                    Address = model.Address?.Trim() ?? "",
                    Password = model.Password
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync("users/register", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", string.IsNullOrWhiteSpace(errorMsg) ? "Số điện thoại/Email này đã được đăng ký tài khoản!" : errorMsg);
                    return View(model);
                }

                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi khi đăng ký tài khoản: {ex.Message}");
                return View(model);
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
                var payload = new
                {
                    Username = model.Username.Trim(),
                    NewPassword = model.NewPassword
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync("users/forgot-password", content);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại trên hệ thống!");
                    return View(model);
                }

                TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công! Hãy đăng nhập bằng mật khẩu mới.";
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi kết nối: {ex.Message}");
                return View(model);
            }
        }
    }
}