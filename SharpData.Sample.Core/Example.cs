using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;
using System;
using static Sharp.Data.Schema.Column;

namespace SharpData.Sample {
    public class Example {

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
                  .Values("foo", "bar")
                  .Values("foo2", "bar")
                  .Values("foo3", "bar");

            var users = client.Select
                              .AllColumns()
                              .From("users")
                              .Where(Filter.Eq("username", "foo"))
                              .OrderBy(OrderBy.Ascending("username"))
                              .SkipTake(0, 2)
                              .Map<User>();

            foreach (var user in users) {
                Console.WriteLine("User: " + user.Username);
            }
        }
    }
}
