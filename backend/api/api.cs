
using backend.api.handlers;

namespace backend.api;

record SignUpReq
{
    string name;
    string mail;
    string pass;

}


record SignInReq
{
    string mail;
    string pass;

}
public class Api
{
    WebApplication app;

    public Api(WebApplication app)
    {
        this.app=app;
    }

    public void RegisterAuthRoutes()
    {
        var router=this.app.MapGroup("/api/auth");
        AuthHandler handler=new();
        router.MapGet("/sign-in",(SignInReq req) =>
        {
            
        });
        router.MapPost("/sign-up",(SignUpReq req) =>
        {
            
        });

    }







}