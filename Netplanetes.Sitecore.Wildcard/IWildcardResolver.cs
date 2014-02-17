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
    /// WildcardResolver interface
    /// </summary>
    public interface IWildcardResolver
    {
        /// <summary>
        /// Initialize Wildcard Resolver
        /// </summary>
        /// <param name="wildcardItem"></param>
        /// <param name="site"></param>
        void Initialize(Item wildcardItem, SiteContext site);
        /// <summary>
        /// Resolve wild card item values from request path.
        /// </summary>
        /// <param name="requestPath"></param>
        /// <returns>key is 0 based wild card item position. value is actual token</returns>
        IDictionary<int, string> ResolveWildcardValues(string requestPath);
        /// <summary>
        /// Create url from actual values.
        /// </summary>
        /// <param name="values">key is 0 based  wildcard item position. value is actual text replacing widcard item</param>
        /// <returns>concrete  url.</returns>
        string CreateUrl(IDictionary<int, string> values);

    }
}
