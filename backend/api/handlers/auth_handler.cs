

using System.Threading.Tasks;
using backend.api.repo;
using backend.api.services;
using backend.Database;
using Microsoft.AspNetCore.Identity;

namespace backend.api.handlers;

public class AuthHandler{

    Services services;
    Repo repos;
    public AuthHandler(Services services,Repo repos)
    {
        this.services=services;
        this.repos=repos;
    }

    public async Task SignIn(Postgres_Context db,string mail,string pass)
    {
        var user=await this.repos.auth_repo.GetUserByMail(db,mail);
        if (user!=null)
        {
            Console.WriteLine("User dosent exist");
            return;
        }

        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(
            user!,
            user!.user_pass,
            pass
        );

        if (result == PasswordVerificationResult.Success)
        {
            
            return;
        }
        else
        {
            
            return;
        }
    }

    public async Task SignUp(Postgres_Context db,string mail,string pass,string name)
    {
        var (is_saved,err)=await this.repos.auth_repo.InsertUser(name,mail,pass,db);

        if (err!=null)
        {
            Console.WriteLine($"Server:failed to insert the user: \n{err}");
        
            return;
        }



        return;
    }
}