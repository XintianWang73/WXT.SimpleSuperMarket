namespace WXT.SuperMarket.Data.Entities
{
    /// <summary>
    /// Defines the <see cref="Customer" />
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the PassWord
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// Gets or sets the EmaiAddress
        /// </summary>
        public string EmaiAddress { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return $"Id = {Id} UserName = {UserName} EmailAddress = {EmaiAddress} PhoneNumber = {PhoneNumber}";
        }
    }
}
