using System;
using System.Collections.Generic;

namespace BlockchainConsole
{
    public interface IBlock
    {
        int BlockNumber { get; }
        List<Transaction> Data { get; }
        byte[] Hash { get; set; }
        byte[] Difficulty { get; }
        int Nonce { get; set; }
        byte[] PreviousHash { get; set; }
        DateTime TimeStamp { get; }
        byte[] MerkleRoot { get; }
    }

}
