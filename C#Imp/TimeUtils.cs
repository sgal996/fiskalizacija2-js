using System;

namespace Fiskalizacija2 {
    public static class TimeUtils {
        public static string GetCurrentDateTimeString() {
            var now = DateTime.Now;
            return $"{now:yyyy-MM-ddTHH:mm:ss}.{now:fff}0";
        }
    }
}
