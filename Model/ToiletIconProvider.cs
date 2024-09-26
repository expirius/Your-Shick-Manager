﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Model
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
    }
}