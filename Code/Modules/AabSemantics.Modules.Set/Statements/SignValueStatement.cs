using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Statements
{
	public class SignValueStatement : Statement<SignValueStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; private set; }

		public IConcept Sign
		{ get; private set; }

		public IConcept Value
		{ get; private set; }

		#endregion

		public SignValueStatement(String id, IConcept concept, IConcept sign, IConcept value)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageSetModule>().Statements.Names.SignValue),
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageSetModule>().Statements.Hints.SignValue))
		{
			Update(id, concept, sign, value);
		}

		public void Update(String id, IConcept concept, IConcept sign, IConcept value)
		{
			Update(id);
			Concept = concept.EnsureNotNull(nameof(concept));
			Sign = sign.EnsureNotNull(nameof(sign)).EnsureHasAttribute<IConcept, IsSignAttribute>(nameof(sign));
			Value = value.EnsureNotNull(nameof(value)).EnsureHasAttribute<IConcept, IsValueAttribute>(nameof(value));
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Concept;
			yield return Sign;
			yield return Value;
		}

		#region Consistency checking

		public override System.Boolean Equals(SignValueStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Concept == Concept &&
						other.Sign == Sign &&
						other.Value == Value;
			}
			else return false;
		}

		public System.Boolean CheckHasSign(IEnumerable<IStatement> statements)
		{
			return HasSignStatement.GetSigns(statements, Concept, true).Select(hs => hs.Sign).Contains(Sign);
		}

		#endregion

		public static SignValueStatement GetSignValue(IEnumerable<IStatement> statements, IConcept concept, IConcept sign)
		{
			var signValues = statements.OfType<SignValueStatement>().ToList();
			var signValue = signValues.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign);
			if (signValue != null)
			{
				return signValue;
			}
			else
			{
				foreach (var parent in statements.GetParentsOneLevel<IConcept, IsStatement>(concept))
				{
					var parentValue = GetSignValue(statements, parent, sign);
					if (parentValue != null)
					{
						return parentValue;
					}
				}
				return null;
			}
		}

		public static List<SignValueStatement> GetSignValues(IEnumerable<IStatement> statements, IConcept concept, System.Boolean recursive)
		{
			var result = new List<SignValueStatement>();
			var signValues = statements.OfType<SignValueStatement>().ToList();
			result.AddRange(signValues.Where(sv => sv.Concept == concept));
			if (recursive)
			{
				foreach (var parentSigns in statements.GetParentsOneLevel<IConcept, IsStatement>(concept).Select(c => GetSignValues(statements, c, true)))
				{
					foreach (var signValue in parentSigns)
					{
						if (result.FirstOrDefault(sv => sv.Sign == signValue.Sign) == null)
						{
							result.AddRange(parentSigns);
						}
					}
				}
			}
			return result;
		}
	}
}
