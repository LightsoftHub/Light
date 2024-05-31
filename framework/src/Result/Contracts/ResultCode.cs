namespace Light.Contracts
{
    public enum ResultCode
    {
        Unknown = 0,
        Ok = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        Error = 500,
    }
}
