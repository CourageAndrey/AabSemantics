using System.Runtime.Serialization;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[DataContract]
	public class ConceptAnswer : Answer
	{
		#region Properties

		[DataMember]
		public Concept Concept
		{ get; set; }

		#endregion

		#region Constructors

		public ConceptAnswer()
			: base()
		{ }

		public ConceptAnswer(Semantics.Answers.ConceptAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concept = new Concept(answer.Result);
		}

		#endregion
	}
}