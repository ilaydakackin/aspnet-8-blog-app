using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;


        public PostController(IPostRepository postrepository, ICommentRepository commentRepository)
        {
            _postRepository = postrepository;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index([FromQuery] string? tag)
        {
            var claims = User.Claims;
            var posts = _postRepository.Posts
                .Include(m => m.User)
                .Where(i => i.IsActive);
            
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.url == tag));
            }
            var postList = await posts.ToListAsync();
            return View(new PostViewModel { Posts = postList });
        }

        public async Task<IActionResult> Deteils(string url)
        {
            return View(await _postRepository
                .Posts
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(p => p.url == url));
        }

        [HttpPost]
        public JsonResult AddComment(int PostId, string Text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);
            var entity = new Comment
            {
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                UserId = int.Parse(userId ?? "")
            };
            _commentRepository.CreateComment(entity);
            return Json(new
            {
                username,
                Text,
                entity.PublishedOn,
                avatar

            });

        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _postRepository.CreatePosts(
                    new Post
                    {
                        Title = model.Title,
                        Content = model.Content,
                        url = model.url,
                        UserId = int.Parse(userId ?? ""),
                        PublishedOn = DateTime.Now,
                        Image = "p1.jpg",
                        IsActive = false
                    }
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRepository.Posts;

            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(i => i.UserId == userId);
            }
            return View(await posts.ToListAsync());
        }
        [HttpGet]
        [Authorize]
        public IActionResult Edit(int? id)
        {
            var post = _postRepository.Posts.FirstOrDefault(i => i.PostId == id);

            return View(new CreatePostViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                Decription = post.Description,
                url = post.url,
                IsActive = post.IsActive

            });
        }
        [Authorize]
        [HttpPost]
        public IActionResult Edit(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Post
                {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Decription,
                    Content = model.Content,
                    url = model.url,

                };

                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityToUpdate.IsActive = model.IsActive;
                }
                _postRepository.EditPosts(entityToUpdate);
                return RedirectToAction("List");
            }
            return View(model);
        }
    }
}
