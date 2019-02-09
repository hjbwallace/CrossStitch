namespace CrossStitch.Core.Interfaces
{
    public enum StatusType
    {
        Good, RequiresAttention, Bad
    }

    public interface IEntityWithStatus<TStatus>
    {
        TStatus Status { get; }
        StatusType StatusType { get; }
    }
}