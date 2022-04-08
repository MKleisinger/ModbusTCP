using System.Linq;

using Modbus.Extensions;

namespace Modbus.IO {
    public class MultiBitWriteRequest : MultiWriteRequest {
        public byte[] Outputs { get; set; }

        public MultiBitWriteRequest(MBAPHeader mbap, byte[] requestBytes) {
            this.MBAP = mbap;
            InitFromBytes(requestBytes);
        }

        protected override void InitFromBytes(byte[] bytes) {
            base.InitFromBytes(bytes);

            this.Outputs = bytes.TakeAllStartingAt(6).ToArray();
        }
    }
}
