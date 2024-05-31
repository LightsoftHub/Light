using System.Net;

namespace Light.Application.Common.Exceptions;

public class UnauthorizedException(string message = "Unauthorized") : ExceptionBase(message, HttpStatusCode.Unauthorized);