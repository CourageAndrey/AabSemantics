using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public sealed class ConsistsOfStatement : Statement<ConsistsOfStatement>, IParentChild<Concept>
	{
		#region Properties

		public Concept Parent
		{ get { return _parent; } }

		public Concept Child
		{ get { return _child; } }

		private readonly Concept _parent;
		private readonly Concept _child;

		#endregion

		public ConsistsOfStatement(Concept parent, Concept child)
			: base(new Func<ILanguage, string>(language => language.StatementNames.Composition), new Func<ILanguage, string>(language => language.StatementHints.Composition))
		{
			if (parent == null) throw new ArgumentNullException("parent");
			if (child == null) throw new ArgumentNullException("child");

			_parent = parent;
			_child = child;
		}

		public override IEnumerable<Concept> GetChildConcepts()
		{
			yield return _parent;
			yield return _child;
		}

		#region Description

		protected override Func<string> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.Composition;
		}

		protected override IDictionary<string, INamed> GetDescriptionParameters()
		{
			return new Dictionary<string, INamed>
			{
				{ "#PARENT#", _parent },
				{ "#CHILD#", _child },
			};
		}

		#endregion

		#region Consistency checking

		public override bool Equals(ConsistsOfStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other._parent == _parent &&
						other._child == _child;
			}
			else return false;
		}

		#endregion
	}
}
