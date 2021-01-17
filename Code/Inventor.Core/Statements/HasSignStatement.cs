﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Base;

namespace Inventor.Core.Statements
{
	public sealed class HasSignStatement : Statement<HasSignStatement>
	{
		#region Properties

		public IConcept Concept
		{ get { return _concept; } }

		public IConcept Sign
		{ get { return _sign; } }

		private IConcept _concept;
		private IConcept _sign;

		#endregion

		public HasSignStatement(IConcept concept, IConcept sign)
			: base(new Func<ILanguage, String>(language => language.StatementNames.HasSign), new Func<ILanguage, String>(language => language.StatementHints.HasSign))
		{
			Update(concept, sign);
		}

		public void Update(IConcept concept, IConcept sign)
		{
			if (concept == null) throw new ArgumentNullException("concept");
			if (sign == null) throw new ArgumentNullException("sign");

			_concept = concept;
			_sign = sign;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return _concept;
			yield return _sign;
		}

		#region Description

		protected override Func<String> GetDescriptionText(ILanguageStatementFormatStrings language)
		{
			return () => language.HasSign;
		}

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ "#CONCEPT#", _concept },
				{ "#SIGN#", _sign },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(HasSignStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other._concept == _concept &&
						other._sign == _sign;
			}
			else return false;
		}

		public Boolean CheckSignDuplication(IEnumerable<HasSignStatement> hasSigns, IEnumerable<IsStatement> clasifications)
		{
			var signs = hasSigns.Where(hs => hs.Concept == _concept).Select(hs => hs.Sign).ToList();
			foreach (var parent in clasifications.GetParentsAllLevels(_concept))
			{
				foreach (var parentSign in hasSigns.Where(hs => hs.Concept == parent).Select(hs => hs.Sign))
				{
					if (signs.Contains(parentSign))
					{
						return false;
					}
				}
			}
			return true;
		}

		#endregion

		public static List<HasSignStatement> GetSigns(IEnumerable<IStatement> statements, IConcept concept, Boolean recursive)
		{
			var result = new List<HasSignStatement>();
			var hasSigns = statements.OfType<HasSignStatement>().ToList();
			result.AddRange(hasSigns.Where(sv => sv.Concept == concept));
			if (recursive)
			{
				foreach (var parentSigns in statements.GetParentsOneLevel<IConcept, IsStatement>(concept).Select(c => GetSigns(statements, c, true)))
				{
					result.AddRange(parentSigns);
				}
			}
			return result;
		}
	}
}
