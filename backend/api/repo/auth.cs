


using backend.Database;
using Microsoft.AspNetCore.Identity;

public class Auth_repo
{
    public Auth_repo()
    {
        
    }

    public async Task<(bool is_saved,Exception? err)> InsertUser(string name,string mail,string pass,Postgres_Context db)
    {
        try
        {

            var hasher = new PasswordHasher<User>();
            User user=new();

            string hashedPassword = hasher.HashPassword(user, pass);

            user.user_id=Guid.NewGuid();
            user.user_name=name;
            user.user_mail=mail;
            user.user_pass=hashedPassword;

            db.UsersTable.Add(user);

            await db.SaveChangesAsync();
            return (true,null);

        }
        catch (System.Exception err)
        {
            return (false,err);
        }
        
    }

    public async Task<User?> GetUserByMail(Postgres_Context db,string mail)
    {
        try
        {
            User? user=db.UsersTable.FirstOrDefault(user=>user.user_mail==mail);
            return user;
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}