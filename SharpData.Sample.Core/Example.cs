using SharpData;
using SharpData.Filters;
using SharpData.Schema;
using System;
using static SharpData.Schema.Column;

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

            var username = "foo4";
            var password = "bar4";
            client.ExecSqlFormattable($"insert into users (username, password) values ({username}, {password})");

            var users = client.Select
                              .AllColumns()
                              .From("users")
                              .Where(Filter.Eq("username", "foo"))
                              .OrderBy(OrderBy.Ascending("username"))
                              .SkipTake(0, 2)
                              .Map<User>();

            Console.WriteLine("Count: " + users.Count);
            foreach (var user in users) {
                Console.WriteLine("User: " + user.Username);
            }
            
            var filter = "foo%";
            var foos = client.QueryFormattable($"select username, password from users where username like {filter}").Map<Foo>();
            Console.WriteLine("Count: " + foos.Count);
            foreach (var foo in foos) {
                Console.WriteLine("Foo: " + foo.Username);
            }
            Console.WriteLine("End----");
        }
        private class Foo { public string Username { get; set; } }
    }
}
