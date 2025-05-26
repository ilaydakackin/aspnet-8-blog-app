using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.ConcreteEfCore
{
    public static class SeedData
    {
        //Static: geri dönüşü olmayan
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateAsyncScope().ServiceProvider.GetService<BlogContext>();
            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                    new Tag { Text = "C#", url = "web-programlama", Color = TagColors.warning },
                    new Tag { Text = "ASP.NET", url = "asp-net", Color = TagColors.primary },
                    new Tag { Text = "php", url = "php", Color = TagColors.success },
                    new Tag { Text = "Python", url = "python", Color = TagColors.warning },
                    new Tag { Text = "fullstack", url = "fullstack", Color = TagColors.warning },
                    new Tag { Text = "Backend", url = "backend", Color = TagColors.secondary }
                );
                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    var password = BCrypt.Net.BCrypt.HashPassword("password");
                    context.Users.AddRange(
                        new User { UserName = "ilaydakackin", Name = "Ilayda Kackin", EMail = "kackinilayda@gmail.com", Password = password, Image = "myphoto.jpg" },
                        new User { UserName = "muharremkackin", Name = "Muharrem Kackin", EMail = "kackinmuharrem@gmail.com", Password = password, Image = "p2.jpg" }
                    );
                    context.SaveChanges();
                }
                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post
                        {
                            PostId = 1,
                            Title = "Asp.Core",
                            Content = "Asp.Core Kursu Hakkında",
                            Description = "DESC alanı",
                            url = "aspnet-core",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "Photo6.jpg",
                            UserId = 1,

                        },
                        new Post
                        {
                            PostId = 2,
                            Title = "php",
                            Content = "php Kursu Hakkında",
                            Description = "DESC alanı",
                            url = "php",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "Photo6.jpg",
                            UserId = 1,
                        },
                        new Post
                        {
                            PostId = 3,
                            Title = "Django",
                            Content = "Django Kursu Hakkında",
                            Description = "DESC alanı",
                            url = "django",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-9),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "Photo6.jpg",
                            UserId = 1,
                        },
                        new Post
                        {
                            PostId = 4,
                            Title = "React",
                            Content = "React Kursu Hakkında",
                            Description = "DESC alanı",
                            url = "react",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-60),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "Photo6.jpg",
                            UserId = 1,
                        },
                        new Post
                        {
                            PostId = 5,
                            Title = "Angular",
                            Content = "Angular Kursu Hakkında",
                            Description = "DESC alanı",
                            url = "Aangular",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-40),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "Photo6.jpg",
                            UserId = 1,
                        },
                        new Post
                        {
                            PostId = 6,
                            Title = "UI/UX",
                            Content = "UI/UX Kursu Hakkında",
                            Description = "DESC alanı",
                            url = "uı-ux",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-50),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "Photo6.jpg",
                            UserId = 1,
                        }
                    );
                    context.SaveChanges();
                }
                return;
            }
        }
    }
}
