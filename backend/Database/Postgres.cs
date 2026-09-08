

using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class Postgres_Context : DbContext
{
    public Postgres_Context(DbContextOptions<Postgres_Context> options): base(options){}

    public DbSet<User> UsersTable{ get; set; }
}



public class User
{
    public Guid user_id{ get; set; }

    public string user_name{ get; set; } = "";
    public string user_mail{ get; set; } = "";
    public string user_pass{ get; set; } = "";

    public User()
    {
        
    }

}