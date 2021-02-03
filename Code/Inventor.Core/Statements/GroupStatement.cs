using System;
using System.Collections.Generic;

using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public class GroupStatement : Statement<GroupStatement>, IParentChild<IConcept>
	{
		#region Properties

		public IConcept Area
		{ get; private set; }

		public IConcept Concept
		{ get; private set; }

		public IConcept Parent
		{ get { return Area; } }

		public IConcept Child
		{ get { return Concept; } }

		#endregion

		public GroupStatement(IConcept area, IConcept concept)
			: base(new Func<ILanguage, String>(language => language.StatementNames.SubjectArea), new Func<ILanguage, String>(language => language.StatementHints.SubjectArea))
		{
			Update(area, concept);
		}

		public void Update(IConcept area, IConcept concept)
		{
			if (area == null) throw new ArgumentNullException(nameof(area));
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Area = area;
			Concept = concept;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Area;
			yield return Concept;
		}

		#region Description

		protected override Func<String> GetDescriptionText(ILanguageStatements language)
		{
			return () => language.SubjectArea;
		}

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ Strings.ParamArea, Area },
				{ Strings.ParamConcept, Concept },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(GroupStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Area == Area &&
						other.Concept == Concept;
			}
			else return false;
		}

		#endregion
	}
}
