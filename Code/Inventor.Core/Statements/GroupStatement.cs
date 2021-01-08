using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public sealed class GroupStatement : Statement<GroupStatement>
	{
		#region Properties

		public Concept Area
		{ get { return _area; } }

		public Concept Concept
		{ get { return _concept; } }

		private readonly Concept _area;
		private readonly Concept _concept;

		#endregion

		public GroupStatement(Concept area, Concept concept)
			: base(new Func<ILanguage, string>(language => language.StatementNames.SubjectArea), new Func<ILanguage, string>(language => language.StatementHints.SubjectArea))
		{
			if (area == null) throw new ArgumentNullException("area");
			if (concept == null) throw new ArgumentNullException("concept");

			_area = area;
			_concept = concept;
		}

		public override IEnumerable<Concept> GetChildConcepts()
		{
			yield return _area;
			yield return _concept;
		}

		#region Description

		protected override Func<string> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.SubjectArea;
		}

		protected override IDictionary<string, INamed> GetDescriptionParameters()
		{
			return new Dictionary<string, INamed>
			{
				{ "#AREA#", _area },
				{ "#CONCEPT#", _concept },
			};
		}

		#endregion

		#region Consistency checking

		public override bool Equals(GroupStatement other)
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
