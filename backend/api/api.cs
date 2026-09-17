
using System.Threading.Channels;
using backend.api.handlers;
using backend.api.repo;
using backend.api.services;
using backend.api.workers;
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
    Workers workers;

    public Api(WebApplication app)
    {
        this.app=app;

        this.repos=new();

        this.services=new();

        this.handlers=new(this.services,this.repos);

        this.workers=new(this.services);
    }

    public void RegisterAuthRoutes()
    {
        var router=this.app.MapGroup("/auth");
        router.MapPost("/sign-in",async ([FromBody] SignInReq req,[FromServices] Postgres_Context db,[FromServices] JwtService jwt,HttpResponse res) =>
        {
            var mail=req.mail;
            var pass=req.pass;


            Console.WriteLine($"Req came for sign in for mail:{mail} adn pass:{pass}");
            return await this.handlers.auth.SignIn(db,mail,pass,jwt,res);

        });
        router.MapPost("/sign-up",async ([FromBody] SignUpReq req,[FromServices] Postgres_Context db) =>
        {
            var mail=req.mail;
            var pass=req.pass;
            var name=req.name;
            Console.WriteLine($"Req came for sign up for name:{name} mail:{mail} adn pass:{pass}");

            return await this.handlers.auth.SignUp(db,mail,pass,name);

            
        });

        router.MapGet("/is-valid",(HttpContext context) =>
        {
            Console.WriteLine($"Req came for token vali");

            return Results.Ok(context.User.Identity?.IsAuthenticated == true);
        })
        .RequireAuthorization();

    }

    public void RegisterApiRoutes()
    {
        this.RegisterMusicianRoutes();
        



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

        // router.MapPost("/create-playlist",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
            
        //     return await this.handlers.user.CreatePlaylist(db,user_id,playlist_name);

        // });

        // router.MapPost("/delete-playlist",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);

        //     return await this.handlers.user.DeletePlaylist(db,user_id,playlist_id);
        // });

        // router.MapPost("/remove-like",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);

        //     return await this.handlers.user.RemoveLike(db,user_id,song_id);
        // });

        // router.MapPost("/add-like",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);

        //     return await this.handlers.user.RemoveLike(db,user_id,song_id);
        // });

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

        // router.MapGet("/get-song",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     this.handlers.song.GetSong(db,song_id);
        // });

        // router.MapGet("/like-song",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
        //     this.handlers.song.AddLike(db,user_id,song_id);
        // });

        // router.MapGet("/remove-like",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
        //     this.handlers.song.RemoveLike(db,user_id,song_id);
        // });

        // router.MapGet("/add-to-playlist",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
        //     this.handlers.song.AddToPlaylist(db,user_id,playlist_id);
        // });
        
        // router.MapGet("/remove-from-playlist",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
        //     this.handlers.song.RemoveFromPlaylist(db,user_id,playlist_id);            
        // });
    }

    public void RegisterAlbumRoutes()
    {
        var router=this.app.MapGroup("/api/albums");

        router.MapGet("/get-albums",async (HttpContext context,[FromServices] Postgres_Context db)  =>
        {
            Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
        });

        // router.MapGet("/get-album",async (HttpContext context,[FromServices] Postgres_Context db)=>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
        //     this.handlers.album.GetAlbum(db,user_id,album_id);            
         
        // });

        // router.MapGet("/remove-like-albums",async (HttpContext context,[FromServices] Postgres_Context db)=>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
        //     this.handlers.album.RemoveLikeAlbum(db,user_id,album_id);            
        // });

        // router.MapGet("/like-albums",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
        //     this.handlers.album.LikeAlbum(db,user_id,album_id);            
        // });
    }

    public void RegisterMusicianRoutes()
    {
        var router=this.app.MapGroup("/api/musician");
        
        router.MapPost("/musician-reg",async (HttpContext context,[FromServices] Postgres_Context db,MusicianRegReq req) =>
        {
            Guid user_id =Guid.Parse(context.User.FindFirst("user_id")!.Value);
            return await this.handlers.musician.NewMusicianReg(db,user_id,req.musician_name);            
        }).RequireAuthorization();

        router.MapGet("/get-albums",async (HttpContext context,[FromServices] Postgres_Context db) =>
        {
            Guid musician_id =Guid.Parse(context.User.FindFirst("musician_id")!.Value);
            return await this.handlers.musician.GetAlbums(db,musician_id);            
        });

        router.MapGet("/get-songs",async (HttpContext context,[FromServices] Postgres_Context db) =>
        {
            Guid musician_id =Guid.Parse(context.User.FindFirst("musician_id")!.Value);
            this.handlers.musician.GetSongs(db,musician_id);            
        }); 

        // router.MapGet("/create-album",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid musician_id =Guid.Parse(context.User.FindFirst("musician_id")!.Value);
        //     this.handlers.musician.CreateAlbum(db,musician_id,album_name);            
        // });


        // router.MapGet("/change-visibility-song",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid musician_id =Guid.Parse(context.User.FindFirst("musician_id")!.Value);
        //     this.handlers.musician.ChangeVisibilitySong(db,musician_id,song_id,visibility);            
        // });

        // router.MapGet("/change-visibility-album",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid musician_id =Guid.Parse(context.User.FindFirst("musician_id")!.Value);
        //     this.handlers.musician.ChangeVisibilityAlbum(db,musician_id,song_id,visibility);            
            
        // });

        // router.MapGet("/add-song-to-album",async (HttpContext context,[FromServices] Postgres_Context db) =>
        // {
        //     Guid musician_id =Guid.Parse(context.User.FindFirst("musician_id")!.Value);
        //     this.handlers.musician.AddSongToAlbum(db,musician_id,album_id,song_id);            
        // });

        router.MapPost("/start-upload-context", async (HttpContext context,[FromServices] Postgres_Context db,UploadContextReq up_con) =>
        {

            Guid musician_id =Guid.Parse(context.User.FindFirst("musician_id")!.Value);
            var channel = Channel.CreateBounded<bool>(1);

            var receiver = channel.Reader;
            var sender = channel.Writer;
            Upload_worker_task task=new();
            task.context=up_con;
            task.is_context=Allowed.upload_context;
            task.operation="new";
            task.response_sender=sender;
            await this.workers.upload_dispatcher.Send(task);

            bool result=await receiver.ReadAsync();

            if(!result)
            {
                return Results.BadRequest();
            }

            return Results.Ok();
        });
 
        router.MapPost("/chunk-context",async (HttpContext context,[FromServices] Postgres_Context db,[FromForm] ChunkContextReq chunk_context)=>
        {
            Guid musician_id =Guid.Parse(context.User.FindFirst("musician_id")!.Value);

            var channel = Channel.CreateBounded<bool>(1);

            var receiver = channel.Reader;
            var sender = channel.Writer;
            Upload_worker_task task=new();

            task.context=chunk_context;
            task.is_context=Allowed.chunk_context;
            task.operation="new";
            task.response_sender=sender;

            await this.workers.upload_dispatcher.Send(task);

            bool result=await receiver.ReadAsync();

            if(!result)
            {
                return Results.BadRequest();
            }
            
            return Results.Ok();

        }).DisableAntiforgery();


        router.MapPost("/complete-upload",async (HttpContext context,[FromServices] Postgres_Context db,UploadContextReq up_con) =>
        {

            Guid musician_id =Guid.Parse(context.User.FindFirst("musician_id")!.Value);
            var channel = Channel.CreateBounded<bool>(1);

            var receiver = channel.Reader;
            var sender = channel.Writer;
            Upload_worker_task task=new();
            task.context=up_con;
            task.is_context=Allowed.upload_context;
            task.operation="complete";
            task.response_sender=sender;


            await this.workers.upload_dispatcher.Send(task);

            bool result=await receiver.ReadAsync();

            if(!result)
            {
                return Results.BadRequest();
            }

            var file_name=up_con.upload_id+"_"+up_con.song_name;

            var (song_id, err) =
                await this.repos.song_repo.CreateSong(
                    db,
                    file_name,
                    up_con.song_name,
                    musician_id
                );

            if (err != null || song_id == null)
            {
                return Results.BadRequest();
            }

            var (musician, Musicianerr) =
                await this.repos.musician_repo.GetMusicianById(
                    db,
                    musician_id
                );

            if (Musicianerr != null || musician == null)
            {
                return Results.BadRequest();
            }

            var (is_save, Musicianerr2) =
                await this.repos.musician_repo.RegisterSong(
                    db,
                    musician,
                    song_id.Value
                );

            if (Musicianerr2 != null )
            {
                return Results.BadRequest();
            }

            return Results.Ok();
        }).DisableAntiforgery();;
   
    }


}