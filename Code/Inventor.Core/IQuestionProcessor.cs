namespace Inventor.Core
{
	public interface IQuestionProcessor
	{
		IAnswer Process(IProcessingContext context);
	}
}
