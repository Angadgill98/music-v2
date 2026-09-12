
using backend.api.handlers;
using backend.api.repo;
using backend.api.services;
using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.api;

record SignUpReq
{
    public string name { get; set; } = "";
    public string mail { get; set; } = "";
    public string pass { get; set; } = "";

}


record SignInReq
{
    public string mail { get; set; } = "";
    public string pass { get; set; } = "";

}


public class Api
{
    WebApplication app;
    Handlers handlers;
    Services services;
    Repo repos;

    public Api(WebApplication app)
    {
        this.app=app;

        this.repos=new();

        this.services=new();

        this.handlers=new(this.services,this.repos);

        
    }

    public void RegisterAuthRoutes()
    {
        var router=this.app.MapGroup("/auth");
        router.MapPost("/sign-in",async ([FromBody] SignInReq req,[FromServices] Postgres_Context db,[FromServices] JwtService jwt,HttpResponse res) =>
        {
            var mail=req.mail;
            var pass=req.pass;

            
            return await this.handlers.auth.SignIn(db,mail,pass,jwt,res);

        });
        router.MapPost("/sign-up",async ([FromBody] SignUpReq req,[FromServices] Postgres_Context db) =>
        {
            var mail=req.mail;
            var pass=req.pass;
            var name=req.name;

            return await this.handlers.auth.SignUp(db,mail,pass,name);

            
        });

    }

    public void RegisterApiRoutes()
    {

        



    }

    public void RegisterUserProfileRoutes()
    {
        var router=this.app.MapGroup("/api/profile-user");

        router.MapGet("/get-profile",async (HttpContext context,[FromServices] Postgres_Context db) =>
        {
            Guid userId =Guid.Parse(context.User.FindFirst("user_id")!.Value);

            return await this.handlers.user.GetProfileUser(db,userId);

        }).RequireAuthorization();

        router.MapGet("/get-musicain-profile-private",async (HttpContext context,[FromServices] Postgres_Context db) =>
        {
            Guid musician_id =Guid.Parse(context.User.FindFirst("user_musician_id")!.Value);
            
            return await this.handlers.user.GetProfileMusicianPrivate(db,musician_id);

        }).RequireAuthorization();

        router.MapPost("/create-playlist",async (HttpContext context,[FromServices] Postgres_Context db) =>
        {
            Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);

            
        });

        router.MapPost("/delete-playlist",async (HttpContext context,[FromServices] Postgres_Context db) =>
        {
            Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);

            
        });

        router.MapPost("/remove-like",async (HttpContext context,[FromServices] Postgres_Context db) =>
        {
            Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);

            
        });

    }

    public void RegisterDashBoardRoutes()
    {
        var router=this.app.MapGroup("/api/");

        router.MapGet("/dashboard",async () =>
        {
            
        });
    }

    public void RegisterSongRoutes()
    {
        var router=this.app.MapGroup("/api/song");

        router.MapGet("/get-song",async () =>
        {
            
        });

        router.MapGet("/like-song",async () =>
        {
            
        });

        router.MapGet("/remove-like",async () =>
        {
            
        });

        router.MapGet("/add-to-playlist",async () =>
        {
            
        });
        
        router.MapGet("/remove-from-playlist",async () =>
        {
            
        });
    }

    public void RegisterAlbumRoutes()
    {
        var router=this.app.MapGroup("/api/albums");

        router.MapGet("/get-albums",async () =>
        {
            
        });

        router.MapGet("/get-album",async () =>
        {
            
        });

        router.MapGet("/remove-like-albums",async () =>
        {
            
        });

        router.MapGet("/like-albums",async () =>
        {
            
        });
    }

    public void RegisterMusicianRoutes()
    {
        var router=this.app.MapGroup("/api/musician");

        router.MapGet("/get-albums",async () =>
        {
            
        });

        router.MapGet("/get-songs",async () =>
        {
            
        });

        router.MapGet("/create-album",async () =>
        {
            
        });

        router.MapGet("/upload-song",async () =>
        {
            
        });

        router.MapGet("/change-visibility-song",async () =>
        {
            
        });

        router.MapGet("/change-visibility-album",async () =>
        {
            
        });

        router.MapGet("/add-song-to-album",async () =>
        {
            
        });
    }


}