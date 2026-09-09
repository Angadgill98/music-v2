

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

    IResult CreateResponse(bool success,int statusCode,string message,object? data = null)
    {
        return Results.Json(
            new
            {
                success,
                message,
                data
            },
            statusCode: statusCode
        );
    }

    public async Task<IResult> SignIn(Postgres_Context db,string mail,string pass,JwtService jwt,HttpResponse res)
    {
        var user=await this.repos.auth_repo.GetUserByMail(db,mail);
        if (user==null)
        {
            Console.WriteLine("Server: Failed to SignIn User dosent exist");
            return  CreateResponse(
                false,
                401,
                "Invalid email or password"
            );
        }

        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(
            user!,
            user!.user_pass,
            pass
        );


        if (result == PasswordVerificationResult.Success)
        {
            var token=jwt.CreateToken(user);
            res.Cookies.Append(
                "auth_token",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                }
            );
            return CreateResponse(true,200,"Sign in successful",
                new
                {
                    user_id = user.user_id,
                    user_name = user.user_name,
                    user_mail = user.user_mail
                }
            );
        
        }
        else
        {
            Console.WriteLine("Server: Failed SingIn PassWrod dosent match");
            return CreateResponse(false,401,"Invalid email or password");
        }
    }

    public async Task<IResult> SignUp(Postgres_Context db,string mail,string pass,string name)
    {
        var (is_saved,err)=await this.repos.auth_repo.InsertUser(name,mail,pass,db);

        if (err!=null)
        {
            Console.WriteLine($"Server:failed to SignUp the user: \n{err}");
        
            return CreateResponse(false,500,"Failed to create user");
        }



        return CreateResponse(true,201,"User created successfully");
    }



}