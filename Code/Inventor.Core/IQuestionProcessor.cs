namespace Inventor.Core
{
	public interface IQuestionProcessor
	{
		IAnswer Process(IQuestionProcessingContext context);
	}
}
