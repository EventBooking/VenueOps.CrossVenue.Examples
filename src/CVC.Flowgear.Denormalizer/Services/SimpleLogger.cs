using System;
using System.Text;

namespace CVC.Flowgear.Denormalizer.Services
{
    public interface ISimpleLogger
    {
        void Log(string text, Exception ex = null);
        string Read();
    }

    public class SimpleLogger : ISimpleLogger //, ISdkLogger
    {
        private readonly StringBuilder _sb;

        public SimpleLogger()
        {
            _sb= new StringBuilder();
        }

        public void Log(string logText, Exception ex = null)
        {
            var exceptionText = ex == null ? string.Empty : $" - ex: {ex.Message}";
            try
            {
                _sb.AppendLine(logText + exceptionText);
                Console.WriteLine(logText + exceptionText);
            }
            catch
            {
                // ignored
            }
        }

        public string Read()
        {
            return _sb.ToString();
        }
    }
}