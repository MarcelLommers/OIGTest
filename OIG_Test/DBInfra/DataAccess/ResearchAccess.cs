using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OIG_Test.DBInfra.Interfaces;
using OIG_Test.Models;

namespace OIG_Test.DBInfra.DataAccess
{
    public class ResearchAccess: IResearchAccess
    {
        public void Create(Research research)
        {
            using (var context = new DataContext())
            {
                context.Research.Add(research);
                context.SaveChanges();
            }
        }

        public void Update(Research research)
        {
            using (var context = new DataContext())
            {
                context.Research.Update(research);
                context.SaveChanges();
            }
        }

        public void Delete(Research research)
        {
            using (var context = new DataContext())
            {
                context.Research.Remove(research);
                context.SaveChanges();
            }
        }

        public List<Research> GetAllResearches()
        {
            using (var context = new DataContext())
            {
                return context.Research.ToList();
            }
        }


        public Research GetResearch(int? targetId)
        {
            using (var context = new DataContext())
            {
                return context.Research.FirstOrDefault(r => r.ResearchId == targetId);
            }
        }


        public void SaveResearch(Research research)
        {
            using (var context = new DataContext())
            {
                context.Research.Add(research);
                context.SaveChanges();
            }
        }
    }
}
