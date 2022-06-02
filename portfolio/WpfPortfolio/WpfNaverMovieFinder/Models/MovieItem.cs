using Newtonsoft.Json.Linq;

namespace WpfNaverMovieFinder.Models
{
    public class MovieItem
    {
        private string v1;
        private JToken jToken;
        private string v2;
        private string v3;
        private string v4;
        private string v5;
        private string v6;
        private string v7;
        private string v8;

        public string Title { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string subTitle { get; set; }
        public string PubDate { get; set; }
        public string Director { get; set; }
        public string Actor { get; set; }
        public string UserRating { get; set; }

        public MovieItem(string title, Newtonsoft.Json.Linq.JToken jToken, string link, string image, string subtitle, string pubDate, string director, string actor, string userRating)
        {
            Title = title;
            Link = link;
            Image = image;
            subTitle = subtitle;
            PubDate = pubDate;
            Director = director;
            Actor = actor;
            UserRating = userRating;
        }

        public MovieItem(string v1, string v2, string v3, string v4, string v5, string v6, string v7, string v8)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.v5 = v5;
            this.v6 = v6;
            this.v7 = v7;
            this.v8 = v8;
        }
    }
}
