using System;
using Compiler.IDE.Handlers;

namespace Compiler.IDE.Handlers
{
    internal partial class ThreeAddrCodeHandler
    {
        public enum Optimizations
        {
            Alg = 0,
            Decl = 1,
            ConstProp = 2,
            CopyProp = 3,
            ConstFold = 4,
        }
    }
}

internal static class OptsExt
{
    public static string GetString(this ThreeAddrCodeHandler.Optimizations opt)
    {
        switch (opt)
        {
            case ThreeAddrCodeHandler.Optimizations.Alg:
                return "Блочные: алгебраические";
            case ThreeAddrCodeHandler.Optimizations.Decl:
                return "Блочные: объявления";
            case ThreeAddrCodeHandler.Optimizations.ConstProp:
                return "Блочные: протяжка констант";
            case ThreeAddrCodeHandler.Optimizations.CopyProp:
                return "Блочные: протяжка копий";
            case ThreeAddrCodeHandler.Optimizations.ConstFold:
                return "Блочные: сворачивание констант";
            default:
                throw new NotImplementedException($"key {opt} is missing");
        }
    }
}