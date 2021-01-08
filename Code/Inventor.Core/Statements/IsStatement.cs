using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public sealed class IsStatement : Statement<IsStatement>
	{
		#region Properties

		public Concept Parent
		{ get { return _parent; } }

		public Concept Child
		{ get { return _child; } }

		private readonly Concept _parent;
		private readonly Concept _child;

		#endregion

		public IsStatement(Concept parent, Concept child)
			: base(new Func<ILanguage, string>(language => language.StatementNames.Clasification), new Func<ILanguage, string>(language => language.StatementHints.Clasification))
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
			return () => language.Clasification;
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

		public override bool Equals(IsStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other._parent == _parent &&
						other._child == _child;
			}
			else return false;
		}

		public bool CheckCyclic(IEnumerable<IsStatement> statements)
		{
			return !isCyclic(statements, _child, new List<Concept>());
		}

		private bool isCyclic(IEnumerable<IsStatement> allClasifications, Concept concept, List<Concept> chain)
		{
			if (chain.Contains(concept)) return true;

			var clasifications = allClasifications.Where(c => c.Child == concept).ToList();
			if (clasifications.Count == 0)
			{
				return false;
			}
			else
			{
				foreach (var clasification in clasifications)
				{
					if (isCyclic(allClasifications, clasification.Parent, new List<Concept>(chain) {clasification.Child}))
					{
						return true;
					}
				}
				return false;
			}
		}

		#endregion

		#region Lookup

		public static List<Concept> GetParentsTree(IEnumerable<Statement> statements, Concept concept)
		{
			return GetParentsTree(statements.OfType<IsStatement>(), concept);
		}

		public static List<Concept> GetParentsPlainList(IEnumerable<Statement> statements, Concept concept)
		{
			return GetParentsPlainList(statements.OfType<IsStatement>(), concept);
		}

		public static List<Concept> GetParentsTree(IEnumerable<IsStatement> clasifications, Concept concept)
		{
			var result = new List<Concept>();
			foreach (var parent in GetParentsPlainList(clasifications, concept))
			{
				var list = new List<Concept> {parent};
				list.AddRange(GetParentsTree(clasifications, parent));
				list.RemoveAll(result.Contains);
				result.AddRange(list);
			}
			return result;
		}

		public static List<Concept> GetParentsPlainList(IEnumerable<IsStatement> clasifications, Concept concept)
		{
			return clasifications.Where(c => c.Child == concept).Select(c => c.Parent).ToList();
		}

		#endregion
	}
}
