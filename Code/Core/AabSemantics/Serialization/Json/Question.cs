using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Json
{
	/// <summary>
	/// Base JSON surrogate of a question. Unlike the XML counterpart, its preconditions are
	/// converted through <see cref="Statement.Load"/>, so base-type registrations are honoured.
	/// </summary>
	[DataContract]
	public abstract class Question
	{
		#region Properties

		/// <summary>Surrogates of the question's hypothetical preconditions.</summary>
		[DataMember]
		public List<Statement> Preconditions
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		protected Question()
		{
			Preconditions = new List<Statement>();
		}

		/// <summary>Converts a question's preconditions into surrogates.</summary>
		/// <param name="question">Question being converted.</param>
		protected Question(IQuestion question)
		{
			Preconditions = question.Preconditions.Select(statement => Statement.Load(statement)).ToList();
		}

		#endregion

		/// <summary>Converts a question into the surrogate registered for its type.</summary>
		/// <param name="question">Question to convert.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		/// <exception cref="System.NotSupportedException">The question's type is not registered.</exception>
		public static Question Load(IQuestion question)
		{
			var definition = Repositories.Questions.Definitions.GetSuitable(question);
			return definition.GetSerializationSettings<QuestionJsonSerializationSettings>().GetJson(question);
		}

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Reuses the network's existing statements where possible.</param>
		/// <returns>The restored question.</returns>
		public abstract IQuestion Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver);

		static Question()
		{
			RefreshMetadata();
		}

		/// <summary>
		/// Rebuilds the serializer so it knows the currently registered statement types. Call it
		/// again after registering further statement types, otherwise those cannot appear among
		/// serialized preconditions.
		/// </summary>
		public static void RefreshMetadata()
		{
			var questionType = typeof(Question);
			var serializer = new DataContractJsonSerializer(
				questionType,
				Repositories.Statements.GetJsonTypes());
			questionType.DefineCustomJsonSerializer(serializer);
		}
	}

	/// <summary>JSON surrogate of one concrete question type.</summary>
	/// <typeparam name="QuestionT">Question type represented.</typeparam>
	[DataContract]
	public abstract class Question<QuestionT> : Question
		where QuestionT : IQuestion
	{
		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		protected Question()
			: base()
		{ }

		/// <summary>Converts a question's preconditions into surrogates.</summary>
		/// <param name="question">Question being converted.</param>
		protected Question(QuestionT question)
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
