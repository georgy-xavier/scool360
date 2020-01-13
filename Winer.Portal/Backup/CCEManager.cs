using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinBase
{
    public class CCEManager
    {
        int StudentId;
        int ClassId;
        int BatchId;

    }

    public class ScholasticArea
    {
        string OverallGrade_AllSubject;
        public class ComplexFormat
        {
            int SubjectId;
            double FA1_SubExam;
            double FA2_SubExam;
            double SA1_SubExam;
            double FA3_SubExam;
            double FA4_SubExam;
            double SA2_SubExam;
            double Sumof_FirstThree;
            double Sumof_LastThree;
            double Sumof_FA;
            double Sumof_SA;
            double Sumof_All;
            string Garde;


        }

        public class NormalFormat
        {
            double Term1_Score;
            double Term2_Score;
            double Term3_Score;

            double Term1_Grade;
            double Term2_Grade;
            double Term3_Grade;
        }

        // In case of Lower classes Descriptive Area Comes under Scholastic Area
       // var list = new List<DescriptiveArea>();
    }

    public class CoScholasticArea
    {
        string Heading;
      //  var list = new List<DescriptiveArea>();
    }

    public class DescriptiveArea
    {
        int SubjectId;
       // var list = new List<DescriptiveRows>();
      
    }
    public class DescriptiveRows
    {
        string Head;
        string DescriptiveIndicators;
        string Grade;
    }
}
