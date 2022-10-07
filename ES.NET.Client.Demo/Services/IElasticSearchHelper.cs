using Elastic.Clients.Elasticsearch;

namespace ES.NET.Client.Demo.Services;

public abstract class IElasticSearchHelper
{
    public ElasticsearchClient ESClient { get; set; }
}
