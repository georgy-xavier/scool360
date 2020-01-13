using System;
using System.Data;
using System.Configuration;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Data.Odbc;

/// <summary>
/// Summary description for KnowinGroup
/// </summary>
public class KnowinGroup:KnowinGen
{
    public MysqlClass m_MysqlDb;
    private OdbcDataReader m_MyReader=null;
    public int m_GroupId;
    public string m_GroupName;
    public string m_GroupDisc;
    public int m_ParentId;
    public int m_ManagerId;
    public int m_GroupTypeId;
	public KnowinGroup(KnowinGen _Prntobj)
	{
        m_Parent = _Prntobj;
        //m_MyReader = m_Parent.ODBCconnection   ;
        m_MyODBCConn =m_Parent.ODBCconnection   ;
        m_MysqlDb = new MysqlClass(this);
        m_GroupId = 0;
        m_GroupName = "";
        m_GroupDisc="";
        m_ParentId = -1;
        m_ManagerId = -1;
        m_GroupTypeId = 1;
	}

    ~KnowinGroup()
    {
        if (m_MysqlDb != null)
        {
            m_MysqlDb = null;

        } if (m_MyReader != null)
        {
            m_MyReader = null;

        }
       
        //if (m_MysqlDb._myOdbcConn != null) m_MysqlDb._myOdbcConn.Close();
    }

    private void SetParent(int _i_GroupId,int _i_PrntId)
    {
        string newsql;
        OdbcDataReader MyReader1 = null;
        string sql ;

            if (_i_PrntId != -1)
            {
                sql = "SELECT ParentId FROM tblgrouprelation where ChildId=" + _i_PrntId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    while (m_MyReader.Read())
                    {
                        newsql = "INSERT INTO tblgrouprelation (ParentId,ChildId) VALUES (" + int.Parse(m_MyReader.GetValue(0).ToString()) + ", " + _i_GroupId + ")";
                        MyReader1 = m_MysqlDb.ExecuteQuery(newsql);
                    }

                    MyReader1.Close();
                }
                sql = "INSERT INTO tblgrouprelation (ParentId,ChildId) VALUES (" + _i_PrntId + ", " + _i_GroupId + ")";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }


