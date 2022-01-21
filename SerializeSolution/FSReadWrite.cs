namespace SaberSerialize
{
    public static class FSReadWrite
    {
        public static byte[] IntToBytes(int number)
        {
            var bytes = BitConverter.GetBytes(number);
            return bytes;
        }

        public static void WriteInt(FileStream stream, int num)
        {
            var bytes = IntToBytes(num);
            WriteBytes(stream, bytes);
        }

        public static void WriteBytes(FileStream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public static int ReadInt(FileStream stream)
        {
            var bytes = ReadBytes(stream, 4);
            return BitConverter.ToInt32(bytes);
        }

        public static byte[] ReadBytes(FileStream stream, int count)
        {
            var bytes = new byte[count];
            stream.Read(bytes, 0, count);
            return bytes;
        }
    }
}