using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OIG_Test.Models;

namespace OIG_Test.DBInfra.Interfaces
{
    public interface IResearchAccess
    {
        public void Create(Research research);
        public void Update(Research research);
        public void Delete(Research research);

        public List<Research> GetAllResearches();

        public Research GetResearch(int? targetId);

        public void SaveResearch(Research research);
    }
}