        m_MyReader.Close();
    }
    public bool ValidadGroupName(string _gpName)
    {
        bool _valide;
        string sql = "SELECT * FROM tblgroup where GroupName='" + _gpName + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = false;
        }
        else
        {
            _valide = true;
        }
        m_MyReader.Close();
        return _valide;

    }


    private void AddGroup()
    {
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string sql = "INSERT INTO tblgroup(GroupName,Discription,CreatedDate,ModifiedDate,ParentId,ManagerId,GroupTypeId) VALUES ('" + m_GroupName + "', '" + m_GroupDisc + "','" + _strdtNow + "','" + _strdtNow + "'," + m_ParentId + "," + m_ManagerId + "," + m_GroupTypeId + ")";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();
        
    }
    public bool VadidateChild(int _GroupId, int _ChildId)
    {
        bool _valide;
        string sql = "SELECT * FROM tblgrouprelation where ParentId=" + _GroupId + " AND ChildId=" + _ChildId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = true;
        }
        else
        {
            _valide = false;
        }
        m_MyReader.Close();
        return _valide;
    }
    public void CreateNewGroup(string _GroupName, string _GroupDisc, int _ParentId, int _ManagerId,int _GroupTypeId)
    {
        m_GroupName = _GroupName;
        m_GroupDisc = _GroupDisc;
        m_ParentId = _ParentId;
        m_ManagerId = _ManagerId;
        m_GroupTypeId = _GroupTypeId;
        AddGroup();
        string sql = "SELECT Id FROM tblgroup where GroupName='" + m_GroupName + "'";
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {
            m_MyReader.Read();
            m_GroupId = int.Parse(m_MyReader.GetValue(0).ToString());

            SetParent(m_GroupId,m_ParentId);
        }
        m_MyReader.Close();
    }

    public void UpdateGroup(int _GroupID, string _GroupName, string _GroupDisc, int _ParentId, int _ManagerId, int _GroupTypeId)
    {
        m_GroupId = _GroupID;
        m_GroupName = _GroupName;
        m_GroupDisc = _GroupDisc;
        int _old_ParentId = m_ParentId;
        m_ParentId = _ParentId;
        m_ManagerId = _ManagerId;
        m_GroupTypeId = _GroupTypeId;
        UpdateGroupDb();
        if (_old_ParentId != m_ParentId)
        {
            ChangeParent(m_GroupId, _old_ParentId, m_ParentId);
        }

    }

    private void ChangeParent(int _GrpId,int _oldPrntId,int _newPrntId)
    {
        DeleteParent(_GrpId, _oldPrntId);
        SetParent(_GrpId,_newPrntId);
        SetChildToParents(_GrpId, _newPrntId);
    }

    private void SetChildToParents(int _GrpId, int _newPrntId)
    {
        string newsql1;
        string newsql2;
        DataSet myDataset;
        string sql;

        if (_newPrntId != -1)
        {
            newsql2 = "SELECT ChildId FROM tblgrouprelation where ParentId=" + _GrpId + "";
            myDataset = m_MysqlDb.ExecuteQueryReturnDataSet(newsql2);
            newsql1 = "SELECT ParentId FROM tblgrouprelation where ChildId=" + _GrpId + "";
            DataSet myDataset1 = m_MysqlDb.ExecuteQueryReturnDataSet(newsql1);
            m_MysqlDb.CloseExistingConnection();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                if (myDataset1 != null && myDataset1.Tables != null && myDataset1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in myDataset1.Tables[0].Rows)
                    {
                        foreach (DataRow dr in myDataset.Tables[0].Rows)
                        {

                            sql = "INSERT INTO tblgrouprelation (ParentId,ChildId) VALUES (" + int.Parse(dr1[0].ToString()) + ", " + int.Parse(dr[0].ToString()) + ")";
                            m_MysqlDb.ExecuteQuery(sql);

                        }

                    }
                }
            }
            
        }
 
    }

    private void DeleteParent(int _GrpId, int _oldPrntId)
    {
        string newsql1;
        string newsql2;
        DataSet myDataset;
        string sql;

        if (_oldPrntId != -1)
        {
            newsql2 = "SELECT ChildId FROM tblgrouprelation where ParentId=" + _GrpId + "";
            myDataset = m_MysqlDb.ExecuteQueryReturnDataSet(newsql2);
            newsql1 = "SELECT ParentId FROM tblgrouprelation where ChildId=" + _GrpId + "";
            DataSet myDataset1 = m_MysqlDb.ExecuteQueryReturnDataSet(newsql1);
            m_MysqlDb.CloseExistingConnection();
            if (myDataset != null && myDataset.Tables != null && myDataset.Tables[0].Rows.Count > 0)
            {
                if (myDataset1 != null && myDataset1.Tables != null && myDataset1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in myDataset1.Tables[0].Rows)
                    {
                        foreach (DataRow dr in myDataset.Tables[0].Rows)
                        {

                            sql = "DELETE FROM tblgrouprelation WHERE ParentId =" + int.Parse(dr1[0].ToString()) + " AND ChildId=" + int.Parse(dr[0].ToString());
                            m_MysqlDb.ExecuteQuery(sql);
                        }

                    }
                }
            }
            sql = "DELETE FROM tblgrouprelation WHERE ChildId=" + _GrpId;
            m_MysqlDb.ExecuteQuery(sql);
            m_MysqlDb.CloseExistingConnection();
        }

       
    }
    private void UpdateGroupDb()
    {
        string _strdtNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string sql = "UPDATE tblgroup SET GroupName= '" + m_GroupName + "', Discription = '" + m_GroupDisc + "', ModifiedDate = '" + _strdtNow + "', ParentId = " + m_ParentId + ", ManagerId = " + m_ManagerId + ", GroupTypeId = " + m_GroupTypeId + " WHERE Id =" + m_GroupId + "";     
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();

    }

    public void DeleteGroup(int _GroupId)
    {
        FreeGroupMembers(_GroupId);
        string sql = "DELETE FROM tblgroup WHERE Id=" + _GroupId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();
        DeleteParent(_GroupId, m_ParentId);
        FreeChild(_GroupId);
        
    }

    private void FreeGroupMembers(int _GroupId)
    {
        string sql = "DELETE FROM tblgroupusermap WHERE GroupId=" + _GroupId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();
    }

    private void FreeChild(int _GroupId)
    {
        string sql = "DELETE FROM tblgrouprelation WHERE ParentId=" + _GroupId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        sql = "UPDATE tblgroup SET  ParentId =-1 WHERE ParentId =" + _GroupId;     
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        m_MyReader.Close();
    }

    public DataSet GroupMembers(int _GroupId)
    {
        DataSet myDataset;
        string sql = "SELECT UserId FROM tblgroupusermap where GroupId=" + _GroupId ;
        myDataset = m_MysqlDb.ExecuteQuery(sql, "GroupMembers");
        return myDataset;
    }

    public bool IsGroupMember(DataSet _dataSet, int _UserId)
    {
        bool _valide;
        _valide = false;
        if (_dataSet != null && _dataSet.Tables != null && _dataSet.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in _dataSet.Tables[0].Rows)
                {
                if(int.Parse(dr[0].ToString())==_UserId)
                {
                    _valide = true;

                }
                            
                }

        }
        return _valide;
        //throw new Exception("The method or operation is not implemented.");
    }

    public bool HasMember(int _GpId)
    {

        bool _valide;
        string sql = "SELECT * FROM tblgroupusermap where GroupId=" + _GpId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = true;
        }
        else
        {
            _valide = false;
        }
        m_MyReader.Close();
        return _valide;

    }

    public void ADDMember(int _grpid, int _userid)
    {
        string sql = "INSERT INTO tblgroupusermap (GroupId,UserId) VALUES (" + _grpid + ", " + _userid + ")";     
        m_MysqlDb.ExecuteQuery(sql);

    }

    public void DeleteMember(int _grpid, int _userid)
    {
        string sql = "DELETE FROM tblgroupusermap WHERE GroupId =" + _grpid + " AND UserId=" + _userid;
        m_MysqlDb.ExecuteQuery(sql);
    }

    public bool IsManager(int _grpid, int _userid)
    {
        bool _valide;
        string sql = "SELECT * FROM tblgroup where Id=" + _grpid + " And ManagerId=" + _userid;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = true;
        }
        else
        {
            _valide = false;
        }
        m_MyReader.Close();
        m_MysqlDb.CloseExistingConnection();
        return _valide;
    }

    public bool CanDelete(int _GroupID, out string _errMsg)
    {
        _errMsg = "";
        bool _valide=true;
        MysqlClass _newsql=new MysqlClass();
        //string sql = "SELECT Id FROM tblclass where Status=1 AND ParentGroupID=" + _GroupID;
        //m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        //if (m_MyReader.HasRows)
        //{

        //    _valide = false;
        //    _errMsg = "This Group have associated Class ,Please remove it before deletion";
        //}
        /*string sql;
        if (HasModule(1))
        {
            sql = "SELECT Id FROM tbltask where AssGrpId=" + _GroupID;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                _valide = false;
                _errMsg = "This Group have associated Task ,Please remove it before deletion";
            }
           
        }
        if (HasModule(2))
        {
            sql = "SELECT Id FROM tblincidence where AssoGrpId=" + _GroupID;
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {

                _valide = false;
                if (_errMsg == "")
                {
                    _errMsg = "This Group have associated Incidence ,Please remove it before deletion";
                }
                else
                {
                    _errMsg = "This Group have associated Task and Incidence ,Please remove it before deletion";
                }
            }
        }
       */
        string sql = "SELECT Id FROM tblLocationItemStock WHERE (LocationId = " + _GroupID + ")";
        m_MyReader = _newsql.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = false;
            _errMsg = "Cannot delete,Items contained in this location!";

        }


        _newsql.CloseConnection();
        _newsql = new MysqlClass();

        if (_valide)
        {
            sql = "SELECT COUNT(Id) AS Expr1 FROM tblgroup";
            m_MyReader = _newsql.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int _count = 0;
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _count);
                if (_count <= 1)
                {
                    _valide = false;
                    _errMsg = "This is the last group left, atleast one group has to be kept active. You can't remove this group";
                }
            }
        }

        m_MyReader.Close();
        _newsql.CloseConnection();
        return _valide;
    }

    private bool HasModule(int _moduleId)
    {

        bool _valide;
        string sql = "SELECT * FROM tblmodule where Id=" + _moduleId;
        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
        if (m_MyReader.HasRows)
        {

            _valide = true;
        }
        else
        {
            _valide = false;
        }
        m_MyReader.Close();
        return _valide;
    }
}
