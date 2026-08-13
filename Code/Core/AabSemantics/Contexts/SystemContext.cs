using System;

namespace AabSemantics.Contexts
{
	/// <summary>
	/// The root of a context tree. It has no parent and can be instantiated only once,
	/// because a system context backs exactly one semantic network.
	/// </summary>
	public class SystemContext : Context, ISystemContext
	{
		#region Properties

		/// <summary>
		/// Always <c>true</c>: this is the system context.
		/// </summary>
		public override Boolean IsSystem
		{ get { return true; } }

		#endregion

		/// <summary>
		/// Creates a root context.
		/// </summary>
		/// <param name="language">Language for text produced in this context and its children.</param>
		public SystemContext(ILanguage language)
			: base(language, null)
		{ }

		/// <summary>
		/// Creates the semantic network context rooted at this one.
		/// </summary>
		/// <param name="semanticNetwork">Network the new context belongs to.</param>
		/// <returns>The child context bound to the given network.</returns>
		/// <exception cref="InvalidOperationException">This system context has already been instantiated.</exception>
		public ISemanticNetworkContext Instantiate(ISemanticNetwork semanticNetwork)
		{
			if (Children.Count > 0) throw new InvalidOperationException("Impossible to instantiate system context more than once.");

			return new SemanticNetworkContext(Language, this, semanticNetwork);
		}
	}
}