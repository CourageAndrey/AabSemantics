using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	/// <summary>XML surrogate of a <see cref="Questions.IsPartOfQuestion"/>.</summary>
	[XmlType]
	public class IsPartOfQuestion : Question<Questions.IsPartOfQuestion>
	{
		#region Properties

		/// <summary>Identifier of the containing concept.</summary>
		[XmlElement]
		public String Parent
		{ get; set; }

		/// <summary>Identifier of the contained concept.</summary>
		[XmlElement]
		public String Child
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public IsPartOfQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public IsPartOfQuestion(Questions.IsPartOfQuestion question)
			: base(question)
		{
			Parent = question.Parent.ID;
			Child = question.Child.ID;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.IsPartOfQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.IsPartOfQuestion(
				conceptIdResolver.GetConceptById(Child),
				conceptIdResolver.GetConceptById(Parent),
				preconditions);
		}
	}
}
