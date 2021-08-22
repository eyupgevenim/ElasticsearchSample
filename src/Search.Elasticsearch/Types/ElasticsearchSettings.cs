namespace Search.Elastic.Types
{
    using System;

    public class ElasticsearchSettings
    {
        public string AliasName { get; set; }
        public Uri[] Uris { get; set; }
    }
}
