namespace SaberSerialize
{
    public static class Helpers
    {
        public static Dictionary<ListNode, int> MakeIndexesDict(ListNode tailNode, int count)
        {
            var dict = new Dictionary<ListNode, int>(count);
            var currentNode = tailNode;
            for (var i = 0; i < count; i++)
            {
                dict.Add(currentNode, i);
                currentNode = currentNode.Next;
            }

            return dict;
        }

        public static void WriteRandIndex(ListNode node, Dictionary<ListNode, int> allNodesDict, FileStream fs)
        {
            var randIndex = -1;
            var rand = node.Rand;
            if (rand != null && allNodesDict.ContainsKey(rand))
            {
                randIndex = allNodesDict[rand];
            }

            FSReadWrite.WriteInt(fs, randIndex);
        }
    }
}