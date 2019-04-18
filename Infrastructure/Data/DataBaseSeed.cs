using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class DataBaseSeed : IDbSeed
    {
        private readonly ApplicationContext _context;
        public DataBaseSeed(ApplicationContext context)
        {
            _context = context;
        }
        public void Initialize()
        {
            PopulateProjects(37);
            PopulateDevelopers(15);
            RandomlyAssign();
        }
        protected virtual void PopulateProjects(int count)
        {

        }
        protected virtual void PopulateDevelopers(int count)
        {

        }
        protected virtual void RandomlyAssign(int variation = 1)
        {

        }
    }
}
