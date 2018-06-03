using System;

namespace Compiler.IDE
{
    internal enum Optimizations
    {
        Alg = 0,
        Decl = 1,
        ConstProp = 2,
        CopyProp = 3,
        ConstFold = 4,
        Subexpr = 5,
        GlobalConstProp = 6
    }

    internal static class OptsExt
    {
        public static string GetString(this Optimizations opt)
        {
            switch (opt)
            {
                case Optimizations.Alg:
                    return "Блочные: алгебраические";
                case Optimizations.Decl:
                    return "Блочные: объявления";
                case Optimizations.ConstProp:
                    return "Блочные: протяжка констант";
                case Optimizations.CopyProp:
                    return "Блочные: протяжка копий";
                case Optimizations.ConstFold:
                    return "Блочные: сворачивание констант";
                case Optimizations.Subexpr:
                    return "Блочные: общие подвыражения";
                case Optimizations.GlobalConstProp:
                    return "Глобальная: протяжка констант";
                default:
                    throw new NotImplementedException($"key {opt} is missing");
            }
        }
    }
}