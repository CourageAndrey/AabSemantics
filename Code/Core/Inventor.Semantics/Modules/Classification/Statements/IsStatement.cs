using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Classification.Localization;
using Inventor.Semantics.Statements;

namespace Inventor.Semantics.Modules.Classification.Statements
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
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageClassificationModule>().Statements.Names.Clasification),
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageClassificationModule>().Statements.Hints.Clasification))
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

		protected override String GetDescriptionTrueText(ILanguage language)
		{
			return language.GetExtension<ILanguageClassificationModule>().Statements.TrueFormatStrings.Clasification;
		}

		protected override String GetDescriptionFalseText(ILanguage language)
		{
			return language.GetExtension<ILanguageClassificationModule>().Statements.TrueFormatStrings.Clasification;
		}

		protected override String GetDescriptionQuestionText(ILanguage language)
		{
			return language.GetExtension<ILanguageClassificationModule>().Statements.TrueFormatStrings.Clasification;
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

		public override System.Boolean Equals(IsStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Ancestor == Ancestor &&
						other.Descendant == Descendant;
			}
			else return false;
		}

		public System.Boolean CheckCyclic(IEnumerable<IsStatement> statements)
		{
			return !statements.FindPath(typeof(IsStatement), Child, Parent).Any();
		}

		#endregion
	}
}
