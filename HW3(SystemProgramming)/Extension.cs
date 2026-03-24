using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW3_SystemProgramming_
{
    public static class Extension
    {
        public static void ShowLoadingAnimation(this string txt)
        {
            string[] states = [".", "..", "..."];
            string[] colors = {
        "\u001b[38;2;110;231;247m",  // #6EE7F7 cyan
        "\u001b[38;2;167;139;250m",  // #A78BFA purple
        "\u001b[38;2;244;114;182m"   // #F472B6 pink
            };
            TimeSpan countdown = TimeSpan.FromSeconds(5);
            int counter = 0;

            while (countdown > TimeSpan.Zero)
            {
                if (counter > 2)
                    counter = 0;
                Console.Clear();
                Console.WriteLine($"{colors[counter]}{txt}{states[counter]}");
                Console.ResetColor();
                Thread.Sleep(1000);
                countdown = countdown.Add(TimeSpan.FromSeconds(-1));
                counter++;
            }


            Console.Clear();
        }
        public static void ShowErrorMessage(this string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
