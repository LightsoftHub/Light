using System.Net;

namespace Light.Application.Common.Exceptions;

public class ConflictException(string message) : ExceptionBase(message, HttpStatusCode.Conflict);