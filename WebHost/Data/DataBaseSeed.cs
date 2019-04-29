using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Host.Data
{
    public class DataBaseSeed
    {
        private readonly ApplicationContext _context;
        public DataBaseSeed(ApplicationContext context)
        {
            _context = context;
        }
        public void Initialize()
        {
            if (this._context.Projects.Count() == 0)
            {
                PopulateProjects(30);
            }
            if(this._context.Developers.Count() == 0)
            {
                PopulateDevelopers(25);
            }
            if (this._context.Assignments.Count() == 0)
            {
                RandomlyAssign();
            }
        }
        private bool isAssigned(Project project, Developer developer)
        {
            return this._context.Set<ProjectAssignment>().Any(x => x.Developer == developer && x.Project == project);
        }
        protected virtual void PopulateProjects(int count)
        {
            Shuffle(ProjectNames);

            var names = ProjectNames.Take(count).Distinct();

            //var conflicts = _context.Set<Project>().Join(names, p => p.Name, n => n, (p, n) => n).ToArray();

            //names = names.Except(conflicts);

            var proj = new Project[names.Count()];

            int i = 0;
            foreach (var x in names)
            {
                var dates = GetDates();
                proj[i] = new Project
                {
                    Name = x,
                    Description = Faker.Lorem.Paragraph(),
                    StartDate = dates.Item1,
                    EndDate = dates.Item2,
                    Status = GetStatus(dates)
                };
                i++;
            }

            _context.AddRange(proj);

            this._context.SaveChanges();

            Console.WriteLine("Added {0} randomly generated projects", proj.Length);
        }
        protected virtual void PopulateDevelopers(int count)
        {
            Shuffle(FullNames);
            Shuffle(NickNames);

            var names = NickNames.Take(count).Distinct();

            //var conflicts = _context.Set<Developer>().Join(names, p => p.Nickname, n => n, (p, n) => n).ToArray();

            //names = names.Except(conflicts);

            var devs = new Developer[names.Count()];

            int i = 0;
            foreach (var x in names)
            {
                var dates = GetDates();
                devs[i] = new Developer
                {
                    Nickname = x,
                    FullName = FullNames[i]
                };
                i++;
            }

            _context. AddRange(devs);

            this._context.SaveChanges();

            Console.WriteLine("Added {0} randomly generated developers", devs.Length);
        }
        public static ProjectStatus GetStatus(DateTime start, DateTime end)
        {
            if (DateTime.Now < start)
                return ProjectStatus.UnStarted;
            if (DateTime.Now > end)
                return ProjectStatus.Completed;
            return ProjectStatus.InProgress;
        }
        public static ProjectStatus GetStatus(Tuple<DateTime, DateTime> input)
        {
            return GetStatus(input.Item1, input.Item2);
        }
        protected virtual void RandomlyAssign()
        {
            Project[] projs = _context.Set<Project>().ToArray();
            Developer[] devs = _context.Set<Developer>().ToArray();
            List<ProjectAssignment> assigns = new List<ProjectAssignment>();
            for (int i = 0; i < projs.Length; i++)
            {
                for (int j = 0; j < devs.Length; j++)
                {
                    if (rng.Next(1,4)%3==1)
                    {
                        assigns.Add(new ProjectAssignment { ProjectId = projs[i].Id, DeveloperId = devs[j].Id });
                    }
                }
            }
            _context.Assignments.AddRange(assigns);
            _context.SaveChanges();
            Console.WriteLine("Crated {0} connections betwen projects and developers");
        }
        //src https://stackoverflow.com/questions/273313/randomize-a-listt
        private static Random rng = new Random();
        public static void Shuffle<T>(T[] list)
        {
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        private static Tuple<DateTime, DateTime> GetDates()
        {
            var start = DateTime.Now;
            var days = rng.Next(1, 200) - 191; // - 190 to 10 weeks
            start=start.AddDays(days * 7);
            var end = start + TimeSpan.FromDays(rng.Next(7, 186));
            return Tuple.Create(start, end);
        }
        public static string[] NickNames = new[] { "Doc", "Basher", "Little", "King", "Daddy", "Fortuna", "Mania", "Twitch", "Magica", "Silly", "Cookie", "Gator", "Lock", "Rip", "Jade", "Toon", "Piggy", "Angel", "Pipi", "Scoop", "Small", "Bulldog", "Killer", "Pitch", "Robin", "Black Magic", "Blade", "Cutie", "Creed", "Wonder", "Dodo", "Slayer", "Dealer", "Hawkeye", "Magic", "Jackal", "Smitty", "Glide", "Stout", "Ducky", "Sassy", "Hawkeye", "Jazzy", "Tiny", "Shade", "Speed", "Iron", "Artsy", "Dice", "Tricky", "Beauty", "Navigator", "Shadow", "Spud", "Chuck", "Handsome", "Belle", "Slick", "Brick", "Lightning", "Piggy", "Storm", "Reaper", "Tug", "Hooks", "Big Boy", "Porky", "Fancy", "Magician", "Fuzz" };
        public static string[] FullNames = new[] { "Eddard Stark", "Robert Baratheon", "Jaime Lannister", "Catelyn Stark", "Cersei Lannister", "Daenerys Targaryen", "Jorah Mormont", "Viserys Targaryen", "Jon Snow", "Sansa Stark", "Arya Stark", "Robb Stark", "Theon Greyjoy", "Bran Stark ", "Joffrey Baratheon", "Sandor Clegane", "Tyrion Lannister", "Khal Drogo", "Petyr Baelish", "Davos Seaworth", "Samwell Tarly", "Stannis Baratheon", "Melisandre", "Jeor Mormont", "Bronn", "Varys", "Shae", "Margaery Tyrell", "Tywin Lannister", "Talisa Maegyr", "Ygritte", "Gendry", "Tormund Giantsbane", "Brienne of Tarth", "Ramsay Bolton", "Gilly", "Daario Naharis", "Missandei", "Ellaria Sand", "Tommen Baratheon", "Jaqen H'ghar", "Roose Bolton", "The High Sparrow" };
        public static string[] ProjectNames = new[] { "get_packer", "sublime_js", "grunt_dapp", "grunt_dapp", "grunt_dapp", "vagrant_exercise", "vagrant_exercise", "angular_web", "java_bock", "WorkPacker", "freen_java", "test_squen", "rands_bool", "music_code", "java_todes", "scripts_form", "learning_model", "smart_code", "Sounder_Manager", "backbone_mport", "cucume.js", "jsonvil.it", "mochub.com", "quark2", "stelljs", "turekapi", "yii2_roject", "openjy_con", "grunt_worle" };
    }
}
