namespace BlazorPolaris.Utilities
{
    public class CssBuilder
    {
        private readonly StringBuilder _stringBuilder;

        public CssBuilder(string baseClass)
        {
            _stringBuilder = new StringBuilder(baseClass);
        }

        public CssBuilder AddClass(string className, bool condition = true)
        {
            if (condition)
            {
                _stringBuilder.Append(' ').Append(className);
            }
            return this;
        }

        public string Build() => _stringBuilder.ToString().Trim();
    }
} 