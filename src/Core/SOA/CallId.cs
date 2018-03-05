using System;

namespace Nofdev.Core.SOA
{
    /// <summary>
    /// Call ID
    /// </summary>
    public class CallId
    {
        /// <summary>
        /// Current ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Parent ID
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// Root ID
        /// </summary>
        public string Root { get; set; }

        /// <summary>
        /// Get a GUID without connective mark '-'
        /// </summary>
        /// <returns></returns>
        public static string New()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// constructor
        /// </summary>
        public CallId()
        {
            var guid = New();
            Id = Parent = Root = guid;
        }

        /// <summary>
        /// override ToString() method,layout as JSON
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{\"root\":\"{Root}\",\"parent\":\"{Parent}\",\"id\":\"{Id}\"}}";
        }


        /// <summary>
        /// create sub id of current instance
        /// </summary>
        /// <returns></returns>
        public CallId NewSub()
        {
            return new CallId
            {
                Id = New(),
                Parent = Id,
                Root = Root
            };
        }
    }
}