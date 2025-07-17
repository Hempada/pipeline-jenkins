namespace Commons.Data.Results
{
    public sealed class Result<T>
    {
        public T? Data { get; }

        public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

        public bool Valid => Errors == null || !Errors.Any();

        internal Result(T data)
        {
            Data = data;
        }

        internal Result(T data, IEnumerable<Error> errors)
        {
            Data = data;
            Errors = errors;
        }

        internal Result(IEnumerable<Error> errors)
        {
            Errors = errors;
        }

        internal Result(Error error)
        {
            Errors = new[] { error };
        }

        public static implicit operator Result(Result<T> value)
        {
            if (value.Data is null)
            {
                return Result.Fail(value.Errors);
            }

            return Result.Ok(value);
        }

        public static implicit operator Result<T>(Result value)
        {
            return new Result<T>(value.Errors);
        }
    }

    public sealed class Result
    {
        public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

        public bool Valid => Errors == null || !Errors.Any();

        internal Result()
        {
        }

        internal Result(IEnumerable<Error> errors)
        {
            Errors = errors;
        }

        public static Result Ok()
        {
            return new Result();
        }

        public static Result<T> Ok<T>(T data)
        {
            return new Result<T>(data);
        }

        public static ListResult<T> Ok<T>(IEnumerable<T> data)
        {
            return new ListResult<T>(data);
        }

        public static Result Fail(string code, string description)
        {
            return Fail(new Error(code, description));
        }

        public static Result Fail(Error error)
        {
            return Fail(new[] { error });
        }

        public static Result Fail(IEnumerable<Error> errors)
        {
            return new Result(errors);
        }
    }
}