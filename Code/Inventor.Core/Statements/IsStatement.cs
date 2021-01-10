using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public sealed class IsStatement : Statement<IsStatement>, IParentChild<Concept>
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
			: base(new Func<ILanguage, String>(language => language.StatementNames.Clasification), new Func<ILanguage, String>(language => language.StatementHints.Clasification))
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

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.Clasification;
		}

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ "#PARENT#", _parent },
				{ "#CHILD#", _child },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(IsStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other._parent == _parent &&
						other._child == _child;
			}
			else return false;
		}

		public Boolean CheckCyclic(IEnumerable<IsStatement> statements)
		{
			return !isCyclic(statements, _child, new List<Concept>());
		}

		private Boolean isCyclic(IEnumerable<IsStatement> allClasifications, Concept concept, List<Concept> chain)
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
	}
}
