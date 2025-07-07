namespace MoviesApp.Models

{
    public class MovieDirector
    {
        public int MoviesMovieId { get; set; }
        public Movie Movie { get; set; }

        public int DirectorsDirectorId { get; set; }
        public Director Director { get; set; }
    }
}
