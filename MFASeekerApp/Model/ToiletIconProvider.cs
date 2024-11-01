using MFASeekerApp.Model;

namespace MFASeekerApp.Model
{
    public static class ToiletIconProvider
    {
        public static string GetIconPath(Toilet toilet) => toilet.Rating switch
        {
            5 => @"Resources.Icons.rank5_toilet.svg",
            >= 4 => @"Resources.Icons.rank4_toilet.svg",
            >= 3 => @"Resources.Icons.rank3_toilet.svg",
            >= 2 => @"Resources.Icons.rank2_toilet.svg",
            >= 1 => @"Resources.Icons.rank1_toilet.svg",
            _ => @"Resources.Icons.defaultrank_toilet.svg"
        };
        public static string GetIconName(double rating) => rating switch
        {
            5 => "toilet_icon_simillar.png",
            >= 4 => @"toilet_icon_simillar.png",
            >= 3 => @"white_starttoilet2.png.",
            >= 2 => @"white_starttoilet2.png",
            >= 1 => @"white_starttoilet2.png",
            _ => "Resources.Icons.defaultrank_toilet.svg"
        };
    }
}
