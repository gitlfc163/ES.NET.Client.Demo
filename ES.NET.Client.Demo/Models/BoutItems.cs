
namespace ES.NET.Client.Demo.Models;

/// <summary>
/// 用血明细
/// </summary>
public class BoutItems
{
    /// <summary>
    /// 索引Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 科室Id
    /// </summary>
    public int Deptno { get; set; }
    /// <summary>
    /// 科室名称
    /// </summary>
    public string Deptname { get; set; }
    /// <summary>
    /// 血制品编号
    /// </summary>
    public string Bloodno { get; set; }
    /// <summary>
    /// 血制品名称
    /// </summary>
    public string Bloodname { get; set; }
    /// <summary>
    /// 用血量
    /// </summary>
    public float Boutcount { get; set; }
    /// <summary>
    /// 单位
    /// </summary>
    public string Bloodunitname { get; set; }
    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime Bodate { get; set; }

}
