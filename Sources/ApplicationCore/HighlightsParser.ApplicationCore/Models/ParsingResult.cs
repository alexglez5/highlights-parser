using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighlightsParser.ApplicationCore.Models
{
    public class ParsingResult
    {
        public string Title { get; set; }

        public List<string> Highlights { get; set; } = new List<string>();
    }
}
