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
        //data for Spam
        private double baseBet;
        private int maxRand;
        private int pause;
        private RestClient client;
        private string token;
        private TextBlock infoBlock;
        private List<String> сurrencyList;

        private Thread ThreadFoSpam;

        private Spam spamBot;

        private CheckAuthorization authorization;

        public Controller(TextBlock infoBlock, double baseBet, int maxRand, int pause, List<String> сurrencyList)
        {
            //data from UI to SpamBot
            this.infoBlock = infoBlock;
            this.baseBet = baseBet;
            this.maxRand = maxRand;
            this.pause = pause;
            this.сurrencyList = сurrencyList;
        }
        
        public bool Start()
        {
            authorization = new CheckAuthorization();

            int res = authorization.Autorization();
            if (res == 1)
            {
                using (StreamReader sr = new StreamReader(@"token.txt"))
                {
                    token = sr.ReadToEnd();
                }
                client = new RestClient("https://api.paradice.in/api.php");
                spamBot = new Spam(client, token, сurrencyList, maxRand, baseBet, pause, infoBlock);

                ThreadFoSpam = new Thread(new ThreadStart(spamBot.GenerateInfo));
                ThreadFoSpam.IsBackground = true;
                ThreadFoSpam.Start();
                return true;
            }
            else if (res == 0)
            {
                infoBlock.Text = "Non-working token"; 
                return false;
            }
            else
            {
                infoBlock.Text = "Some error. Write to developer";
                return false;
            }
            
        }

        private void Stop()
        {
            ThreadFoSpam.Abort();
        }
    }
}