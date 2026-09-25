namespace BuildingBlocks.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        public bool IsInRole(string role);
    }
}
