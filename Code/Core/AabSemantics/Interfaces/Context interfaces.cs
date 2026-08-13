using System;
using System.Collections.Generic;

namespace AabSemantics
{
	/// <summary>
	/// A node in the context hierarchy. Contexts scope knowledge: each one owns a set of statements
	/// and inherits everything its ancestors hold, which is how hypothetical or question-local facts
	/// can be added without polluting the semantic network.
	/// </summary>
	public interface IContext
	{
		/// <summary>
		/// Language used to render text produced within this context.
		/// </summary>
		ILanguage Language
		{ get; }

		/// <summary>
		/// Statements owned by this context alone, excluding inherited ones.
		/// </summary>
		ICollection<IStatement> Scope
		{ get; }

		/// <summary>
		/// Enclosing context, or <c>null</c> for the root.
		/// </summary>
		IContext Parent
		{ get; }

		/// <summary>
		/// This context together with all of its ancestors — the full set of contexts whose
		/// statements are visible here. Used to filter statements during inference.
		/// </summary>
		ICollection<IContext> ActiveContexts
		{ get; }

		/// <summary>
		/// Contexts created inside this one. Children register themselves on construction
		/// and are expected to be disposed before their parent.
		/// </summary>
		ICollection<IContext> Children
		{ get; }

		/// <summary>
		/// <c>true</c> for the built-in root context that holds engine-level definitions
		/// rather than user knowledge.
		/// </summary>
		Boolean IsSystem
		{ get; }
	}

	/// <summary>
	/// The root context, holding definitions shared by every semantic network in the process.
	/// </summary>
	public interface ISystemContext : IContext
	{
		/// <summary>
		/// Creates the context a semantic network operates in, rooted at this system context.
		/// </summary>
		/// <param name="semanticNetwork">Network the new context belongs to.</param>
		/// <returns>A child context bound to the given network.</returns>
		ISemanticNetworkContext Instantiate(ISemanticNetwork semanticNetwork);
	}

	/// <summary>
	/// The context a semantic network lives in. Everything asked of the network is answered
	/// against this context or a short-lived child of it.
	/// </summary>
	public interface ISemanticNetworkContext : IContext
	{
		/// <summary>
		/// The semantic network this context belongs to.
		/// </summary>
		ISemanticNetwork SemanticNetwork
		{ get; }

		/// <summary>
		/// Creates a disposable child context in which a single question is answered,
		/// so that the question's preconditions stay invisible to the network itself.
		/// </summary>
		/// <param name="question">Question about to be answered.</param>
		/// <param name="language">Language for the answer's text; defaults to this context's language when <c>null</c>.</param>
		/// <returns>A child context that the caller must dispose once the answer is produced.</returns>
		IQuestionProcessingContext CreateQuestionContext(IQuestion question, ILanguage language = null);
	}

	/// <summary>
	/// A throwaway context wrapping one question. Disposing it detaches the context from its
	/// parent and discards the question's preconditions.
	/// </summary>
	public interface IQuestionProcessingContext : ISemanticNetworkContext, IDisposable
	{
		/// <summary>
		/// The question being answered.
		/// </summary>
		IQuestion Question
		{ get; }
	}

	/// <summary>
	/// A question processing context that exposes its question in its concrete type, sparing
	/// question processors a cast.
	/// </summary>
	/// <typeparam name="QuestionT">Concrete question type handled by this context.</typeparam>
	public interface IQuestionProcessingContext<out QuestionT> : IQuestionProcessingContext
		where QuestionT : IQuestion
	{
		/// <summary>
		/// The question being answered, strongly typed.
		/// </summary>
		new QuestionT Question
		{ get; }
	}
}
