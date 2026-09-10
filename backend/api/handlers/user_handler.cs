



using backend.api.repo;
using backend.api.services;

public class UserHandler
{
    
    Services services;
    Repo repos;
    public UserHandler(Services services,Repo repos)
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

    

}