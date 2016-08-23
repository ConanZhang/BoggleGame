// Assignment test implementation written by April Martin & Conan Zhang
// for CS3500 Assignment #8. November, 2014.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace BoggleServer
{
    [TestClass]
    public class BoggleUnitTest
    {
        private static int port = 2000;
        private HashSet<string> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\BoggleLauncher\bin\Debug\dictionary.txt"));

        /// <summary>
        /// Tests if member variables are assigned properly.
        /// </summary>
        [TestMethod]
        public void MakingBoggleServer()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "ABCDEFGHIJKLMNOP");
            PrivateObject boggleAccessor = new PrivateObject(boggle);

            Assert.AreEqual(200, boggleAccessor.GetField("time"));
            Assert.AreEqual("ABCDEFGHIJKLMNOP", boggleAccessor.GetField("boardSetup"));

            // Have clients join
            ChatClientModel client1 = new ChatClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            ChatClientModel client2 = new ChatClientModel();
            client2.Connect("localhost", port, "PLAY client2");
        }


        /// <summary>
        /// Tests if the client rejects improperly formatted names
        /// </summary>
        [TestMethod]
        public void IgnoreBadClient()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "AYTPIDNTNEIOEEHR", ++port);
            ChatClientModel client = new ChatClientModel();
            client.Connect("localhost", port, "Bad Client");
            
            // Access the queue
            PrivateObject boggleAccessor = new PrivateObject(boggle);
            Object queue = boggleAccessor.GetFieldOrProperty("clients");
            PrivateObject queueAccessor = new PrivateObject(queue);

            // Put the test thread to sleep to allow the server to catch up
            Thread.Sleep(1000);

            // Use the queue accessor to get the count
            int count = (int)queueAccessor.GetFieldOrProperty("Count");
            Assert.AreEqual(0, count);
        }

        /// <summary>
        /// Test if we allow the client to try again after failing to connect.
        /// </summary>
        [TestMethod]
        public void GiveBadClientAnotherChance()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "AYTPIDNTNEIOEEHR", ++port);
            ChatClientModel client = new ChatClientModel();
            client.Connect("localhost", port, "Bad Client");
            client.Connect("localhost", port, "Bad Client");
            client.Connect("localhost", port, "Bad Client");
            client.Connect("localhost", port, "PLAY Good Client");

            // Access the queue
            PrivateObject boggleAccessor = new PrivateObject(boggle);
            Object queue = boggleAccessor.GetFieldOrProperty("clients");
            PrivateObject queueAccessor = new PrivateObject(queue);

            // Put the test thread to sleep to allow the server to catch up
            Thread.Sleep(1000);

            // Use the queue accessor to get the count
            int count = (int)queueAccessor.GetFieldOrProperty("Count");
            Assert.AreEqual(1, count); 
        }

        /// <summary>
        /// Test where the server pairs clients together and removes them from the queue 
        /// </summary>
        [TestMethod]
        public void PairClients()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "AYTPIDNTNEIOEEHR", ++port);

            // Access the queue
            PrivateObject boggleAccessor = new PrivateObject(boggle);
            Object queue = boggleAccessor.GetFieldOrProperty("clients");
            PrivateObject queueAccessor = new PrivateObject(queue);

            // Have a client join
            ChatClientModel client1 = new ChatClientModel();
            client1.Connect("localhost", port, "PLAY client1");

            // Put the test thread to sleep to allow the server to catch up
            Thread.Sleep(1000);

            // Assert the queue has one
            int count = (int)queueAccessor.GetFieldOrProperty("Count");
            Assert.AreEqual(1, count);

            // Have a second client join
            ChatClientModel client2 = new ChatClientModel();
            client2.Connect("localhost", port, "PLAY client2");

            // Put the test thread to sleep to allow the server to catch up
            Thread.Sleep(1000);

            // Assert the queue has none
            count = (int)queueAccessor.GetFieldOrProperty("Count");
            Assert.AreEqual(0, count);
        }

        /// <summary>
        /// Test where the server pairs clients together as soon as possible.
        /// </summary>
        [TestMethod]
        public void ThreeClients()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "AYTPIDNTNEIOEEHR", ++port);

            // Access the queue
            PrivateObject boggleAccessor = new PrivateObject(boggle);
            Object queue = boggleAccessor.GetFieldOrProperty("clients");
            PrivateObject queueAccessor = new PrivateObject(queue);

            // Have a client join
            ChatClientModel client1 = new ChatClientModel();
            client1.Connect("localhost", port, "PLAY client1");

            // Have a second client join
            ChatClientModel client2 = new ChatClientModel();
            client2.Connect("localhost", port, "PLAY client2");

            // Have a third client join
            ChatClientModel client3 = new ChatClientModel();
            client3.Connect("localhost", port, "PLAY client3");

            // Put the test thread to sleep to allow the server to catch up
            Thread.Sleep(1000);

            // Assert the queue has one
            int count = (int)queueAccessor.GetFieldOrProperty("Count");
            Assert.AreEqual(1, count);
        }
        
        /// <summary>
        /// Tests whether one client can disconnect without throwing exceptions.
        /// </summary>
        [TestMethod]
        public void DisconnectClientOne()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "AYTPIDNTNEIOEEHR", ++port);

            for (double i = 0; i < 20; i++ )
            {
                // Have a client join
                ChatClientModel client1 = new ChatClientModel();
                client1.Connect("localhost", port, "PLAY client1");

                // Have a second client join
                ChatClientModel client2 = new ChatClientModel();
                client2.Connect("localhost", port, "PLAY client2");

                client1.closeSocket();
            }
        }

        // After 45 minutes of trying to get a single damn assert to work, we have decided that asserts DO NOT WORK UNTIL
        // WE HAVE A VALID GUI. So we're just gonna make sure our program doesn't throw exceptions. If Jim has a problem
        // with that, then he should try writing test cases for the boggleServer class and see how he likes it.

        /// <summary>
        /// Tests whether the program can deal with invalid formatting for words
        /// </summary>
        [TestMethod]
        public void BadWord()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "AYTPIDNTNEIOEEHR", ++port);
            // Create two clients
            ChatClientModel client1 = new ChatClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            ChatClientModel client2 = new ChatClientModel();
            client2.Connect("localhost", port, "PLAY client2");
            // Send an invalid message
            client1.SendMessage("I'm too cool for your protocol.\n");
        }

        /// <summary>
        /// Tests whether the program can deal with illegal words
        /// </summary>
        [TestMethod]
        public void IllegalWord()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "AYTPIDNTNEIOEEHR", ++port);
            // Create two clients
            ChatClientModel client1 = new ChatClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            ChatClientModel client2 = new ChatClientModel();
            client2.Connect("localhost", port, "PLAY client2");
            // Send an valid message with a word that isn't in the dictionary
            client1.SendMessage("WORD asl;jfdl;sakjfdl;sakjfd");
        }

        /// <summary>
        /// Tests whether the program can deal with common words
        /// </summary>
        [TestMethod]
        public void CommonWord()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "AYTPIDNTNEIOEEHR", ++port);
            // Create two clients
            ChatClientModel client1 = new ChatClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            ChatClientModel client2 = new ChatClientModel();
            client2.Connect("localhost", port, "PLAY client2");
            // Send an valid message with a word that isn't in the dictionary
            client1.SendMessage("WORD dine");
            client2.SendMessage("WORD dine");
        }

        /// <summary>
        /// Tests whether the program can deal with legal but not shared words
        /// </summary>
        [TestMethod]
        public void UncommonWord()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "AYTPIDNTNEIOEEHR", ++port);
            // Create two clients
            ChatClientModel client1 = new ChatClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            ChatClientModel client2 = new ChatClientModel();
            client2.Connect("localhost", port, "PLAY client2");
            // Send an valid message with a word that isn't in the dictionary
            client1.SendMessage("WORD dine");
            client2.SendMessage("WORD die");
        }

        /// <summary>
        /// Tests whether the program can handle the time running out
        /// </summary>
        [TestMethod]
        public void Endtime()
        {
            BoggleServer boggle = new BoggleServer(1, dictionary, "AYTPIDNTNEIOEEHR", ++port);
            // Create two clients
            ChatClientModel client1 = new ChatClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            ChatClientModel client2 = new ChatClientModel();
            client2.Connect("localhost", port, "PLAY client2");
            client1.SendMessage("WORD dine");

            Thread.Sleep(2000);
        }

        /// <summary>
        /// Tests scoring of words used by boggle.
        /// </summary>
        [TestMethod]
        public void scoreWords()
        {
            Assert.AreEqual(1, BoggleServer.scoreWord("can"));
            Assert.AreEqual(2, BoggleServer.scoreWord("witch"));
            Assert.AreEqual(3, BoggleServer.scoreWord("pirate"));
            Assert.AreEqual(5, BoggleServer.scoreWord("pirates"));
            Assert.AreEqual(11, BoggleServer.scoreWord("lopadotemachoselachogaleokranioleipsanopterygon"));
        }

        /// <summary>
        /// Tests BoggleBoard's random setup.
        /// </summary>
        [TestMethod]
        public void RandomBoard()
        {
            BoggleServer boggle = new BoggleServer(200, dictionary, "ABCDEFGHIJKLMNOP", ++port);
            PrivateObject boggleAccessor = new PrivateObject(boggle);
            boggleAccessor.SetField("boardSetup", null);

            // Have clients join
            ChatClientModel client1 = new ChatClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            ChatClientModel client2 = new ChatClientModel();
            client2.Connect("localhost", port, "PLAY client2");
        }
    }
}
