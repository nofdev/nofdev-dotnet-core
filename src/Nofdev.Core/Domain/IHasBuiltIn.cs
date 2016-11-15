namespace Nofdev.Core.Domain
{
    /// <summary>
    /// 可能有系统内置的数据
    /// </summary>
    public interface IHasBuiltIn
    {
        bool IsBuiltIn { get; set; }
    }
}