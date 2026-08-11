using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;

namespace AabSemantics.Sample09.EntityFramework
{
	/// <summary>
	/// Code-based EF6 configuration.
	/// On .NET (Core) the "entityFramework" section of App.config is not read anymore,
	/// so the SQL Server provider has to be registered explicitly.
	/// </summary>
	public class SchoolContextConfiguration : DbConfiguration
	{
		public SchoolContextConfiguration()
		{
			SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
			SetProviderFactory(SqlProviderServices.ProviderInvariantName, SqlClientFactory.Instance);
		}
	}
}
