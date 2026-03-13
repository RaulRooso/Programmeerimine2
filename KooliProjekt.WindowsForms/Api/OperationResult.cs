using System.Collections.Generic;

namespace KooliProjekt.WindowsForms
{
    public class OperationResult
    {
        public IDictionary<string, string> PropertyErrors { get; set; } = new Dictionary<string, string>();
        public IList<string> Errors { get; set; } = new List<string>();
        public bool HasErrors => PropertyErrors?.Count > 0 || Errors?.Count > 0;
    }
}