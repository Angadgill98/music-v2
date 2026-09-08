


using backend.Database;

public class Auth_repo
{
    public Auth_repo()
    {
        
    }

    public async Task<(bool is_saved,Exception? err)> InsertUser(string name,string mail,string pass,Postgres_Context db)
    {
        try
        {
            User user=new();

            user.user_id=Guid.NewGuid();
            user.user_name=name;
            user.user_mail=mail;
            user.user_pass=pass;

            db.UsersTable.Add(user);

            await db.SaveChangesAsync();
            return (true,null);

        }
        catch (System.Exception err)
        {
            return (false,err);
        }
        
    }
}