using System.Runtime.Serialization;

using AabSemantics.Serialization.Json;

namespace AabSemantics.Sample07.CustomModule.Json
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
