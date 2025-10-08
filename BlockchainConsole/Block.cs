using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace BlockchainConsole
{
    [Serializable]
    public class Block : IBlock
    {
        public Block()
        {

        }

        public Block(List<Transaction> data, int blockNumber, byte[] difficulty)
        {
            BlockNumber = blockNumber;
            Data = data;
            Difficulty = difficulty;
            Nonce = 0;
            PreviousHash = new byte[] { 0x00 };
            TimeStamp = DateTime.Now;
            MerkleRoot = GenerateMerkleRoot();
        }

        [DataMember(Name = "BlockNumber")]
        public int BlockNumber { get; set; }

        [DataMember(Name = "Data")]
        public List<Transaction> Data { get; set; }

        [DataMember(Name = "Hash")]
        public byte[] Hash { get; set; }

        [DataMember(Name = "Difficulty")]
        public byte[] Difficulty { get; set; }

        [DataMember(Name = "Nonce")]
        public int Nonce { get; set; }

        [DataMember(Name = "PreviousHash")]
        public byte[] PreviousHash { get; set; }

        [DataMember(Name = "TimeStamp")]
        public DateTime TimeStamp { get; set; }

        [DataMember(Name = "MerkleRoot")]
        public byte[] MerkleRoot { get; set; }

        public override string ToString()
        {
            return $"Block #: {BlockNumber}\n" +
                $"Hash: {Convert.ToBase64String(Hash)}\n" +
                $"Prev: {Convert.ToBase64String(PreviousHash)}\n" +
                $"Difficulty: {Convert.ToBase64String(Difficulty)}\n" +
                $"Nonce: {Nonce}\n" +
                $"Timestamp: {TimeStamp}\n" +
                $"MerkleRoot: {Convert.ToBase64String(MerkleRoot)}\n" +
                $"Transactions: {Data.Count.ToString()}";
        }

        public static byte[] GenerateHashFrom2(byte[] hash1, byte[] hash2)
        {
            using (SHA512 sha = new SHA512Managed())
            using (MemoryStream st = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(st))
            {
                bw.Write(hash1);
                bw.Write(hash2);
                var strArray = st.ToArray();
                return sha.ComputeHash(strArray);
            }
        }

        public object[] IterateTree(object[] hashableArray)
        {
            List<object> nextLevel = new List<object>();
            for (int x = 0; x < hashableArray.Length; x += 2)
            {
                byte[] hash1 = Utilities.ObjectToByteArray(hashableArray[x]);
                byte[] hash2 = new byte[] { };
                if ((x + 1) < hashableArray.Length)
                {
                    hash2 = Utilities.ObjectToByteArray(hashableArray[x + 1]);
                }
                byte[] resultHash = GenerateHashFrom2(hash1, hash2);
                nextLevel.Add(resultHash);
            }

            object[] nextLevelArray = nextLevel.ToArray();

            //continue iterating up the tree until there is only one resulting hash
            if (nextLevelArray.Length > 1)
            {
                return IterateTree(nextLevelArray);
            }

            return nextLevelArray;
        }

        public byte[] GenerateMerkleRoot()
        {
            byte[] root = new byte[] { };
            object[] transactionArray = Data.ToArray();
            object[] finalArray = IterateTree(transactionArray);
            if (finalArray.Length > 0)
            {
                root = (byte[])finalArray[0];
            }
            return root;
        }
    }

}
