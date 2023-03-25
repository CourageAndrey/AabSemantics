using System.Runtime.Serialization;

using Inventor.Semantics;
using Inventor.Semantics.Serialization.Json;

namespace Samples.Semantics.Sample07.CustomModule.Json
{
	[DataContract]
	public class CustomAnswer : Answer
	{
		#region Constructors

		public CustomAnswer()
		{ }

		public CustomAnswer(Sample07.CustomModule.CustomAnswer answer, ILanguage language)
			: base(answer, language)
		{ }

		#endregion
	}
}
