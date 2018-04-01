
namespace PeopleListFozzy
{
    public class People
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Salary { get; set; }
        public string Id { get; set; }

        public People() { }

        public People(string FullName, string Position, string Salary, string Id)
        {
            this.Id = Id;
            this.FullName = FullName;
            this.Position = Position;
            this.Salary = Salary;
        }
    }
}
