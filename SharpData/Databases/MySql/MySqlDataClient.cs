namespace SharpData.Databases.MySql {
	public class MySqlDataClient : DataClient {
	    public MySqlDataClient(IDatabase database, Dialect dialect) : base(database, dialect) {
	    }
	}
}