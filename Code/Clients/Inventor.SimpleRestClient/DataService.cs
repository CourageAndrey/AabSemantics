using Inventor.Semantics;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Test.Sample;

namespace Inventor.SimpleRestClient
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
