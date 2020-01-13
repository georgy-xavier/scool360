using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WC.WinerSchool.BOL;
using WC.WinerSchool.DL;
using System.Web.Script.Serialization;

namespace WC.WinerSchool.BL
{
    public class WinerPortalBLClass
    {

        public string GetSchoolStrength()
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                DataManager objDL = new DataManager();
                var schools = objDL.GetAllSchoolStrength();
                return oSerializer.Serialize(schools);
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

        public string GetSchoolStaffsStrength()
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                DataManager objDL = new DataManager();
                var schools = objDL.GetAllSchoolStaffStrength();
                return oSerializer.Serialize(schools);
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

       

        public string GetSchoolTotalFees(string json)
        {
            string strReturn = Constants.ReturnStatus.FAILURE;
            try
            {

                #region Deserialize JSON
                if (string.IsNullOrEmpty(json))
                    return strReturn;
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                new System.Web.Script.Serialization.JavaScriptSerializer();
                FeeCollectionFilter filter = oSerializer.Deserialize<FeeCollectionFilter>(json);

                if (filter == null)
                    return strReturn;
                #endregion

                DataManager objDL = new DataManager();
                var schools = objDL.GetAllSchoolTotalFee(filter);
                return oSerializer.Serialize(schools);
            }
            catch
            {
                strReturn = Constants.ReturnStatus.FAILURE;
            }
            return strReturn;
        }

    }
}
