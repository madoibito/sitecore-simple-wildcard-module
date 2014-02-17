using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Netplanetes.Sitecore.Wildcard
{
    /// <summary>
    /// Simple Widlcard Resolver.
    /// </summary>
    public class SimpleWildcardResolver : IWildcardResolver
    {
        #region properties
        public Item WildcardItem { get; set; }
        public SiteContext Site { get; set; }

        protected static string WildcardUrlString
        {
            get
            {
                return Settings.GetSetting("Netplanetes.WildcardUrlString", ",-w-,");
            }
        }

        protected static string WildcardItemName
        {
            get
            {
                return Settings.GetSetting("Netplanetes.WildcardItemName", "*");
            }
        }

        private IList<WildcardToken> _itemPathTokens = null;

        protected IList<WildcardToken> ItemPathTokens
        {
            get
            {
                if (_itemPathTokens == null)
                {
                    int length = Site.StartPath.Length;
                    string relativePath = WildcardItem.Paths.FullPath.Substring(length);
                    _itemPathTokens = ConvertToTokens(relativePath);
                }

                return _itemPathTokens;
            }
        }
        private IList<WildcardToken> _reversedItemPathTokens = null;

        protected IList<WildcardToken> ReversedItemPathTokens
        {
            get
            {
                if (_reversedItemPathTokens == null)
                {
                    _reversedItemPathTokens = this.ItemPathTokens.Reverse().ToList();
                }
                return _reversedItemPathTokens;
            }
        }

        private string _wildcardItemUrl = null;
        protected string WildcardItemUrl
        {
            get
            {
                if (_wildcardItemUrl == null)
                {
                    _wildcardItemUrl = LinkManager.GetItemUrl(this.WildcardItem);
                }
                return _wildcardItemUrl;
            }
        }
        #endregion

        #region methods
        public void Initialize(Item wildcardItem, SiteContext site)
        {
            this.Site = site;
            this.WildcardItem = wildcardItem;
        }

        public IDictionary<int, string> ResolveWildcardValues(string requestPath)
        {
            IList<WildcardToken> requestTokens = ConvertToTokens(requestPath);
            return ResolveMapping(requestTokens);
        }

        private IDictionary<int, string> ResolveMapping(IList<WildcardToken> requestTokens)
        {
            Stack<string> stack = new Stack<string>();
            IList<WildcardToken> reversedRequetTokens = requestTokens.Reverse().ToList();

            for (int i = 0; i < ReversedItemPathTokens.Count; ++i)
            {
                WildcardToken token = ReversedItemPathTokens[i];
                if (token.IsWildcard && i < reversedRequetTokens.Count)
                {
                    stack.Push(reversedRequetTokens[i].Token);
                }
            }

            return ToDictionary(stack.ToArray());
        }

        /// <summary>
        /// convert array to Dictionary.
        /// Key is element position in the array. Value is element.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private IDictionary<int, T> ToDictionary<T>(IEnumerable<T> array)
        {
            Dictionary<int, T> dic = new Dictionary<int, T>();
            int i = 0;
            foreach (var a in array)
            {
                dic.Add(i++, a);
            }
            return dic;
        }

        /// <summary>
        /// convert to tokens from item path or request path string.
        /// </summary>
        /// <param name="itemPathOrRequestPath"></param>
        /// <returns></returns>
        private IList<WildcardToken> ConvertToTokens(string itemPathOrRequestPath)
        {
            // split by /.
            string[] tokens = itemPathOrRequestPath.Split('/');
            if (tokens.Length > 0)
            {
                // remove query parameter
                string last = tokens[tokens.Length - 1];
                int idx = last.IndexOf('?');
                if (idx >= 0)
                {
                    last = last.Substring(0, idx);
                }
                // remove extension
                tokens[tokens.Length - 1] = System.IO.Path.GetFileNameWithoutExtension(last);

            }

            var nonEmptyTokens = tokens.Where(x => x.Length > 0);

            return nonEmptyTokens.Select((x, pos) => new WildcardToken(x, x.Equals(WildcardItemName), pos)).ToList();
        }
        /// <summary>
        /// Create Wildcard Item url. wildcard is replaced with values.
        /// </summary>
        /// <param name="values">key is wildcard position. value is token to be replaced</param>
        /// <returns></returns>
        public string CreateUrl(IDictionary<int, string> values)
        {
            string[] itemTokens = SplitWildcardItemUrlByWildcardUrlString();

            // create url
            StringBuilder builder = new StringBuilder();
            int cnt = 0;
            foreach (var item in itemTokens)
            {
                builder.Append(item);
                if (values.ContainsKey(cnt))
                {
                    builder.Append(values[cnt++]);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Splite wild cared item with wildcard url string.
        /// notice: * is converted to ,-w-, by default setting.
        /// </summary>
        /// <returns></returns>
        private string[] SplitWildcardItemUrlByWildcardUrlString()
        {
            // split url by wildcard item url name.
            return Regex.Split(this.WildcardItemUrl, WildcardUrlString);
        }
        #endregion
    }
}
