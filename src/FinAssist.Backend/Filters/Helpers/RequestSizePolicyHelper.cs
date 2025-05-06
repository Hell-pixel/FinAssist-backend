using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using static System.MidpointRounding;
using static FinAssist.Backend.Filters.Helpers.RequestSizePolicyHelper.DataUnit;

namespace FinAssist.Backend.Filters.Helpers
{
    public static class RequestSizePolicyHelper
    {
        public enum DataUnit
        {
            B,
            KiB,
            MiB,
            GiB
        }

        private static TFeature? GetRequestBodySizeFeature<TFeature>(this ActionContext context)
        {
            return context.HttpContext.Features.Get<TFeature>();
        }

        public static long? GetRequestBodySizeLimit(this ActionContext context)
        {
            return context.GetRequestBodySizeFeature<IHttpMaxRequestBodySizeFeature>()?.MaxRequestBodySize;
        }

        public static double? GetRequestBodySizeLimit(this ActionContext context, DataUnit unit)
        {
            return ConvertTo(context.GetRequestBodySizeFeature<IHttpMaxRequestBodySizeFeature>()?.MaxRequestBodySize,
                unit);
        }

        public static double? GetRequestContentSize(this ActionContext context, DataUnit unit)
        {
            return ConvertTo(context.HttpContext.Request.Headers.ContentLength, unit);
        }

        public static double Round(double? size, MidpointRounding rounding = ToEven)
        {
            return size is null ? default : Math.Round(size.Value, rounding);
        }

        public static double? ConvertTo(long? bytes, DataUnit unit)
        {
            if (bytes is null) return default;

            return unit switch
            {
                B => bytes,
                KiB => (double) bytes / 1024,
                MiB => (double) bytes / 1024 / 1024,
                GiB => (double) bytes / 1024 / 1024 / 1024,
                _ => bytes
            };
        }
    }
}