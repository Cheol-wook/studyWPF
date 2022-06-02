namespace WpfNaverMovieFinder.Models
{
    public class MovieItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string PubDate { get; set; }
        public string Director { get; set; }
        public string Actor { get; set; }
        public string UserRating { get; set; }

        public MovieItem(string title, string link, string image, string pubDate, string director, string actor, string userRating)
        {
            Title = title;
            Link = link;
            Image = image;
            PubDate = pubDate;
            Director = director;
            Actor = actor;
            UserRating = userRating;
        }
    }
}
