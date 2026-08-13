using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using AabSemantics.Metadata;
using AabSemantics.Serialization;
using AabSemantics.Serialization.Json;

namespace AabSemantics.Modules.Boolean.Json
{
	/// <summary>JSON surrogate of the "is this statement true" question.</summary>
	[DataContract]
	public class CheckStatementQuestion : Question<Questions.CheckStatementQuestion>
	{
		#region Properties

		/// <summary>Surrogate of the statement being checked.</summary>
		[DataMember]
		public Statement Statement
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public CheckStatementQuestion()
			: base()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public CheckStatementQuestion(Questions.CheckStatementQuestion question)
			: base(question)
		{
			Statement = Statement.Load(question.Statement);
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
			RefreshMetadata();
			RefreshMetadataStatement();
		}

		/// <summary>
		/// Rebuilds the JSON serializer so it knows every currently registered statement type.
		/// Call it again after registering further statement types, otherwise they cannot be
		/// serialized inside this question.
		/// </summary>
		public static void RefreshMetadataStatement()
		{
			var checkStatementType = typeof(CheckStatementQuestion);
			var serializer = new DataContractJsonSerializer(
				checkStatementType,
				Repositories.Statements.GetJsonTypes());
			checkStatementType.DefineCustomJsonSerializer(serializer);
		}
	}
}
