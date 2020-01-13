using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;

namespace WinBase
{

    public class TransportationClass : KnowinGen
    {
        public MysqlClass m_MysqlDb;
        public MysqlClass m_TransationDb = null;
        private OdbcDataReader m_MyReader = null;

        private string m_TransMenuStr;
        private string m_SubTranstMenuStr;

        public TransportationClass(KnowinGen _Prntobj)
        {
            m_Parent = _Prntobj;
            m_MyODBCConn = m_Parent.ODBCconnection;
            m_UserName = m_Parent.LoginUserName;
            m_MysqlDb = new MysqlClass(this);
            m_TransMenuStr = "";
            m_SubTranstMenuStr = "";

        }
        ~TransportationClass()
        {
            if (m_MysqlDb != null)
            {
                m_MysqlDb = null;

            }
            if (m_MyReader != null)
            {
                m_MyReader = null;

            }

        }

        public void CreateTansationDb()
        {

            if (m_TransationDb != null)
            {

                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

            m_TransationDb = new MysqlClass(this);
            m_TransationDb.MyBeginTransaction();

        }

        public void EndSucessTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionCommit();
                m_TransationDb = null;
            }


        }

        public void EndFailTansationDb()
        {
            if (m_TransationDb != null)
            {
                m_TransationDb.TransactionRollback();
                m_TransationDb = null;
            }

        }


        public string GetTransMangMenuString(int _roleid)
        {
            CLogging logger = CLogging.GetLogObject();
            string _MenuStr;
            if (m_TransMenuStr == "")
            {
                _MenuStr = "<ul><li><a href=\"TransportationHome.aspx\">Transportation Manager</a></li>";
                logger.LogToFile("GetTransMangMenuString", "user is sending Request to select menu tblaction ", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND ((tblroleactionmap.ModuleId=26 AND tblaction.ActionType='Link') OR tblaction.ActionType='TransLink')";
                logger.LogToFile("GetTransMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    logger.LogToFile("GetTransMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    while (m_MyReader.Read())
                    {
                        _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                    }
                }
                _MenuStr = _MenuStr + "</ul>";
                logger.LogToFile("GetTransMangMenuString", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Close();
                m_TransMenuStr = _MenuStr;
                logger.LogToFile("GetTransMangMenuString", " exits from module  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            }
            else
            {
                _MenuStr = m_TransMenuStr;
            }
            logger.LogToFile("GetTransMangMenuString", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            return _MenuStr;
        }



        public string GetSubTransMangMenuString(int _roleid)
        {

            CLogging logger = CLogging.GetLogObject();
            string _MenuStr;
            if (m_SubTranstMenuStr == "")
            {
                _MenuStr = "<ul class=\"block\"><li><a href=\"VehicleDetails.aspx\">Vehicle Details</a></li>";
                logger.LogToFile(" GetSubTransMangMenuString", "user is sending Request", 'I', CLogging.PriorityEnum.LEVEL_MEDIUM_IMPORTANT, LoginUserName);
                string sql = "SELECT DISTINCT tblaction.MenuName, tblaction.Link FROM tblaction INNER JOIN  tblroleactionmap ON tblaction.Id = tblroleactionmap.ActionId WHERE  tblroleactionmap.RoleId=" + _roleid + " AND ((tblroleactionmap.ModuleId=26 AND tblaction.ActionType='SubLink') or tblaction.ActionType='StudTransLink') order by tblaction.`Order` ";
                logger.LogToFile("GetSubTransMangMenuString", " Executing Query " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    logger.LogToFile("GetSubTransMangMenuString", " Reading Success " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                    while (m_MyReader.Read())
                    {
                        _MenuStr = _MenuStr + "<li><a href=\"" + m_MyReader.GetValue(1).ToString() + "\">" + m_MyReader.GetValue(0).ToString() + "</a></li>";
                    }

                }
                _MenuStr = _MenuStr + "</ul>";
                logger.LogToFile("GetSubTransMangMenuString", " Closing myreader  " + sql, 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
                m_MyReader.Close();
                m_SubTranstMenuStr = _MenuStr;

            }
            else
            {
                _MenuStr = m_SubTranstMenuStr;
            }
            logger.LogToFile("GetSubTransMangMenuString", "Exiting from module", 'I', CLogging.PriorityEnum.LEVEL_DEBUG, LoginUserName);
            return _MenuStr;
        }

        public bool VehicleRegistrationNoExists(string _VehicleRegNo)
        {
            bool _Exists = false;
            string sql = "SELECT tbl_tr_vehicle.RegNo from tbl_tr_vehicle where tbl_tr_vehicle.RegNo='" + _VehicleRegNo + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _Exists = true;
            }
            return _Exists;
        }

        public bool VehicleNoExists(string _VehicleNo)
        {
            bool _Exists = false;
            string sql = "SELECT tbl_tr_vehicle.VehicleNo from tbl_tr_vehicle where tbl_tr_vehicle.VehicleNo='" + _VehicleNo + "'";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                _Exists = true;
            }
            return _Exists;
        }


        public object getVehs(int rId)
        {
            int Vehs = 0;
            string sql = "select count(distinct(VehicleId)) from tbl_tr_trips where routeId=" + rId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    Vehs = int.Parse(m_MyReader.GetValue(0).ToString());
                }
            }
            return Vehs;
        }
        public object getTrips(int rId)
        {
            int Trips = 0;
            string sql = "select count(Id) from tbl_tr_trips where routeId=" + rId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    Trips = int.Parse(m_MyReader.GetValue(0).ToString());
                }
            }
            return Trips;
        }

        public OdbcDataReader getDestinsNotinRoute(int RouteId)
        {
            string sql = "select tbl_tr_destinations.Id, tbl_tr_destinations.Destination from tbl_tr_destinations where tbl_tr_destinations.Id not in (select tbl_tr_routedestinations.DestinationId from tbl_tr_routedestinations where tbl_tr_routedestinations.RouteId = " + RouteId + ")  ORDER BY  `tbl_tr_destinations`.`Destination`";
            return m_MysqlDb.ExecuteQuery(sql);
        }


        public OdbcDataReader getDestinsInRoute(int RouteId)
        {
            string sql = "select tbl_tr_destinations.Id, tbl_tr_destinations.Destination from tbl_tr_destinations inner join tbl_tr_routedestinations on tbl_tr_destinations.Id = tbl_tr_routedestinations.DestinationId where tbl_tr_routedestinations.RouteId = " + RouteId + "  ORDER BY  tbl_tr_routedestinations.DestinationOrder";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public void DeleteDestin(int RouteId, int DestinId)
        {
            string sql = "delete from tbl_tr_routedestinations where RouteId=" + RouteId + " and DestinationId=" + DestinId + "";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public void UpdateRouteTimeDistanc(int RouteId)
        {
            int time = 0;
            double dist = 0;
            string sql = "select Max(Distance),Max(Time) from tbl_tr_routedestinations where Routeid=" + RouteId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    if (m_MyReader.GetValue(1).ToString() != "" && m_MyReader.GetValue(0).ToString() != "")
                    {
                        time = int.Parse(m_MyReader.GetValue(1).ToString());
                        dist = double.Parse(m_MyReader.GetValue(0).ToString());
                        sql = "update tbl_tr_route set OneSideDistance=" + dist + ", OneSideTime=" + time + "  where id=" + RouteId + "";
                    }
                    else
                    {
                        sql = "update tbl_tr_route set OneSideDistance= 0, OneSideTime= 0  where id=" + RouteId + "";
                    }
                }
            }

            m_MyReader = m_MysqlDb.ExecuteQuery(sql);


        }
        public void AddRouteTimeDistanc(int RouteId)
        {
            int time = 0;
            double dist = 0;
            string sql = "select Max(Distance),Max(Time) from tbl_tr_routedestinations where Routeid=" + RouteId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    time = int.Parse(m_MyReader.GetValue(1).ToString());
                    dist = double.Parse(m_MyReader.GetValue(0).ToString());
                }
            }

