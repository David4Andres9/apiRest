using System;
using System.Collections.Generic;

namespace APIREST_GET.Models
{
    public partial class MarvelCharacterViewModel
    {
        public class Thumbnail
        {
            public string Path { get; set; }
            public string Extension { get; set; }
        }

        public class Item
        {
            public string ResourceURI { get; set; }
            public string Name { get; set; }
        }

        public class Comics
        {
            public int Available { get; set; }
            public List<Item> Items { get; set; }
        }

        public class Series
        {
            public int Available { get; set; }
            public List<Item> Items { get; set; }
        }

        public class Stories
        {
            public int Available { get; set; }
            public List<Item> Items { get; set; }
        }

        public class Events
        {
            public int Available { get; set; }
            public List<Item> Items { get; set; }
        }
        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Thumbnail Thumbnail { get; set; }
            public string ResourceURI { get; set; }
            public Comics Comics { get; set; }
            public Series Series { get; set; }
            public Stories Stories { get; set; }
            public Events Events { get; set; }
            public List<Url> Urls { get; set; }
        }

        public class Url
        {
            public string UrlType { get; set; }
            public string Link { get; set; }
        }
        public class MarvelApiResponse
        {
            public int Code { get; set; }
            public string Status { get; set; }
            public string Copyright { get; set; }
            public string AttributionText { get; set; }
            public string AttributionHTML { get; set; }
            public string Etag { get; set; }
            public Data Data { get; set; }
        }

        public class Data
        {
            public int Offset { get; set; }
            public int Limit { get; set; }
            public int Total { get; set; }
            public int Count { get; set; }
            public List<Result> Results { get; set; }
        }
    }
}