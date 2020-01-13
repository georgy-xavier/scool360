using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinBase
{
   public class OtherFeeArguments : EventArgs
    {
        private string m_FeeName;
        public string FeeName
        {
            get
            {
                return m_FeeName;
            }
        }

        private double m_Amout;
        public double Amount
        {
            get
            {
                return m_Amout;
            }
        }

        private string m_BatchName;
        public string BatchName
        {
            get
            {
                return m_BatchName;
            }
        }


        private int m_SchId;
        public int SchId
        {
            get
            {
                return m_SchId;
            }
        }


        private string m_FeeId;
        public string Feeid
       {
           get 
           {
               return m_FeeId;
           }
       }
        private string m_FeeStudId;
        public string FeeStudId
        {
            get
            {
                return m_FeeStudId;
            }
        }


       
       private string m_Period;
       public string Period
       {
           get 
           {
               return m_Period;
           }
       }
       
       private string m_Status;
       public string Status
       {
           get
           {
               return m_Status;
           }
       }


       private string m_LastDate;
       public string LastDate
       {
           get
           {
               return m_LastDate;
           }
       }
       

       
       private double m_Deduction;
       public double Deduction
       {
           get 
           {
               return m_Deduction;
           }
       }


       private double m_Arrear;
       public double Arrear
       {
           get 
           {
               return m_Arrear;
           }
       }

       
       private double m_Fine;
       public double Fine
       {
           get 
           {
               return m_Fine;
           }
       }
       
       
       
       private int m_Regular;
       public int Regular
       {
           get 
           {
               return m_Regular;
           }
       }


       private int m_CollectionType;
       public int CollectionType
       {
           get
           {
               return m_CollectionType;
           }
       }


       private string m_PeriodId;
       public string PeriodId
       {
           get
           {
               return m_PeriodId;
           }
       }

       private string m_BatchId;
       public string BatchId
       {
           get
           {
               return m_BatchId;
           }
       }
       private string m_DueDate;
       public string DueDate
       {
           get
           {
               return m_DueDate;
           }
       }

       public OtherFeeArguments(int _SchId, string _FeeStudentId, string _FeeId, string _AccountName, string _BatchName, string _Period, string _PeriodId, string _Status, double _BalanceAmount, string _LastDate, double _Deduction, double _Arrear, double _Fine, int _Regular, int _CollectionType, string _BatchId, string _DueDate)
        {
            m_SchId = _SchId;
            m_FeeStudId = _FeeStudentId;
            m_FeeId = _FeeId;
            m_FeeName = _AccountName;
            m_BatchName = _BatchName;
            m_Period = _Period;
            m_Status =_Status;
            m_Amout = _BalanceAmount;
            m_LastDate = _LastDate;
            m_Deduction = _Deduction;
            m_Arrear = _Arrear;
            m_Fine = _Fine;
            m_Regular = _Regular;
            m_CollectionType = _CollectionType;
            m_PeriodId = _PeriodId;
            m_BatchId = _BatchId;
            m_DueDate = _DueDate;
        }
    }
}



