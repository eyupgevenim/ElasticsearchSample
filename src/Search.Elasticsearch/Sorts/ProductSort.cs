namespace Search.Elastic.Sorts
{
    using Nest;
    using System.Collections.Generic;

    public class ProductSort: Abstraction.ISort
    {
        private const string FieldQuantity = "quantity";
        private const string FieldPrice = "price";
        private const string FieldName = "name";
        private readonly IDictionary<string, SortOrder> sortFields = new Dictionary<string, SortOrder>();

        public void OrderByQuantity(int sortOrder)
        {
            if (!sortFields.ContainsKey(FieldQuantity))
                sortFields.Add(FieldQuantity, (SortOrder)sortOrder);
        }
        public void OrderByPice(int sortOrder)
        {
            if (!sortFields.ContainsKey(FieldPrice))
                sortFields.Add(FieldPrice, (SortOrder)sortOrder);
        }

        public void OrderByName(int sortOrder)
        {
            if (!sortFields.ContainsKey(FieldName))
                sortFields.Add(FieldName, (SortOrder)sortOrder);
        }

        public SortDescriptor<T> Get<T>(SortDescriptor<T> sortDescriptor) where T: class
        {
            foreach (var sortField in sortFields)
                sortDescriptor.Field(f => f.Order(sortField.Value).Field(sortField.Key));

            return sortDescriptor;
        }
    }
}
