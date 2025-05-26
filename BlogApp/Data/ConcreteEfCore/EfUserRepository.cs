using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.ConcreteEfCore
{
    public class EfUserRepository : IUserRepository

    {
        private BlogContext _context;
        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<User> Users => _context.Users.AsQueryable();


        //Tüm Create işlemleri için ortak bir metod
        public void CreateUsers(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EMail == email);
        }

        public async Task<User?> GetByIdAsync(int Id)
        {
            return await _context.Users.FirstOrDefaultAsync(m => m.UserId == Id);
        }

        public async Task<User> GetByResetTokenAsync(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpire > DateTime.UtcNow);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


    }
}
