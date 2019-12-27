using System.Linq;

using Inventor.Core;
using Inventor.Core.Localization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inventor.Test
{
    [TestClass]
    public class ConsistencyTest
    {
        [TestMethod]
        public void TestLanguageNames()
        {
            var questionType = typeof (Question);
            var languageNames = typeof(ILanguageQuestionNames).GetProperties().Select(p => p.Name).ToList();
            languageNames.RemoveAll(p => p.StartsWith("Param"));
            foreach (var type in questionType.Assembly.GetTypes().Where(t => questionType.IsAssignableFrom(t) && !t.IsAbstract))
            {
                if (!languageNames.Remove(type.Name))
                {
                    Assert.IsTrue(false, string.Format("Type {0} hasn't localization name.", type.FullName));
                }
            }
            if (languageNames.Count > 0)
            {
                Assert.IsTrue(false, "There are unbound localization names!");
            }
        }
    }
}
