namespace EntityDataModel
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string Publisher { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"{Title.ToString()}, {AuthorName.ToString()} {AuthorSurname.ToString()}, {Publisher.ToString()}";
        }
    }
}