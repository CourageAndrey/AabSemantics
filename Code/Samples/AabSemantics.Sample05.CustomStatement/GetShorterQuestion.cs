using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Sample05.CustomStatement
{
	public class GetShorterQuestion : Question
	{
		public IConcept Person
		{ get; }

		public GetShorterQuestion(IConcept person)
		{
			Person = person.EnsureNotNull(nameof(person));
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<GetShorterQuestion, IsTallerThanStatement>()
				.WithTransitives(
					statements => Task.FromResult(true),
					c => c.SemanticNetwork.Statements
						.OfType<IsTallerThanStatement>()
						.Where(s => s.TallerPerson == c.Question.Person)
						.Select(s => new NestedQuestion(
							new GetShorterQuestion(s.ShorterPerson),
							new IStatement[] { s })),
					true)
				.Where(s => s.TallerPerson == Person)
				.SelectAllConceptsAsync(
					statement => statement.ShorterPerson,
					question => question.Person,
					"#TALLER#",
					language => $"#TALLER# is taller than",
					concepts => concepts.Distinct());
		}
	}
}
