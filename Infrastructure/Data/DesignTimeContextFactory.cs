namespace Infrastructure
{
    using Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            return new ApplicationContext((new DbContextOptionsBuilder()).UseSqlServer(@"Server=.;Database=TestProjectDB;Trusted_Connection=True;").Options);
        }
    }
}
