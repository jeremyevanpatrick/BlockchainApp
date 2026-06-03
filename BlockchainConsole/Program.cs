using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;

namespace BlockchainConsole
{
    class Program
    {
        static string transactionsPath;
        static string blockchainStoragePath;
        
        static void LogBlockchain(BlockChain chain)
        {
            string json = JsonConvert.SerializeObject(chain, Formatting.Indented);
            File.WriteAllText(blockchainStoragePath, json);

            Console.WriteLine(chain.LastOrDefault()?.ToString());
            Console.WriteLine($"Chain is Valid: {chain.IsValid()}");
            Console.WriteLine($"================================================================");
        }

        static void GenerateBlock(BlockChain chain, List<Transaction> data)
        {
            IBlock prevBlock = chain.Items.Last<IBlock>();
            int nextBlockNumber = prevBlock.BlockNumber + 1;
            byte[] diff = prevBlock.Difficulty;
            chain.Add(new Block(data, nextBlockNumber, diff));
            LogBlockchain(chain);
        }

        static void Main(string[] args)
        {
            bool waitForUserInput = true;
            if (args != null)
            {

                for (int x=0; x<args.Length; x++)
                {
                    if (x == 0)
                    {
                        transactionsPath = args[x];
                    }
                    else if (x == 1)
                    {
                        blockchainStoragePath = args[x];
                    }
                    else if (x == 2)
                    {
                        if (args[x] == "WebApp")
                        {
                            waitForUserInput = false;
                        }
                    }
                }

            }

            if (string.IsNullOrEmpty(transactionsPath))
            {
                transactionsPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    ConfigurationManager.AppSettings["TransactionsPath"]);
            }

            if (string.IsNullOrEmpty(blockchainStoragePath))
            {
                blockchainStoragePath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, 
                    ConfigurationManager.AppSettings["BlockchainStoragePath"]);
            }

            BlockChain chain = null;

            bool initNewBlockChain = false;

            if (File.Exists(blockchainStoragePath))
            {
                //read existing blockchain
                using (StreamReader sr = new StreamReader(blockchainStoragePath))
                {
                    string json = sr.ReadToEnd();
                    List<Block> blocks = JsonConvert.DeserializeObject<List<Block>>(json);

                    //clear the blockchain and start over after every 128 blocks to save space
                    if (blocks != null && blocks.Count <= 128)
                    {
                        chain = new BlockChain(blocks);
                    }
                    else
                    {
                        initNewBlockChain = true;
                    }

                }
            }
            else
            {
                initNewBlockChain = true;
            }

            if (initNewBlockChain)
            {
                //initialize new blockchain
                byte[] initDifficulty = new byte[] { 0x00, 0x00 };
                int initBlockNumber = 0;
                List<Transaction> initTransactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Notes = "Hello world!"
                    }
                };
                Block genesis = new Block(initTransactions, initBlockNumber, initDifficulty);
                chain = new BlockChain(genesis);
                LogBlockchain(chain);
            }

            //Generate blocks containing 2 KB groups of transactions
            List<Transaction> transactions = new List<Transaction>();
            if (File.Exists(transactionsPath))
            {
                //read transactions data
                using (StreamReader sr = new StreamReader(transactionsPath))
                {
                    string json = sr.ReadToEnd();
                    transactions = JsonConvert.DeserializeObject<List<Transaction>>(json);

                    List<Transaction> tenKbListOfTransactions = new List<Transaction>();

                    foreach (Transaction t in transactions)
                    {
                        //if adding the next transaction would exceed 2 KB
                        List<Transaction> tempList = tenKbListOfTransactions.ToList();
                        tempList.Add(t);
                        if (Utilities.ObjectToByteArray(tempList).Length > 2000)
                        {
                            //generate the block with this 2 KB group of transactions
                            GenerateBlock(chain, tenKbListOfTransactions);
                            //reset the transaction list
                            tenKbListOfTransactions = new List<Transaction>();
                        }

                        tenKbListOfTransactions.Add(t);
                    }
                    GenerateBlock(chain, tenKbListOfTransactions);

                }

            }

            if (waitForUserInput)
            {
                Console.ReadLine();
            }

        }

    }
}
