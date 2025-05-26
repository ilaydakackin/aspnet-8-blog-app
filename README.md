# Blog App - ASP.NET Core MVC

Bu proje, ASP.NET Core MVC framework'ü kullanılarak geliştirilmiş tam özellikli bir blog uygulamasıdır. Kullanıcı yönetimi, post yayınlama, yorum sistemi ve e-posta servisleri gibi modern bir blog platformunun temel özelliklerini içerir.

## 📋 Proje Özeti

Blog App, kullanıcıların kayıt olabileceği, blog yazıları yazabileceği, yorum yapabileceği ve profil yönetimi yapabileceği kapsamlı bir web uygulamasıdır. Proje, modern web geliştirme teknikleri ve best practice'leri kullanılarak geliştirilmiştir.

## 🚀 Ana Özellikler

### 👤 Kullanıcı Yönetimi
- **Kullanıcı Kaydı**: E-posta doğrulama ile güvenli kayıt sistemi
- **Giriş/Çıkış**: Cookie tabanlı authentication
- **Profil Yönetimi**: Kullanıcı bilgilerini düzenleme ve profil resmi yükleme
- **Şifre Sıfırlama**: E-posta ile güvenli şifre sıfırlama
- **Rol Bazlı Yetkilendirme**: Admin ve normal kullanıcı rolleri

### 📝 Blog İşlevleri
- **Post Oluşturma**: Zengin metin editörü ile blog yazısı yazma
- **Post Düzenleme**: Mevcut yazıları güncelleme
- **Tag Sistemi**: Yazıları kategorilere ayırma
- **Yorum Sistemi**: Yazılara yorum yapabilme
- **Post Onaylama**: Admin onayı ile yayınlama

### 📧 E-posta Servisleri
- **SMTP Entegrasyonu**: Gmail SMTP ile e-posta gönderimi
- **Şifre Sıfırlama**: Güvenli token tabanlı sistem
- **Bildirim E-postaları**: Sistem bildirimleri

## 🛠️ Kullanılan Teknolojiler

### Backend
- **ASP.NET Core 6.0**: Web framework
- **Entity Framework Core**: ORM ve veritabanı işlemleri
- **SQL Server**: Veritabanı
- **Identity Framework**: Kullanıcı yönetimi
- **BCrypt.NET**: Şifre hashleme

### Frontend
- **Razor Pages**: Server-side rendering
- **Bootstrap**: UI framework
- **jQuery**: Client-side scripting

### E-posta & Güvenlik
- **MailKit**: E-posta gönderimi
- **Cookie Authentication**: Oturum yönetimi
- **Anti-forgery Tokens**: CSRF koruması

## 📁 Proje Yapısı

```
BlogApp/
├── Controllers/
│   ├── PostController.cs         # Blog post işlemleri
│   ├── UsersController.cs        # Kullanıcı yönetimi
│   └── HomeController.cs         # Ana sayfa
├── Models/
│   ├── ViewModels/
│   │   ├── LoginViewModel.cs     # Giriş formu
│   │   ├── RegisterViewModel.cs  # Kayıt formu
│   │   ├── ProfileEditViewModel.cs # Profil düzenleme
│   │   └── PostViewModel.cs      # Blog post görünümü
│   ├── AppUser.cs               # Identity kullanıcı modeli
│   └── AppRole.cs               # Identity rol modeli
├── Entity/
│   ├── User.cs                  # Kullanıcı entity
│   ├── Post.cs                  # Blog post entity
│   ├── Comment.cs               # Yorum entity
│   └── Tag.cs                   # Tag entity
├── Data/
│   ├── Abstract/                # Repository interface'leri
│   │   ├── IUserRepository.cs
│   │   ├── IPostRepository.cs
│   │   └── ICommentRepository.cs
│   └── ConcreteEfCore/         # Repository implementasyonları
│       ├── BlogContext.cs       # DbContext
│       ├── EfUserRepository.cs
│       ├── SeedData.cs         # Test verileri
│       └── BlogAppSeedData.cs  # Identity seed data
├── Services/
│   ├── IEmailSender.cs         # E-posta service interface
│   ├── SmtpEmailSender.cs      # SMTP implementasyonu
│   ├── IMailService.cs         # Mail service interface
│   └── MailService.cs          # MailKit implementasyonu
├── Middlewares/
│   └── CheckUserExistMiddleware.cs # Kullanıcı kontrolü
└── Program.cs                   # Uygulama yapılandırması
```

