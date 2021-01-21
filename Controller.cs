using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using RestSharp;
using Label = System.Reflection.Emit.Label;

namespace paradiceinSpamBot
{
    public class Controller
    {
        private double baseBet;
        private int maxRand;
        private int pause;
        private Thread ThreadFoSpam;

        private RestClient client;
        private string token;
        private TextBlock infoBlock;
        private Spam spamBot;

        private List<String> сurrencyList;
        public Controller(TextBlock infoBlock, double baseBet, int maxRand, int pause, List<String> сurrencyList)
        {
            this.infoBlock = infoBlock;
            using (StreamReader sr = new StreamReader(@"token.txt"))
            {
                token = sr.ReadToEnd();
            }

            this.baseBet = baseBet;
            this.maxRand = maxRand;
            this.pause = pause;
            this.сurrencyList = сurrencyList;

            client = new RestClient("https://api.paradice.in/api.php");
            spamBot = new Spam(client,token, сurrencyList, this.maxRand, this.baseBet,this.pause, this.infoBlock);
        }

        public void Start()
        {
            ThreadFoSpam = new Thread(new ThreadStart(spamBot.GenerateInfo));
            ThreadFoSpam.Start();
        }
    }
}