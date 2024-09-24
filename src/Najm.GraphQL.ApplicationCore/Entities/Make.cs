namespace Najm.GraphQL.ApplicationCore.Entities
{
    public partial class Make
    {
        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        
        public bool? Active { get; set; }
    }
}
