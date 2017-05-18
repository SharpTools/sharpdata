using System;

namespace SharpData {
    public class SharpDbConfig {
        public string DbProviderName { get; set; }
        public IDataProvider DataProvider { get; set; }
    	public Database Database { get; set; }
    	public DataClient DataClient { get; set; }
    }
}