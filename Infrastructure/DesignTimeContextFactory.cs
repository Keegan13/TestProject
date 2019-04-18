namespace Infrastructure
{
    using Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            return new ApplicationContext((new DbContextOptionsBuilder()).UseSqlServer(@"Server=.\SQLExpress;AttachDbFilename=./ProjectDb.mdf;Database=ProjectManagmentSystem;Trusted_Connection = true;").Options);
        }
    }
}
