using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.Utilities
{
    public static class InputHelper
    {
        public static bool IsKeyAChar(Key inputKey)
        {
            return inputKey >= Key.A && inputKey <= Key.Z;
        }

        public static bool IsKeyADigit(Key inputKey)
        {
            return inputKey >= Key.D0 && inputKey <= Key.D9;
        }
    }
}
