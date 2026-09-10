
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
        var router=this.app.MapGroup("/api/auth");
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

    public void RegisterUserRoutes()
    {
        var router=this.app.MapGroup("/api/");
    }

}