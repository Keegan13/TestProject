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
        public virtual string[] FullNames => new[] { "Eddard Stark", "Robert Baratheon", "Jaime Lannister", "Catelyn Stark", "Cersei Lannister", "Daenerys Targaryen", "Jorah Mormont", "Viserys Targaryen", "Jon Snow", "Sansa Stark", "Arya Stark", "Robb Stark", "Theon Greyjoy", "Bran Stark ", "Joffrey Baratheon", "Sandor Clegane", "Tyrion Lannister", "Khal Drogo", "Petyr Baelish", "Davos Seaworth", "Samwell Tarly", "Stannis Baratheon", "Melisandre", "Jeor Mormont", "Bronn", "Varys", "Shae", "Margaery Tyrell", "Tywin Lannister", "Talisa Maegyr", "Ygritte", "Gendry", "Tormund Giantsbane", "Brienne of Tarth", "Ramsay Bolton", "Gilly", "Daario Naharis", "Missandei", "Ellaria Sand", "Tommen Baratheon", "Jaqen H'ghar", "Roose Bolton", "The High Sparrow" };
    }
}
