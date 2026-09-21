namespace BuildingBlocks.Common
{
    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value, bool success, Error error)
            : base(success, error)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(value, true, Error.None);
        public static new Result<T> Failure(Error error) => new(default!, false, error);
    }
}
