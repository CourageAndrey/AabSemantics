using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Base;

namespace Inventor.Core.Statements
{
	public sealed class IsStatement : Statement<IsStatement>, IParentChild<IConcept>
	{
		#region Properties

		public IConcept Parent
		{ get { return _parent; } }

		public IConcept Child
		{ get { return _child; } }

		private IConcept _parent;
		private IConcept _child;

		#endregion

		public IsStatement(IConcept parent, IConcept child)
			: base(new Func<ILanguage, String>(language => language.StatementNames.Clasification), new Func<ILanguage, String>(language => language.StatementHints.Clasification))
		{
			Update(parent, child);
		}

		public void Update(IConcept parent, IConcept child)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			if (child == null) throw new ArgumentNullException("child");

			_parent = parent;
			_child = child;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
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
			return !isCyclic(statements, _child, new List<IConcept>());
		}

		private Boolean isCyclic(IEnumerable<IsStatement> allClasifications, IConcept concept, List<IConcept> chain)
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
					if (isCyclic(allClasifications, clasification.Parent, new List<IConcept>(chain) { clasification.Child }))
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
