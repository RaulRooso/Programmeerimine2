using System.Collections.Generic;

namespace KooliProjekt.WindowsForms
{
    public class PagedResult<T> : PagedResultBase
    {
        public IList<T> Results { get; set; } = new List<T>();
    }
}