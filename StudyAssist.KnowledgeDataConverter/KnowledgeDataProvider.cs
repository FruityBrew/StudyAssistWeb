using StudyAssist.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Interfaces;

namespace StudyAssist.KnowledgeDataConverter
{
    internal class KnowledgeDataProvider : IKnowledgeDataProvider
    {
        public Task<int> SaveCatalog(Catalog catalog)
        {
            throw new NotImplementedException();
        }
    }
}
