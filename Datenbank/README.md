# SQLite Dokumentation

## 1 Software

### 1.1 NuGet Paket
dotnet add package Microsoft.Data.Sqlite --version 8.0.0

### 1.2 Bibliotheken

Häufig verwendete Bibliotheken:

- Microsoft.Data.Sqlite.SqliteCommand
- Microsoft.Data.Sqlite.SqliteConnection
- Microsoft.Data.Sqlite.SqliteConnectionStringBuilder
- Microsoft.Data.Sqlite.SqliteDataReader
- Microsoft.Data.Sqlite.SqliteException
- Microsoft.Data.Sqlite.SqliteFactory
- Microsoft.Data.Sqlite.SqliteParameter
- Microsoft.Data.Sqlite.SqliteTransaction

## 2 Beschreibung

Es gibt 4 Möglichkeiten um SQLite in C# zu verwenden:

**Entity Framework (EF) Core**
Beim EF Core ist es so das man keine Ahnung von C# haben muss um SQL-Abfragen zu generieren.

Abfragen

    using (var db = new BloggingContext())
    {
        var blogs = db.Blogs
            .Where(b => b.Rating > 3)
            .OrderBy(b => b.Url)
            .ToList();
    }

Speichern von Daten

    using (var db = new BloggingContext())
    {
        var blog = new Blog { Url = "http://sample.com" };
        db.Blogs.Add(blog);
        db.SaveChanges();
    }

**SQLite-NET**
SQLite-NET funktioniert wie das EF Core, aber ist eigentlich für Handys gedacht. Weil es viel Leichtgewicht hat und trotzdem mit einer guten Geschwindigkeit rennt. 

    public class Stock {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(8)]
        public string Symbol { get; set; }
    }

Abfragen

    var table = db.Table<Stock> ();
    foreach (var s in table) {
        Console.WriteLine (s.Id + " " + s.Symbol);
    }

Speichern von Daten

    if (db.Table<Stock> ().Count() == 0) {
            var newStock = new Stock ();
            newStock.Symbol = "AAPL";
            db.Insert (newStock);
            newStock = new Stock ();
            newStock.Symbol = "GOOG";
            db.Insert (newStock);
            newStock = new Stock ();
            newStock.Symbol = "MSFT";
            db.Insert (newStock);
        }

**Dapper**
Wenn man mehr Leistung und mehr Leichtgewicht haben will ist Dapper eine gute Option. Bei Dapper schreibt auch seine eigenen SQL-Abfragen.

Abfragen

    IEnumerable<Blog> result = sqlconn.Query<Blog>(@"
        SELECT *
        FROM Blog
        ORDER BY Name");

Speichern von Daten

    var blog = new Blog { Name = name };
    sqlconn.Execute(@"
                    INSERT INTO
                            Blog (Name)
                            VALUES (@name)", new { name = blog.Name }
                        );

**ADO .NET**
Eine Verbindung zu einer Datenbank muss man selber herstellen. SQL Abfragen werden selber erstellt. Der Vorteil von ADO .NET ist das wir wissen was im Code vor sich geht.

Verbindung mit der Datenbank wird aufgebaut

        const string connectionString =
            "Data Source=(local);Initial Catalog=Northwind;"
            + "Integrated Security=true";

        SqlConnection connection = new(connectionString)

Abfrage 

        const string queryString =
            "SELECT ProductID, UnitPrice, ProductName from dbo.products "
                + "WHERE UnitPrice > @pricePoint "
                + "ORDER BY UnitPrice DESC;";

Speichern von Daten

            const int paramValue = 5;
            SqlCommand command = new(queryString, connection);
            command.Parameters.AddWithValue("@pricePoint", paramValue);

## 3 Quellen
https://www.nuget.org/packages/Microsoft.Data.Sqlite

https://learn.microsoft.com/de-de/dotnet/standard/data/sqlite/?tabs=netcore-cli

https://www.youtube.com/watch?v=h9c7TZb2QuU&t=351s

