using System.Collections.Generic;
using System.Linq;

namespace Search.API.Models
{
    public class FilterOption
    {
        private const string ProductNameKey = "product.name";
        private const string ProductPriceKey = "product.price";
        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, SortOrder> Sorts { get; set; } = new Dictionary<string, SortOrder>();
        
        public int From { get; set; } = 0;
        public int Size { get; set; } = 10;

        #region Filters

        public string Name
        {
            get
            {
                if (Filters.TryGetValue(ProductNameKey, out List<string> names))
                {
                    return names.FirstOrDefault();
                }

                return string.Empty;
            }
        }

        public (double Min, double Max) Prices
        {
            get
            {
                if (Filters.TryGetValue(ProductPriceKey, out List<string> prices))
                {
                    if (!double.TryParse(prices.FirstOrDefault(), out double minPrice))
                    {
                        minPrice = default(double);
                    }

                    if (!double.TryParse(prices.LastOrDefault(), out double maxPrice))
                    {
                        maxPrice = double.MaxValue;
                    }

                    if (maxPrice == default(double))
                    {
                        maxPrice = double.MaxValue;
                    }

                    return (Min: minPrice, Max: maxPrice);
                }

                return (Min: 0, Max: double.MaxValue);
            }
        }

        #endregion

        #region Sorts

        public int? SortByName=> Sorts.TryGetValue(ProductNameKey, out SortOrder sort) ? (int)sort : null; 
        public int? SortByPrice => Sorts.TryGetValue(ProductPriceKey, out SortOrder sort) ? (int)sort : null;

        #endregion
    }

    public enum SortOrder
    {
        Ascending = 0,
        Descending = 1
    }
}
