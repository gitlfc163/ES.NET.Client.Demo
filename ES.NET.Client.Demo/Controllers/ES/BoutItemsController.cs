
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ES.NET.Client.Demo.Models;
using ES.NET.Client.Demo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace ES.NET.Client.Demo.Controllers.ES;

/// <summary>
/// 血库明细索引控制器
/// </summary>
public class BoutItemsController : AreaController
{
    private readonly ElasticSetting elasticSetting;
    private readonly ElasticsearchClient esClient;

    public BoutItemsController(IOptionsSnapshot<AppSetting> options,IElasticSearchHelper esClientConnHelp)
    {
        if (options == null || options.Value == null)
            throw new ArgumentNullException(nameof(options));

        if(esClientConnHelp.ESClient==null)
            throw new ArgumentNullException(nameof(esClientConnHelp));

        elasticSetting = options.Value.ElasticSetting;
        esClient = esClientConnHelp.ESClient;
    }

    /// <summary>
    /// 创建索引
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> IndexAsync(BoutItems entity)
    {
        if (entity == null || entity.Id <= 0) return NoContent();

        var response = await esClient.IndexAsync(entity, request => request.Index(elasticSetting.IndexSetting.BOutIndex));

        if (response.IsValid)
        {
            Console.WriteLine($"Index document with ID {response.Id} succeeded.");
        }

        return Ok(response);
    }

    /// <summary>
    /// 获取文档
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var response = await esClient.GetAsync<BoutItems>(1, idx => idx.Index(elasticSetting.IndexSetting.BOutIndex));
        var entity = response.Source;

        return Ok(entity);
    }
    /// <summary>
    /// 搜索文档(lambda方式)
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> SearchAsync(string deptname)
    {
        var response = await esClient.SearchAsync<BoutItems>(s => s
                            .Index(elasticSetting.IndexSetting.BOutIndex)
                            .From(0)
                            .Size(10)
                            .Query(q => q
                                .Term(t => t.Deptname, deptname)
                            )
                        );

        if (response.IsValid)
        {
            var entity = response.Documents.FirstOrDefault();
            return Ok(entity);
        }
        else
        {
            return NotFound();
        }
    }
    /// <summary>
    /// 搜索文档(实体对象方式)
    /// </summary>
    /// <param name="deptname"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> SearchRequestAsync(string deptname)
    {
        var request = new SearchRequest(elasticSetting.IndexSetting.BOutIndex)
        {
            From = 0,
            Size = 10,
            Query = new TermQuery("deptname") { Value = deptname }
        };
        var response = await esClient.SearchAsync<BoutItems>(request);

        if (response.IsValid)
        {
            var entity = response.Documents.FirstOrDefault();
            return Ok(entity);
        }
        else
        {
            return NotFound();
        }
    }
    /// <summary>
    /// 更新文档
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UpdateAsync(BoutItems entity)
    {
        if (entity == null || entity.Id <= 0) return NoContent();

        var response = await esClient.UpdateAsync<BoutItems, object>(elasticSetting.IndexSetting.BOutIndex, 1, u => u
         .Doc(entity));

        if (response.IsValid)
        {
            Console.WriteLine("Update document succeeded.");
        }
        return Ok(response);
    }

    /// <summary>
    /// 删除文档
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return NoContent();

        var response = await esClient.DeleteAsync(elasticSetting.IndexSetting.BOutIndex, id);

        if (response.IsValid)
        {
            Console.WriteLine("Delete document succeeded.");
        }
        return Ok(response);
    }
}
