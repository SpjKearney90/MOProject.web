namespace MOProject.Models
{
    public class BlogPost
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }

        // 👇 Add this line
        public string ImageUrl { get; set; }
    }
}

