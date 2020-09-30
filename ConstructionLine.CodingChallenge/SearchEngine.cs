using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly List<SizeCount> _sizeCounts;
        private readonly List<ColorCount> _colorCounts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            _sizeCounts = new List<SizeCount>();
            _colorCounts = new List<ColorCount>();

            shirts.GroupBy(s => s.Size).ToList().ForEach(size => _sizeCounts.Add(new SizeCount() { Size = size.Key, Count = size.Count() }));
            shirts.GroupBy(s => s.Color).ToList().ForEach(color => _colorCounts.Add(new ColorCount() { Color = color.Key, Count = color.Count() }));
        }


        public SearchResults Search(SearchOptions options)
        {
            var shirts = from shirt in _shirts
                         join size in options.Sizes on shirt.Size.Id equals size.Id into sizegrp
                         from sizeitem in sizegrp.DefaultIfEmpty(shirt.Size)
                         join color in options.Colors on shirt.Color.Id equals color.Id into colorgrp
                         from coloritem in colorgrp.DefaultIfEmpty(shirt.Color)
                         select shirt;

            var sizeCounts = from sizecount in _sizeCounts
                             join size in options.Sizes on sizecount.Size.Id equals size.Id
                             select sizecount;

            var colorCounts = from colorcount in _colorCounts
                             join color in options.Colors on colorcount.Color.Id equals color.Id into sizegrp
                             select colorcount;

            return new SearchResults
            {
                Shirts = shirts.ToList(),
                SizeCounts = sizeCounts.ToList(),
                ColorCounts = colorCounts.ToList()
            };
        }
    }
}