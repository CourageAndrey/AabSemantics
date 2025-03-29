﻿using System;
using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Statements
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

		public GroupStatement(String id, IConcept area, IConcept concept)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageSetModule>().Statements.Names.SubjectArea),
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageSetModule>().Statements.Hints.SubjectArea))
		{
			Update(id, area, concept);
		}

		public void Update(String id, IConcept area, IConcept concept)
		{
			Update(id);
			Area = area.EnsureNotNull(nameof(area));
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Area;
			yield return Concept;
		}

		#region Consistency checking

		public override System.Boolean Equals(GroupStatement other)
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
