using System;
using System.Collections.Generic;

using Inventor.Core;
using Inventor.Core.Statements;
using Inventor.Core.Localization;

namespace Samples._05.CustomStatement
{
	public class IsTallerThanStatement : Statement<IsTallerThanStatement>, IParentChild<IConcept>
	{
		#region Properties

		public IConcept TallerPerson
		{ get; }

		public IConcept ShorterPerson
		{ get; }

		public IConcept Parent
		{ get { return TallerPerson; } }

		public IConcept Child
		{ get { return ShorterPerson; } }

		#endregion

		public IsTallerThanStatement(IConcept tallerPerson, IConcept shorterPerson)
			: base(null, new LocalizedStringConstant(language => "Is taller that"))
		{
			if (tallerPerson == null) throw new ArgumentNullException(nameof(tallerPerson));
			if (shorterPerson == null) throw new ArgumentNullException(nameof(shorterPerson));

			TallerPerson = tallerPerson;
			ShorterPerson = shorterPerson;
		}


		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return TallerPerson;
			yield return ShorterPerson;
		}

		protected override string GetDescriptionText(ILanguageStatements language)
		{
			// IMPORTANT: It is dirty 'hack' to perform such comparisons.
			// Please, do not repeat it within your projects.
			// It is connected with the way, how Language questions are organized:
			// TrueStatementFormatStrings, FalseStatementFormatStrings and QuestionStatementFormatStrings are ILanguageStatements.
			if (language == Language.Default.TrueStatementFormatStrings)
			{
				return "#TALLER# is taller than #SHORTER#.";
			}
			else if (language == Language.Default.FalseStatementFormatStrings)
			{
				return "It's false, that #TALLER# is taller than #SHORTER#.";
			}
			else if (language == Language.Default.QuestionStatementFormatStrings)
			{
				return "Is #TALLER# taller than #SHORTER#?";
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		protected override IDictionary<string, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<String, IKnowledge>
			{
				{ "#TALLER#", TallerPerson },
				{ "#SHORTER#", ShorterPerson },
			};
		}

		public override bool Equals(IsTallerThanStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.TallerPerson == TallerPerson &&
						other.ShorterPerson == ShorterPerson;
			}
			else return false;
		}
	}

	public static class SubjectStatementExtensions
	{
		public static IsTallerThanStatement IsTallerThan(this StatementBuilder builder, IConcept shorter)
		{
			var statement = new IsTallerThanStatement(builder.Subject, shorter);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static IsTallerThanStatement IsShorterThan(this StatementBuilder builder, IConcept taller)
		{
			var statement = new IsTallerThanStatement(taller, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
