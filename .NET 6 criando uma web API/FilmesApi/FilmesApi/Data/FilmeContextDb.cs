using FilmesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Data;

public class FilmeContextDb : DbContext
{
    public FilmeContextDb(DbContextOptions<FilmeContextDb> options) : base(options)
    {

    }

    public DbSet<FilmeModel> Filmes { get; set; }
}
