﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hdp.CoreRx.Models
{
    public class ElectionArticle
    {
        public int Id {get;set;} = 0;
        public string Title {get;set;} = "";
        public string Body {get;set;} = "";

        [JsonProperty(PropertyName="media_type")]
        public MediaType Type {get;set;} = MediaType.Image;

        [JsonProperty(PropertyName="image_url")]
        public string ImageUrl {get;set;} = "";

        [JsonProperty(PropertyName="video_url")]
        public string VideoUrl {get;set;} = "";

        [JsonProperty(PropertyName="created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string VideoImageUrl { get; set; } = "";
        public string VideoId { get; set; } = "";

        public enum MediaType
        {
            None,
            Image,
            Video
        }
    }
}

