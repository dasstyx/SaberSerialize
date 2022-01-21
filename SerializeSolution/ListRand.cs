using System.Text;

namespace SaberSerialize
{
    public class ListNode
    {
        public string Data;
        public ListNode Next;
        public ListNode Prev;
        public ListNode Rand; // произвольный элемент внутри списка
    }

    public class ListRand
    {
        public int Count;
        public ListNode Head;
        public ListNode Tail;

        

        /// <summary>
        /// - 4байт количество нод
        // каждая нода:
        // - 4байт длина строки
        // - закодированная строка
        // - 4байт индекс rand
        /// </summary>
        /// <param name="s"></param>
        public void Serialize(FileStream s)
        {
            var tailNode = Tail;
            // O(1) добавления и доступа, если не придется расширять
            Dictionary<ListNode, int> allNodesDict = Helpers.MakeIndexesDict(tailNode, Count);

            byte[] countBytes = FSReadWrite.IntToBytes(Count);
            FSReadWrite.WriteBytes(s, countBytes);

            for (var i = 0; i < Count; i++)
            {
                byte[] strData = Encoding.Default.GetBytes(tailNode.Data);
                byte[] strLen = FSReadWrite.IntToBytes(strData.Length);
                FSReadWrite.WriteBytes(s, strLen);
                FSReadWrite.WriteBytes(s, strData);

                Helpers.WriteRandIndex(tailNode, allNodesDict, s);

                tailNode = tailNode.Next;
            }
        }

        public void Deserialize(FileStream s)
        {
            var count = FSReadWrite.ReadInt(s);

            var nodes = new ListNode[count];
            var futureRands = new int[count];


            for (var i = 0; i < count; i++)
            {
                var node = new ListNode();
                nodes[i] = node;

                int strLen = FSReadWrite.ReadInt(s);

                byte[] strBytes = FSReadWrite.ReadBytes(s, strLen);
                string strData = Encoding.Default.GetString(strBytes);
                int randIndex = FSReadWrite.ReadInt(s);

                futureRands[i] = randIndex;

                node.Data = strData;
                if (i != 0)
                {
                    nodes[i - 1].Next = node;
                    node.Prev = nodes[i - 1];
                }
            }

            for (var i = 0; i < nodes.Length; i++)
            {
                ListNode node = nodes[i];
                int randIndex = futureRands[i];
                if (randIndex != -1)
                {
                    node.Rand = nodes[randIndex];
                }
            }

            Tail = nodes[0];
            Head = nodes[count - 1];
            Count = count;
        }
    }
}