            sql = "update tbl_tr_route set OneSideDistance=" + dist + ", OneSideTime=" + time + "  where id=" + RouteId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);

        }



        public OdbcDataReader getVehiclesAll()
        {
            string sql = "select tbl_tr_vehicletype.VehicleType, tbl_tr_vehicle.VehicleNo, tbl_tr_vehicle.Id from tbl_tr_vehicle inner join tbl_tr_vehicletype on  tbl_tr_vehicle.TypeId = tbl_tr_vehicletype.Id";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public OdbcDataReader getTripsAll()
        {
            //string sql = "select tripname, id from tbl_tr_trips";
            string sql = "select CONCAT(  TIME_FORMAT(tbl_tr_trips.StartTime,'%h:%i %p'),'- ',tbl_tr_trips.TripName) as tripname, id from tbl_tr_trips";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet getDestinationsAll()
        {
            string sql = "select Destination, id from tbl_tr_destinations";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public DataSet getStudentsTrips(int classid, int vehicleid, int tripid, int destinationid,int CurrentBatchId)
        {
            #region set
            //string sql = "", classqry = "", vehicleqry = "", tripqry = "", destinqry = "";
            //string classequelsymbol = " != ", vehicleequelsymbol = " != ", destinequelsymbol = " != ", tripequelsymbol = " != ";
            //sql = "select tblstudent.StudentName, tblstudent.Sex, tblstudent.RollNo,tblclass.ClassName, tblstudent.Address, tbl_tr_studtripmap.DestinationId,  tbl_tr_studtripmap.ToTripId,tbl_tr_studtripmap.FromTripId,tblstudent.Id from tbl_tr_studtripmap inner join tblstudent on tbl_tr_studtripmap.StudId = tblstudent.Id  inner join tblclass on  tblstudent.LastClassId = tblclass.Id ";
            //if (classid > 0)
            //{
            //    classequelsymbol = " = ";
            //}
            //if (vehicleid > 0)
            //{
            //    vehicleqry = " and tbl_tr_trips.VehicleId =" + vehicleid + "";
            //}
            //if (tripid > -1)
            //{
            //    tripequelsymbol = " = ";
            //}
            //if (destinationid > -1)
            //{
            //    destinequelsymbol = " = ";
            //}

            //classqry = "tblclass.Id " + classequelsymbol + " " + classid + " ";
            //destinqry = "tbl_tr_studtripmap.DestinationId " + destinequelsymbol + " " + destinationid + " ";
            //tripqry = "( tbl_tr_studtripmap.FromTripId  " + tripequelsymbol + " " + tripid + "  or  tbl_tr_studtripmap.ToTripId " + tripequelsymbol + " " + tripid + " )";

            //string searchqry = "";
            //if (vehicleid > 0)
            //{
            //    searchqry = sql + " inner join tbl_tr_trips on tbl_tr_studtripmap.FromTripId = tbl_tr_trips.Id where " + classqry + vehicleqry + " and " + tripqry + " and " + destinqry;
            //    searchqry = searchqry + " union " + sql + " inner join tbl_tr_trips on tbl_tr_studtripmap.ToTripId = tbl_tr_trips.Id where " + classqry + vehicleqry + " and " + tripqry + " and " + destinqry;
            //}
            //else
            //{
            //    searchqry = sql + " where " + classqry + vehicleqry + " and " + tripqry + " and " + destinqry;
            //}
            #endregion

            string classequel = "!=0", destinequel = "!=-1", Tripquel = "!=-1";
            string Vehicleinnerjoin = "", innerjoincon = "";
            if (destinationid > -1)
                destinequel = "=" + destinationid;
            if (classid > 0)
                classequel = "=" + classid;
            if (vehicleid > 0)
            {
                Vehicleinnerjoin = "INNER JOIN tbl_tr_trips ON tbl_tr_studtripmap.FromTripId=tbl_tr_trips.Id";
                innerjoincon = "tbl_tr_trips.VehicleId=" + vehicleid + " AND";
            }
            if (tripid > -1)
                Tripquel = "=" + tripid;

            string FinalQuery = "SELECT tblstudentclassmap.StudentId as Id,tblstudent.StudentName as StudentName,tblclass.ClassName as ClassName,tblstudentclassmap.RollNo as RollNo,tblstudent.Sex as Sex,tblstudent.Address  as Address from tblstudentclassmap INNER join tblstudent on tblstudent.Id=tblstudentclassmap.StudentId inner join tblclass on tblclass.Id=tblstudentclassmap.ClassId where tblstudentclassmap.StudentId  in (select tbl_tr_studtripmap.StudId from tbl_tr_studtripmap " + Vehicleinnerjoin + " where tbl_tr_studtripmap.DestinationId  " + destinequel + " AND " + innerjoincon + " (tbl_tr_studtripmap.FromTripId   " + Tripquel + "  or  tbl_tr_studtripmap.ToTripId  " + Tripquel + " )) AND tblstudentclassmap.BatchId=" + CurrentBatchId + " AND  tblstudentclassmap.ClassId" + classequel + "  order by tblstudentclassmap.RollNo";
            DataSet Ds_Report= m_MysqlDb.ExecuteQueryReturnDataSet(FinalQuery);
            Ds_Report.Tables[0].Columns.Add("DesId",typeof(string));
            Ds_Report.Tables[0].Columns.Add("ToTripId", typeof(string));
            Ds_Report.Tables[0].Columns.Add("FromTripId", typeof(string));
            int stuId = 0;
            foreach (DataRow dr in Ds_Report.Tables[0].Rows)
            {
                int.TryParse(dr["Id"].ToString(), out stuId);
                string sql = "SELECT tbl_tr_studtripmap.DestinationId,tbl_tr_studtripmap.ToTripId,tbl_tr_studtripmap.FromTripId from tbl_tr_studtripmap where tbl_tr_studtripmap.StudId="+stuId;
                DataSet Tripds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
                foreach (DataRow d_dr in Tripds.Tables[0].Rows)
                {
                    dr["DesId"] = d_dr["DestinationId"].ToString();
                    dr["ToTripId"] = d_dr["ToTripId"].ToString();
                    dr["FromTripId"] = d_dr["FromTripId"].ToString();
                }
            }
            return Ds_Report;
        }

        public DataSet getStudentsFromClass(int classid,int batchid)
        {
            string sql = "select tblstudent.Id, tblstudent.StudentName, tblstudent.RollNo, tblstudent.Sex, tblstudent.Address from tblstudent inner join tblstudentclassmap  on tblstudentclassmap.StudentId= tblstudent.Id where tblstudent.LastClassId = " + classid + " and tblstudent.status = 1 and tblstudentclassmap.BatchId=" + batchid + " and tblstudent.UseBus=1 and tblstudent.Id not in (select studid from tbl_tr_studtripmap) order by tblstudent.RollNo";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public int getStudentCountFromClass(int classid, int batchid)
        {
            int _TotalSeats = 0;
            string sql = "select count(tblstudent.Id) from tblstudent inner join tblstudentclassmap  on tblstudentclassmap.StudentId= tblstudent.Id where tblstudent.LastClassId = " + classid + " and tblstudent.status = 1 and tblstudentclassmap.BatchId=" + batchid + " and tblstudent.UseBus=1 ";
            //string sql = "select count(tblstudent.Id) from tblstudent inner join tblstudentclassmap  on tblstudentclassmap.StudentId= tblstudent.Id where tblstudent.LastClassId = " + classid + " and tblstudent.status = 1 and tblstudentclassmap.BatchId=" + batchid + " and tblstudent.UseBus=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out _TotalSeats);
            }
            return _TotalSeats;
        }

        public OdbcDataReader getScholTrips(int Destination, int type)
        {
            string sql = "select tbl_tr_trips.Id, CONCAT(  TIME_FORMAT(tbl_tr_trips.StartTime,'%h:%i %p'),'- ',tbl_tr_trips.TripName) from tbl_tr_trips inner join tbl_tr_tripdestinations on tbl_tr_trips.Id = tbl_tr_tripdestinations.TripId inner join tbl_tr_destinations on tbl_tr_destinations.Id = tbl_tr_tripdestinations.DestinationId where tbl_tr_destinations.Id = " + Destination + " and tbl_tr_trips.RouteTypeId = " + type + " order by tbl_tr_trips.StartTime";
            return m_MysqlDb.ExecuteQuery(sql);
        }

        public bool SufficientCapacity(int tripid, int type)
        {
            bool valid = false;
            string sql = "select tbl_tr_trips.Occupied from tbl_tr_trips inner join tbl_tr_vehicle on tbl_tr_vehicle.Id = tbl_tr_trips.VehicleId where tbl_tr_trips.Id = " + tripid + " and ((tbl_tr_trips.capacity >  tbl_tr_trips.Occupied) or ( tbl_tr_vehicle.Capacity > tbl_tr_trips.Occupied ))";
            if (type == 0)
            {
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            }
            else
            {
                m_MyReader = m_TransationDb.ExecuteQuery(sql);
            }
            if (m_MyReader.HasRows)
            {
                valid = true;
            }
            else
            {
                sql = "select tbl_tr_trips.Occupied from tbl_tr_trips  where tbl_tr_trips.Id = " + tripid + "   and (tbl_tr_trips.capacity >  tbl_tr_trips.Occupied) ";
                if (type == 0)
                {
                    m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                }
                else
                {
                    m_MyReader = m_TransationDb.ExecuteQuery(sql);
                }
                if (m_MyReader.HasRows)
                {
                    valid = true;
                }
            }

            return valid;
        }

        public void StudTripMap(int studid, int destinid, int fromtripid, int totripid)
        {
            if (!SufficientCapacity(fromtripid, 0))
            {
                fromtripid = 0;
            }
            if (!SufficientCapacity(totripid, 0))
            {
                totripid = 0;
            }
            string sql = "insert into tbl_tr_studtripmap (studId,DestinationId,FromTripId,ToTripId) values (" + studid + "," + destinid + "," + fromtripid + "," + totripid + ")";
            m_MysqlDb.ExecuteQuery(sql);
            sql = "update tbl_tr_trips set Occupied = Occupied+1 where Id=" + fromtripid + " ";
            m_MysqlDb.ExecuteQuery(sql);
            sql = "update tbl_tr_trips set Occupied = Occupied+1 where Id=" + totripid + " ";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public string GetDestination(int destinid)
        {
            string destination = "None";
            string sql = "select destination,Cost from tbl_tr_destinations where id =" + destinid + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    destination = m_MyReader.GetValue(0).ToString();
                }
            }
            return destination;

        }

        public string GetTripName(int tripId)
        {
            string trip = "None";
            string sql = "select CONCAT(  TIME_FORMAT(tbl_tr_trips.StartTime,'%h:%i %p'),'- ',tbl_tr_trips.TripName) tripname from tbl_tr_trips where id =" + tripId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    trip = m_MyReader.GetValue(0).ToString();
                }
            }
            return trip;
        }

        public void UpdateStudTripMap(int studId, int Oldfrom, int oldto, int newDestin, int newFrom, int newTo, out string message)
        {

            string sql = "";
             message = "";
             if (newFrom > 0)
             {
                 if (!SufficientCapacity(newFrom, 1))
                 {
                     if (newFrom != Oldfrom)
                     {
                         message = "Trip filled..Cannot update from trip... ";
                     }
                     newFrom = Oldfrom;

                 }
             }
             if (newTo > 0)
             {

                 if (!SufficientCapacity(newTo, 1))
                 {
                     if (newTo != oldto)
                     {
                         if (message == "")
                         {
                             message = "Trip filled..Cannot update To trip...";
                         }
                         else
                         {
                             message = "Trips filled..Cannot update from trip and To trip...";
                         }

                     }
                     newTo = oldto;

                 }
             }
          

            sql = "update tbl_tr_studtripmap set DestinationId=" + newDestin + ", FromTripId=" + newFrom + ", ToTripId=" + newTo + " where StudId=" + studId + "";
            m_TransationDb.ExecuteQuery(sql);
            if (newFrom != Oldfrom)
            {
                sql = "update tbl_tr_trips set Occupied = Occupied+1 where Id=" + newFrom + " ";
                m_TransationDb.ExecuteQuery(sql);
                sql = "update tbl_tr_trips set Occupied = Occupied-1 where Id=" + Oldfrom + " ";
                m_TransationDb.ExecuteQuery(sql);
            }
            if (newTo != oldto)
            {
                sql = "update tbl_tr_trips set Occupied = Occupied-1 where Id=" + oldto + " ";
                m_TransationDb.ExecuteQuery(sql);
                sql = "update tbl_tr_trips set Occupied = Occupied+1 where Id=" + newTo + " ";
                m_TransationDb.ExecuteQuery(sql);
            }
        }

        public void DeleteStudTripMap(int studId, int Oldfrom, int oldto)
        {
            string sql = "Delete From tbl_tr_studtripmap where studId=" + studId + "";
            m_TransationDb.ExecuteQuery(sql);
            sql = "update tbl_tr_trips set Occupied = Occupied-1 where Id=" + Oldfrom + " ";
            m_TransationDb.ExecuteQuery(sql);
            sql = "update tbl_tr_trips set Occupied = Occupied-1 where Id=" + oldto + " ";
            m_TransationDb.ExecuteQuery(sql);
        }

        public void DeleteRoute(int id)
        {
            string sql = "DELETE from tbl_tr_route where tbl_tr_route.Id='" + id + "'";
            m_TransationDb.ExecuteQuery(sql);
            sql = "DELETE from tbl_tr_routedestinations where tbl_tr_routedestinations.RouteId='" + id + "'";
            m_TransationDb.ExecuteQuery(sql);
            sql = "delete from tbl_tr_tripdestinations where tbl_tr_tripdestinations.TripId in (select tbl_tr_trips.Id from tbl_tr_trips where tbl_tr_trips.RouteId = " + id + " )";
            m_TransationDb.ExecuteQuery(sql);
            sql = "DELETE from tbl_tr_trips where RouteId='" + id + "'";
            m_TransationDb.ExecuteQuery(sql);
        }

        public bool CanRemoveDestin(int RouteId, int DestinId)
        {
            bool CanRemove = true;
            string sql = "select tbl_tr_studtripmap.StudId from tbl_tr_studtripmap where tbl_tr_studtripmap.DestinationId = " + DestinId + " and tbl_tr_studtripmap.FromTripId in (select distinct tbl_tr_tripdestinations.TripId from tbl_tr_tripdestinations where tbl_tr_tripdestinations.TripId in (select tbl_tr_trips.Id from tbl_tr_trips where tbl_tr_trips.RouteId = " + RouteId + "))";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                CanRemove = false;
            }
            return CanRemove;
        }

        public bool CanDeleteTrip(int _TripId)
        {
            bool candelete = true;
            string sql = "   select tbl_tr_studtripmap.StudId  from tbl_tr_studtripmap where tbl_tr_studtripmap.FromTripId="+_TripId+"  or tbl_tr_studtripmap.ToTripId="+_TripId+"";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                candelete = false;
            }
            return candelete;
        }

        public void DeleteTrip(int _TripId)
        {
            string sql = "DELETE from tbl_tr_trips where tbl_tr_trips.Id=" + _TripId;
            m_MysqlDb.ExecuteQuery(sql);
            sql = "DELETE from tbl_tr_tripdestinations where tbl_tr_tripdestinations.TripId=" + _TripId;
            m_MysqlDb.ExecuteQuery(sql);
        }

        public int GetMaxCapacity(int capacity, int vehId)
        {
            int maxCapacity = 0;
            string sql = "select capacity from tbl_tr_vehicle where id = " + vehId + " and capacity >= " + capacity + " ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    maxCapacity = int.Parse(m_MyReader.GetValue(0).ToString());
                }
            }
            return maxCapacity;
        }

        public string GetVehInfo(int vehId)
        {
            string Vehicle = "";
            string sql = "SELECT concat(tbl_tr_vehicletype.VehicleType,'/', tbl_Tr_vehicle.VehicleNo) from tbl_tr_vehicle inner join tbl_tr_vehicletype on tbl_tr_vehicletype.Id = tbl_tr_vehicle.TypeId where tbl_tr_vehicle.Id = " + vehId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    Vehicle = m_MyReader.GetValue(0).ToString();
                }
            }
            return Vehicle;
        }

        public void GetTripInfo(int vehId, out int trips, out double dist, out string tripnames)
        {
            trips = 0;
            dist = 0;
            tripnames = "None";
            string sql = "select count( tbl_Tr_trips.Id) as Trips,sum(tbl_Tr_trips.Distance ) + sum(tbl_Tr_trips.ExtraDistance ) as Distance from tbl_Tr_trips where tbl_Tr_trips.VehicleId = " + vehId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    if (m_MyReader.GetValue(0).ToString() != "")
                        trips = int.Parse(m_MyReader.GetValue(0).ToString());
                    if (m_MyReader.GetValue(1).ToString() != "")
                        dist = double.Parse(m_MyReader.GetValue(1).ToString());
                }
            }

            sql = " select tbl_Tr_trips.TripName from tbl_Tr_trips where tbl_Tr_trips.VehicleId  = " + vehId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                tripnames = "<table width=\"100%\">";
                while (m_MyReader.Read())
                {

                    tripnames = tripnames + "<tr><td>" + m_MyReader.GetValue(0).ToString() + " </td></tr>";
                }
                tripnames = tripnames + "</table>";
            }
        }

        public void GetDestinTime(int destinid, int tripId, out string starttime, out string endtime, out int time)
        {
            starttime = "";
            endtime = "";
            time = 0;
            string sql = "   SELECT  tbl_tr_trips.StartTime, tbl_tr_trips.EndTime, tbl_tr_routedestinations.`Time` from  tbl_tr_routedestinations inner join tbl_tr_trips on tbl_tr_trips.RouteId = tbl_tr_routedestinations.RouteId where  tbl_tr_routedestinations.DestinationId = " + destinid + " and tbl_tr_trips.Id = " + tripId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    starttime = m_MyReader.GetValue(0).ToString();
                    endtime = m_MyReader.GetValue(1).ToString();
                    time = int.Parse(m_MyReader.GetValue(2).ToString());
                }
            }
        }

        public bool StudExistsInTrip(int _TripId)
        {
            bool exists = false;
            string sql = "select studid from tbl_tr_studtripmap where FromTripId = " + _TripId + " or TotripId = " + _TripId + " ";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                exists = true;
            }
            return exists;
        }

        public void UpdateTripTimeDist(int id, TimeSpan TotTime, double dist)
        {
            string sql = "update tbl_tr_trips set EndTime = ADDTIME(TIME_FORMAT(StartTime,'%H:%i:%s'), '" + TotTime + "'), Distance = " + dist + " where RouteId = " + id + " ";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetDestinationDetails()
        {
            OdbcDataReader Des_reader = null;
            DataSet Destination_Ds = new DataSet();
            DataTable dt;
            DataRow _dr;
            Destination_Ds.Tables.Add(new DataTable("DestinationDetails"));
            dt = Destination_Ds.Tables["DestinationDetails"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Destination");
            dt.Columns.Add("Distance");
            string sql = "";
            string _sql = "";
            sql = "select tbl_tr_destinations.Id, tbl_tr_destinations.Destination from tbl_tr_destinations";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {
                    _sql = "select tbl_tr_routedestinations.Distance  from tbl_tr_routedestinations where tbl_tr_routedestinations.DestinationId=" + int.Parse(m_MyReader.GetValue(0).ToString()) + "";
                    Des_reader = m_MysqlDb.ExecuteQuery(_sql);
                    _dr = Destination_Ds.Tables["DestinationDetails"].NewRow();
                    if (Des_reader.HasRows)
                    {
                        _dr["Distance"] = Des_reader.GetValue(0).ToString();
                    }
                    else
                    {
                        _dr["Distance"] = "";
                    }
                    _dr["Id"] = m_MyReader.GetValue(0).ToString();
                    _dr["Destination"] = m_MyReader.GetValue(1).ToString();
                    Destination_Ds.Tables["DestinationDetails"].Rows.Add(_dr);
                }

            }
            return Destination_Ds;
        }

        public void SaveData(int Id, double cost)
        {
            string sql = "";
            sql = "Update tbl_tr_destinations set Cost=" + cost + " where Id=" + Id + "";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public double GetCost(int Id)
        {
            string sql = "";
            double Cost = 0.0;
            sql = "select tbl_tr_destinations.Cost from tbl_tr_destinations Where Id=" + Id + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                Cost = double.Parse(m_MyReader.GetValue(0).ToString());
            }
            return Cost;
        }

        public int GetFeeAccountType(int FeeAcntId)
        {
            int TypeId = 0;
            string sql = "";
            sql = " Select tblfeeaccount.FrequencyId from tblfeeaccount  where tblfeeaccount.Id=" + FeeAcntId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                TypeId = int.Parse(m_MyReader.GetValue(0).ToString());
            }
            return TypeId;
        }

        public DataSet GetPeriod()
        {
            DataSet Period_Ds = new DataSet();
            string sql = "SELECT  tblperiod.Id, tblperiod.Period from tblperiod where tblperiod.FrequencyId=4 order by tblperiod.Order";
            Period_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Period_Ds;
        }

        public DataSet GetStudentDetails(int LocationId, int batchId, int PeriodId,int currentbatchId,int classId)
        {

            DataSet StudentDetails_Ds = new DataSet();
            DataTable dt;
            DataRow _dr;
            StudentDetails_Ds.Tables.Add(new DataTable("StudentDetails"));
            dt = StudentDetails_Ds.Tables["StudentDetails"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Destination");
            dt.Columns.Add("StudentName");
            dt.Columns.Add("ClassName");
            dt.Columns.Add("Sex");
            dt.Columns.Add("classid");
            dt.Columns.Add("StudentId");
            OdbcDataReader studreader = null;
            string sql = "";
            string _sql = "";
            if (LocationId > 0)
            {
                sql = "select tbl_tr_destinations.Destination, tbl_tr_studtripmap.StudId, tbl_tr_studtripmap.DestinationId from tbl_tr_destinations inner join tbl_tr_studtripmap  on tbl_tr_studtripmap.DestinationId= tbl_tr_destinations.Id where tbl_tr_studtripmap.DestinationId=" + LocationId + "";
            }
            else
            {
                sql = "select tbl_tr_destinations.Destination, tbl_tr_studtripmap.StudId, tbl_tr_studtripmap.DestinationId from tbl_tr_destinations inner join tbl_tr_studtripmap  on tbl_tr_studtripmap.DestinationId= tbl_tr_destinations.Id";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {

                    _sql = " SELECT distinct tblfeestudent.Id from tblfeestudent INNER join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId where tblfeeschedule.FeeId=100 AND tblfeeschedule.PeriodId=" + PeriodId + " AND tblfeeschedule.BatchId=" + batchId + " AND tblfeestudent.StudId=" + int.Parse(m_MyReader.GetValue(1).ToString()) + "";
                    studreader = m_MysqlDb.ExecuteQuery(_sql);
                    if (!studreader.HasRows)
                    {
                        if (classId > 0)
                        {
                            _sql = "select tblstudent.Id, tblstudent.StudentName, tblclass.ClassName,tblclass.Id AS `classid`, tblstudent.Sex from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND  tblstudentclassmap.BatchId=" + currentbatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId where  tblstudent.Id=" + int.Parse(m_MyReader.GetValue(1).ToString()) + " and  tblstudentclassmap.ClassId=" + classId + " and tblstudent.UseBus=1";
                        }
                        else
                        {
                            _sql = "select tblstudent.Id, tblstudent.StudentName, tblclass.ClassName,tblclass.Id AS `classid`, tblstudent.Sex from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND  tblstudentclassmap.BatchId=" + currentbatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId where  tblstudent.Id=" + int.Parse(m_MyReader.GetValue(1).ToString()) + " and tblstudent.UseBus=1";
                        }
                        studreader = m_MysqlDb.ExecuteQuery(_sql);
                        if (studreader.HasRows)
                        {
                            _dr = StudentDetails_Ds.Tables["StudentDetails"].NewRow();
                            _dr["Id"] = m_MyReader.GetValue(2).ToString();
                            _dr["Destination"] = m_MyReader.GetValue(0).ToString();
                            _dr["StudentName"] = studreader.GetValue(1).ToString();
                            _dr["ClassName"] = studreader.GetValue(2).ToString();
                            _dr["Sex"] = studreader.GetValue(4).ToString();
                            _dr["classid"] = studreader.GetValue(3).ToString();
                            _dr["StudentId"] = studreader.GetValue(0).ToString();
                            StudentDetails_Ds.Tables["StudentDetails"].Rows.Add(_dr);
                        }
                    }
                }

            }
            return StudentDetails_Ds;
        }

        public void SaveDetailsToTable(int BatchId, DateTime DueDate, DateTime LastDate, int classid, int PeriodId, double amount, int studid)
        {
            //Id,FeeId,BatchId,Duedate,LastDate,Status,ClassId,PeriodId,Amount
            string sql = "";
            sql = "INSERT INTO tblfeeschedule(ClassId,PeriodId,FeeId,BatchId,Duedate,LastDate,Amount) VALUES(" + classid + "," + PeriodId + ",100," + BatchId + ",'" + DueDate.Date.ToString("s") + "','" + LastDate.Date.ToString("s") + "'," + amount + ")";
            m_MysqlDb.ExecuteQuery(sql);
            sql = " SELECT tblfeeschedule.Id FROM tblfeeschedule WHERE tblfeeschedule.ClassId=" + classid + " AND tblfeeschedule.PeriodId=" + PeriodId + " AND tblfeeschedule.BatchId=" + BatchId + " AND tblfeeschedule.FeeId=100";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int scheduleId = int.Parse(m_MyReader.GetValue(0).ToString());
                sql = "INSERT INTO tblfeestudent(SchId,StudId,Amount,BalanceAmount,Status) VALUES(" + scheduleId + "," + studid + "," + amount + "," + amount + ",'Scheduled')";
                m_MysqlDb.ExecuteQuery(sql);

            }
            //Id,SchId,StudId,Amount,BalanceAmount,Status
        }

        public DataSet GetRouteId()
        {
            DataSet Route_Ds = new DataSet();
            string sql = "";
            sql = "select tbl_tr_route.Id,RouteName from tbl_tr_route";
            Route_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Route_Ds;
        }

        public OdbcDataReader getTripCapacity(int RoutId, int value)
        {
            string sql = "";
            if (value == 1)
            {
                sql = " SELECT  sum(capacity) from tbl_tr_trips inner join tbl_tr_route on tbl_tr_route.Id= tbl_tr_trips.RouteId where  tbl_tr_trips.RouteId=" + RoutId + "";
            }
            else if (value == 0)
            {
                sql = " SELECT  tbl_tr_trips.Id,capacity,TripName from tbl_tr_trips inner join tbl_tr_route on tbl_tr_route.Id= tbl_tr_trips.RouteId where  tbl_tr_trips.RouteId=" + RoutId + "";
            }

            return m_MysqlDb.ExecuteQuery(sql);
        }

        public int getFloorOccupancy(int RoutId, int value)
        {
            int occupancy = 0;
            string sql = "";
            if (value == 1)
            {
                sql = "select sum(tbl_tr_trips.Occupied) from tbl_tr_trips where tbl_tr_trips.RouteId=" + RoutId + "";
            }
            else if (value == 0)
            {
                sql = "select tbl_tr_trips.Occupied from tbl_tr_trips where tbl_tr_trips.Id=" + RoutId + "";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(), out occupancy);
            }
            return occupancy;
        }

        public DataSet GetClassDetails()
        {
            DataSet Class_Ds = new DataSet();
            string sql = "";
            sql = "select Id,ClassName from tblclass where Status=1";
            Class_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            return Class_Ds;
        }

        public DataSet GetTripDetails(int ClassId, int DestinationId, string Sex, int status, int TripId,string routeid)
        {
            DataSet StudentTrip_Ds = new DataSet();
            string sql = "";
            //SELECT tblstudent.StudentName, tblstudent.Address, tblstudent.Sex, tblstudent.Id, 
            //tbl_tr_destinations.Destination,  tblclass.ClassName from tblstudent inner join tblclass 
            //on tblclass.Id= tblstudent.LastClassId inner join tbl_tr_studtripmap on tbl_tr_studtripmap.StudId=
            //    tblstudent.Id  INNER join tbl_tr_destinations on tbl_tr_studtripmap.DestinationId=tbl_tr_destinations.Id 
            //where tblstudent.`status`=1

            if (ClassId > 0)
            {
                sql = "SELECT tblstudent.StudentName, tblstudent.Address, tblstudent.Sex, tblstudent.Id,tbl_tr_destinations.Destination, tbl_tr_studtripmap.DestinationId,tblclass.ClassName from tblstudent inner join tblclass on tblclass.Id= tblstudent.LastClassId inner join tbl_tr_studtripmap on tbl_tr_studtripmap.StudId= tblstudent.Id INNER join tbl_tr_destinations on tbl_tr_studtripmap.DestinationId=tbl_tr_destinations.Id where tblstudent.LastClassId=" + ClassId + " and tblstudent.status=1 and tbl_tr_studtripmap.DestinationId in(select tbl_tr_routedestinations.DestinationId   from tbl_tr_routedestinations where tbl_tr_routedestinations.RouteId=" + routeid + ")";
            }
            else
            {
                sql = "SELECT tblstudent.StudentName, tblstudent.Address, tblstudent.Sex, tblstudent.Id,tbl_tr_destinations.Destination,tbl_tr_studtripmap.DestinationId,tblclass.ClassName from tblstudent inner join tblclass on tblclass.Id= tblstudent.LastClassId inner join tbl_tr_studtripmap on tbl_tr_studtripmap.StudId= tblstudent.Id INNER join tbl_tr_destinations on tbl_tr_studtripmap.DestinationId=tbl_tr_destinations.Id  where tblstudent.status=1 and tbl_tr_studtripmap.DestinationId in(select tbl_tr_routedestinations.DestinationId   from tbl_tr_routedestinations where tbl_tr_routedestinations.RouteId=" + routeid + ")";
            }
            if (Sex == "Male")
            {
                sql = sql + " and tblstudent.Sex='" + Sex + "'";
            }
            else if ((Sex == "Female"))
            {
                sql = sql + " and tblstudent.Sex='" + Sex + "'";
            }
            if (DestinationId > 0)
            {
                sql = sql + " and tbl_tr_studtripmap.DestinationId=" + DestinationId + "";
            }
            if (status == 0)
            {
                string _sql = "select tbl_tr_trips.RouteTypeId from tbl_tr_trips where tbl_tr_trips.Id=" + TripId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                if (m_MyReader.HasRows)
                {
                    if (int.Parse(m_MyReader.GetValue(0).ToString()) == 2)
                    {
                        sql = sql + " and tbl_tr_studtripmap.FromTripId=" + TripId + " ";
                    }
                    if (int.Parse(m_MyReader.GetValue(0).ToString()) == 3)
                    {
                        sql = sql + " and tbl_tr_studtripmap.ToTripId=" + TripId + " ";
                    }
                }
                //if(tripid==2)
                //{
                //and (tbl_tr_studtripmap.FromTripId!=0)
                //}
                //ELSE if(tripid==3)
                //{
                // and tbl_tr_studtripmap.ToTripId!=0)
                //}
               // sql = sql + " and (tbl_tr_studtripmap.FromTripId=" + TripId + " or tbl_tr_studtripmap.ToTripId=" + TripId + ")";
            }
            if (status == 1)
            {

                string _sql = "select tbl_tr_trips.RouteTypeId from tbl_tr_trips where tbl_tr_trips.Id=" + TripId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
                if (m_MyReader.HasRows)
                {
                    if (int.Parse(m_MyReader.GetValue(0).ToString()) == 2)
                    {
                        sql = sql + " and tbl_tr_studtripmap.FromTripId=0";
                    }
                    if (int.Parse(m_MyReader.GetValue(0).ToString()) == 3)
                    {
                        sql = sql + " and tbl_tr_studtripmap.ToTripId=0";
                    }
                }
                //sql = sql + " and (tbl_tr_studtripmap.FromTripId=0 and tbl_tr_studtripmap.ToTripId=0)";
            }
            StudentTrip_Ds = m_MysqlDb.ExecuteQueryReturnDataSet(sql);

            return StudentTrip_Ds;
        }

        public void SaveDataToTable(int TripId, int studentId, int destinationId, int routeId, out bool destination)
        {
            string _sql = "";
            string tempsql = "";           
            destination = false;
            OdbcDataReader Destinationreader = null;
            int newdestinationid = 0;
            _sql = "select tbl_tr_routedestinations.DestinationId from tbl_tr_routedestinations where tbl_tr_routedestinations.RouteId="+routeId+"";
            Destinationreader = m_MysqlDb.ExecuteQuery(_sql);
            if (Destinationreader.HasRows)
            {
                while (Destinationreader.Read())
                {
                    int.TryParse(Destinationreader.GetValue(0).ToString(), out newdestinationid);
                    if (newdestinationid == destinationId)
                    {
                        destination = true;
                        // tempsql = ",DestinationId=" + newdestinationid + "";
                    }

                }
            }
            if (destination)
            {
                //StudId,DestinationId,FromTripId,ToTripId
                string sql = "select tbl_tr_trips.RouteTypeId from tbl_tr_trips where Id=" + TripId + "";
                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    int routetypeId = int.Parse(m_MyReader.GetValue(0).ToString());

                    if (routetypeId == 1)
                    {
                        sql = " Update tbl_tr_studtripmap set FromTripId=" + TripId + ",ToTripId=" + TripId + " " + tempsql + " where  StudId=" + studentId + "";
                        m_MysqlDb.ExecuteQuery(sql);

                    }
                    else if (routetypeId == 2)
                    {
                        sql = " Update tbl_tr_studtripmap set FromTripId=" + TripId + " " + tempsql + " where  StudId=" + studentId + "";
                        m_MysqlDb.ExecuteQuery(sql);
                    }
                    else if (routetypeId == 3)
                    {
                        sql = " Update tbl_tr_studtripmap set ToTripId=" + TripId + " " + tempsql + " where StudId=" + studentId + "";
                        m_MysqlDb.ExecuteQuery(sql);
                    }


                }
            }
            else
            {

            }
        }

        public void RemoveDataFromTable(int TripId, int studentId, int destinationId)
        {
            string sql = "";
            sql = "Update tbl_tr_studtripmap set FromTripId=0,ToTripId=0 where DestinationId=" + destinationId + " and StudId=" + studentId + "";
            m_MysqlDb.ExecuteQuery(sql);
        }

        public DataSet GetScheduledStudentDetails(int LocationId, int batchId, int PeriodId,int classid, int currentbatchId)
        {
            DataSet StudentDetails_Ds = new DataSet();
            DataTable dt;
            DataRow _dr;
            StudentDetails_Ds.Tables.Add(new DataTable("StudentDetails"));
            dt = StudentDetails_Ds.Tables["StudentDetails"];
            dt.Columns.Add("Id");
            dt.Columns.Add("Destination");
            dt.Columns.Add("StudentName");
            dt.Columns.Add("ClassName");
            dt.Columns.Add("Sex");
            dt.Columns.Add("classid");
            dt.Columns.Add("StudentId");
            OdbcDataReader studreader = null;
            string sql = "";
            string _sql = "";
            if (LocationId > 0)
            {
                sql = "select tbl_tr_destinations.Destination, tbl_tr_studtripmap.StudId, tbl_tr_studtripmap.DestinationId from tbl_tr_destinations inner join tbl_tr_studtripmap  on tbl_tr_studtripmap.DestinationId= tbl_tr_destinations.Id where tbl_tr_studtripmap.DestinationId=" + LocationId + "";
            }
            else
            {
                sql = "select tbl_tr_destinations.Destination, tbl_tr_studtripmap.StudId, tbl_tr_studtripmap.DestinationId from tbl_tr_destinations inner join tbl_tr_studtripmap  on tbl_tr_studtripmap.DestinationId= tbl_tr_destinations.Id";
            }
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                while (m_MyReader.Read())
                {

                    _sql = " SELECT distinct tblfeestudent.Id from tblfeestudent INNER join tblfeeschedule on tblfeeschedule.Id= tblfeestudent.SchId where tblfeeschedule.FeeId=100 AND tblfeeschedule.PeriodId=" + PeriodId + " AND tblfeeschedule.BatchId=" + batchId + " AND tblfeestudent.StudId=" + int.Parse(m_MyReader.GetValue(1).ToString()) + "";
                    studreader = m_MysqlDb.ExecuteQuery(_sql);
                    if (studreader.HasRows)
                    {
                       
                        if (classid > 0)
                        {
                            _sql = "select tblstudent.Id, tblstudent.StudentName, tblclass.ClassName,tblclass.Id AS `classid`, tblstudent.Sex from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND  tblstudentclassmap.BatchId=" + currentbatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId where  tblstudent.Id=" + int.Parse(m_MyReader.GetValue(1).ToString()) + " and  tblstudentclassmap.ClassId=" + classid + "";
                        }
                        else
                        {
                            _sql = "select tblstudent.Id, tblstudent.StudentName, tblclass.ClassName,tblclass.Id AS `classid`, tblstudent.Sex from tblstudent inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id AND  tblstudentclassmap.BatchId=" + currentbatchId + "  INNER join tblclass on tblclass.Id= tblstudentclassmap.ClassId where  tblstudent.Id=" + int.Parse(m_MyReader.GetValue(1).ToString()) + "";
                        }
                        studreader = m_MysqlDb.ExecuteQuery(_sql);
                        if (studreader.HasRows)
                        {
                            _dr = StudentDetails_Ds.Tables["StudentDetails"].NewRow();
                            _dr["Id"] = m_MyReader.GetValue(2).ToString();
                            _dr["Destination"] = m_MyReader.GetValue(0).ToString();
                            _dr["StudentName"] = studreader.GetValue(1).ToString();
                            _dr["ClassName"] = studreader.GetValue(2).ToString();
                            _dr["Sex"] = studreader.GetValue(4).ToString();
                            _dr["classid"] = studreader.GetValue(3).ToString();
                            _dr["StudentId"] = studreader.GetValue(0).ToString();
                            StudentDetails_Ds.Tables["StudentDetails"].Rows.Add(_dr);
                        }
                    }
                }

            }
            return StudentDetails_Ds;
        }
        
        public bool HaveStudentInClass(int _ClassId, int _Batchid,DataSet AssociatedClass, out DataSet Class_Ds)
        {

            bool _valid = false; ;
            
            string sql = "";
            Class_Ds = new DataSet();
            if (_ClassId == 0)
            {
                DataTable dt;
                DataRow _dr;
                Class_Ds.Tables.Add(new DataTable("Class"));
                dt = Class_Ds.Tables["Class"];
                dt.Columns.Add("Id");
                if (AssociatedClass != null && AssociatedClass.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in AssociatedClass.Tables[0].Rows)
                    {
                        sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId=tblstudent.Id where tblstudent.Status=1  AND tblstudentclassmap.BatchId=" + _Batchid + " and tblstudentclassmap.ClassId=" + int.Parse(dr["Id"].ToString()) + "";
                        m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                        if (m_MyReader.HasRows)
                        {
                            _dr = Class_Ds.Tables["Class"].NewRow();
                            _dr["Id"] = dr["Id"].ToString();
                            Class_Ds.Tables["Class"].Rows.Add(_dr);
                        }
                    }
                }
                if (Class_Ds != null && Class_Ds.Tables[0].Rows.Count>0)
                {
                    _valid = true;
                }
            }
            else
            {
                sql = "SELECT tblstudentclassmap.StudentId FROM tblstudentclassmap inner join tblstudent on tblstudentclassmap.StudentId=tblstudent.Id where tblstudent.Status=1  AND tblstudentclassmap.BatchId=" + _Batchid + " and tblstudentclassmap.ClassId=" + _ClassId;

                m_MyReader = m_MysqlDb.ExecuteQuery(sql);
                if (m_MyReader.HasRows)
                {
                    _valid = true;
                }               
                m_MyReader.Close();
            }
           
            return _valid;
        }


        public void EditFee_Intransaportation(int classid, int StudID, int periodId, int batchId, double feeamount)
        {
            string sql = "", tempsql = "";
            //FeeId,BatchId,Duedate,LastDate,Status,ClassId,PeriodId,Amount
            sql = "select tblfeeschedule.Id from  tblfeeschedule where  tblfeeschedule.FeeId=100 and BatchId=" + batchId + " and ClassId=" + classid + " and PeriodId=" + periodId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                //Id,SchId,StudId,Amount,BalanceAmount,Status
                if (feeamount == 0)
                {
                    tempsql = " ,tblfeestudent.Status='fee Exemtion'";

                }
                sql = "Update tblfeestudent set Amount=" + feeamount + ",BalanceAmount=" + feeamount + " " + tempsql + " where StudId=" + StudID + " and SchId=" + int.Parse(m_MyReader.GetValue(0).ToString()) + " ";
                m_MysqlDb.ExecuteQuery(sql);
            }
            
        }

        public void UpdateTripOccupancy(int TripId,int count,int value)
        {
            string sql = "";
            int occupied = 0;
            int capacity = 0;
            string _sql = "select capacity,Occupied from tbl_tr_trips where tbl_tr_trips.Id="+TripId+"";
            m_MyReader = m_MysqlDb.ExecuteQuery(_sql);
            if (m_MyReader.HasRows)
            {
                occupied = int.Parse(m_MyReader.GetValue(1).ToString());
                capacity = int.Parse(m_MyReader.GetValue(0).ToString());
                if (value == 1)
                {
                    sql = "Update tbl_tr_trips set Occupied=" + (occupied + count) + " where tbl_tr_trips.Id=" + TripId + " ";
                }
                else if (value == 0)
                {
                    sql = "Update tbl_tr_trips set Occupied=" + (occupied - count) + " where tbl_tr_trips.Id=" + TripId + " ";
                }
                m_MysqlDb.ExecuteQuery(sql);
            }
        }

        public DataSet getDestinationsUnderRoute(int routId)
        {
            string sql = "select Destination, id from tbl_tr_destinations where tbl_tr_destinations.Id in(select  tbl_tr_routedestinations.DestinationId from tbl_tr_routedestinations where tbl_tr_routedestinations.RouteId="+routId+")";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }

        public int GetMaxCapacityOfTrip(int TripId, int routId)
        {
            int MaxCapacity = 0;
            string sql = "";
            sql = "select tbl_tr_trips.capacity from tbl_tr_trips WHERE tbl_tr_trips.Id="+TripId+" and tbl_tr_trips.RouteId="+routId+"";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                int.TryParse(m_MyReader.GetValue(0).ToString(),out  MaxCapacity);
            }
            return MaxCapacity;
        }

        public  DateTime Parse(string _StrDate, string _Time)
        {
            DateTime _outDate;
            string[] _DateArray = _StrDate.Split('/');// store DD MM YYYY
            int _Day, _Month, _Year, _Hour, _Minute;
            string _AP_PM;
            _Day = int.Parse(_DateArray[0]);// day
            _Month = int.Parse(_DateArray[1]);// Month
            _Year = int.Parse(_DateArray[2]);// Year
            string[] _TimeArray = _Time.Split(':');// store hh:mm AM/PM
            _Hour = int.Parse(_TimeArray[0]);// hour
            _TimeArray = _TimeArray[1].Split(' ');
            _Minute = int.Parse(_TimeArray[0]);
            _AP_PM = _TimeArray[1].Trim();
            if (_AP_PM == "PM" && _Hour != 12)
                _Hour = _Hour + 12;
            else if (_AP_PM == "AM" && _Hour == 12)
                _Hour = 0;
            _outDate = new DateTime(_Year, _Month, _Day, _Hour, _Minute, 0);
            return _outDate;
        }

        public void UpdateStudFeeSchedule_InTransporation(DateTime _duedate, DateTime _lastdate, int classid, int periodId, int batchId)
        {
            string sql = "";
            sql = "select tblfeeschedule.Id from tblfeeschedule where  tblfeeschedule.FeeId=100 and tblfeeschedule.PeriodId=" + periodId + " and tblfeeschedule.ClassId=" + classid + "  and tblfeeschedule.BatchId=" + batchId + "";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if(m_MyReader.HasRows)
            {
                int svheduleid = int.Parse(m_MyReader.GetValue(0).ToString());
                sql = "UPDATE tblfeeschedule SET Duedate= '" + _duedate.Date.ToString("s") + "', LastDate = '" + _lastdate.Date.ToString("s") + "' WHERE Id =" + svheduleid;
                m_MysqlDb.ExecuteQuery(sql);
                m_MyReader.Close();
            }
        }

        public string Getstudname(int StudID)
        {
            string name = "";
            string sql = "";
            sql = "select tblstudent.StudentName from tblstudent WHERE tblstudent.Id="+StudID+" and tblstudent.Status=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                name = m_MyReader.GetValue(0).ToString();
            }
            return name;
        }

        public string Getclassname(int classid)
        {
            string name = "";
            string sql = "";
            sql = "select tblclass.ClassName from tblclass WHERE tblclass.Id="+classid+" and tblclass.Status=1";
            m_MyReader = m_MysqlDb.ExecuteQuery(sql);
            if (m_MyReader.HasRows)
            {
                name = m_MyReader.GetValue(0).ToString();
            }
            return name;
        }        

        public DataSet GetTripMapDetails(int classid, int destinationid)
        {
            string sql = "",tempsql="",innersql="";
            DataSet TripMapDs = new DataSet();          
          
            if (destinationid > 0)
            {
                tempsql = tempsql + " and tbl_tr_studtripmap.DestinationId=" + destinationid + "";
            }

            sql = "select tblstudent.StudentName, tbl_tr_destinations.Destination, tbl_tr_studtripmap.ToTripId, tbl_tr_studtripmap.FromTripId from tblstudent inner join tbl_tr_studtripmap on tbl_tr_studtripmap.StudId= tblstudent.Id INNER join tbl_tr_destinations on tbl_tr_destinations.Id= tbl_tr_studtripmap.DestinationId  where tblstudent.LastClassId=" + classid + " " + tempsql + "";
            TripMapDs = m_MysqlDb.ExecuteQueryReturnDataSet(sql);
            if (TripMapDs != null && TripMapDs.Tables[0].Rows.Count > 0)
            {
                TripMapDs.Tables[0].Columns.Add("FromtripName");
                TripMapDs.Tables[0].Columns.Add("ToTripName");
                TripMapDs.Tables[0].Columns.Add("RouteName");
                OdbcDataReader fromtripreader = null, totripreader = null;

                foreach (DataRow _dr in TripMapDs.Tables[0].Rows)
                {
                    innersql = "";
                    innersql = "   select tbl_tr_trips.TripName, tbl_tr_route.RouteName from tbl_tr_trips  inner join tbl_tr_route on tbl_tr_trips.RouteId= tbl_tr_route.Id  where tbl_tr_trips.Id=" + _dr["FromTripId"].ToString() + " ";
                    fromtripreader = m_MysqlDb.ExecuteQuery(innersql);
                    if (fromtripreader.HasRows)
                    {
                        _dr["FromtripName"] = fromtripreader.GetValue(0).ToString();
                        _dr["RouteName"] = fromtripreader.GetValue(1).ToString();
                        
                    }
                    else
                    {
                        _dr["FromtripName"] = "";
                        _dr["RouteName"] = "";
                        

                    }
                    fromtripreader.Close();
                    innersql = "";
                    if (_dr["RouteName"].ToString() == "")
                    {
                        innersql = "   select tbl_tr_trips.TripName, tbl_tr_route.RouteName from tbl_tr_trips  inner join tbl_tr_route on tbl_tr_trips.RouteId= tbl_tr_route.Id  where tbl_tr_trips.Id=" + _dr["ToTripId"].ToString() + " ";
                        totripreader = m_MysqlDb.ExecuteQuery(innersql);
                        if (totripreader.HasRows)
                        {
                            _dr["ToTripName"] = totripreader.GetValue(0).ToString();
                            _dr["RouteName"] = totripreader.GetValue(1).ToString();
                        }
                        else
                        {
                            _dr["ToTripName"] = "";
                            _dr["RouteName"] = "";
                        }
                    }
                    else
                    {
                        innersql = "   select tbl_tr_trips.TripName from tbl_tr_trips where tbl_tr_trips.Id=" + _dr["ToTripId"].ToString() + " ";
                        totripreader = m_MysqlDb.ExecuteQuery(innersql);
                        if (totripreader.HasRows)
                        {
                            _dr["ToTripName"] = totripreader.GetValue(0).ToString();
                        }
                        else
                        {
                            _dr["ToTripName"] = "";
                        }
                    }
                    
                    totripreader.Close();
                }
            }
            return TripMapDs;
        }

        public DataSet GetTransportationFeeReport(int _periodid, int _classid, int _batchid)
        {
          string sql = "",tempsql="";
          if (_classid > 0)
          {
              tempsql = " and  tblstudent.LastClassId=" + _classid + " ";
          }
          sql = "select tblstudent.StudentName, tblfeestudent.StudId, tblfeestudent.Amount- tblfeestudent.BalanceAmount as Amount, tblfeestudent.BalanceAmount, tblfeestudent.`Status`, tblperiod.Period, tblclass.ClassName   from tblstudent inner join tblfeestudent on tblfeestudent.StudId= tblstudent.Id inner join tblfeeschedule on tblfeestudent.SchId= tblfeeschedule.Id inner join tblstudentclassmap on tblstudentclassmap.StudentId= tblstudent.Id inner join tblperiod on tblperiod.Id= tblfeeschedule.PeriodId inner join tblclass on tblfeeschedule.ClassId= tblclass.Id  where tblfeeschedule.FeeId=100 and tblfeeschedule.PeriodId=" + _periodid + "  and tblstudent.UseBus=1  and tblstudentclassmap.BatchId=" + _batchid + " " + tempsql + "";
            return m_MysqlDb.ExecuteQueryReturnDataSet(sql);
        }
    }
}
