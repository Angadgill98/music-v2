
using backend.api.handlers;
using backend.api.repo;
using backend.api.services;
using backend.Database;

namespace backend.api;

record SignUpReq
{
    public readonly string name;
    public readonly string mail;
    public readonly string pass;

}


record SignInReq
{
    public readonly string mail;
    public readonly string pass;

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
        router.MapGet("/sign-in",(SignInReq req,Postgres_Context db) =>
        {
            var mail=req.mail;
            var pass=req.pass;

            this.handlers.auth.SignIn(db,mail,pass);

        });
        router.MapPost("/sign-up",(SignUpReq req,Postgres_Context db) =>
        {
            var mail=req.mail;
            var pass=req.pass;
            var name=req.name;

            this.handlers.auth.SignUp(db,mail,pass,name);

            
        });

    }







}