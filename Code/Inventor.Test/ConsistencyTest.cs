using System.Globalization;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;

namespace Inventor.Test
{
	[TestFixture]
	public class ConsistencyTest
	{
		[Test]
		public void TestLanguageNames()
		{
			var questionType = typeof(IQuestion);
			var languageNames = typeof(ILanguageQuestionNames).GetProperties().Select(p => p.Name).ToList();
			languageNames.RemoveAll(p => p.StartsWith("Param"));
			foreach (var type in questionType.Assembly.GetTypes().Where(t => questionType.IsAssignableFrom(t) && !t.IsAbstract))
			{
				if (!languageNames.Remove(type.Name))
				{
					Assert.IsTrue(false, string.Format(CultureInfo.InvariantCulture, "questionType {0} hasn't localization name.", type.FullName));
				}
			}
			if (languageNames.Count > 0)
			{
				Assert.IsTrue(false, "There are unbound localization names!");
			}
		}
	}
}
