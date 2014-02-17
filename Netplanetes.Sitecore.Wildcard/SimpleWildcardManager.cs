using Sitecore.Data.Items;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netplanetes.Sitecore.Wildcard
{
    /// <summary>
    /// Simple Light weight Wildcard Manager.
    /// It is intended to use easily and work without configuration.
    /// </summary>
    public class SimpleWildcardManager
    {
        public IWildcardResolver CreateResolver(Item wildcardItem)
        {
            return CreateResolver(wildcardItem, global::Sitecore.Context.Site);
        }
        public IWildcardResolver CreateResolver(Item wildcardItem, SiteContext site)
        {
            return BuildResolver(wildcardItem, site);
        }

        protected virtual IWildcardResolver BuildResolver(Item wildcardItem, SiteContext site)
        {
            SimpleWildcardResolver resolver = new SimpleWildcardResolver();
            resolver.Initialize(wildcardItem, site);
            return resolver;
        }
    }
}
