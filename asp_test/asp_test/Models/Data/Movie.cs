using System;
using System.Collections.Generic;

namespace asp_test.Models.Data
{
    public partial class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = null!;
        public double Price { get; set; }
    }
}
