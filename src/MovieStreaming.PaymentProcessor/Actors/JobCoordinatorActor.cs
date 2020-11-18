using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka.Actor;
using Akka.DI.Core;
using MovieStreaming.PaymentProcessor.Messages;

namespace MovieStreaming.PaymentProcessor.Actors
{
       internal class JobCoordinatorActor : ReceiveActor     
    {
        private readonly IActorRef _paymentWorker;
        private int _numberOfRemainingPayments;

        public JobCoordinatorActor()
        {
            _paymentWorker = Context.ActorOf(
                Context.DI().Props<PaymentWorkerActor>(), "PaymentWorker");


            Receive<ProcessFileMessage>(
                message =>
                {
                    StartNewJob(message.FileName);
                });


            Receive<PaymentSentMessage>(
                message =>
                {
                    Console.WriteLine("Receipt {0} for account {1}", message.ReceiptReference, message.AccountNumber);

                    _numberOfRemainingPayments--;

                    var jobIsComplete = _numberOfRemainingPayments == 0;

                    if (jobIsComplete)
                    {
                        Context.System.Terminate();
                    }
                });
        }


        private void StartNewJob(string fileName)
        {
            List<SendPaymentMessage> requests = ParseCsvFile(fileName);

            _numberOfRemainingPayments = requests.Count();

            foreach (var sendPaymentMessage in requests)
            {
                _paymentWorker.Tell(sendPaymentMessage);
            }
        }



        // This could be delegated to a lower level actor to act like an error handler
        private List<SendPaymentMessage> ParseCsvFile(string fileName)
        {
            var messagesToSend = new List<SendPaymentMessage>();

            var fileLines = File.ReadAllLines(fileName);

            foreach (var line in fileLines)
            {
                var values = line.Split(',');

                var message = new SendPaymentMessage(
                                    values[0],
                                    values[1],
                                    int.Parse(values[3]),
                                    decimal.Parse(values[2]));

                messagesToSend.Add(message);
            }

            return messagesToSend;
        }
     
    }
}