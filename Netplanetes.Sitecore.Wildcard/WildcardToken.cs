using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netplanetes.Sitecore.Wildcard
{
    /// <summary>
    /// Represents item path or url token splitted by /
    /// </summary>
    public class WildcardToken
    {
        #region properties
        public bool IsWildcard { get; private set; }
        public string Token { get; private set; }
        public int Position { get; private set; }
        #endregion

        #region constructor
        public WildcardToken(string token, bool isWildcard, int position)
        {
            this.Token = token;
            this.IsWildcard = isWildcard;
            this.Position = position;
        }
        #endregion

    }
}
