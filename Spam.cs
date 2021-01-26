using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using RestSharp;
using Label = System.Reflection.Emit.Label;

namespace paradiceinSpamBot
{
    public class Spam
    {
        private RestClient client;
        private RestRequest request;

        private string statistic;

        private int pause;

        private List<String> сurrencyList;

        private string token;

        private string сurrency;

        private int maxRand;

        private double bet;

        private Random rand;

        private TextBlock infoBlock;

        private int wins, losses;

        private string jsonString = "";

        private double profit;
        private string profitS;
        private double wagered;
        private string wageredS;

        private int spamCount = 0;

        public Spam(RestClient client, string token, List<String> сurrencyList, int maxRand, double bet, int pause, TextBlock infoBlock)
        {
            this.infoBlock = infoBlock;

            this.client = client;
            request = new RestRequest(Method.POST);

            this.token = token;

            this.pause = pause;

            this.сurrencyList = сurrencyList;

            сurrencyList = new List<string>();

            rand = new Random();

            this.maxRand = maxRand;

            this.bet = bet;

            AddParametersToRequest();

            statistic = "";
        }

        private void AddParametersToRequest()
        {
            client.Timeout = -1;
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\"");
            request.AddHeader("DNT", "1");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("Authorization", token);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "*/*");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Sec-Fetch-Site", "same-site");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Cookie", "__cfduid=d05ec5fcc4e574b8c77a5e81dc51caa411607638398; _ga=GA1.2.1474332697.1608739211");

            string s = "";
            request.AddParameter("application/json", s, ParameterType.RequestBody);
        }

        private void GenerateCurrency()
        {
            if (сurrencyList.Count == 1)
            {
                сurrency = сurrencyList[0];
            }
            else
            {
                сurrency = сurrencyList[rand.Next(0, сurrencyList.Count)];
            }
        }

        private void GenerateStatistic()
        {
            bool flag = true;
             profit = 0;
             wagered = 0;

            wins = 0;
            losses = 0;

            string tmp;

            int count = rand.Next(50, maxRand);
            double curentBet = bet;

            for (int j = 0; j < count; j++)
            {
                int countPart = rand.Next(0,100);

                if (rand.Next(0, 100) > 50)
                {
                    profit += curentBet;
                    wagered += curentBet;

                    tmp = $"{profit:f8}";
                    statistic += tmp.Replace(",", ".") + ",";
                    wins++;

                    curentBet = bet;

                    flag = true;
                }
                else
                {
                    profit -= curentBet;
                    wagered += curentBet;

                    tmp = $"{profit:f8}";
                    statistic += tmp.Replace(",", ".") + ",";
                    losses++;
                    curentBet *= 2.0;

                    flag = false;
                }
            }

            statistic = statistic.Remove(statistic.Length - 1);
        }


        private string s1 = "{\r\n    \"operationName\": null,\r\n    \"variables\": {\r\n        \"message\": \"My statistics\",\r\n        \"channelId\": \"1\",\r\n        \"payload\": \"{\\\"currency\\\":\\\"";
        private string s2 = "\\\",\\\"betsAmount\\\":";
        private string s3 = ",\\\"totalProfit\\\":";
        private string s4 = ",\\\"wins\\\":";
        private string s5 = ",\\\"losses\\\":";
        private string s6 = ",\\\"chartData\\\":[";
        private string s7 = "]}\"\r\n    },\r\n    \"query\": \"mutation ($channelId: ID!, $message: String!, $payload: String) {\\n  chatAddMessage(channelId: $channelId, message: $message, payload: $payload) {\\n    ...FRAGMENT_MESSAGE\\n    __typename\\n  }\\n}\\n\\nfragment FRAGMENT_MESSAGE on ChatMessage {\\n  id\\n  body\\n  message {\\n    body\\n    balance {\\n      amount\\n      currency\\n      __typename\\n    }\\n    transaction {\\n      amount\\n      date\\n      currency\\n      status\\n      __typename\\n    }\\n    rain {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      users\\n      currency\\n      amount\\n      amounts\\n      loyaltyLevelIds\\n      __typename\\n    }\\n    tips {\\n      author {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      recipient {\\n        id\\n        login\\n        lastActivity\\n        avatar\\n        __typename\\n      }\\n      currency\\n      amount\\n      message\\n      isPrivate\\n      __typename\\n    }\\n    news {\\n      text\\n      imageUrl\\n      buttonUrl\\n      buttonText\\n      __typename\\n    }\\n    achievement {\\n      levelId\\n      data {\\n        key\\n        value\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  createdAt\\n  isDeleted\\n  payload\\n  likes {\\n    count\\n    likedBy\\n    __typename\\n  }\\n  user {\\n    id\\n    login\\n    avatar\\n    userRoles\\n    lastActivity\\n    loyaltyLevel {\\n      level {\\n        id\\n        category\\n        level\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  type\\n  __typename\\n}\\n\"\r\n}";

        
        public void GenerateInfo()
        {
            while (true)
            {
                wagered = 0;
                profit = 0;
                wins = 0;
                losses = 0;
                statistic = "";

                GenerateCurrency();
                GenerateStatistic();

                wageredS = $"{wagered:f8}";
                wageredS = wageredS.Replace(",", ".");

                profitS = $"{profit:f8}";
                profitS = profitS.Replace(",", ".");

                jsonString = s1 + сurrency + s2 + wageredS + s3 + profitS + s4 + wins + s5 + losses + s6 + statistic +
                             s7;
                request.Parameters[10].Value = jsonString;
                GoPost();

                //пропихнуть проверку на отправку запроса

                spamCount++;
                InfOut();
                Thread.Sleep(pause*60000);
            }
        }

        private void InfOut()
        {
            infoBlock.Dispatcher.Invoke(new Action(() => infoBlock.Text = spamCount.ToString()));
        }
        private void GoPost()
        {
           IRestResponse response = client.Execute(request);
           var a = response.Content;
        }

         
    }
}