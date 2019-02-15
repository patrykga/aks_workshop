using Kuber.Models;

namespace Kuber.Mvc.Models
{
    public class AppEnvironmentModel
    {
        public EnvironmentModel ApiEnvironement { get; set; }
        public EnvironmentModel MvcEnvironement { get; set; }
    }
}
