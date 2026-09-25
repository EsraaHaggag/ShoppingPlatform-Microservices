using BuildingBlocks.Interfaces;

namespace BuildingBlocks.Implementation
{
    public class CurrentUserService : ICurrentUserService
    {
        private static readonly Guid DefaultDeveloperUserId =
    Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

        public Guid UserId => DefaultDeveloperUserId;

        public bool IsInRole(string role)
        {
            if (UserId == DefaultDeveloperUserId)
            {
                return true;
            }

            return false;
        }
    }
}
