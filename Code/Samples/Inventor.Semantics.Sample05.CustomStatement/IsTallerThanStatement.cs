using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Localization;

namespace Samples.Semantics.Sample05.CustomStatement
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

		protected override string GetDescriptionTrueText(ILanguage language)
		{
			return "#TALLER# is taller than #SHORTER#.";
		}

		protected override string GetDescriptionFalseText(ILanguage language)
		{
			return "It's false, that #TALLER# is taller than #SHORTER#.";
		}

		protected override string GetDescriptionQuestionText(ILanguage language)
		{
			return "Is #TALLER# taller than #SHORTER#?";
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
