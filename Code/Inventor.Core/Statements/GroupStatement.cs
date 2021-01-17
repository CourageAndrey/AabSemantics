using System;
using System.Collections.Generic;

using Inventor.Core.Base;

namespace Inventor.Core.Statements
{
	public sealed class GroupStatement : Statement<GroupStatement>
	{
		#region Properties

		public IConcept Area
		{ get { return _area; } }

		public IConcept Concept
		{ get { return _concept; } }

		private IConcept _area;
		private IConcept _concept;

		#endregion

		public GroupStatement(IConcept area, IConcept concept)
			: base(new Func<ILanguage, String>(language => language.StatementNames.SubjectArea), new Func<ILanguage, String>(language => language.StatementHints.SubjectArea))
		{
			Update(area, concept);
		}

		public void Update(IConcept area, IConcept concept)
		{
			if (area == null) throw new ArgumentNullException("area");
			if (concept == null) throw new ArgumentNullException("concept");

			_area = area;
			_concept = concept;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return _area;
			yield return _concept;
		}

		#region Description

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.SubjectArea;
		}

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ "#AREA#", _area },
				{ "#CONCEPT#", _concept },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(GroupStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other._area == _area &&
						other._concept == _concept;
			}
			else return false;
		}

		#endregion
	}
}
