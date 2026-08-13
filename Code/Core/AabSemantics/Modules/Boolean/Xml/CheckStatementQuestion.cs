using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Metadata;
using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Boolean.Xml
{
	/// <summary>
	/// XML surrogate of the "is this statement true" question. Its static constructor teaches the
	/// serializer every registered statement type, so the nested statement stays polymorphic.
	/// </summary>
	[XmlType]
	public class CheckStatementQuestion : Question<Questions.CheckStatementQuestion>
	{
		#region Properties

		/// <summary>Surrogate of the statement being checked.</summary>
		[XmlElement]
		public Statement Statement
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public CheckStatementQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">The statement's type is not registered.</exception>
		public CheckStatementQuestion(Questions.CheckStatementQuestion question)
			: base(question)
		{
			var statementType = question.Statement.GetType();
			var statementDefinition = Repositories.Statements.Definitions[statementType];
			var xmlSettings = statementDefinition.GetSerializationSettings<StatementXmlSerializationSettings>();
			Statement = xmlSettings.GetXml(question.Statement);
		}

		#endregion

		/// <summary>Rebuilds the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already rebuilt by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.CheckStatementQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.CheckStatementQuestion(
				Statement.SaveOrReuse(conceptIdResolver, statementIdResolver),
				preconditions);
		}

		static CheckStatementQuestion()
		{
			typeof(CheckStatementQuestion).DefineTypeOverrides(new[]
			{
				new XmlHelper.PropertyTypes(nameof(Statement), typeof(CheckStatementQuestion), Repositories.Statements.Definitions.Values.ToDictionary(
					definition => definition.GetSerializationSettings<StatementXmlSerializationSettings>().XmlElementName,
					definition => definition.GetSerializationSettings<StatementXmlSerializationSettings>().XmlType)),
			});
		}
	}
}
