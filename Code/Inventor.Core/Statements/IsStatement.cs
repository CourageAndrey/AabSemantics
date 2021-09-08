using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public class IsStatement : Statement<IsStatement>, IParentChild<IConcept>
	{
		#region Properties

		public IConcept Ancestor
		{ get; private set; }

		public IConcept Descendant
		{ get; private set; }

		public IConcept Parent
		{ get { return Ancestor; } }

		public IConcept Child
		{ get { return Descendant; } }

		#endregion

		public IsStatement(String id, IConcept ancestor, IConcept descendant)
			: base(id, new Func<ILanguage, String>(language => language.StatementNames.Clasification), new Func<ILanguage, String>(language => language.StatementHints.Clasification))
		{
			Update(id, ancestor, descendant);
		}

		public void Update(String id, IConcept ancestor, IConcept descendant)
		{
			if (ancestor == null) throw new ArgumentNullException(nameof(ancestor));
			if (descendant == null) throw new ArgumentNullException(nameof(descendant));

			Update(id);
			Ancestor = ancestor;
			Descendant = descendant;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Ancestor;
			yield return Descendant;
		}

		#region Description

		protected override String GetDescriptionText(ILanguageStatementsPart language)
		{
			return language.Clasification;
		}

		protected override IDictionary<String, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<String, IKnowledge>
			{
				{ Strings.ParamParent, Ancestor },
				{ Strings.ParamChild, Descendant },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(IsStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Ancestor == Ancestor &&
						other.Descendant == Descendant;
			}
			else return false;
		}

		public Boolean CheckCyclic(IEnumerable<IsStatement> statements)
		{
			return !isCyclic(statements, Descendant, new List<IConcept>());
		}

		private Boolean isCyclic(IEnumerable<IsStatement> allClasifications, IConcept concept, List<IConcept> chain)
		{
			if (chain.Contains(concept)) return true;

			var clasifications = allClasifications.Where(c => c.Descendant == concept).ToList();
			if (clasifications.Count == 0)
			{
				return false;
			}
			else
			{
				foreach (var clasification in clasifications)
				{
					if (isCyclic(allClasifications, clasification.Ancestor, new List<IConcept>(chain) { clasification.Descendant }))
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
