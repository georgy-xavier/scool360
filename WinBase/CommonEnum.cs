using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinBase
{
    public class CommonEnum
    {
        public enum OnlineTransactionDisplayStatus { PaymentNotCompleted = 1, BillGenerationPending = 2, PaymentSuccess = 3, PaymentFailure = 4 }; 
        public enum OnlineTransactionStatus { PaymentStarted = 1, PaymentSuccess = 2, PaymentSuccessButHashNotMatched = 3, BillPending = 4, Billed = 5, PaymentFailure = 6, PaymentFailureButHashNotMatched=7 }; 
    }
}
