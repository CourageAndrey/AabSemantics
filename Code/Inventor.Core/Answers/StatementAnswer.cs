namespace Inventor.Core.Answers
{
	public class StatementAnswer : Answer, IAnswer<IStatement>
	{
		#region Properties

		public IStatement Result
		{ get; }

		#endregion

		public StatementAnswer(IStatement result, FormattedText description, IExplanation explanation)
			: base(description, explanation, result == null)
		{
			Result = result;
		}
	}
}
