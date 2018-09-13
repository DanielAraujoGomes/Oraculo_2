using System;
using System.Collections.Generic;
using StackExchange.Redis;
using System.Threading;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            var redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();

            db.StringSet("A",1);
            var value = db.StringGet("A");

            Console.WriteLine(value);

            
            var sub = redis.GetSubscriber();
            var pub = redis.GetSubscriber();

            //pub.Publish("perguntas", "teste");

           
            

           sub.Subscribe("perguntas", (ch, msg) =>
           {
               //pub.Publish("perguntas", "teste");               
               var prefixo = msg.ToString().Substring(0, msg.ToString().IndexOf(":"));
               var pergunta = msg.ToString().Substring(msg.ToString().IndexOf(":")+1); ;
               
               Console.WriteLine("Mensagem: " + msg.ToString() + "\nPrefixo:" + prefixo + "\nPergunta:" + pergunta + "\nResposta:" + PesquisaResposta(pergunta) );
               
               //pub.Publish("perguntas", "\nResposta:" + PesquisaResposta(pergunta));

           });

           pub.Publish("perguntas", "P1:Quanto e 1 * 1 ?");

           Console.ReadKey();           

        }

        public static string PesquisaResposta(string pergunta)
        {
            
            if(pergunta == "Quanto e 1 * 1 ?")
            {
                return "1";
            }

            return "Não sei";
            
        }
    }
}
