using Microsoft.Extensions.Options;

namespace SQLFormatter.Models
{
    public class RequestWithOptionsAndSQL
    {
        public string Sql { get; set; }
        public FormattingOptions Options { get; set; }
    }
}
