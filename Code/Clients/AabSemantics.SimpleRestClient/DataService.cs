using AabSemantics.IntegrationTests;
using AabSemantics.Localization;

namespace AabSemantics.SimpleRestClient
{
	public interface IDataService
	{
		ISemanticNetwork GetSemanticNetwork();
	}

	public class DataService : IDataService
	{
		private readonly ISemanticNetwork _semanticNetwork;

		public DataService()
		{
			_semanticNetwork = new SemanticNetwork(Language.Default);
			_semanticNetwork.CreateCombinedTestData();
		}

		public ISemanticNetwork GetSemanticNetwork()
		{
			return _semanticNetwork;
		}
	}
}
