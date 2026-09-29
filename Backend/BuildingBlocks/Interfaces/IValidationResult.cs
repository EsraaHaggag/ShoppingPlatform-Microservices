using BuildingBlocks.Common;

namespace BuildingBlocks.Interfaces
{
    public interface IValidationResult
    {
        public static readonly Error ValidationError = new(
            "ValidationError",
            "A validation error occurred.");

        Error[] Errors { get; }
    }
}
