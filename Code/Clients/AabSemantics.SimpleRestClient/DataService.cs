using AabSemantics.Localization;
using AabSemantics.Test.Sample;

namespace AabSemantics.SimpleRestClient
{
	public interface IDataService
	{
		ISemanticNetwork GetSemanticNetwork();
	}

	public class DataService : IDataService
	{
		private readonly ISemanticNetwork _semanticNetwork = new TestSemanticNetwork(Language.Default).SemanticNetwork;

		public ISemanticNetwork GetSemanticNetwork()
		{
			return _semanticNetwork;
		}
	}
}
