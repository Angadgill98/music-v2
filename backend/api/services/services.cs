
using backend.api.repo;

namespace backend.api.services;

public class Services
{
    Auth_Service auth_service;
    public Services()
    {
        this.auth_service=new();
    }    
}