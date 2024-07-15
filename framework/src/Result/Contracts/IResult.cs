using System.Collections.Generic;

namespace Light.Contracts
{
    public interface IResult
    {
        ResultCode Code { get; }

        bool Succeeded { get; }

        bool IsFailed { get; }

        string Message { get; }

        IEnumerable<string> Errors { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}