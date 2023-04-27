using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Utils;

namespace Samples.Semantics.Sample05.CustomStatement
{
	public class IsTallerQuestion : Question
	{
		public IConcept TallerPerson
		{ get; }

		public IConcept ShorterPerson
		{ get; }

		public IsTallerQuestion(IConcept tallerPerson, IConcept shorterPerson)
		{
			TallerPerson = tallerPerson.EnsureNotNull(nameof(tallerPerson));
			ShorterPerson = shorterPerson.EnsureNotNull(nameof(shorterPerson));
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsTallerQuestion, IsTallerThanStatement>()
				.WithTransitives(
					statements => true,
					c => c.SemanticNetwork.Statements
						.OfType<IsTallerThanStatement>()
						.Where(s => s.TallerPerson == c.Question.TallerPerson)
						.Select(s => new NestedQuestion(
							new IsTallerQuestion(s.ShorterPerson, c.Question.ShorterPerson),
							new IStatement[] { s })))
				.Where(s => s.TallerPerson == TallerPerson && s.ShorterPerson == ShorterPerson)
				.SelectBooleanIncludingChildren(
					statements => statements.Count > 0,
					language => "Yes, #TALLER# is taller than #SHORTER#.",
					language => "No, #SHORTER# is taller than #TALLER#.",
					new Dictionary<String, IKnowledge>
					{
						{ "#TALLER#", TallerPerson },
						{ "#SHORTER#", ShorterPerson },
					});
		}
	}
}
