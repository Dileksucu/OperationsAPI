using Microsoft.EntityFrameworkCore;
using OperationsAPI.Models;

namespace OperationsAPI.Data
{
    public class OperationsAPIDbContext:DbContext
    {
        public OperationsAPIDbContext(DbContextOptions options):base(options) 
        { 
            //dependency ınjection 
        }

        public DbSet<Operation> Operations { get; set; } //  db models
    }
}
