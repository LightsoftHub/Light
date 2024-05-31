using System.Net;

namespace Light.Application.Common.Exceptions;

public class InternalServerErrorException(string message) : ExceptionBase(message, HttpStatusCode.InternalServerError);