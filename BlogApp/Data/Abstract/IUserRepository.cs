using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void CreateUsers(User user);

        Task<User> GetByEmailAsync(string email);
        Task<User> GetByResetTokenAsync(string token);
        Task<User?> GetByIdAsync(int Id);
        Task UpdateAsync(User user);


    }

}
