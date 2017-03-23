using Sharp.Data;
using Sharp.Data.Filters;
using System;
using System.Linq;
using static Sharp.Data.Schema.Column;

namespace SharpData.Sample {
    public class Sample {

        public void Start(IDataClient client) {
            if (client.TableExists("users")) {
                client.RemoveTable("users");
            }

            client.AddTable("users",
                AutoIncrement("id").AsPrimaryKey(),
                String("username", 50).NotNull(),
                String("password", 50).NotNull()
            );
            client.AddUniqueKey("un_users", "users", "username");

            client.Insert
                  .Into("users")
                  .Columns("username", "password")
                  .Values("foo", "bar");

            var users = client.Select
                              .AllColumns()
                              .From("users")
                              .Where(Filter.Eq("username", "foo"))
                              .SkipTake(0, 1)
                              .Map<User>();

            foreach (var user in users) {
                Console.WriteLine("User: " + user.Username);
            }
        }
    }
}
