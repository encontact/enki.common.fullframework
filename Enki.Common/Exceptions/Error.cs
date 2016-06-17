using System;

namespace Enki.Common.Exceptions
{
    public class Error
    {
        public string error { get; set; }
        public string innerExceptionMessage { get; set; }
        public string stackTrace { get; set; }

        public Error() { }

        public Error(string error, string innerException, string stackTrace)
        {
            this.error = error;
            this.innerExceptionMessage = innerException;
            this.stackTrace = stackTrace;
        }

        public Error(Exception exception)
        {
            error = exception.Message;
            innerExceptionMessage = GetInnerExceptionsMessage(exception);
            stackTrace = exception.StackTrace;
        }

        private static string GetInnerExceptionsMessage(Exception e)
        {
            if (e.InnerException == null || string.IsNullOrWhiteSpace(e.InnerException?.Message)) return "";
            var inner = GetInnerExceptionsMessage(e.InnerException);
            inner = string.IsNullOrWhiteSpace(inner) ? "" : " --> " + inner;
            return e.Message + inner;
        }
    }
}
