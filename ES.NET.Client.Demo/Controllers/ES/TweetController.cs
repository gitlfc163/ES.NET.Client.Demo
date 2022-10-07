﻿
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ES.NET.Client.Demo.Models;
using ES.NET.Client.Demo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;


namespace ES.NET.Client.Demo.Controllers.ES;

/// <summary>
/// 发送消息
/// </summary>
public class TweetController : AreaController
{
    private readonly ElasticSetting elasticSetting;
    private readonly ElasticsearchClient esClient;

    public TweetController(IOptionsSnapshot<AppSetting> options,IESClientConnHelp esClientConnHelp)
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
    /// <param name="tweet"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> IndexAsync(Tweet tweet)
    {
        if (tweet == null || tweet.Id <= 0) return NoContent();

        var response = await esClient.IndexAsync(tweet, request => request.Index(elasticSetting.IndexSetting.TweetIndex));

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
        var response = await esClient.GetAsync<Tweet>(1, idx => idx.Index(elasticSetting.IndexSetting.TweetIndex));
        var tweet = response.Source;

        return Ok(tweet);
    }
    /// <summary>
    /// 搜索文档(lambda方式)
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> SearchAsync(string userName)
    {
        var response = await esClient.SearchAsync<Tweet>(s => s
                            .Index(elasticSetting.IndexSetting.TweetIndex)
                            .From(0)
                            .Size(10)
                            .Query(q => q
                                .Term(t => t.User, userName)
                            )
                        );

        if (response.IsValid)
        {
            var tweet = response.Documents.FirstOrDefault();
            return Ok(tweet);
        }
        else
        {
            return NotFound();
        }
    }
    /// <summary>
    /// 搜索文档(实体对象方式)
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> SearchRequestAsync(string userName)
    {
        var request = new SearchRequest(elasticSetting.IndexSetting.TweetIndex)
        {
            From = 0,
            Size = 10,
            Query = new TermQuery("user") { Value = userName }
        };
        var response = await esClient.SearchAsync<Tweet>(request);

        if (response.IsValid)
        {
            var tweet = response.Documents.FirstOrDefault();
            return Ok(tweet);
        }
        else
        {
            return NotFound();
        }
    }
    /// <summary>
    /// 更新文档
    /// </summary>
    /// <param name="tweet"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UpdateAsync(Tweet tweet)
    {
        if (tweet == null || tweet.Id <= 0) return NoContent();

        var response = await esClient.UpdateAsync<Tweet, object>(elasticSetting.IndexSetting.TweetIndex, 1, u => u
         .Doc(tweet));

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

        var response = await esClient.DeleteAsync(elasticSetting.IndexSetting.TweetIndex, id);

        if (response.IsValid)
        {
            Console.WriteLine("Delete document succeeded.");
        }
        return Ok(response);
    }
}
