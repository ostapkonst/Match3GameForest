using System;

namespace Match3GameForest
{
    public static class RandomExtension
    {
        // https://stackoverflow.com/questions/25275873/generate-random-boolean-probability

        public static bool NextBool(this Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100f;
        }

        public static T NextEnum<T>(this Random r)
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(r.Next(values.Length));
        }
    }
}
