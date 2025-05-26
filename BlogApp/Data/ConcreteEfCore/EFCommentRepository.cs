using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.ConcreteEfCore
{
    public class EfCommentRepository : ICommentRepository

    {
        private BlogContext _context;
        public EfCommentRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Comment> Comments => _context.Comments.AsQueryable();


        //Tüm Create işlemleri için ortak bir metod
        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
    }
}
