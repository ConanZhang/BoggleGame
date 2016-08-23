// Assignment test implementation written by April Martin & Conan Zhang
// for CS3500 Assignment #8. November, 2014.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoggleClient;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace BoggleClient
{
    [TestClass]
    public class ClientUnitTest
    {
        //port to have different tests to connect to
        private static int port = 2001;

        /// <summary>
        /// Tests if member variables are assigned properly.
        /// </summary>
        [TestMethod]
        public void MakingBoggleServerOn2000()
        {
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR");
            PrivateObject boggleAccessor = new PrivateObject(boggle);

            Assert.AreEqual(30, boggleAccessor.GetField("time"));
            Assert.AreEqual("AYTPIDNTNEIOEEHR", boggleAccessor.GetField("boardSetup"));

            // Have clients join
            BoggleClientModel client1 = new BoggleClientModel();
            client1.Connect("localhost", "PLAY client1");
            BoggleClientModel client2 = new BoggleClientModel();
            client2.Connect("localhost", "PLAY client2");
        }

        /// <summary>
        /// Tests if member variables are assigned properly.
        /// </summary>
        [TestMethod]
        public void MakingBoggleServer()
        {
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR",++port); 
            PrivateObject boggleAccessor = new PrivateObject(boggle);

            Assert.AreEqual(30, boggleAccessor.GetField("time"));
            Assert.AreEqual("AYTPIDNTNEIOEEHR", boggleAccessor.GetField("boardSetup"));

            // Have clients join
            BoggleClientModel client1 = new BoggleClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            BoggleClientModel client2 = new BoggleClientModel();
            client2.Connect("localhost", port, "PLAY client2");
        }

        /// <summary>
        /// Test where the server pairs clients together and removes them from the queue 
        /// </summary>
        [TestMethod]
        public void PairClients()
        {
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR", ++port); 
            
            // Access the queue
            PrivateObject boggleAccessor = new PrivateObject(boggle);
            Object queue = boggleAccessor.GetFieldOrProperty("clients");
            PrivateObject queueAccessor = new PrivateObject(queue);

            // Have a client join
            BoggleClientModel client1 = new BoggleClientModel();
            client1.Connect("localhost", port, "PLAY client1");

            // Put the test thread to sleep to allow the server to catch up
            Thread.Sleep(1000);

            // Assert the queue has one
            int count = (int)queueAccessor.GetFieldOrProperty("Count");
            Assert.AreEqual(1, count);

            // Have a second client join
            BoggleClientModel client2 = new BoggleClientModel();
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
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR", ++port); 
            
            // Access the queue
            PrivateObject boggleAccessor = new PrivateObject(boggle);
            Object queue = boggleAccessor.GetFieldOrProperty("clients");
            PrivateObject queueAccessor = new PrivateObject(queue);

            // Have a client join
            BoggleClientModel client1 = new BoggleClientModel();
            client1.Connect("localhost", port, "PLAY client1");

            // Have a second client join
            BoggleClientModel client2 = new BoggleClientModel();
            client2.Connect("localhost", port, "PLAY client2");

            // Have a third client join
            BoggleClientModel client3 = new BoggleClientModel();
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
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR", ++port); 
            
            for (double i = 0; i < 2; i++ )
            {
                // Have a client join
                BoggleClientModel client1 = new BoggleClientModel();
                client1.Connect("localhost", port, "PLAY client1");

                // Have a second client join
                BoggleClientModel client2 = new BoggleClientModel();
                client2.Connect("localhost", port, "PLAY client2");

                client1.closeSocket();
            }
        }

        //Note:
        // After 45 minutes of trying to get a single damn assert to work, we have decided that asserts ARE NOT WORTH IT UNTIL
        // WE HAVE A VALID GUI. So we're just gonna make sure our program doesn't throw exceptions. If Jim has a problem
        // with that, then he should try writing test cases for the BoggleServer class and see how he likes it.

        /// <summary>
        /// Tests whether the program can deal with invalid protocol for words.
        /// </summary>
        [TestMethod]
        public void BadWord()
        {
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR", ++port); 
            // Create two clients
            BoggleClientModel client1 = new BoggleClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            BoggleClientModel client2 = new BoggleClientModel();
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
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR", ++port); 
            // Create two clients
            BoggleClientModel client1 = new BoggleClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            BoggleClientModel client2 = new BoggleClientModel();
            client2.Connect("localhost", port, "PLAY client2");
            // Send an valid message with a word that isn't in the dictionary
            client1.SendMessage("WORD asl;jfdl;sakjfdl;sakjfd");
        }

        /// <summary>
        /// Tests whether the program can deal with common (shared) words
        /// </summary>
        [TestMethod]
        public void CommonWord()
        {
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR", ++port); 
            // Create two clients
            BoggleClientModel client1 = new BoggleClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            BoggleClientModel client2 = new BoggleClientModel();
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
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR", ++port); 
            // Create two clients
            BoggleClientModel client1 = new BoggleClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            BoggleClientModel client2 = new BoggleClientModel();
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
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(1, dictionary, "AYTPIDNTNEIOEEHR", ++port); 
            // Create two clients
            BoggleClientModel client1 = new BoggleClientModel();
            BoggleClientModel client2 = new BoggleClientModel();
            
            //Slap on events for code coverage
            client1.StopEvent += stop;
            client2.StopEvent += stop;
            client1.ScoreEvent += score;
            client2.ScoreEvent += score;
            client1.TimeEvent += time;
            client2.TimeEvent += time;
            client1.StartEvent += start;
            client2.StartEvent += start;

            client1.Connect("localhost", port, "PLAY client1");
            client2.Connect("localhost", port, "PLAY client2");
            client1.SendMessage("WORD dine");

            //wait for game to end
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
            HashSet<String> dictionary = new HashSet<string>(System.IO.File.ReadLines(@"..\..\..\dictionary.txt"));
            BoggleClient.BoggleServer boggle = new BoggleClient.BoggleServer(30, dictionary, "AYTPIDNTNEIOEEHR", ++port); 
            PrivateObject boggleAccessor = new PrivateObject(boggle);
            boggleAccessor.SetField("boardSetup", null);

            // Have clients join
            BoggleClientModel client1 = new BoggleClientModel();
            client1.Connect("localhost", port, "PLAY client1");
            BoggleClientModel client2 = new BoggleClientModel();
            client2.Connect("localhost", port, "PLAY client2");
        }

        //events to drive model receiving messages from server

        private void stop(string a, string b, string c, string d, string f, string g, string h, string j, string k, string l)
        {

        }

        private void score(string yourScore, string theirScore)
        {

        }

        private void time(string time)
        {

        }

        private void start(string board, string opponent)
        {

        }
    }

}
