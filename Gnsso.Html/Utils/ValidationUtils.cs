using System;

namespace Gnsso.Html
{
    public static class ValidationUtils
    {
        public static void NotNull(object argument, string name)
        {
            if (argument == null) throw new ArgumentNullException(name);
        }
    }
}
