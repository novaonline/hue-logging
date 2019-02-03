using Cassandra;
using Cassandra.Mapping;
using HueLogging.Standard.Source.Cassandra.Datastax.Maps;
using HueLogging.Standard.Source.Cassandra.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLogging.Standard.Source.Cassandra.Datastax
{
	/// <summary>
	/// The Cassandra Driver Base class that every connection to Cassandra should inhereit.
	/// </summary>
	public class DatastaxCassandraDriver : ICassandraDriver
	{
		/// <summary>
		/// Logging service
		/// </summary>
		protected readonly ILogger<DatastaxCassandraDriver> Logging;

		/// <summary>
		/// Cassnadra Cluster
		/// </summary>
		protected readonly ICluster CassandraCluster;

		/// <summary>
		/// Cassandra Session
		/// </summary>
		protected readonly ISession CassandraSession;

		/// <summary>
		/// Cassandra Mappings
		/// </summary>
		protected readonly IMapper CassandraMapper;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="logging"></param>
		/// <param name="configuration"></param>
		public DatastaxCassandraDriver(ILogger<DatastaxCassandraDriver> logging, IConfiguration config)
		{
			Logging = logging;

			CassandraCluster = Cluster
				.Builder()
				.AddContactPoint(config["Cassandra:Host"])
				.Build();

			CassandraSession = CassandraCluster.Connect(config["Cassandra:HueLoggingKeySpace"]);

			CassandraSession.UserDefinedTypes.Define(HueLoggingMappings.GetUdtMaps());

			CassandraMapper = new Mapper(CassandraSession);

			MappingConfiguration.Global.Define<HueLoggingMappings>();

		}


		/// <summary>
		/// Alias for FetchAsync
		/// </summary>
		/// <typeparam name="R"></typeparam>
		/// <param name="query"></param>
		/// <param name="p"></param>
		/// <returns></returns>
		public async Task<IEnumerable<R>> QueryAsync<R>(string query, int pageSize, params object[] p) => await CassandraMapper.FetchPageAsync<R>(pageSize, null, query, p);

	}
}
