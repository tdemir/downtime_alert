using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Helpers.Datatable
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DataRequest
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public List<Order> order { get; set; }
        public Search search { get; set; }
        public List<Filter> filters { get; set; }

        public DataRequest()
        {
            if (filters == null)
                filters = new List<Filter>();
            if (order == null)
                order = new List<Order>();
            if (columns == null)
                columns = new List<Column>();
        }
    }

    [Serializable]
    public class Filter
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public int Operand { get; set; }
    }

    [Serializable]
    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }

        public Search search { get; set; }
    }

    [Serializable]
    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    [Serializable]
    public class Search
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
}
