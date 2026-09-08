

using System.Threading.Tasks;
using backend.api.repo;
using backend.api.services;
using backend.Database;

namespace backend.api.handlers;

public class AuthHandler{

    Services services;
    Repo repos;
    public AuthHandler(Services services,Repo repos)
    {
        this.services=services;
        this.repos=repos;
    }

    public void SignIn(Postgres_Context db,string mail,string pass)
    {
        
    }

    public async Task<(bool,Exception?)> SignUp(Postgres_Context db,string mail,string pass,string name)
    {
        var (is_saved,err)=await this.repos.auth_repo.InsertUser(name,mail,pass,db);

        if (err!=null)
        {
            Console.WriteLine($"Server:failed to insert the user: \n{err}");
        
            return (is_saved,err);

        }



        return (is_saved,err);
    }
}