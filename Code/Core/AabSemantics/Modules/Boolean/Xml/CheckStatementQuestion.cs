﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Metadata;
using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Boolean.Xml
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
			var xmlSettings = statementDefinition.GetSerializationSettings<StatementXmlSerializationSettings>();
			Statement = xmlSettings.GetXml(question.Statement);
		}

		#endregion

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
