using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.ConcreteEfCore
{
    public class EfTagRepository : ITagRepository

    {
        private BlogContext _context;
        public EfTagRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Tag> Tags => _context.Tags.AsQueryable();


        //Tüm Create işlemleri için ortak bir metod
        public void CreateTags(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }

        //public void CreateTags(Tag tag)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
