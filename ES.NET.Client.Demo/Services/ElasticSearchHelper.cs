using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using ES.NET.Client.Demo.Models;
using Microsoft.Extensions.Options;

namespace ES.NET.Client.Demo.Services;


public class ElasticSearchHelper: IElasticSearchHelper
{
    //ES连接

    /// <summary>
    /// ElasticSearchConn
    /// builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));
    /// </summary>
    /// <param name="options"></param>
    public ElasticSearchHelper(IOptionsSnapshot<AppSetting> options)
    {
        if (options == null || options.Value == null)
            throw new ArgumentNullException(nameof(options));

        if (this.ESClient == null)
        {
            ElasticSetting elasticSetting = options.Value.ElasticSetting;
            var settings = new ElasticsearchClientSettings(new Uri(elasticSetting.Uri))
                .CertificateFingerprint(elasticSetting.Fingerprint)
                .Authentication(new BasicAuthentication(elasticSetting.UserName, elasticSetting.Password));
            //.Authentication(new ApiKey(elasticSetting.ApiKey));

            this.ESClient = new ElasticsearchClient(settings);
        }
    }

    
}
