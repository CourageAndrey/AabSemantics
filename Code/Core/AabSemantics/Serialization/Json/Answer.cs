using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using AabSemantics.Metadata;
using AabSemantics.Serialization.Json.Answers;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Json
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
			return definition.GetJsonSerializationSettings<AnswerJsonSerializationSettings>().GetJson(answer, language);
		}

		public virtual IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.Answer(
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))),
				IsEmpty);
		}

		static Answer()
		{
			RefreshMetadata();
		}

		public static void RefreshMetadata()
		{
			var statementTypes = Repositories.Statements.GetJsonTypes();
			foreach (var answerType in new[]
			{
				typeof(Answer),
				typeof(BooleanAnswer),
				typeof(ConceptAnswer),
				typeof(ConceptsAnswer),
				typeof(StatementAnswer),
				typeof(StatementsAnswer),
			})
			{
				var serializer = new DataContractJsonSerializer(
					answerType,
					statementTypes);
				answerType.DefineCustomJsonSerializer(serializer);
			}
		}
	}
}
