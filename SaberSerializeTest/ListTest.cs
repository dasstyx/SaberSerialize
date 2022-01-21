using System.IO;
using NUnit.Framework;
using SaberSerialize;

namespace SaberSerializeTest
{
    [TestFixture]
    public class ListTest
    {
        [SetUp]
        public void Setup()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            streamPath = Path.Join(currentDirectory, "list.bin");
        }

        private string streamPath;

        [Test]
        public void TestList()
        {
            var list = PrepareNewListRand();


            using (var fstream = new FileStream(streamPath, FileMode.OpenOrCreate))
            {
                list.Serialize(fstream);
            }

            ListRand outList;
            using (var fstream = new FileStream(streamPath, FileMode.Open))
            {
                outList = new ListRand();
                outList.Deserialize(fstream);
            }

            var tailExpect = list.Tail;
            var tailActual = outList.Tail;
            var headExpect = list.Head;
            var headActual = outList.Head;

            AssertNode(tailExpect, tailActual);
            AssertNode(headExpect, headActual);

            AssertNode(tailExpect.Next, tailActual.Next);
            AssertNode(tailExpect.Next.Next, tailActual.Next.Next);
            AssertNode(headActual.Prev, headActual.Prev);
            AssertNode(headActual.Prev.Prev, headActual.Prev.Prev);

            AssertNode(tailActual.Rand, tailActual.Rand);
            AssertNode(tailActual.Next.Rand, tailActual.Next.Rand);
        }

        private void AssertNode(ListNode expected, ListNode actual)
        {
            Assert.AreEqual(expected.Data, actual.Data);
        }

        private ListRand PrepareNewListRand()
        {
            var node1 = new ListNode();
            var node2 = new ListNode();
            var node3 = new ListNode();

            node1.Next = node2;
            node2.Next = node3;
            node2.Prev = node1;
            node3.Prev = node2;

            node1.Data = "abc";
            node2.Data = "cba";
            node3.Data = "";

            node1.Rand = node3;
            node3.Rand = node1;
            node2.Rand = node2;

            var list = new ListRand();
            list.Tail = node1;
            list.Head = node3;
            list.Count = 3;

            return list;
        }
    }
}