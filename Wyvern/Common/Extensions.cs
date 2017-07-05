namespace Noside.Common
{
    public static class Extensions
    {
        public static T[] Init<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
            return array;
        }
    }
}
