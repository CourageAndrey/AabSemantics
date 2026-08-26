using System;
using System.Linq;
using System.Reflection;
using System.Threading;

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
		/// <param name="cancellationToken">
		/// Cancels answering the question. When no cancellable token is given and this context is
		/// itself a question context, the enclosing question's token is inherited: a nested question
		/// cannot outlive the question that asked it, even if its processor forgot to pass the token on.
		/// </param>
		/// <returns>A child context the caller must dispose once the answer is produced.</returns>
		public IQuestionProcessingContext CreateQuestionContext(IQuestion question, ILanguage language = null, CancellationToken cancellationToken = default)
		{
			if (!cancellationToken.CanBeCanceled)
			{
				var parentQuestionContext = this as IQuestionProcessingContext;
				if (parentQuestionContext != null)
				{
					cancellationToken = parentQuestionContext.CancellationToken;
				}
			}

			var concreteContextType = typeof(QuestionProcessingContext<>).MakeGenericType(question.GetType());
			var contextConstructor = concreteContextType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
			var resultContext = contextConstructor.Invoke(new Object[] { this, question, language, cancellationToken }) as IQuestionProcessingContext;
			foreach (var statement in question.Preconditions)
			{
				statement.Context = resultContext;
				resultContext.SemanticNetwork.Statements.Add(statement);
			}
			return resultContext;
		}
	}
}