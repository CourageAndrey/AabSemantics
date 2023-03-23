using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Json
{
	[DataContract]
	public class Answer
	{
		#region Properties

		[DataMember]
		public String Description
		{ get; set; }

		[DataMember]
		public List<Statement> Explanation
		{ get; set; }

		[DataMember]
		public Boolean IsEmpty
		{ get; set; }

		#endregion

		#region Constructors

		public Answer()
			: this(String.Empty, new List<Statement>(), true)
		{ }

		public Answer(String description, List<Statement> explanation, Boolean isEmpty)
		{
			Description = description;
			Explanation = explanation;
			IsEmpty = isEmpty;
		}

		public Answer(IAnswer answer, ILanguage language)
			: this(
				TextRepresenters.PlainString.Represent(answer.Description, language).ToString(),
				answer.Explanation.Statements.Select(statement => Statement.Load(statement)).ToList(),
				answer.IsEmpty)
		{ }

		#endregion

		public static Answer Load(IAnswer answer, ILanguage language)
		{
			var definition = Repositories.Answers.Definitions.GetSuitable(answer);
			return definition.GetJson(answer, language);
		}
	}
}
