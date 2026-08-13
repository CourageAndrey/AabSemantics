using System;
using System.Linq;
using System.Reflection;

namespace AabSemantics.Contexts
{
	/// <summary>
	/// A context bound to a semantic network. Serves both as the network's own context and as
	/// the base for the short-lived contexts questions are answered in.
	/// </summary>
	public class SemanticNetworkContext : Context, ISemanticNetworkContext
	{
		#region Properties

		/// <summary>
		/// The semantic network this context belongs to.
		/// </summary>
		public ISemanticNetwork SemanticNetwork
		{ get; }

		/// <summary>
		/// Always <c>false</c>: only the root context is a system context.
		/// </summary>
		public override Boolean IsSystem
		{ get { return false; } }

		#endregion

		/// <summary>
		/// Creates a context bound to a semantic network.
		/// </summary>
		/// <param name="language">Language for text produced in this context.</param>
		/// <param name="parent">Enclosing context.</param>
		/// <param name="semanticNetwork">Network this context belongs to.</param>
		public SemanticNetworkContext(ILanguage language, IContext parent, ISemanticNetwork semanticNetwork)
			: base(language, parent)
		{
			SemanticNetwork = semanticNetwork;
		}

		/// <summary>
		/// Creates a disposable child context for one question and adds the question's
		/// preconditions to the network under that context, so that disposing it removes them again.
		/// </summary>
		/// <param name="question">Question about to be answered.</param>
		/// <param name="language">Language for the answer's text; keeps this context's language when <c>null</c>.</param>
		/// <returns>A child context the caller must dispose once the answer is produced.</returns>
		public IQuestionProcessingContext CreateQuestionContext(IQuestion question, ILanguage language = null)
		{
			var concreteContextType = typeof(QuestionProcessingContext<>).MakeGenericType(question.GetType());
			var contextConstructor = concreteContextType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
			var resultContext = contextConstructor.Invoke(new Object[] { this, question, language }) as IQuestionProcessingContext;
			foreach (var statement in question.Preconditions)
			{
				statement.Context = resultContext;
				resultContext.SemanticNetwork.Statements.Add(statement);
			}
			return resultContext;
		}
	}
}