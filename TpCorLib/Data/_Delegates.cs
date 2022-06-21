using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Teleperformance.Data
{
	public delegate TReturn CreatorFromSingleResultSetSqlDataReader<TReturn>(SqlDataReader reader);

	public delegate IEnumerable<TReturn> CreatorFromMultipleResultSetSqlDataReader<TReturn>(SqlDataReader reader);
}