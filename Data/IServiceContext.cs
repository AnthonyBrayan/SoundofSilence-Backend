
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
namespace Data
{
    public interface IServiceContext : IDisposable
    {
        DbSet<Rol> Rol { get; set; }
        DbSet<Users> Users { get; set; }
        int SaveChanges();
        IQueryable<T> Set<T>() where T : class;
        //Task<int> SaveChangesAsync();
        // Si necesitas agregar más métodos que sean propios de DbContext, puedes hacerlo aquí.
    }
}