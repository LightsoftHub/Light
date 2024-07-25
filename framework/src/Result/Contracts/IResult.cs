namespace Light.Contracts
{
    public interface IResult
    {
        string Code { get; }

        bool Succeeded { get; }

        string Message { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}