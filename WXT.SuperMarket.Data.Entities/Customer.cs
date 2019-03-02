namespace WXT.SuperMarket.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public string EmaiAddress { get; set; }

        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return $"Id = {Id} UserName = {UserName} EmailAddress = {EmaiAddress} PhoneNumber = {PhoneNumber}";
        }
    }
}
