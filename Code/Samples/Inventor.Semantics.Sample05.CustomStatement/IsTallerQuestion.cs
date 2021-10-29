using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics;
using Inventor.Semantics.Questions;

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
			if (tallerPerson == null) throw new ArgumentNullException(nameof(tallerPerson));
			if (shorterPerson == null) throw new ArgumentNullException(nameof(shorterPerson));

			TallerPerson = tallerPerson;
			ShorterPerson = shorterPerson;
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
