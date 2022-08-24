using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class Counter<T>
    {
        public T Value { get; set; }
        public int Count { get; set; }
    }
}