## 🏗️ Kurulum ve Çalıştırma

### Gereksinimler
- .NET 6.0 SDK veya üzeri
- SQL Server (LocalDB desteklenir)
- Visual Studio 2022 veya VS Code
- Gmail hesabı (E-posta servisi için)

### Adım 1: Projeyi Klonlayın
```bash
git clone [repository-url]
cd BlogApp
```

### Adım 2: Veritabanı Ayarları
`appsettings.json` dosyasında connection string'i güncelleyin:
```json
{
  "ConnectionStrings": {
    "SQLServerConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogAppDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Adım 3: E-posta Ayarları
`appsettings.json` dosyasına e-posta ayarlarını ekleyin:
```json
{
  "EmailSender": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSSL": true,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

### Adım 4: Migration ve Veritabanı Oluşturma
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Adım 5: Uygulamayı Çalıştırın
```bash
dotnet run
```

## 💾 Veritabanı Modeli

### Ana Tablolar
- **Users**: Kullanıcı bilgileri ve profil verileri
- **Posts**: Blog yazıları ve metadata
- **Comments**: Yazılara yapılan yorumlar
- **Tags**: Kategori/etiket sistemi
- **AspNetUsers**: Identity kullanıcı tablosu
- **AspNetRoles**: Identity rol tablosu

### İlişkiler
- User → Posts (1:N)
- User → Comments (1:N)
- Post → Comments (1:N)
- Post ↔ Tags (N:N)

## 🔐 Güvenlik Özellikleri

### Kimlik Doğrulama
```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
    });
```

### Şifre Güvenliği
- BCrypt ile hash'leme
- Minimum 6 karakter gerekliliği
- Güçlü şifre politikaları

### CSRF Koruması
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ProfileEdit(ProfileEditViewModel model)
```

## 📧 E-posta Servisleri

### SMTP Yapılandırması
```csharp
public class SmtpEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient(_host, _port)
        {
            Credentials = new NetworkCredential(_username, _password),
            EnableSsl = _enableSSL
        };
        return client.SendMailAsync(new MailMessage(_username ?? "", email, subject, message) 
        { 
            IsBodyHtml = true 
        });
    }
}
```

### MailKit Implementasyonu
```csharp
public async Task SendEmailAsync(string email, string subject, string message)
{
    var mimeMessage = new MimeMessage();
    mimeMessage.From.Add(new MailboxAddress("Blog App", "sender@gmail.com"));
    mimeMessage.To.Add(new MailboxAddress("User", email));
    mimeMessage.Subject = subject;
    mimeMessage.Body = new TextPart(TextFormat.Html) { Text = message };
    
    using var smtp = new SmtpClient();
    await smtp.ConnectAsync("smtp.gmail.com", 587, false);
    smtp.Authenticate("username", "password");
    await smtp.SendAsync(mimeMessage);
}
```

## 🎯 Repository Pattern

### Interface Tanımı
```csharp
public interface IUserRepository
{
    IQueryable<User> Users { get; }
    void CreateUsers(User user);
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByResetTokenAsync(string token);
    Task UpdateAsync(User user);
}
```

### Entity Framework Implementasyonu
```csharp
public class EfUserRepository : IUserRepository
{
    private BlogContext _context;
    
