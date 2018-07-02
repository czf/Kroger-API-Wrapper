using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger.Requests
{
    public class SearchAllRequest
    {
        public const int DEFAULT_ITEM_COUNT = 24;

        public int Start { get; set; }
        public int Count { get; set; }
        public int Tab { get; set; }
        public string Query { get; set; }

        public SearchAllRequest()
        {
            Count = DEFAULT_ITEM_COUNT;
        }

        public void SetStart(int pageNumber)
        {
            Start = (Count * pageNumber) - Count;
        }
    }
}
