using System;

namespace SNet
{
    public class DataBuffer
    {
        public int EndIndex;
        public byte[] Buffer;
        private readonly int defaultSize = 1024*4;
        public int Capacity;
        private readonly object lockObj = new object();
        
        public int Length
        {
            get
            {
                return EndIndex+1;
            }
        }

        public DataBuffer()
        {
            Buffer = new byte[defaultSize];
            Capacity = defaultSize;
        }

        public void CheckBuffer(int len)
        {
            lock (lockObj)
            {
                if (len >= Capacity)
                {
                    //需要扩容
                    Capacity *= 2;
                    byte[] newBuffer = new byte[Capacity];
                    Buffer = newBuffer;
                }
            }
        }
    }
}