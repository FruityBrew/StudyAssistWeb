using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyAssist.Infrastructure.Data;
using StudyAssist.Infrastructure.Data.DataModel;

namespace InfrastructureTests
{
    class Context : StudyAssistContext
    {
        
    }

    [TestClass]
    public class ProblemRepositoryTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var v = Effort.DbConnectionFactory.CreateTransient();

            using (var StudyAssistContext = new StudyAssistContext)
            {
                
            }
        }
    }
}
