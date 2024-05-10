using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.ResultType
{
    public class Result
    {
        private readonly ILogger _logger;

            public Result(ILogger logger)

            {

                _logger = logger;

            }

            internal Result(bool succeeded, string message, object entity = default, string exception = null)

            {

                Succeeded = succeeded;

                Message = message;

                ExceptionError = exception;

                Entity = entity;

            }

            internal Result(bool succeeded, object entity = default)

            {

                Succeeded = succeeded;

                Entity = entity;

            }

            public bool Succeeded { get; set; }

            public object Entity { get; set; }

            public string Error { get; set; }

            public string ExceptionError { get; set; }

            public string Message { get; set; }

            public static Result Success(object entity)

            {

                ILogger logger = ApplicationLogging.LoggerFactory.CreateLogger(nameof(entity));

                logger.LogInformation("Request successful!");

                return new Result(true, "Request successful!", entity);

            }

            public static Result Success(Type request, string message)

            {

                ILogger logger = ApplicationLogging.LoggerFactory.CreateLogger(nameof(request));

                logger.LogInformation(message);

                return new Result(true, message);

            }

            public static Result Success(object entity, string message, Type request)

            {

                ILogger logger = new LoggerFactory().CreateLogger(nameof(request));

                logger.LogInformation(message, entity);

                return new Result(true, message, entity);

            }

            public static Result Success(object entity, Type request)

            {

                ILogger logger = ApplicationLogging.LoggerFactory.CreateLogger(nameof(request));

                logger.LogInformation("Request successful!", entity);

                return new Result(true, entity);

            }

            public static Result Success<T>(string message)

            {

                ILogger logger = ApplicationLogging.CreateLogger<T>();

                logger.LogInformation(message);

                return new Result(true, message);

            }

            public static Result Success<T>(object entity)

            {

                ILogger logger = ApplicationLogging.CreateLogger<T>();

                logger.LogInformation($"{Environment.NewLine} {JsonConvert.SerializeObject(entity)}");

                return new Result(true, entity);

            }

            public static Result Success<T>(string message, object entity)

            {

                ILogger logger = ApplicationLogging.CreateLogger<T>();

                logger.LogInformation($"{message} {Environment.NewLine} {JsonConvert.SerializeObject(entity)}");

                return new Result(true, message, entity);

            }

            public static Result Success<T>(object entity, T request)

            {

                ILogger logger = ApplicationLogging.CreateLogger<T>();

                logger.LogInformation("Request successul!");

                return new Result(true, entity);

            }

            public static Result Success<T>(object entity, T request, string message)

            {

                ILogger logger = ApplicationLogging.CreateLogger<T>();

                logger.LogInformation(message);

                return new Result(true, message);

            }

            public static Result Success<T>(object entity, string message, T request)

            {

                ILogger logger = ApplicationLogging.CreateLogger<T>();

                logger.LogInformation(message);

                return new Result(true, message, entity);

            }

            public static Result Failure(Type request, string error)

            {

                ILogger logger = ApplicationLogging.LoggerFactory.CreateLogger(nameof(request));

                logger.LogError(error);

                return new Result(false, error);

            }

            public static Result Failure(Type request, string prefixMessage, Exception ex)

            {

                ILogger logger = ApplicationLogging.LoggerFactory.CreateLogger(nameof(request));

                logger.LogError($"{prefixMessage} Error: {ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");

                return new Result(false, $"{prefixMessage} Error: {ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");

            }

            public static Result Failure(string error)

            {

                return new Result(false, error);

            }

            public static Result Failure<T>(string error)

            {

                ILogger logger = ApplicationLogging.CreateLogger<T>();

                logger.LogError(error);

                return new Result(false, error);

            }

            public static Result Failure<T>(T request, string error)

            {

                ILogger logger = ApplicationLogging.CreateLogger<T>();

                logger.LogError(error);

                return new Result(false, error);

            }
            public static class ApplicationLogging

            {

                public static ILoggerFactory LoggerFactory { get; } = new LoggerFactory();

                public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();

            }
    }
}
