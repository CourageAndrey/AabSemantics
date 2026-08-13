using System;
using System.Collections.Generic;

using AabSemantics.Utils;

namespace AabSemantics.Contexts
{
	/// <summary>
	/// Base implementation of <see cref="IContext"/>: owns a statement scope and links itself
	/// into the context tree on construction.
	/// </summary>
	public abstract class Context : IContext
	{
		#region Properties

		/// <summary>
		/// Language used to render text produced within this context.
		/// </summary>
		public ILanguage Language
		{ get; protected set; }

		/// <summary>
		/// Statements owned by this context alone, excluding inherited ones.
		/// </summary>
		public ICollection<IStatement> Scope
		{ get; }

		/// <summary>
		/// Enclosing context, or <c>null</c> for the root.
		/// </summary>
		public IContext Parent
		{ get; }

		/// <summary>
		/// This context and all of its ancestors. Computed once on first access, so a context
		/// must not be re-parented after the hierarchy has been read.
		/// </summary>
		public ICollection<IContext> ActiveContexts
		{ get { return _activeContexts ?? (_activeContexts = GetHierarchy()); } }

		private ICollection<IContext> _activeContexts;

		/// <summary>
		/// Contexts created inside this one.
		/// </summary>
		public ICollection<IContext> Children
		{ get; }

		/// <summary>
		/// <c>true</c> for the built-in root context.
		/// </summary>
		public abstract Boolean IsSystem
		{ get; }

		#endregion

		/// <summary>
		/// Initializes the context and registers it with its parent.
		/// </summary>
		/// <param name="language">Language for text produced in this context.</param>
		/// <param name="parent">Enclosing context, or <c>null</c> when this is the root.</param>
		protected Context(ILanguage language, IContext parent)
		{
			Language = language;
			Scope = new List<IStatement>();
			Children = new SynchronizedCollection<IContext>();

			Parent = parent;
			if (parent != null)
			{
				parent.Children.Add(this);
			}
		}

		/// <summary>
		/// Walks from this context up to the root, collecting every context on the way.
		/// </summary>
		/// <returns>This context and all of its ancestors.</returns>
		public ICollection<IContext> GetHierarchy()
		{
			IContext context = this;
			var hierarchy = new HashSet<IContext>();
			while (context != null)
			{
				hierarchy.Add(context);
				context = context.Parent;
			}
			return hierarchy;
		}
	}
}
