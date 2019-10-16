using System;

namespace Unity3dAzure.WebSockets {
  public class BinaryEventArgs : EventArgs {
    public byte[] Data { get; private set; }

    public BinaryEventArgs(byte[] data) {
      this.Data = data;
    }
  }
}
