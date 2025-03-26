namespace NemoUtility
{
    public class ColorUtility
    {
        public static UnityEngine.Color ConvertColor(System.Drawing.Color color)
        {
            return new UnityEngine.Color((float)color.R / 255, (float)color.G / 255, (float)color.B / 255, (float)color.A / 255);
        }
    }
}

