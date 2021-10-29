using System;
using System.Linq;

using Inventor.Semantics;
using Inventor.Semantics.Questions;

namespace Samples.Semantics.Sample05.CustomStatement
{
	public class GetShorterQuestion : Question
	{
		public IConcept Person
		{ get; }

		public GetShorterQuestion(IConcept person)
		{
			if (person == null) throw new ArgumentNullException(nameof(person));

			Person = person;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<GetShorterQuestion, IsTallerThanStatement>()
				.WithTransitives(
					statements => true,
					c => c.SemanticNetwork.Statements
						.OfType<IsTallerThanStatement>()
						.Where(s => s.TallerPerson == c.Question.Person)
						.Select(s => new NestedQuestion(
							new GetShorterQuestion(s.ShorterPerson),
							new IStatement[] { s })),
					true)
				.Where(s => s.TallerPerson == Person)
				.SelectAllConcepts(
					statement => statement.ShorterPerson,
					question => question.Person,
					"#TALLER#",
					language => $"#TALLER# is taller than",
					concepts => concepts.Distinct());
		}
	}
}
