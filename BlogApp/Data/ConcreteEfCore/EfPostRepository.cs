using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.ConcreteEfCore
{
    public class EfPostRepository : IPostRepository

    {
        private BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Post> Posts => _context.Posts.AsQueryable();

        //Tüm Create işlemleri için ortak bir metod
        public void CreatePosts(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void EditPosts(Post post)
        {
            var entity = _context.Posts.FirstOrDefault(i => i.PostId == post.PostId);
            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.url = post.url;
                entity.IsActive = post.IsActive;

                _context.SaveChanges();
            }
        }
    }
}
