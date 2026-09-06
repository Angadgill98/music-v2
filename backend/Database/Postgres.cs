

using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class Postgres_Context : DbContext
{
    public Postgres_Context(DbContextOptions<Postgres_Context> options): base(options){}
}