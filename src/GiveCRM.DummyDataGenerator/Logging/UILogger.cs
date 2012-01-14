using System;

namespace GiveCRM.DummyDataGenerator.Logging
{
    internal class UILogger
    {
        public Action<string> LogAction{get;set;}

        public void Log(string logString)
        {
            this.LogAction(logString);
        }
    }
}