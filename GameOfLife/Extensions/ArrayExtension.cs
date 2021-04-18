using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Extensions
{
    public static class ArrayExtension
    {
        public static void Fill<T>(this T[] array, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException(
                    $"Prameter {nameof(array)} should not be null");
            }

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
    }
}
