using System.Net;

namespace Light.Application.Common.Exceptions;

public class ForbiddenException(string message) : ExceptionBase(message, HttpStatusCode.Forbidden);