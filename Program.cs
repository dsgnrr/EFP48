using Dapper;
using EFP48.DapperCore.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace EFP48
{
    public class Program
    {


        public static void Main(string[] args)
        {
            // TODO: Відокремити логіку створення таблиць та данних в окремі методи
            string connectionString = @"
Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\semenyuk_o\Documents\GitHub\EFP48\db_dapper.mdf;Integrated Security=True
";
            using IDbConnection connection = new SqlConnection(connectionString);

            string query = @"
DROP TABLE IF EXISTS Users;
CREATE TABLE Users
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Surname NVARCHAR(255) NOT NULL,
    Age INT NOT NULL
);
";
            //connection.Execute(query);

            var users = new List<User>
            {
                new User{ Id = Guid.NewGuid(), Name="Tom", Surname= "Due", Age= 25},
                new User{ Id = Guid.NewGuid(), Name="Alex", Surname="Jackson", Age= 25},
                new User{ Id = Guid.NewGuid(), Name="Bill", Surname="White", Age= 25},
            };

            query = @"INSERT INTO Users(Id, Name, Surname, Age) VALUES(@Id, @Name, @Surname, @Age)";
            //connection.Execute(query, users);

            query = @"SELECT * FROM Users as u ORDER BY u.Age DESC";

            var q_users = connection.Query<User>(query).ToList();

            if(q_users != null)
            {
                foreach (var item in q_users)
                {
                    Console.WriteLine(item);
                }
            }

            Guid searchId = Guid.Parse("53650437-bf01-459f-88ac-4da60f0e00ff");


            query = @"SELECT u.Name, u.Surname FROM Users as u WHERE u.Id = @Id";
            var user = connection.QueryFirstOrDefault<UserCard>(query, new { Id = searchId });
            


            if(user != null) Console.WriteLine(user);

            query = @"
DROP TABLE IF EXISTS Posts;
CREATE TABLE Posts
(
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(200),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    DeletedAt DATETIME DEFAULT NULL,
    CONSTRAINT fk_post_user FOREIGN KEY (UserId) REFERENCES Users(Id)
);
";

            //connection.Execute(query);

            query = @"INSERT INTO Posts(UserId, Title) VALUES(@UserId, @Title)";

            var list = new[]
            {
                new {UserId = Guid.Parse("53650437-bf01-459f-88ac-4da60f0e00ff"), Title= "Oh this a great day"},
                new {UserId = Guid.Parse("ccfca5d4-9084-4edf-9e1f-17bb36f23b81"), Title= "Hello world"}
            }.ToList();

            //connection.Execute(query, list);

            // використовуємо тільки якщо не використовуємо using. Connection це некерованний ресурс.
            //connection.Close();

            query = @"
SELECT u.Name, u.Surname, p.Title
FrOm Users u
JOIN Posts p ON u.Id = p.UserId;
";

            var result = connection.Query<User, Post, Post>(
                query,
                (user, post) =>
                {
                    post.User = user;
                    return post;
                }, splitOn:"Title");

            Console.WriteLine("Count: {0}", result.Count());

            foreach (var item in result)
            {
                Console.WriteLine(item.Title);
                Console.WriteLine(item.User);
            }

        }
    }
    class UserCard
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public override string ToString()
        {
            return $@"
----------------------
Name -> {Name}
Surname -> {Surname}
----------------------
";
        }
    }
}
