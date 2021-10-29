using System;
using System.Linq;

using Inventor.Semantics;
using Inventor.Semantics.Questions;

namespace Samples.Semantics.Sample05.CustomStatement
{
	public class GetTallerQuestion : Question
	{
		public IConcept Person
		{ get; }

		public GetTallerQuestion(IConcept person)
		{
			if (person == null) throw new ArgumentNullException(nameof(person));

			Person = person;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<GetTallerQuestion, IsTallerThanStatement>()
				.WithTransitives(
					statements => true,
					c => c.SemanticNetwork.Statements
						.OfType<IsTallerThanStatement>()
						.Where(s => s.ShorterPerson == c.Question.Person)
						.Select(s => new NestedQuestion(
							new GetTallerQuestion(s.TallerPerson),
							new IStatement[] { s })),
					true)
				.Where(s => s.ShorterPerson == Person)
				.SelectAllConcepts(
					statement => statement.TallerPerson,
					question => question.Person,
					"#SHORTER#",
					language => $"#SHORTER# is shorter than",
					concepts => concepts.Distinct());
		}
	}
}
