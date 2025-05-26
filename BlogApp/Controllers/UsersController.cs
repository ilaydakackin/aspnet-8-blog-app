using BlogApp.Data.Abstract;
using BlogApp.Data.ConcreteEfCore;
using BlogApp.Entity;
using BlogApp.Models;
using BlogApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService; // Mail servisi için

        public UsersController(IUserRepository userRepository, IMailService mailService, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _mailService = mailService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.EMail == model.EMail);
                if (user == null)
                {
                    _userRepository.CreateUsers(new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        EMail = model.EMail,
                        Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        Image = "avatar.jpg"
                    });
                    return RedirectToAction("Login");
                }

            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı adı ya da E-Mail kullanımda.");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(x => x.EMail == model.EMail);


                if (isUser != null)
                {

                    if (!BCrypt.Net.BCrypt.Verify(model.Password, isUser.Password))
                    {
                        ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış.");
                        return View(model);
                    }

                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()),
                        new Claim(ClaimTypes.Name, isUser.UserName ?? ""),
                        new Claim(ClaimTypes.GivenName, isUser.Name ?? ""),
                        new Claim(ClaimTypes.UserData, isUser.Image ?? ""),
                        new Claim(ClaimTypes.Email, isUser.EMail ?? "")
                    };

                    if (isUser.EMail == "kackinilayda@gmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    //Beni hatırla seçeneği için
                    var authProporties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProporties
                    );
                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Profile(string username)
        {
            var user = _userRepository.Users
                .Include(x => x.Posts)
                .Include(x => x.Comments)
                .ThenInclude(x => x.Post)
                .FirstOrDefault(m => m.UserName == username);

            return View(user);
        }

        [HttpGet]
        public IActionResult ProfileEdit()
        {
            var id = HttpContext.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            if (id == null)
            {
                return NotFound();
            }

            var user = _userRepository.Users
               .Include(x => x.Posts)
               .Include(x => x.Comments)
               .ThenInclude(x => x.Post)
               .FirstOrDefault(m => m.UserId == Convert.ToInt32(id.Value));

           
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEdit(ProfileEditViewModel model, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var id = HttpContext.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier);
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userRepository.GetByIdAsync(Convert.ToInt32(id.Value));
            if (user == null)
            {
                return NotFound();
            }

            // Kullanıcı bilgilerini güncelle
            user.EMail = model.EMail;
            user.UserName = model.UserName;
            user.Name = model.Name;     
            user.Password = model.Password;
            user.Country = model.Country;
            user.City = model.City;
            user.Phone = model.Phone;
            user.Language = model.Language;
            user.Website = model.Website;

            if (Image != null && Image.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(Image.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Sadece JPG ve PNG formatındaki resimler yüklenebilir.");
                    return View(model);
                }

                // Eski resmi sil (varsa)
                if (!string.IsNullOrEmpty(user.Image))
                {
                    string existingFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", user.Image);
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }
                }

                // Yeni resmin adını oluştur ve kaydet
                string newFileName = Guid.NewGuid().ToString() + extension;
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", newFileName);

                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                user.Image = newFileName; // Yeni resim adını kaydet
            }
            await _userRepository.UpdateAsync(user);

            return RedirectToAction("Profile", new { username = model.UserName });
        }


        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest("E-posta adresi boş olamaz.");

                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                    return NotFound("Bu e-posta adresi sistemde kayıtlı değil.");

                // Rastgele bir şifre sıfırlama token'ı oluştur
                user.ResetToken = Guid.NewGuid().ToString();
                user.ResetTokenExpire = DateTime.UtcNow.AddHours(1); // Token 1 saat geçerli

                await _userRepository.UpdateAsync(user);

                // Kullanıcıya e-posta gönderme
                string resetLink = Url.Action("ResetPassword", "Users", new { token = user.ResetToken }, Request.Scheme);
                await _mailService.SendEmailAsync(email, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için <a href='{resetLink}'>buraya tıklayın</a>.");

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Ok("Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.");
        }

        public IActionResult ResetPassword([FromQuery] string token)
        {
            ViewBag.Token = token;
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string yeniSifre)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(yeniSifre))
                return BadRequest("Token veya şifre boş olamaz.");

            var user = await _userRepository.GetByResetTokenAsync(token);
            if (user == null)
                return BadRequest("Geçersiz veya süresi dolmuş token.");

            // Yeni şifreyi hashleyerek kaydet
            user.Password = BCrypt.Net.BCrypt.HashPassword(yeniSifre);
            user.ResetToken = null; // Token'ı sıfırlayalım
            user.ResetTokenExpire = null;

            await _userRepository.UpdateAsync(user);

            return Ok("Şifreniz başarıyla sıfırlandı.");
        }




    }
}



