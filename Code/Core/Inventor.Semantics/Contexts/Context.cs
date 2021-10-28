using System;
using System.Collections.Generic;

namespace Inventor.Semantics.Contexts
{
	public abstract class Context : IContext
	{
		#region Properties

		public ILanguage Language
		{ get; protected set; }

		public ICollection<IStatement> Scope
		{ get; }

		public IContext Parent
		{ get; }

		public ICollection<IContext> ActiveContexts
		{ get { return _activeContexts ?? (_activeContexts = GetHierarchy()); } }

		private ICollection<IContext> _activeContexts;

		public ICollection<IContext> Children
		{ get; }

		public abstract Boolean IsSystem
		{ get; }

		#endregion

		protected Context(ILanguage language, IContext parent)
		{
			Language = language;
			Scope = new List<IStatement>();
			Children = new List<IContext>();

			Parent = parent;
			if (parent != null)
			{
				parent.Children.Add(this);
			}
		}

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
