using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Names.SignValue),
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Hints.SignValue))
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

		public async Task<System.Boolean> CheckHasSignAsync(IEnumerable<IStatement> statements)
		{
			var signs = await HasSignStatement.GetSignsAsync(statements, Concept, true);
			return (await signs.Select(hs => hs.Sign).ToListAsync()).Contains(Sign);
		}

		#endregion

		public static async Task<SignValueStatement> GetSignValueAsync(IEnumerable<IStatement> statements, IConcept concept, IConcept sign)
		{
			var signValues = await statements.OfType<SignValueStatement>().ToListAsync();
			var signValue = await signValues.FirstOrDefaultAsync(sv => sv.Concept == concept && sv.Sign == sign);
			if (signValue != null)
			{
				return signValue;
			}
			else
			{
				foreach (var parent in await statements.GetParentsOneLevelAsync<IConcept, IsStatement>(concept))
				{
					var parentValue = await GetSignValueAsync(statements, parent, sign);
					if (parentValue != null)
					{
						return parentValue;
					}
				}

				return null;
			}
		}

		public static async Task<List<SignValueStatement>> GetSignValuesAsync(IEnumerable<IStatement> statements, IConcept concept, System.Boolean recursive)
		{
			var result = new List<SignValueStatement>();
			var signValues = await statements.OfType<SignValueStatement>().ToListAsync();
			result.AddRange(signValues.Where(sv => sv.Concept == concept));

			if (recursive)
			{
				foreach (var parent in await statements.GetParentsOneLevelAsync<IConcept, IsStatement>(concept))
				{
					var parentSignValues = await GetSignValuesAsync(statements, parent, true);
					foreach (var signValue in parentSignValues)
					{
						if (! await result.AnyAsync(sv => sv.Sign == signValue.Sign))
						{
							result.AddRange(parentSignValues);
						}
					}
				}
			}
			return result;
		}

		public System.Boolean CheckHasSign(IEnumerable<IStatement> statements)
		{
			return CheckHasSignAsync(statements).Await();
		}

		public static SignValueStatement GetSignValue(IEnumerable<IStatement> statements, IConcept concept, IConcept sign)
		{
			return GetSignValueAsync(statements, concept, sign).Await();
		}

		public static List<SignValueStatement> GetSignValues(IEnumerable<IStatement> statements, IConcept concept, System.Boolean recursive)
		{
			return GetSignValuesAsync(statements, concept, recursive).Await();
		}
	}
}
