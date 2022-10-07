using Elastic.Clients.Elasticsearch;

namespace ES.NET.Client.Demo.Services;

public abstract class IESClientConnHelp
{
    public ElasticsearchClient ESClient { get; set; }
}
