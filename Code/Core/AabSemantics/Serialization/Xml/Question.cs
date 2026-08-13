using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>
	/// Base XML surrogate of a question. As with <see cref="Statement"/>, <c>Load</c> converts a
	/// question <em>into</em> a surrogate and <c>Save</c> restores it <em>from</em> one.
	/// </summary>
	[XmlType]
	public abstract class Question
	{
		#region Properties

		/// <summary>Surrogates of the question's hypothetical preconditions.</summary>
		[XmlArray(nameof(Preconditions))]
		public List<Statement> Preconditions
		{ get; set; } = new List<Statement>();

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		protected Question()
		{ }

		/// <summary>Converts a question's preconditions into surrogates.</summary>
		/// <param name="question">Question being converted.</param>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">A precondition's exact type is not registered; base types are not tried here.</exception>
		protected Question(IQuestion question)
		{
			var statementSerializers = Repositories.Statements.Definitions.ToDictionary(
				definition => definition.Key,
				definition => (StatementXmlSerializationSettings) definition.Value.GetXmlSerializationSettings());

			Preconditions = question.Preconditions.Select(statement => statementSerializers[statement.GetType()].GetXml(statement)).ToList();
		}

		#endregion

		/// <summary>Converts a question into the surrogate registered for its type.</summary>
		/// <param name="question">Question to convert.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		/// <exception cref="System.NotSupportedException">The question's type is not registered.</exception>
		public static Question Load(IQuestion question)
		{
			var definition = Repositories.Questions.Definitions.GetSuitable(question);
			return definition.GetSerializationSettings<QuestionXmlSerializationSettings>().GetXml(question);
		}

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Reuses the network's existing statements where possible.</param>
		/// <returns>The restored question.</returns>
		public abstract IQuestion Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver);

		static Question()
		{
			typeof(Question).DefineTypeOverrides(new[]
			{
				new XmlHelper.PropertyTypes(nameof(Preconditions), typeof(Question), Repositories.Statements.Definitions.Values.ToDictionary(
					definition => definition.GetSerializationSettings<StatementXmlSerializationSettings>().XmlElementName,
					definition => definition.GetSerializationSettings<StatementXmlSerializationSettings>().XmlType)),
			});
		}
	}

	/// <summary>XML surrogate of one concrete question type.</summary>
	/// <typeparam name="QuestionT">Question type represented.</typeparam>
	[XmlType]
	public abstract class Question<QuestionT> : Question
		where QuestionT : IQuestion
	{
		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		protected Question()
		{ }

		/// <summary>Converts a question's preconditions into surrogates.</summary>
		/// <param name="question">Question being converted.</param>
		protected Question(IQuestion question)
			: base(question)
		{ }

		#endregion

		/// <summary>Restores the preconditions, then delegates to <see cref="SaveImplementation"/>.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Reuses the network's existing statements where possible.</param>
		/// <returns>The restored question.</returns>
		public override IQuestion Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return SaveImplementation(
				conceptIdResolver,
				statementIdResolver,
				Preconditions.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver)));
		}

		/// <summary>Restores the question in its concrete type.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected abstract QuestionT SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions);
	}
}
