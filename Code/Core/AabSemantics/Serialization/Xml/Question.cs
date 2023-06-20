using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Xml
{
	[XmlType]
	public abstract class Question
	{
		#region Properties

		[XmlArray(nameof(Preconditions))]
		public List<Statement> Preconditions
		{ get; set; } = new List<Statement>();

		#endregion

		#region Constructors

		protected Question()
		{ }

		protected Question(IQuestion question)
		{
			var statementSerializers = Repositories.Statements.Definitions.ToDictionary(
				definition => definition.Key,
				definition => (StatementXmlSerializationSettings) definition.Value.GetXmlSerializationSettings());

			Preconditions = question.Preconditions.Select(statement => statementSerializers[statement.GetType()].GetXml(statement)).ToList();
		}

		#endregion

		public static Question Load(IQuestion question)
		{
			var definition = Repositories.Questions.Definitions.GetSuitable(question);
			return definition.GetXmlSerializationSettings<QuestionXmlSerializationSettings>().GetXml(question);
		}

		public abstract IQuestion Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver);

		static Question()
		{
			typeof(Question).DefineTypeOverrides(new[]
			{
				new XmlHelper.PropertyTypes(nameof(Preconditions), typeof(Question), Repositories.Statements.Definitions.Values.ToDictionary(
					definition => definition.GetXmlSerializationSettings<StatementXmlSerializationSettings>().XmlElementName,
					definition => definition.GetXmlSerializationSettings<StatementXmlSerializationSettings>().XmlType)),
			});
		}
	}

	[XmlType]
	public abstract class Question<QuestionT> : Question
		where QuestionT : IQuestion
	{
		#region Constructors

		protected Question()
		{ }

		protected Question(IQuestion question)
			: base(question)
		{ }

		#endregion

		public override IQuestion Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return SaveImplementation(
				conceptIdResolver,
				statementIdResolver,
				Preconditions.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver)));
		}

		protected abstract QuestionT SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions);
	}
}
