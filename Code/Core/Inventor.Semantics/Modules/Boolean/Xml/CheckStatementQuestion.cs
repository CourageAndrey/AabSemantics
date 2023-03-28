using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.Boolean.Xml
{
	[XmlType]
	public class CheckStatementQuestion : Question<Questions.CheckStatementQuestion>
	{
		#region Properties

		[XmlElement]
		public Statement Statement
		{ get; set; }

		#endregion

		#region Constructors

		public CheckStatementQuestion()
		{ }

		public CheckStatementQuestion(Questions.CheckStatementQuestion question)
			: base(question)
		{
			var statementType = question.Statement.GetType();
			var statementDefinition = Repositories.Statements.Definitions[statementType];
			var xmlSettings = statementDefinition.GetXmlSerializationSettings<StatementXmlSerializationSettings>();
			Statement = xmlSettings.GetXml(question.Statement);
		}

		#endregion

		protected override Questions.CheckStatementQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.CheckStatementQuestion(
				Statement.Save(conceptIdResolver),
				preconditions);
		}

		static CheckStatementQuestion()
		{
			var serializedType = typeof(CheckStatementQuestion);

			var attributeOverrides = new XmlAttributeOverrides();

			var statementAttributes = new XmlAttributes();
			foreach (var definition in Repositories.Statements.Definitions.Values)
			{
				var xmlSettings = definition.GetXmlSerializationSettings<StatementXmlSerializationSettings>();
				statementAttributes.XmlElements.Add(new XmlElementAttribute(xmlSettings.XmlType.Name, xmlSettings.XmlType));
			}
			attributeOverrides.Add(serializedType, nameof(Statement), statementAttributes);

			var serializer = new XmlSerializer(serializedType, attributeOverrides);
			serializedType.DefineCustomXmlSerializer(serializer);
		}
	}
}
