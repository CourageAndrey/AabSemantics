using Inventor.Semantics;
using Inventor.Semantics.Answers;

namespace Samples.Semantics.Sample07.CustomModule
{
	public class CustomAnswer : Answer, IAnswer<IText>
	{
		public IText Result
		{ get { return Description; } }

		public CustomAnswer(IText result, IExplanation explanation)
			: base(result, explanation, false)
		{ }
	}
}
