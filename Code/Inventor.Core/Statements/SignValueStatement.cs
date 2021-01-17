using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Base;

namespace Inventor.Core.Statements
{
	public sealed class SignValueStatement : Statement<SignValueStatement>
	{
		#region Properties

		public IConcept Concept
		{ get { return _concept; } }

		public IConcept Sign
		{ get { return _sign; } }

		public IConcept Value
		{ get { return _value; } }

		private IConcept _concept;
		private IConcept _sign;
		private IConcept _value;

		#endregion

		public SignValueStatement(IConcept concept, IConcept sign, IConcept value)
			: base(new Func<ILanguage, String>(language => language.StatementNames.SignValue), new Func<ILanguage, String>(language => language.StatementHints.SignValue))
		{
			Update(concept, sign, value);
		}

		public void Update(IConcept concept, IConcept sign, IConcept value)
		{
			if (concept == null) throw new ArgumentNullException("concept");
			if (sign == null) throw new ArgumentNullException("sign");
			if (value == null) throw new ArgumentNullException("value");

			_concept = concept;
			_sign = sign;
			_value = value;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return _concept;
			yield return _sign;
			yield return _value;
		}

		#region Description

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.SignValue;
		}

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ "#CONCEPT#", _concept },
				{ "#SIGN#", _sign },
				{ "#VALUE#", _value },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(SignValueStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other._concept == _concept &&
						other._sign == _sign &&
						other._value == _value;
			}
			else return false;
		}

		public Boolean CheckHasSign(IEnumerable<IStatement> statements)
		{
			return HasSignStatement.GetSigns(statements, _concept, true).Select(hs => hs.Sign).Contains(_sign);
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
