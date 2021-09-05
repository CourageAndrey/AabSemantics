using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;

namespace Inventor.Core.Statements
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
			: base(id, new Func<ILanguage, String>(language => language.StatementNames.SignValue), new Func<ILanguage, String>(language => language.StatementHints.SignValue))
		{
			Update(id, concept, sign, value);
		}

		public void Update(String id, IConcept concept, IConcept sign, IConcept value)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (sign == null) throw new ArgumentNullException(nameof(sign));
			if (value == null) throw new ArgumentNullException(nameof(value));
			if (!sign.HasAttribute<IsSignAttribute>()) throw new ArgumentException("Sign concept has to be marked as IsSign Attribute.", nameof(sign));
			if (!value.HasAttribute<IsValueAttribute>()) throw new ArgumentException("Value concept has to be marked as IsValue Attribute.", nameof(value));

			Update(id);
			Concept = concept;
			Sign = sign;
			Value = value;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Concept;
			yield return Sign;
			yield return Value;
		}

		#region Description

		protected override String GetDescriptionText(ILanguageStatements language)
		{
			return language.SignValue;
		}

		protected override IDictionary<String, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<String, IKnowledge>
			{
				{ Strings.ParamConcept, Concept },
				{ Strings.ParamSign, Sign },
				{ Strings.ParamValue, Value },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(SignValueStatement other)
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

		public Boolean CheckHasSign(IEnumerable<IStatement> statements)
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

		public static List<SignValueStatement> GetSignValues(IEnumerable<IStatement> statements, IConcept concept, Boolean recursive)
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
