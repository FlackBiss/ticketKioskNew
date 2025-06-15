using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Models
{
    public class EventQueryParameters
    {
        // Параметр page, по умолчанию 1
        public int Page { get; set; } = 1;

        // Дата "до", соответствует dateTimeAt[before]
        [AliasAs("dateTimeAt[before]")]
        public DateTime? DateBefore { get; set; }

        // Дата "после", соответствует dateTimeAt[after]
        [AliasAs("dateTimeAt[after]")]
        public DateTime? DateAfter { get; set; }

        // Текст поиска, соответствует title
        [AliasAs("title")]
        public string? SearchText { get; set; }

        // ID схемы, соответствует scheme.id
        [AliasAs("scheme.id")]
        public int? SchemeId { get; set; }

        // ID схемы, соответствует scheme.id
        [AliasAs("type")]
        public string? ticketType { get; set; }
    }
}
