﻿using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
namespace BlogApp.Data.ConcreteEfCore
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<User> Users => Set<User>();

    }
}
