using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Metadata;

namespace AabSemantics.Statements
{
	public class CustomStatement : Statement<CustomStatement>
	{
		#region Properties

		public String Type
		{ get { return _definition.Type; } }

		public IDictionary<String, IConcept> Concepts
		{ get; private set; }

		private readonly CustomStatementDefinition _definition;

		#endregion

		public CustomStatement(
			String id,
			String type,
			IDictionary<String, IConcept> concepts = null)
			: base(
				id,
				Repositories.CustomStatements[type].CreateName(this),
				Repositories.CustomStatements[type].CreateHint(this))
		{
			_definition = Repositories.CustomStatements[type];

			Update(id, concepts);
		}

		public void Update(String id, IDictionary<String, IConcept> concepts)
		{
			Update(id);

			Concepts = new Dictionary<string, IConcept>(concepts ?? new Dictionary<String, IConcept>());
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			return Concepts.Values;
		}

		#region Description

		protected override String GetDescriptionTrueText(ILanguage language)
		{
			return _definition.GetDescriptionTrueText(this, language);
		}

		protected override String GetDescriptionFalseText(ILanguage language)
		{
			return _definition.GetDescriptionFalseText(this, language);
		}

		protected override String GetDescriptionQuestionText(ILanguage language)
		{
			return _definition.GetDescriptionQuestionText(this, language);
		}

		protected override IDictionary<String, IKnowledge> GetDescriptionParameters()
		{
			return Concepts.ToDictionary(
				p => $"#{p.Key}#",
				p => p.Value as IKnowledge);
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(CustomStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Type == Type &&
						other.Concepts.SequenceEqual(Concepts);
			}
			else return false;
		}

		#endregion
	}
}
