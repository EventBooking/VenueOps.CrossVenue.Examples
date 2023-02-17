using System;
using System.Text;

namespace CVC.Flowgear.Denormalizer.Helpers
{
    public static class ExceptionHelper
    {
        public static string Render(Exception ex, string logging = "")
        {
            var message = new StringBuilder();

            try
            {

                if (!string.IsNullOrWhiteSpace(logging))
                    message.AppendLine(logging);

                if (ex is AggregateException agg)
                {
                    foreach (var inner in agg.Flatten().InnerExceptions)
                    {
                        var text = RenderSingle(inner);
                        message.AppendLine(text);
                    }
                }
                else
                {
                    var text = RenderSingle(ex);
                    message.AppendLine(text);
                }
            }
            catch
            {
                message.AppendLine("Hit exception when trying to render");
            }

            return message.ToString();
        }

        public static string RenderSingle(Exception ex)
        {
            return ex.InnerException == null
                ? "EXCEPTION: " + ex + Environment.NewLine + Environment.NewLine
                : "OUTER: " + ex.Message + Environment.NewLine
                  + "INNER: " + ex.InnerException + Environment.NewLine
                  + Environment.NewLine;
        }
    }
}