namespace Commons.Data.Results
{
    public sealed class ListResult<T>
    {
        public IEnumerable<T>? Data { get; }

        public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();


        public bool HasErrors()
        {
            return Errors != null && Errors.Any();
        }

        internal ListResult(IEnumerable<T> data)
        {
            Data = data;
        }

        internal ListResult(IEnumerable<T> data, IEnumerable<Error> errors)
        {
            Data = data;
            Errors = errors;
        }

        internal ListResult(IEnumerable<Error> errors)
        {
            Errors = errors;
        }

        internal ListResult(Error error)
        {
            Errors = new[] { error };
        }

        public static implicit operator Result(ListResult<T> value)
        {
            if (value.Data is null)
            {
                return Result.Fail(value.Errors);
            }

            return Result.Ok(value);
        }

        public static implicit operator ListResult<T>(Result value)
        {
            return new ListResult<T>(value.Errors);
        }
    }
}
