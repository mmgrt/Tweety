using System;
using System.Collections.Generic;
using System.Text;

namespace Tweety
{

    /// <summary>
    /// Exception thrown by Tweety.
    /// </summary>
    public class TweetyException : Exception
    {
        internal TweetyException(string message) : base(message)
        {

        }


    }
}
