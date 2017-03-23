# SharpData
An awesome ORM for querying and modifying databases

Nuget package: **install-package sharpdata**

Supports: 
- SqlServer
- Oracle
- Postgre
- Mysql
- SqLite.

# Usage

```
using (var client = SharpFactory.Default.CreateDataClient(SqlClientFactory.Instance, "Data Source=(localdb)\\MSSQLLocalDB; Integrated Security=True;")) {
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
          .Values("foo1", "bar")
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
```
