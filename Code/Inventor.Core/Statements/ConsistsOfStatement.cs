using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public sealed class ConsistsOfStatement : Statement<ConsistsOfStatement>
	{
		#region Properties

		public override string Hint
		{ get { return LanguageEx.CurrentEx.StatementHints.Composition; } }

		public Concept Parent
		{ get { return _parent; } }

		public Concept Child
		{ get { return _child; } }

		private readonly Concept _parent;
		private readonly Concept _child;

		#endregion

		public ConsistsOfStatement(Concept parent, Concept child)
			: base(() => LanguageEx.CurrentEx.StatementNames.Composition)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			if (child == null) throw new ArgumentNullException("child");

			_parent = parent;
			_child = child;
		}

		public override IList<Concept> ChildConcepts
		{ get { return new List<Concept> { _parent, _child }.AsReadOnly(); } }

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

		#region Lookup

		public static List<Concept> GetContainingParents(IEnumerable<Statement> statements, Concept concept)
		{
			return statements.OfType<ConsistsOfStatement>().Where(c => c.Child == concept).Select(c => c.Parent).ToList();
		}

		public static List<Concept> GetContainingParts(IEnumerable<Statement> statements, Concept concept)
		{
			return statements.OfType<ConsistsOfStatement>().Where(c => c.Parent == concept).Select(c => c.Child).ToList();
		}

		#endregion
	}
}
