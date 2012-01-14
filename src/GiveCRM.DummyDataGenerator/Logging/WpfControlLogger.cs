using System;
using Ninject.Extensions.Logging;

namespace GiveCRM.DummyDataGenerator.Logging
{
    public class WpfControlLogger : ILogger
    {
        /// <summary>
        /// Logs the specified message with Warn severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Warn(string format, params object[] args)
        {}

        /// <summary>
        /// Logs the specified exception with Warn severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param><param name="args">Any arguments required for the format template.</param>
        public void Warn(Exception exception, string format, params object[] args)
        {}

        /// <summary>
        /// Logs the specified message with Error severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Error(string format, params object[] args)
        {}

        /// <summary>
        /// Logs the specified exception with Error severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Error(Exception exception, string format, params object[] args)
        {}

        /// <summary>
        /// Logs the specified message with Fatal severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Fatal(string format, params object[] args)
        {}

        /// <summary>
        /// Logs the specified exception with Fatal severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Fatal(Exception exception, string format, params object[] args)
        {}

        /// <summary>
        /// Gets the type associated with the logger.
        /// </summary>
        public Type Type{get {return this.GetType();}}

        /// <summary>
        /// Gets a value indicating whether messages with Debug severity should be logged.
        /// </summary>
        public bool IsDebugEnabled{get {return false;}}

        /// <summary>
        /// Gets a value indicating whether messages with Info severity should be logged.
        /// </summary>
        public bool IsInfoEnabled{get {return false;}}

        /// <summary>
        /// Gets a value indicating whether messages with Trace severity should be logged.
        /// </summary>
        public bool IsTraceEnabled{get {return false;}}

        /// <summary>
        /// Gets a value indicating whether messages with Warn severity should be logged.
        /// </summary>
        public bool IsWarnEnabled{get {return true;}}

        /// <summary>
        /// Gets a value indicating whether messages with Error severity should be logged.
        /// </summary>
        public bool IsErrorEnabled{get {return true;}}

        /// <summary>
        /// Gets a value indicating whether messages with Fatal severity should be logged.
        /// </summary>
        public bool IsFatalEnabled{get {return true;}}

        #region Debug, Info and Trace levels not supported.
        /// <summary>
        /// Logs the specified message with Debug severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Debug(string format, params object[] args)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Logs the specified exception with Debug severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Debug(Exception exception, string format, params object[] args)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Logs the specified message with Info severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Info(string format, params object[] args)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Logs the specified exception with Info severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Info(Exception exception, string format, params object[] args)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Logs the specified message with Trace severity.
        /// </summary>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Trace(string format, params object[] args)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Logs the specified exception with Trace severity.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The message or format template.</param>
        /// <param name="args">Any arguments required for the format template.</param>
        public void Trace(Exception exception, string format, params object[] args)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}