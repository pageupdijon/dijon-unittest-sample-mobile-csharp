using System;

namespace UnitTestSampleForDijon.Services.Interfaces
{
    public interface ILoggerService
    {
        void Error(string message, Exception ex = null);

        void Info(string message);

        void Warning(string message);

        void Debug(string message);

        void Fatal(string message, Exception ex = null);
    }
}
