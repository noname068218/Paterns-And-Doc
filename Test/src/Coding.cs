public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var users = new List<User>
{
    new User { Name = "Alex", Age = 17 },
    new User { Name = "John", Age = 25 },
    new User { Name = "Maria", Age = 30 },
    new User { Name = "Anna", Age = 17 },
    new User { Name = "Mark", Age = 25 }
};