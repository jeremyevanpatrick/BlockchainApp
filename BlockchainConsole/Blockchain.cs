using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BlockchainConsole
{
    public class BlockChain : IEnumerable<IBlock>
    {
        private List<IBlock> _items = new List<IBlock>();

        public BlockChain(IBlock genesis)
        {
            genesis.Hash = genesis.MineHash();
            Items.Add(genesis);
        }

        public BlockChain(List<Block> existingBlocks)
        {
            foreach (Block b in existingBlocks)
            {
                Items.Add(b);
            }
        }

        public void Add(IBlock item)
        {
            if (Items.LastOrDefault() != null)
            {
                item.PreviousHash = Items.LastOrDefault()?.Hash;
            }

            item.Hash = item.MineHash();
            Items.Add(item);
        }

        public int Count => Items.Count;
        public IBlock this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }
        public List<IBlock> Items
        {
            get => _items;
            set => _items = value;
        }

        public IEnumerator<IBlock> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }

    public static class BlockChainExtension
    {
        public static byte[] GenerateHash(this IBlock block)
        {
            byte[] hash = null;

            using (SHA512 sha = new SHA512Managed())
            using (MemoryStream st = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(st))
            {
                bw.Write(block.Nonce);
                bw.Write(block.TimeStamp.ToBinary());
                bw.Write(block.PreviousHash);
                bw.Write(block.MerkleRoot);
                var strArray = st.ToArray();
                hash = sha.ComputeHash(strArray);

            }

            return hash;
        }

        public static byte[] MineHash(this IBlock block)
        {
            if (block.Difficulty == null) throw new ArgumentNullException(nameof(block.Difficulty));

            byte[] hash = new byte[0];
            int d = block.Difficulty.Length;
            while (!hash.Take(d).SequenceEqual(block.Difficulty))
            {
                block.Nonce++;
                hash = block.GenerateHash();
            }

            return hash;
        }

        public static bool IsValid(this IBlock block)
        {
            var bk = block.GenerateHash();
            return block.Hash.SequenceEqual(bk);

        }

        public static bool IsValidPrevBlock(this IBlock block, IBlock prevBlock)
        {
            if (prevBlock == null) throw new ArgumentNullException(nameof(prevBlock));

            var prev = prevBlock.GenerateHash();
            return prevBlock.IsValid() && block.PreviousHash.SequenceEqual(prev);
        }

        public static bool IsValid(this IEnumerable<IBlock> items)
        {
            var enumerable = items.ToList();

            //return enumerable.Zip(enumerable.Skip(1), Tuple.Create).All(block => block.Item2.IsValid() && block.Item2.IsValidPrevBlock(block.Item1));

            for (int x = 0; x < enumerable.Count; x++)
            {
                if (x > 0)
                {
                    IBlock block1 = enumerable[x - 1];
                    IBlock block2 = enumerable[x];

                    if (!block2.IsValid())
                    {
                        return false;
                    }

                    if (!block2.IsValidPrevBlock(block1))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }

}