    public IQueryable<User> Users => _context.Users.AsQueryable();
    
    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.EMail == email);
    }
}
```

## 🚀 Önemli Özellikler

### Seed Data ile Test Verileri
```csharp
public static void TestVerileriniDoldur(IApplicationBuilder app)
{
    var context = app.ApplicationServices.CreateAsyncScope()
        .ServiceProvider.GetService<BlogContext>();
    
    if (!context.Tags.Any())
    {
        context.Tags.AddRange(
            new Tag { Text = "C#", url = "web-programlama", Color = TagColors.warning },
            new Tag { Text = "ASP.NET", url = "asp-net", Color = TagColors.primary }
        );
        context.SaveChanges();
    }
}
```

### Async/Await Pattern
- Tüm veritabanı işlemleri asenkron
- Performans optimizasyonu
- Non-blocking I/O operations

### File Upload Sistemi
```csharp
public async Task<IActionResult> ProfileEdit(ProfileEditViewModel model, IFormFile Image)
{
    if (Image != null && Image.Length > 0)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(Image.FileName).ToLower();
        
        if (allowedExtensions.Contains(extension))
        {
            string newFileName = Guid.NewGuid().ToString() + extension;
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", newFileName);
            
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                await Image.CopyToAsync(fileStream);
            }
        }
    }
}
```

## 🎨 Frontend Özellikleri

### Responsive Design
- Bootstrap 5 kullanımı
- Mobile-first approach
- Cross-browser uyumluluk

### AJAX ile Dinamik Yorum Sistemi
```javascript
[HttpPost]
public JsonResult AddComment(int PostId, string Text)
{
    var entity = new Comment
    {
        Text = Text,
        PublishedOn = DateTime.Now,
        PostId = PostId,
        UserId = int.Parse(userId ?? "")
    };
    
    return Json(new { username, Text, entity.PublishedOn, avatar });
}
```

## 🔧 Yapılandırma

### Dependency Injection
```csharp
// Repository'ler
builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

// Servisler
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IMailService, MailService>();
```

### Custom Routing
```csharp
app.MapControllerRoute(
    name: "post-details",
    pattern: "posts/details/{url}",
    defaults: new { controller = "Post", action = "Details" }
);

app.MapControllerRoute(
    name: "posts_by_tag",
    pattern: "posts/tag/{tag}",
    defaults: new { controller = "Post", action = "Index" }
);
```

## 🚨 Hata Yönetimi ve Debugging

### Yaygın Hatalar

1. **E-posta Gönderim Hatası**
   - Gmail App Password kullanımı gerekli
   - SMTP ayarlarını kontrol edin

2. **Veritabanı Bağlantı Hatası**
   - Connection string'i doğrulayın
   - SQL Server'ın çalıştığından emin olun

3. **Authentication Sorunları**
   - Cookie ayarlarını kontrol edin
   - Claims yapılandırmasını gözden geçirin

## 📚 Öğrenme Hedefleri

Bu proje aşağıdaki konuları kapsar:
- **ASP.NET Core MVC**: Model-View-Controller pattern
- **Entity Framework Core**: Code-First approach
- **Identity Framework**: Kullanıcı yönetimi
- **Repository Pattern**: Veri erişim katmanı
- **Dependency Injection**: IoC Container
- **Email Services**: SMTP ve MailKit
- **Security**: Authentication, Authorization, CSRF
- **File Upload**: Resim yükleme ve yönetimi
- **Async Programming**: Task-based operations

## 🎯 Geliştirme Önerileri

1. **Caching**: Redis ile performans artırımı
2. **Logging**: Serilog entegrasyonu
3. **API**: Web API endpoints ekleme
4. **Testing**: Unit ve Integration testleri
5. **Docker**: Containerization
6. **CI/CD**: GitHub Actions ile deployment

## 📖 Kaynaklar

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [MailKit Documentation](https://github.com/jstedfast/MailKit)

## 🤝 Katkıda Bulunma

Bu proje eğitim amaçlı geliştirilmiştir. Katkılarınızı ve önerilerinizi memnuniyetle karşılıyoruz!

## 📝 Lisans

Bu proje MIT lisansı altında yayınlanmıştır ve eğitim amaçlı kullanım için serbesttir.
# aspnet-8-blog-app
