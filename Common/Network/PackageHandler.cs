using System;
using System.Text;
using Newtonsoft.Json;

namespace Common
{
    public static class PackageHandler
    {
        public static int defaultSize = 1024;
        public static byte[] Bytes = new byte[defaultSize];
        
        public static int endIndex = -1;
        private static int capacity = defaultSize;

        public static int Remain { get { return capacity - Length; } }

        public static int Length { get { return endIndex+1; } }
        

        public static string Write(byte[] data ,out NetOpcode opcode)
        {
            int count = data.Length;
            //检测容量
            if (Remain < count)
            {
                int newCap = capacity;
                while (true)
                {
                    newCap += 1024;
                    if (newCap - Length >= count)
                    {
                        break;
                    }
                }

                byte[] newBuffer = new byte[newCap];
                capacity = newCap;
                Array.Copy(Bytes,0,newBuffer,0,Length);
                Bytes = newBuffer;
            }      
            
            //写入
            Array.Copy(data,0,Bytes,endIndex+1,count);
            endIndex += count;
            
            //解包
            //判断长度是否大于8个字节(包头=(包长+Opcode))
            if (Length <= 8)
            {
                opcode = NetOpcode.None;
                return null;
            }
            
            int readIndex = 0;
            int bodyLength = BitConverter.ToInt32(Bytes, readIndex);
            readIndex += 4;
            //判断剩余长度是否大于包体长度
            if (Length-4 < bodyLength)
            {
                opcode = NetOpcode.None;
                return null;
            }
            int intOpcode = BitConverter.ToInt32(Bytes, readIndex);
            opcode = (NetOpcode)intOpcode;
            readIndex += 4;

            string msgJson = Encoding.UTF8.GetString(Bytes, readIndex, bodyLength - 4);
            
            //重置
            Array.Copy(Bytes,0,Bytes,0,bodyLength+4);
            endIndex = -1;
            // object msg = JsonConvert.DeserializeObject<object>(msgJson);
            return msgJson;
        }
        
    }
}