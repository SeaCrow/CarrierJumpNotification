using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CarrierJumpNotification
{
    static class ColorMischief
    {
        private static Color[] Gradient =
        {
                    Color.FromArgb(255, 255, 104, 0),
                    Color.FromArgb(255, 255, 255, 0),
                    Color.FromArgb(255, 128, 255, 0),
                    Color.FromArgb(255, 0, 255, 0),
                    Color.FromArgb(255, 0, 255, 128),
                    Color.FromArgb(255, 0, 255, 255),
                    Color.FromArgb(255, 0, 128, 255),
                    Color.FromArgb(255, 0, 0, 255),
                    Color.FromArgb(255, 127, 0, 255),
                    Color.FromArgb(255, 255, 0, 255),
                    Color.FromArgb(255, 255, 0, 127),
                    Color.FromArgb(255, 255, 0, 0),
                    Color.FromArgb(255, 128, 128, 128),
                    Color.FromArgb(255, 191, 191, 191),
                    Color.FromArgb(255, 255, 255, 255)
                };

        public static Color GetColorFromGradient(int index)
        {
            if (index > Gradient.Length)
                return Gradient.Last();

            if (index < 0)
                return Gradient.First();

            return Gradient[index];
        }

        public static Color GetSmoothColor(double colorCode)
        {
            if ((colorCode % 1) == 0)
                return GetColorFromGradient((int)colorCode);

            int totalValue = (int)colorCode;

            double fractalValue = colorCode - totalValue;

            Color one = GetColorFromGradient(totalValue);
            Color two = GetColorFromGradient(totalValue + 1);

            Color result = Color.FromRgb(
                CalculateWeightedAverage(one.R,two.R,fractalValue),
                CalculateWeightedAverage(one.G, two.G, fractalValue),
                CalculateWeightedAverage(one.B, two.B, fractalValue)
                );

            return result;
        }

        private static byte CalculateWeightedAverage(byte one, byte two, double weight)
        {
            return (byte)((one * (1 - weight)) + (two * weight));
        }

        public static int GradientSize { get { return Gradient.Length; } }

        public static Color SelectionColor(Color baseColor)
        {
            List<BId> bytes = new List<BId>();
            bytes.Add(new BId() { id = 1, value = baseColor.R });
            bytes.Add(new BId() { id = 2, value = baseColor.G });
            bytes.Add(new BId() { id = 3, value = baseColor.B });

            bytes = bytes.OrderBy(b => b.value).ToList();

            if (bytes[0].value == bytes[1].value && bytes[1].value == bytes[2].value)
            {
                bytes[0].Dec(40);
                bytes[1].Dec(40);
                bytes[2].Dec(40);
            }
            else
            {
                bytes[2].Dec(40);
                bytes[1].Inc(101);
            }

            return Color.FromRgb(bytes.Find(b => b.id == 1).value, bytes.Find(b => b.id == 2).value, bytes.Find(b => b.id == 3).value);
        }

        public static Color BackgroundColor(Color baseColor)
        {
            List<BId> bytes = new List<BId>();
            bytes.Add(new BId() { id = 1, value = baseColor.R });
            bytes.Add(new BId() { id = 2, value = baseColor.G });
            bytes.Add(new BId() { id = 3, value = baseColor.B });

            bytes = bytes.OrderBy(b => b.value).ToList();

            if (bytes[0].value == bytes[1].value && bytes[1].value == bytes[2].value)
            {
                bytes[0].Dec(187);
                bytes[1].Dec(187);
                bytes[2].Dec(187);
            }
            else
            {
                bytes[2].Dec(187);
                bytes[1].Dec(86);
            }

            return Color.FromRgb(bytes.Find(b => b.id == 1).value, bytes.Find(b => b.id == 2).value, bytes.Find(b => b.id == 3).value);
        }

        class BId
        {
            public byte value = 0;
            public byte id = 0;

            public void Inc(int Value)
            {
                if ((value + Value) > 255)
                    value = 255;
                else
                    value += (byte)Value;
            }
            public void Dec(int Value)
            {
                if ((value - Value) < 0)
                    value = 0;
                else
                    value -= (byte)Value;
            }
        }

        public static IEnumerable<Visual> GetChildren(this Visual parent, bool recurse = true)
        {
            if (parent != null)
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    // Retrieve child visual at specified index value.
                    var child = VisualTreeHelper.GetChild(parent, i) as Visual;

                    if (child != null)
                    {
                        yield return child;

                        if (recurse)
                        {
                            foreach (var grandChild in child.GetChildren(true))
                            {
                                yield return grandChild;
                            }
                        }
                    }
                }
            }
        }
    }
}
