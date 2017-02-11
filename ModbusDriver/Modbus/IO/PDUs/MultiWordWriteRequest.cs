using System.Linq;

using Modbus.Extensions;

namespace Modbus.IO
{
    public class MultiWordWriteRequest : MultiWriteRequest
    {
        public ushort[] Values { get; set; }

        public MultiWordWriteRequest(MBAPHeader mbap, byte[] requestBytes)
        {
            this.MBAP = mbap;
            InitFromBytes(requestBytes);
        }

        protected override void InitFromBytes(byte[] bytes)
        {
            base.InitFromBytes(bytes);

            this.Values = bytes.TakeAllStartingAt(6)
                .ToUnsignedShorts()
                .ToArray();    
        }
    }
}
