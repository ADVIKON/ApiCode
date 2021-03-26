using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.ServiceModel.Web;
using EuforyServices.DataContract;
using EuforyServices.ServiceContract;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Configuration;
using System.Globalization;
using System.IO;
using System.Web.Http.Cors;
using System.Web.Hosting;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.ServiceModel;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Stripe;
using System.Net.Mail;
using System.Data.OleDb;
using RestSharp;
using System.Collections;

namespace EuforyServices.ServiceImplementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EuforyServices : IEuforyServices
    {

        //Genre Web Services
        #region Genre
        public List<ResponceGenre> GetAllGenre()
        {
            List<ResponceGenre> result = new List<ResponceGenre>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {
                //and titlesubcategoryId in(35,7,9,10,36,40)
                SqlCommand cmd = new SqlCommand("select top 10 titleSubcategoryId,titleSubcategoryName, titlecategoryId from tblTitleSubCategory where titlecategoryId =1 and titlesubcategoryId in(40,15,11,35,10,14,7,36,9,13) order by TitleSubCategoryName", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceGenre()
                    {
                        SubcategoryId = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        SubCategoryName = ds.Tables[0].Rows[i][1].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                return result;
            }
        }


        #endregion

        //Genre Title Web Services
        #region Genre
        public List<ResponceGenreTitles> GetGenreTitles(DataGenreTitles data)
        {
            List<ResponceGenreTitles> result = new List<ResponceGenreTitles>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("select top 50 titles.TitleID,ltrim(titles.Title) as Title,titles.Time, ltrim(Artists.Name) as ArtistName from titles inner join Artists on titles.ArtistID=  Artists.ArtistID where title <>'' and TitleSubCategoryId= '" + data.SubcategoryId + "' order by titles.Title", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceGenreTitles()
                    {
                        TitleId = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        Title = ds.Tables[0].Rows[i][1].ToString(),
                        Time = ds.Tables[0].Rows[i][2].ToString(),
                        ArtistName = ds.Tables[0].Rows[i][3].ToString(),
                        SongUrl = "http://85.195.82.94/oggfiles/" + ds.Tables[0].Rows[i][0].ToString() + ".ogg",
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion

        #region Stream
        public List<ResponceStream> GetOnlineStream(DataStream data)
        {
            List<ResponceStream> result = new List<ResponceStream>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {
                string str = "";
                if (string.IsNullOrEmpty(data.OwnerCustomerId) == false)
                {
                    str = "Select streamid, streamnameApp,StreamLinkApp,imgpath from  tblOnlineStreaming_App where dfclientid=" + data.OwnerCustomerId + " order by streamnameApp";
                }
                else
                {
                    str = "select os.streamid, os.streamnameApp,os.StreamLinkApp,os.imgpath from tbAssignMobileStreamToken ams";
                    str = str + " inner join tblOnlineStreaming_App os on os.streamid = ams.streamid ";
                    str = str + " where ams.tokenid=" + data.TokenId;
                    str = str + " order by os.streamnameApp";
                }
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceStream()
                    {
                        StreamId = ds.Tables[0].Rows[i]["streamid"].ToString(),
                        StreamName = ds.Tables[0].Rows[i]["streamnameApp"].ToString(),
                        StreamLink = ds.Tables[0].Rows[i]["StreamLinkApp"].ToString(),
                        StreamImgPath = ds.Tables[0].Rows[i]["imgpath"].ToString(),
                        check = false,
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                return result;
            }
        }


        #endregion


        #region CheckUserLogin
        public List<ResponceUserToen> CheckUserLogin(DataClientToken data)
        {
            List<ResponceUserToen> result = new List<ResponceUserToen>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("spGetTokenRights_Android '" + data.UserName + "', '" + data.TokenNo + "' , '" + data.DeviceId + "','" + data.PlayerType + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceUserToen()
                    {
                        Response = "0",
                    });
                    return result;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceUserToen()
                    {
                        Response = "1",
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion

        #region CheckUserRights
        public List<ResponceUserRights> CheckUserRights(DataUserRights data)
        {
            List<ResponceUserRights> result = new List<ResponceUserRights>();

            try
            {
                SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);
                SqlCommand cmd = new SqlCommand("spGetTokenExpiryStatus_Android_Dam '" + data.DeviceId + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceUserRights()
                    {
                        Response = "0",
                        LeftDays = "0",
                        TokenId = "0",
                    });
                    return result;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceUserRights()
                    {
                        Response = ds.Tables[0].Rows[0][0].ToString(),
                        LeftDays = ds.Tables[0].Rows[0][1].ToString(),
                        TokenId = ds.Tables[0].Rows[0][2].ToString(),

                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion

        #region MiddleImage
        public List<ResponceMiddleImage> GetMiddleImage(DataStream data)
        {
            List<ResponceMiddleImage> result = new List<ResponceMiddleImage>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {

                SqlCommand cmd = new SqlCommand("Select imgpath from  tblMiddleImage_App where tokenid= " + data.TokenId, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceMiddleImage()
                    {
                        ImgPath = ds.Tables[0].Rows[i][0].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                return result;
            }
        }


        #endregion



        #region GetSplPlaylist
        public List<ResponceSplSplaylist> GetSplPlaylist(DataSplPlaylist data)
        {
            List<ResponceSplSplaylist> result = new List<ResponceSplSplaylist>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetSpecialPlaylistSchedule_Mobile " + data.WeekNo + "," + data.TokenId + "," + data.DfClientId + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceSplSplaylist()
                    {
                        pScid = Convert.ToInt32(ds.Tables[0].Rows[i]["pSchid"]),
                        dfclientid = Convert.ToInt32(ds.Tables[0].Rows[i]["dfClientId"]),
                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        splPlaylistName = ds.Tables[0].Rows[i]["splPlaylistName"].ToString(),
                        StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString(),
                        EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString(),
                        FormatId = Convert.ToInt32(ds.Tables[0].Rows[i]["FormatId"]),

                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region GetSplPlaylistTitles
        public List<ResponceSplSplaylistTitle> GetSplPlaylistTitles(DataSplPlaylistTile data)
        {
            List<ResponceSplSplaylistTitle> result = new List<ResponceSplSplaylistTitle>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetSpecialPlaylists_Titles " + data.splPlaylistId + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                string url = "";
                string mtypeFormat = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Audio")
                    {
                        mtypeFormat = ".mp3";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Video")
                    {
                        mtypeFormat = ".mp4";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Image")
                    {
                        mtypeFormat = ".jpg";
                    }


                    url = "http://api.advikon.com/mp3files/" + ds.Tables[0].Rows[i]["titleId"].ToString() + mtypeFormat;
                    result.Add(new ResponceSplSplaylistTitle()
                    {
                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        titleId = Convert.ToInt32(ds.Tables[0].Rows[i]["titleId"]),
                        Title = ds.Tables[0].Rows[i]["Title"].ToString(),
                        tTime = ds.Tables[0].Rows[i]["Time"].ToString(),
                        ArtistID = Convert.ToInt32(ds.Tables[0].Rows[i]["ArtistID"]),
                        arName = ds.Tables[0].Rows[i]["arName"].ToString(),
                        AlbumID = Convert.ToInt32(ds.Tables[0].Rows[i]["AlbumID"]),
                        alName = ds.Tables[0].Rows[i]["alName"].ToString(),
                        TitleUrl = url,
                        FileSize = ds.Tables[0].Rows[i]["filesize"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion








        #region GetSplPlaylist Live
        public List<ResponceSplSplaylist> GetSplPlaylistLive(DataSplPlaylist data)
        {
            List<ResponceSplSplaylist> result = new List<ResponceSplSplaylist>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetSpecialPlaylistSchedule " + data.WeekNo + "," + data.TokenId + "," + data.DfClientId + " ", con);

                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                int isSepration = 0;
                int isSepration_old = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToBoolean(ds.Tables[0].Rows[i]["isShowDefault"]) == true)
                    {
                        isSepration = 0;
                    }
                    else
                    {
                        isSepration = 1;
                    }
                    if (Convert.ToBoolean(ds.Tables[0].Rows[i]["isShowDefault"]) == true)
                    {
                        isSepration_old = 1;

                    }
                    else
                    {
                        isSepration_old = 0;

                    }
                    result.Add(new ResponceSplSplaylist()
                    {
                        pScid = Convert.ToInt32(ds.Tables[0].Rows[i]["pSchid"]),
                        dfclientid = Convert.ToInt32(ds.Tables[0].Rows[i]["dfClientId"]),
                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        splPlaylistName = ds.Tables[0].Rows[i]["splPlaylistName"].ToString(),
                        StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString(),
                        EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString(),
                        IsSeprationActive = isSepration_old,
                        IsSeprationActive_New = isSepration,
                        IsFadingActive = Convert.ToInt32(ds.Tables[0].Rows[i]["IsFadingActive"]),
                        FormatId = 0,
                        IsMute = ds.Tables[0].Rows[i]["IsMute"].ToString(),
                        VolumeLevel = ds.Tables[0].Rows[i]["VolumeLevel"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region GetSplPlaylistTitles Live
        public List<ResponceSplSplaylistTitle> GetSplPlaylistTitlesLive(DataSplPlaylistTile data)
        {
            List<ResponceSplSplaylistTitle> result = new List<ResponceSplSplaylistTitle>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string mtypeFormat = "";
                SqlCommand cmd = new SqlCommand("GetSpecialPlaylists_Titles " + data.splPlaylistId + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                string url = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Audio")
                    {
                        mtypeFormat = ".mp3";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Video")
                    {
                        mtypeFormat = ".mp4";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Image")
                    {
                        mtypeFormat = ".jpg";
                    }

                    url = "http://api.advikon.com/mp3files/" + ds.Tables[0].Rows[i]["titleId"].ToString() + mtypeFormat;

                    result.Add(new ResponceSplSplaylistTitle()
                    {

                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        titleId = Convert.ToInt32(ds.Tables[0].Rows[i]["titleId"]),
                        Title = ds.Tables[0].Rows[i]["Title"].ToString(),
                        tTime = ds.Tables[0].Rows[i]["Time"].ToString(),
                        ArtistID = Convert.ToInt32(ds.Tables[0].Rows[i]["ArtistID"]),
                        arName = ds.Tables[0].Rows[i]["arName"].ToString(),
                        AlbumID = Convert.ToInt32(ds.Tables[0].Rows[i]["AlbumID"]),
                        alName = ds.Tables[0].Rows[i]["alName"].ToString(),
                        srno = Convert.ToInt32(ds.Tables[0].Rows[i]["srno"]),
                        TitleUrl = url,
                        TitleUrl2 = url,
                        FileSize = ds.Tables[0].Rows[i]["filesize"].ToString(),
                        TimeInterval = Convert.ToInt32(ds.Tables[0].Rows[i]["imgInterval"]),
                        IsLoop = false,
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region CheckUserLogin Live
        public List<ResponceUserToen> CheckUserLoginLive(DataClientToken data)
        {
            List<ResponceUserToen> result = new List<ResponceUserToen>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string lType = "";
                if (data.PlayerType == "LinuxPI")
                {
                    lType = "Linux";
                }
                else
                {
                    lType = data.PlayerType;
                }
                SqlCommand cmd = new SqlCommand("spGetTokenRights_Mobile '" + data.UserName + "', '" + data.TokenNo + "' , '" + data.DeviceId + "','" + lType + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceUserToen()
                    {
                        Response = "0",
                    });
                    return result;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceUserToen()
                    {
                        Response = "1",
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion



        #region CheckUserRights Live
        public List<ResponceUserRights> CheckUserRightsLive(DataUserRights data)
        {
            List<ResponceUserRights> result = new List<ResponceUserRights>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            try
            {
                string mType = "";
                SqlCommand cmd = new SqlCommand("spGetTokenExpiryStatus_Mobile '" + data.DeviceId + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceUserRights()
                    {
                        Response = "0",
                        LeftDays = "0",
                        TokenId = "0",
                    });
                    con.Close();
                    return result;
                }
                if (Convert.ToInt32(ds.Tables[0].Rows[0][8]) == 1)
                {
                    mType = "Video";
                }
                else
                {
                    mType = "Audio";
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    result.Add(new ResponceUserRights()
                    {
                        Response = ds.Tables[0].Rows[0][0].ToString(),
                        LeftDays = ds.Tables[0].Rows[0]["LeftDays"].ToString(),
                        TokenId = ds.Tables[0].Rows[0]["tokenid"].ToString(),
                        dfClientId = ds.Tables[0].Rows[0]["dfClientId"].ToString(),
                        CountryId = Convert.ToInt32(ds.Tables[0].Rows[0]["countryId"]),
                        StateId = Convert.ToInt32(ds.Tables[0].Rows[0]["stateId"]),
                        Cityid = Convert.ToInt32(ds.Tables[0].Rows[0]["cityId"]),
                        IsStopControl = Convert.ToInt32(ds.Tables[0].Rows[0]["IsStopControl"]),
                        MediaType = mType,
                        FcmId = ds.Tables[0].Rows[0]["FcmID"].ToString(),
                        scheduleType = ds.Tables[0].Rows[0]["scheduleType"].ToString(),
                        LogoId = ds.Tables[0].Rows[i]["AppLogoId"].ToString(),
                        IsIndicatorActive = ds.Tables[0].Rows[0]["IsIndicatorActive"].ToString(),
                        Rotation = ds.Tables[0].Rows[0]["Rotation"].ToString(),
                        IsDemoToken = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDemoToken"]),
                        TotalShot = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalShot"]),
                        DeviceType = ds.Tables[0].Rows[0]["DeviceType"].ToString(),
                        RebootTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[0]["RebootTime"]),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion




        #region PrayerTiming Live
        public List<ResponcePrayerTiming> PrayerTiming(DataPrayerTiming data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            string str = "";
            List<ResponcePrayerTiming> result = new List<ResponcePrayerTiming>();

            try
            {
                SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
                str = "spGetPrayerData " + data.Month + "," + data.Cityid + "," + data.CountryId + "," + data.StateId + "," + data.TokenId + "";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponcePrayerTiming()
                    {
                        Response = "0",
                    });
                    return result;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponcePrayerTiming()
                    {
                        Response = "1",
                        pId = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        sDate = string.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[i][1]),
                        eDate = string.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[i][2]),
                        sTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i][3]),
                        eTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i][4]),
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region Advt Live
        public List<ResponceAdvt> AdvtSchedule(DataAdvtSch data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            string str = "";
            List<ResponceAdvt> result = new List<ResponceAdvt>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                str = "spGetAdvtAdmin_NativeOnly '" + data.CurrentDate + "','NativeCR'," + data.DfClientId + "," + data.WeekNo + "," + data.CityId + "," + data.DfClientId + "," + data.CountryId + "," + data.StateId + "," + data.TokenId + "";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceAdvt()
                    {
                        Response = "0",
                    });
                    con.Close();
                    return result;
                }


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceAdvt()
                    {
                        Response = "1",
                        AdvtId = Convert.ToInt32(ds.Tables[0].Rows[i]["AdvtId"]),
                        AdvtName = ds.Tables[0].Rows[i]["AdvtDisplayName"].ToString(),
                        PlayingType = ds.Tables[0].Rows[i]["playingType"].ToString(),
                        sDate = string.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[i]["AdvtStartDate"]),
                        eDate = string.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[i]["AdvtEndDate"]),
                        AdvtFilePath = ds.Tables[0].Rows[i]["AdvtFilePath"].ToString(),

                        IsTime = Convert.ToByte(ds.Tables[0].Rows[i]["IsTime"]),
                        sTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i]["AdvtTime"]),

                        IsMinute = Convert.ToByte(ds.Tables[0].Rows[i]["IsMinute"]),
                        TotalMinutes = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMinutes"]),
                        IsSong = Convert.ToByte(ds.Tables[0].Rows[i]["IsSong"]),
                        TotalSongs = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalSongs"]),
                        SrNo = Convert.ToInt32(ds.Tables[0].Rows[i]["srNo"]),

                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region Advt Linux Live Only
        public List<ResponceAdvtLinux> AdvtScheduleLinuxOnly(DataAdvtSchLinux data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            string str = "";
            List<ResponceAdvtLinux> result = new List<ResponceAdvtLinux>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                str = "spGetAdvtAdmin_LinuxOnly '" + data.MonthNo + "','NativeCR'," + data.DfClientId + "," + data.WeekNo + "," + data.CityId + "," + data.DfClientId + "," + data.CountryId + "," + data.StateId + "," + data.TokenId + "";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceAdvtLinux()
                    {
                        Response = "0",
                    });
                    con.Close();
                    return result;
                }


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceAdvtLinux()
                    {
                        Response = "1",
                        AdvtId = Convert.ToInt32(ds.Tables[0].Rows[i]["AdvtId"]),
                        AdvtName = ds.Tables[0].Rows[i]["AdvtDisplayName"].ToString(),
                        PlayingType = ds.Tables[0].Rows[i]["playingType"].ToString(),
                        AdvtFilePath = ds.Tables[0].Rows[i]["AdvtFilePath"].ToString(),

                        IsTime = Convert.ToByte(ds.Tables[0].Rows[i]["IsTime"]),
                        sTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i]["AdvtTime"]),

                        IsMinute = Convert.ToByte(ds.Tables[0].Rows[i]["IsMinute"]),
                        TotalMinutes = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMinutes"]),
                        IsSong = Convert.ToByte(ds.Tables[0].Rows[i]["IsSong"]),
                        TotalSongs = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalSongs"]),
                        SrNo = Convert.ToInt32(ds.Tables[0].Rows[i]["srNo"]),

                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion



        #region Token Played Songs Status
        public List<ResponcePlayedSong> PlayedSongsStatus(DataPlayedSong data)
        {
            List<ResponcePlayedSong> result = new List<ResponcePlayedSong>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string lType = "";
                SqlCommand cmd = new SqlCommand("spSaveTokenPlayedSongs_Status " + data.TokenId + ", '" + data.PlayedDateTime + "' , " + data.TitleId + "," + data.ArtistId + "," + data.splPlaylistId + "", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                result.Add(new ResponcePlayedSong()
                {
                    Response = "1",
                });
                con.Close();
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayedSong()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region Played Advertisement Status
        public List<ResponcePlayedAdvt> PlayedAdvertisementStatus(DataPlayedAdvt data)
        {
            List<ResponcePlayedAdvt> result = new List<ResponcePlayedAdvt>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string lType = "";
                SqlCommand cmd = new SqlCommand("spTokenAdvt_Status " + data.TokenId + ", " + data.AdvtId + ",'" + data.PlayedDate + "','" + data.PlayedTime + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                result.Add(new ResponcePlayedAdvt()
                {
                    Response = "1",
                });
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                result.Add(new ResponcePlayedAdvt()
                {
                    Response = "0",
                });
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region Played Prayer Status
        public List<ResponcePlayedPrayer> PlayedPrayerStatus(DataPlayedPrayer data)
        {
            List<ResponcePlayedPrayer> result = new List<ResponcePlayedPrayer>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string lType = "";
                SqlCommand cmd = new SqlCommand("spTokenPrayer_Status " + data.TokenId + ",'" + data.PlayedDate + "','" + data.PlayedTime + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                result.Add(new ResponcePlayedPrayer()
                {
                    Response = "1",
                });
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayedPrayer()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion



        #region Player Login Status
        public List<ResponcePlayerLogin> PlayerLoginStatus(DataPlayerLogin data)
        {
            List<ResponcePlayerLogin> result = new List<ResponcePlayerLogin>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string lType = "";
                SqlCommand cmd = new SqlCommand("spTokenLogin_Status " + data.TokenId + ",'" + data.LoginDate + "','" + data.LoginTime + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                result.Add(new ResponcePlayerLogin()
                {
                    Response = "1",
                });
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayerLogin()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion

        #region Player Logout Status
        public List<ResponcePlayerLogout> PlayerLogoutStatus(DataPlayerLogout data)
        {
            List<ResponcePlayerLogout> result = new List<ResponcePlayerLogout>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {

                SqlCommand cmd = new SqlCommand("spTokenLogOut_Status " + data.TokenId + ",'" + data.LogoutDate + "','" + data.LogoutTime + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                result.Add(new ResponcePlayerLogout()
                {
                    Response = "1",
                });
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayerLogout()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region Player Heart Beat Status
        public List<ResponcePlayerHeart> PlayerHeartBeatStatus(DataPlayerHeart data)
        {
            List<ResponcePlayerHeart> result = new List<ResponcePlayerHeart>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {

                SqlCommand cmd = new SqlCommand("spTokenOverDue_Status " + data.TokenId + ",'" + data.HeartbeatDateTime + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                result.Add(new ResponcePlayerHeart()
                {
                    Response = "1",
                });
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayerHeart()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion



        #region JSON Functions

        #region Token Played Songs Status Stream
        public List<ResponcePlayedSong> PlayedSongsStatusStream(List<DataPlayedSong> data)
        {
            string rSave = "0";
            rSave = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.GetDirectoryName(rSave) + "\\data.txt";
            string WriteData = "";
            DateTime custDateTime = DateTime.Now;

            List<ResponcePlayedSong> result = new List<ResponcePlayedSong>();
            List<SongsArray> resultSong = new List<SongsArray>();
            SqlConnection conMain = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";

                DataTable dtInsert = new DataTable();
                dtInsert.Columns.Add("TokenId", typeof(int));
                dtInsert.Columns.Add("PlayDTP", typeof(DateTime));
                dtInsert.Columns.Add("TitleId", typeof(int));
                dtInsert.Columns.Add("ArtistId", typeof(int));
                dtInsert.Columns.Add("splPlaylistId", typeof(int));
                dtInsert.Columns.Add("playdate", typeof(DateTime));

                foreach (var Player in data)
                {
                    //if (Player.TokenId.ToString() == "1894")
                    //{
                    //    WriteData = "" + Player.TokenId + ", " + Player.PlayedDateTime + " , " + Player.TitleId + "," + Player.ArtistId + "," + Player.splPlaylistId + ", {0} ";
                    //    using (StreamWriter writer = new StreamWriter(path, true))
                    //    {
                    //        writer.WriteLine(string.Format(WriteData, custDateTime.ToString("dd/MMM/yyyy hh:mm:ss tt")));
                    //        writer.Close();
                    //    }
                    //}




                    if (Player.TokenId != 0)
                    {
                        DataRow nr = dtInsert.NewRow();
                        var k = string.Format(fi, "{0:HH:mm:ss}", Convert.ToDateTime(Player.PlayedDateTime));
                        nr["TokenId"] = Player.TokenId;
                        nr["PlayDTP"] = "01-01-1900 " + k;
                        nr["TitleId"] = Player.TitleId;
                        nr["ArtistId"] = Player.ArtistId;
                        nr["splPlaylistId"] = Player.splPlaylistId;
                        nr["playdate"] = string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Player.PlayedDateTime));
                        dtInsert.Rows.Add(nr);
                        //SqlCommand cmd = new SqlCommand("spSaveTokenPlayedSongs_Status " + Player.TokenId + ", '" + Player.PlayedDateTime + "' , " + Player.TitleId + "," + Player.ArtistId + "," + Player.splPlaylistId + ",'" + Player.PlayedDateTime + "'", conMain);
                        //cmd.CommandType = System.Data.CommandType.Text;
                        //if (conMain.State == ConnectionState.Closed)
                        //{
                        //    conMain.Open();
                        //}
                        //cmd.ExecuteNonQuery();
                    }
                    resultSong.Add(new SongsArray()
                    {
                        Response = "1",
                        returnPlayedDateTime = Player.PlayedDateTime,
                        returnTitleId = Player.TitleId.ToString()
                    });
                }
                if (dtInsert.Rows.Count > 0)
                {

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conMain))
                    {

                        SqlBulkCopyColumnMapping TokenId =
                           new SqlBulkCopyColumnMapping("TokenId", "TokenId");
                        bulkCopy.ColumnMappings.Add(TokenId);
                        SqlBulkCopyColumnMapping PlayDTP =
                           new SqlBulkCopyColumnMapping("PlayDTP", "PlayDTP");
                        bulkCopy.ColumnMappings.Add(PlayDTP);
                        SqlBulkCopyColumnMapping TitleId =
                           new SqlBulkCopyColumnMapping("TitleId", "TitleId");
                        bulkCopy.ColumnMappings.Add(TitleId);
                        SqlBulkCopyColumnMapping ArtistId =
                           new SqlBulkCopyColumnMapping("ArtistId", "ArtistId");
                        bulkCopy.ColumnMappings.Add(ArtistId);
                        SqlBulkCopyColumnMapping splPlaylistId =
                         new SqlBulkCopyColumnMapping("splPlaylistId", "splPlaylistId");
                        bulkCopy.ColumnMappings.Add(splPlaylistId);
                        SqlBulkCopyColumnMapping playdate =
                           new SqlBulkCopyColumnMapping("playdate", "playdate");
                        bulkCopy.ColumnMappings.Add(playdate);
                        bulkCopy.DestinationTableName = "dbo.tbTokenPlayedSongs_Live";
                        if (conMain.State == ConnectionState.Closed)
                        {
                            conMain.Open();
                        }
                        bulkCopy.WriteToServer(dtInsert);

                    }
                }
                result.Add(new ResponcePlayedSong()
                {
                    Response = "1",
                    SongArray = resultSong
                });

                //}
                conMain.Close();
                return result;

            }
            catch (Exception ex)
            {
                WriteData = ex.ToString();
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(string.Format(WriteData, custDateTime.ToString("dd/MMM/yyyy hh:mm:ss tt")));
                    writer.Close();
                }
                result.Add(new ResponcePlayedSong()
                {
                    Response = "0",
                    SongArray = new List<SongsArray>
                        {
                           new SongsArray { Response = "0" },
                           new SongsArray {  returnPlayedDateTime ="0"},
                           new SongsArray {  returnTitleId = "0"}

                        }
                });
                HttpContext.Current.Response.StatusCode = 1;
                conMain.Close();
                return result;
            }
        }


        #endregion


        #region Played Advertisement Status Stream
        public List<ResponcePlayedAdvt> PlayedAdvertisementStatusStream(List<DataPlayedAdvtNew> data)
        {
            List<ResponcePlayedAdvt> result = new List<ResponcePlayedAdvt>();
            List<SongsArray> resultAds = new List<SongsArray>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string rSave = "0";

                foreach (var Player in data)
                {
                    if (Player.TokenId != 0)
                    {
                        string lType = "spTokenAdvt_Status " + Player.TokenId + ", " + Player.AdvtId + ",'" + string.Format("{0:dd-MMM-yyyy}", Player.PlayedDate) + "','" + string.Format("{0:hh:mm:ss}", Player.PlayedTime) + "'";
                        SqlCommand cmd = new SqlCommand(lType, con);
                        cmd.CommandType = System.Data.CommandType.Text;
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        cmd.ExecuteNonQuery();
                        resultAds.Add(new SongsArray()
                        {
                            Response = "1",
                            returnPlayedDateTime = Player.PlayedDate + " " + Player.PlayedTime,
                            returnTitleId = Player.AdvtId.ToString()
                        });
                    }
                }
                result.Add(new ResponcePlayedAdvt()
                {
                    Response = "1",
                    SongArray = resultAds
                });
                con.Close();

                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayedAdvt()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region Played Prayer Status Stream
        public List<ResponcePlayedPrayer> PlayedPrayerStatusStream(List<DataPlayedPrayer> data)
        {
            List<ResponcePlayedPrayer> result = new List<ResponcePlayedPrayer>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string rSave = "0";
                foreach (var Player in data)
                {
                    rSave = data[0].TokenId.ToString();
                    if (rSave == "0")
                    {
                        result.Add(new ResponcePlayedPrayer()
                        {
                            Response = "0",
                        });
                    }
                    else
                    {
                        string lType = "";
                        SqlCommand cmd = new SqlCommand("spTokenPrayer_Status " + Player.TokenId + ",'" + Player.PlayedDate + "','" + Player.PlayedTime + "'", con);
                        cmd.CommandType = System.Data.CommandType.Text;
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        SqlDataAdapter ad = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        ad.Fill(ds);
                        result.Add(new ResponcePlayedPrayer()
                        {
                            Response = "1",
                        });
                    }
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayedPrayer()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region Player Login Status Stream
        public List<ResponcePlayerLogin> PlayerLoginStatusStream(List<DataPlayerLogin> data)
        {
            List<ResponcePlayerLogin> result = new List<ResponcePlayerLogin>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string rSave = "0";

                foreach (var Player in data)
                {
                    rSave = data[0].TokenId.ToString();
                    if (rSave == "0")
                    {
                        result.Add(new ResponcePlayerLogin()
                        {
                            Response = "0",
                        });
                    }
                    else
                    {
                        string lType = "";
                        SqlCommand cmd = new SqlCommand("spTokenLogin_Status " + Player.TokenId + ",'" + Player.LoginDate + "','" + Player.LoginTime + "'", con);
                        cmd.CommandType = System.Data.CommandType.Text;
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        SqlDataAdapter ad = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        ad.Fill(ds);
                        result.Add(new ResponcePlayerLogin()
                        {
                            Response = "1",
                        });
                    }
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayerLogin()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion



        #region Player Logout Status Stream
        public List<ResponcePlayerLogout> PlayerLogoutStatusStream(List<DataPlayerLogout> data)
        {
            List<ResponcePlayerLogout> result = new List<ResponcePlayerLogout>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string rSave = "0";
                foreach (var Player in data)
                {
                    rSave = data[0].TokenId.ToString();
                    if (rSave == "0")
                    {
                        result.Add(new ResponcePlayerLogout()
                        {
                            Response = "0",
                        });
                    }
                    else
                    {

                        SqlCommand cmd = new SqlCommand("spTokenLogOut_Status " + Player.TokenId + ",'" + Player.LogoutDate + "','" + Player.LogoutTime + "'", con);
                        cmd.CommandType = System.Data.CommandType.Text;
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        SqlDataAdapter ad = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        ad.Fill(ds);
                        result.Add(new ResponcePlayerLogout()
                        {
                            Response = "1",
                        });
                    }
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayerLogout()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region Player Heart Beat Status Stream
        public List<ResponcePlayerHeart> PlayerHeartBeatStatusStream(List<DataPlayerHeart> data)
        {
            string rSave = "0";
            List<ResponcePlayerHeart> result = new List<ResponcePlayerHeart>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                foreach (var Player in data)
                {
                    rSave = data[0].TokenId.ToString();
                    if (rSave == "0")
                    {
                        result.Add(new ResponcePlayerHeart()
                        {
                            Response = "0",
                        });
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("spTokenOverDue_Status " + Player.TokenId + ",'" + Player.HeartbeatDateTime + "'", con);
                        cmd.CommandType = System.Data.CommandType.Text;
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        SqlDataAdapter ad = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        ad.Fill(ds);

                        result.Add(new ResponcePlayerHeart()
                        {
                            Response = "1",
                        });
                    }
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePlayerHeart()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion

        #endregion


        #region Sander Video API's

        # region GetSplPlaylistVideo
        public List<ResponceSplSplaylist> GetSplPlaylistVideo(DataSplPlaylist data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            List<ResponceSplSplaylist> result = new List<ResponceSplSplaylist>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["VideoCon"].ConnectionString);
            int isSepration_old = 0;
            try
            {
                string str = "";
                //string 
                //SqlCommand cmd = new SqlCommand("GetSpecialPlaylistSchedule " + data.WeekNo + "," + data.TokenId + "," + data.DfClientId + " ", con);
                //if ((data.TokenId == 1357) || (data.TokenId == 1361) || (data.TokenId == 1376))
                //{
                //    str = "GetSpecialTempPlaylistSchedule 5," + data.TokenId + "," + data.DfClientId + " ,'" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(DateTime.Now)) + "'";
                //}
                //else
                //{
                str = "GetSpecialTempPlaylistSchedule " + data.WeekNo + "," + data.TokenId + "," + data.DfClientId + " ,'" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(DateTime.Now)) + "'";
                // }
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var e_time = "";
                    if (string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i]["EndTime"]) == "11:59 PM")
                    {
                        e_time = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"]).AddSeconds(59).ToString();
                    }
                    else
                    {
                        e_time = ds.Tables[0].Rows[i]["EndTime"].ToString();
                    }
                    int isShowDefaultVar = 0;
                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["isShowDefault"]) == 1)
                    {
                        isShowDefaultVar = 0;
                    }
                    else
                    {
                        isShowDefaultVar = 1;
                    }
                    if (Convert.ToBoolean(ds.Tables[0].Rows[i]["isShowDefault"]) == true)
                    {
                        isSepration_old = 1;
                    }
                    else
                    {
                        isSepration_old = 0;
                    }
                    result.Add(new ResponceSplSplaylist()
                    {
                        pScid = Convert.ToInt32(ds.Tables[0].Rows[i]["pSchid"]),
                        dfclientid = Convert.ToInt32(ds.Tables[0].Rows[i]["dfClientId"]),
                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        splPlaylistName = ds.Tables[0].Rows[i]["splPlaylistName"].ToString(),
                        StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString(),
                        EndTime = e_time,
                        IsSeprationActive = isSepration_old,
                        IsSeprationActive_New = isShowDefaultVar,
                        IsFadingActive = Convert.ToInt32(ds.Tables[0].Rows[i]["IsFadingActive"]),
                        FormatId = 0,
                        IsMute = ds.Tables[0].Rows[i]["IsMute"].ToString(),
                        PercentageValue = ds.Tables[0].Rows[i]["PercentageValue"].ToString(),
                        TotalCount = ds.Tables[0].Rows[i]["TotalCount"].ToString(),
                        VolumeLevel = ds.Tables[0].Rows[i]["VolumeLevel"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region GetSplPlaylistTitlesVideo
        public List<ResponceSplSplaylistTitle> GetSplPlaylistTitlesVideo(DataSplPlaylistTile data)
        {
            List<ResponceSplSplaylistTitle> result = new List<ResponceSplSplaylistTitle>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["VideoCon"].ConnectionString);

            try
            {
                string mtypeFormat = "";
                SqlCommand cmd = new SqlCommand("GetSpecialPlaylists_Titles " + data.splPlaylistId + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                string url = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Audio")
                    {
                        mtypeFormat = ".mp3";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Video")
                    {
                        mtypeFormat = ".mp4";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Image")
                    {
                        mtypeFormat = ".jpg";
                    }



                    url = "http://api.advikon.com/mp3files/" + ds.Tables[0].Rows[i]["titleId"].ToString() + mtypeFormat;

                    result.Add(new ResponceSplSplaylistTitle()
                    {

                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        titleId = Convert.ToInt32(ds.Tables[0].Rows[i]["titleId"]),
                        Title = ds.Tables[0].Rows[i]["Title"].ToString().Trim(),
                        tTime = ds.Tables[0].Rows[i]["Time"].ToString().Trim(),
                        ArtistID = Convert.ToInt32(ds.Tables[0].Rows[i]["ArtistID"]),
                        arName = ds.Tables[0].Rows[i]["arName"].ToString().Trim(),
                        AlbumID = Convert.ToInt32(ds.Tables[0].Rows[i]["AlbumID"]),
                        alName = ds.Tables[0].Rows[i]["alName"].ToString().Trim(),
                        srno = Convert.ToInt32(ds.Tables[0].Rows[i]["srno"]),
                        TitleUrl = url,
                        TitleUrl2 = url,
                        FileSize = ds.Tables[0].Rows[i]["filesize"].ToString(),
                        TimeInterval = Convert.ToInt32(ds.Tables[0].Rows[i]["imgInterval"]),
                        IsLoop = false,
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #region CheckUserLoginVideo
        public List<ResponceUserToen> CheckUserLoginVideo(DataClientToken data)
        {
            List<ResponceUserToen> result = new List<ResponceUserToen>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["VideoCon"].ConnectionString);

            try
            {
                string lType = "";
                if (data.PlayerType == "LinuxPI")
                {
                    lType = "Linux";
                }
                else
                {
                    lType = data.PlayerType;
                }
                SqlCommand cmd = new SqlCommand("spGetTokenRights_Mobile '" + data.UserName + "', '" + data.TokenNo + "' , '" + data.DeviceId + "','" + lType + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceUserToen()
                    {
                        Response = "0",
                    });
                    con.Close();
                    return result;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceUserToen()
                    {
                        Response = "1",
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion

        #region CheckUserRightsVideo
        public List<ResponceUserRights> CheckUserRightsVideo(DataUserRights data)
        {
            List<ResponceUserRights> result = new List<ResponceUserRights>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["VideoCon"].ConnectionString);

            try
            {
                string mType = "";
                SqlCommand cmd = new SqlCommand("spGetTokenExpiryStatus_Mobile '" + data.DeviceId + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceUserRights()
                    {
                        Response = "0",
                        LeftDays = "0",
                        TokenId = "0",
                    });
                    con.Close();
                    return result;
                }
                if (Convert.ToInt32(ds.Tables[0].Rows[0][8]) == 1)
                {
                    mType = "Video";
                }
                else
                {
                    mType = "Audio";
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceUserRights()
                    {
                        Response = ds.Tables[0].Rows[0][0].ToString(),
                        LeftDays = ds.Tables[0].Rows[0][1].ToString(),
                        TokenId = ds.Tables[0].Rows[0][2].ToString(),
                        dfClientId = ds.Tables[0].Rows[0][3].ToString(),
                        CountryId = Convert.ToInt32(ds.Tables[0].Rows[0][4]),
                        StateId = Convert.ToInt32(ds.Tables[0].Rows[0][5]),
                        Cityid = Convert.ToInt32(ds.Tables[0].Rows[0][6]),
                        IsStopControl = Convert.ToInt32(ds.Tables[0].Rows[0][7]),
                        MediaType = mType,
                        scheduleType = ds.Tables[0].Rows[0]["scheduleType"].ToString(),
                        IsDemoToken = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDemoToken"]),
                        TotalShot = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalShot"]),

                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion



        #region AdvtScheduleVideoLinuxOnly
        public List<ResponceAdvtLinux> AdvtScheduleVideoLinuxOnly(DataAdvtSchLinux data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            string str = "";
            List<ResponceAdvtLinux> result = new List<ResponceAdvtLinux>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["VideoCon"].ConnectionString);

            try
            {
                str = "spGetAdvtAdmin_LinuxOnly '" + data.MonthNo + "','NativeCR'," + data.DfClientId + "," + data.WeekNo + "," + data.CityId + "," + data.DfClientId + "," + data.CountryId + "," + data.StateId + "," + data.TokenId + "";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceAdvtLinux()
                    {
                        Response = "0",
                    });
                    con.Close();
                    return result;
                }


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceAdvtLinux()
                    {
                        Response = "1",
                        AdvtId = Convert.ToInt32(ds.Tables[0].Rows[i]["AdvtId"]),
                        AdvtName = ds.Tables[0].Rows[i]["AdvtDisplayName"].ToString(),
                        PlayingType = ds.Tables[0].Rows[i]["playingType"].ToString(),
                        AdvtFilePath = ds.Tables[0].Rows[i]["AdvtFilePath"].ToString(),

                        IsTime = Convert.ToByte(ds.Tables[0].Rows[i]["IsTime"]),
                        sTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i]["AdvtTime"]),

                        IsMinute = Convert.ToByte(ds.Tables[0].Rows[i]["IsMinute"]),
                        TotalMinutes = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMinutes"]),
                        IsSong = Convert.ToByte(ds.Tables[0].Rows[i]["IsSong"]),
                        TotalSongs = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalSongs"]),
                        SrNo = Convert.ToInt32(ds.Tables[0].Rows[i]["srNo"]),

                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion


        #endregion



        #region GetStreamPlaylistSchedule
        public List<ResponceStreamPlaylist> GetStreamPlaylistSchedule()
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            List<ResponceStreamPlaylist> result = new List<ResponceStreamPlaylist>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {

                SqlCommand cmd = new SqlCommand("GetStreamPlaylistSchedule", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceStreamPlaylist()
                    {
                        pSchid = Convert.ToInt32(ds.Tables[0].Rows[i]["pSchid"]),
                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        startTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i]["StartTime"]),
                        EndTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i]["EndTime"]),
                        StreamName = ds.Tables[0].Rows[i]["splPlaylistName"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                return result;
            }
        }


        #endregion



        #region GetCustomerContent
        public List<ResponceCustomerContent> GetCustomerContent(DataCustomerContent data)
        {
            List<ResponceCustomerContent> result = new List<ResponceCustomerContent>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string mtypeFormat = ".mp3";

                SqlCommand cmd = new SqlCommand("GetCustomerContent " + data.dfClientId + ",'" + data.MediaType + "' ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);

                if (data.MediaType.ToString() == "Video")
                {
                    mtypeFormat = ".mp4";
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceCustomerContent()
                    {
                        titleId = Convert.ToInt32(ds.Tables[0].Rows[i]["titleId"]),
                        Title = ds.Tables[0].Rows[i]["Title"].ToString(),
                        tTime = ds.Tables[0].Rows[i]["Time"].ToString(),
                        ArtistID = Convert.ToInt32(ds.Tables[0].Rows[i]["ArtistID"]),
                        arName = ds.Tables[0].Rows[i]["arName"].ToString(),
                        AlbumID = Convert.ToInt32(ds.Tables[0].Rows[i]["AlbumID"]),
                        alName = ds.Tables[0].Rows[i]["alName"].ToString(),
                        TitleUrl = "http://api.advikon.com/mp3files/" + ds.Tables[0].Rows[i]["titleId"].ToString() + mtypeFormat,
                        MediaType = data.MediaType,
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion




        #region Stream
        public List<ResponceStream> GetCustomerStreams(DataUserRights data)
        {
            List<ResponceStream> result = new List<ResponceStream>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string st = "";
                st = "select *  from tbStreaming_App order by streamNameapp ";

                //st = "select distinct tbStreaming_App.*  from tbStreaming_App";
                //st = st + " inner join tbAssignMobileStreamToken on tbStreaming_App.streamid= tbAssignMobileStreamToken.streamId  ";
                //st = st + " inner join AMPlayerTokens on AMPlayerTokens.tokenid= tbAssignMobileStreamToken.tokenid ";
                //st = st + " where AMPlayerTokens.code= '" + data.DeviceId + "' ";
                //st = st + " order by tbStreaming_App.streamNameapp ";


                SqlCommand cmd = new SqlCommand(st, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceStream()
                    {
                        StreamName = ds.Tables[0].Rows[i]["streamnameApp"].ToString(),
                        StreamLink = ds.Tables[0].Rows[i]["StreamLinkApp"].ToString(),
                        StreamImgPath = ds.Tables[0].Rows[i]["imgpath"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                return result;
            }
        }


        public List<ResponceStreamLinux> GetCustomerTokenStreamsUrlLinux(DataCustomerTokenId data)
        {
            List<ResponceStreamLinux> result = new List<ResponceStreamLinux>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string st = "select distinct tbStreaming.*  from tbStreaming";
                st = st + " inner join tbAssignStreamToken on tbStreaming.streamid= tbAssignStreamToken.streamId  ";
                st = st + " where tbAssignStreamToken.tokenid= " + data.Tokenid + " ";
                st = st + " order by tbStreaming.streamName ";
                SqlCommand cmd = new SqlCommand(st, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceStreamLinux()
                    {
                        StreamName = ds.Tables[0].Rows[i]["streamname"].ToString(),
                        StreamLink = ds.Tables[0].Rows[i]["StreamLink"].ToString(),
                        StreamId = ds.Tables[0].Rows[i]["streamid"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                return result;
            }
        }



        #endregion



        public List<ResponceSplSplaylistTitle> GetTokenContent(DataTokenId data)
        {
            List<ResponceSplSplaylistTitle> result = new List<ResponceSplSplaylistTitle>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["VideoCon"].ConnectionString);

            try
            {
                string mtypeFormat = "";
                SqlCommand cmd = new SqlCommand("GetTokenContent " + data.Tokenid + " , " + data.WeekId + "", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                string url = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Audio")
                    {
                        mtypeFormat = ".mp3";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Video")
                    {
                        mtypeFormat = ".mp4";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Image")
                    {
                        mtypeFormat = ".jpg";
                    }



                    url = "http://api.advikon.com/mp3files/" + ds.Tables[0].Rows[i]["titleId"].ToString() + mtypeFormat;

                    result.Add(new ResponceSplSplaylistTitle()
                    {

                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        titleId = Convert.ToInt32(ds.Tables[0].Rows[i]["titleId"]),
                        Title = ds.Tables[0].Rows[i]["Title"].ToString().Trim(),
                        tTime = ds.Tables[0].Rows[i]["Time"].ToString().Trim(),
                        ArtistID = Convert.ToInt32(ds.Tables[0].Rows[i]["ArtistID"]),
                        arName = ds.Tables[0].Rows[i]["arName"].ToString().Trim(),
                        AlbumID = Convert.ToInt32(ds.Tables[0].Rows[i]["AlbumID"]),
                        alName = ds.Tables[0].Rows[i]["alName"].ToString().Trim(),
                        srno = Convert.ToInt32(ds.Tables[0].Rows[i]["srno"]),
                        TitleUrl = url,
                        TitleUrl2 = url,
                        FileSize = ds.Tables[0].Rows[i]["filesize"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
            //List<ResponceUserRights> result = new List<ResponceUserRights>();
            //SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            //try
            //{
            //    string mType = "";
            //    SqlCommand cmd = new SqlCommand("spGetTokenContent_Mobile '" + data.Tokenid + "'", con);
            //    cmd.CommandType = System.Data.CommandType.Text;
            //    if (con.State == ConnectionState.Closed) { con.Open(); }
            //    SqlDataAdapter ad = new SqlDataAdapter(cmd);
            //    DataSet ds = new DataSet();
            //    ad.Fill(ds);
            //    if (ds.Tables[0].Rows.Count == 0)
            //    {
            //        result.Add(new ResponceUserRights()
            //        {
            //            Response = "0",
            //            LeftDays = "0",
            //            TokenId = "0",
            //        });
            //        con.Close();
            //        return result;
            //    }
            //    if (Convert.ToInt32(ds.Tables[0].Rows[0][8]) == 1)
            //    {
            //        mType = "Video";
            //    }
            //    else
            //    {
            //        mType = "Audio";
            //    }
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        result.Add(new ResponceUserRights()
            //        {
            //            Response = ds.Tables[0].Rows[0][0].ToString(),
            //            LeftDays = ds.Tables[0].Rows[0][1].ToString(),
            //            TokenId = ds.Tables[0].Rows[0][2].ToString(),
            //            dfClientId = ds.Tables[0].Rows[0][3].ToString(),
            //            CountryId = Convert.ToInt32(ds.Tables[0].Rows[0][4]),
            //            StateId = Convert.ToInt32(ds.Tables[0].Rows[0][5]),
            //            Cityid = Convert.ToInt32(ds.Tables[0].Rows[0][6]),
            //            IsStopControl = Convert.ToInt32(ds.Tables[0].Rows[0][7]),
            //            MediaType = mType,
            //        });
            //    }
            //    con.Close();
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    con.Close();
            //    HttpContext.Current.Response.StatusCode = 1;
            //    return result;
            //}
        }

        public List<ResponceDownloadingProcess> TokenWiseDownloadingProcess(DataDownloadStatus data)
        {
            List<ResponceDownloadingProcess> result = new List<ResponceDownloadingProcess>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string TotalSong = "";
                string free = "";
                string TotalSpace = "";
                string Timezone = "";
                if (string.IsNullOrEmpty(data.FreeSpace) == true)
                {
                    free = "0";
                }
                else
                {
                    free = data.FreeSpace;
                }
                TotalSong = data.totalSong;
                if (string.IsNullOrEmpty(data.TotalSpace) == true)
                {
                    TotalSpace = "0";
                }
                else
                {
                    TotalSpace = data.TotalSpace;
                }
                if (string.IsNullOrEmpty(data.TimeZone) == true)
                {
                    Timezone = "";
                }
                else
                {
                    Timezone = data.TimeZone;
                }
                string lType = "SaveDownloadsongStatus " + data.TokenId + ", " + TotalSong + ",'" + data.verNo + "', " + free + "," + TotalSpace + ",'" + Timezone + "'";
                SqlCommand cmd = new SqlCommand(lType, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                result.Add(new ResponceDownloadingProcess()
                {
                    Response = "1",
                });
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponceDownloadingProcess()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public List<ResponcePublish> CheckTokenPublish(DataCustomerTokenId data)
        {
            List<ResponcePublish> result = new List<ResponcePublish>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {

                SqlCommand cmd = new SqlCommand("select isnull(IsPublishUpdate,0) as IsPublishUpdate, isnull(TotalShot,0) as TotalShot, isnull(DispenserAlert,'') as DispenserAlert, isnull(IsShowShotToast,0) as IsShowShotToast from AMPlayerTokens where tokenid= " + data.Tokenid + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);



                if (ds.Rows.Count == 0)
                {
                    result.Add(new ResponcePublish()
                    {
                        Response = "0",
                        IsPublishUpdate = "0",
                        DispenserAlert = "",
                        TotalShot = 0,
                        IsDemoToken = false

                    });
                    con.Close();
                    return result;
                }
                else
                {
                    if (Convert.ToBoolean(ds.Rows[0]["IsPublishUpdate"]) == true)
                    {
                        result.Add(new ResponcePublish()
                        {
                            Response = "1",
                            IsPublishUpdate = "1",
                            DispenserAlert = ds.Rows[0]["DispenserAlert"].ToString(),
                            TotalShot = Convert.ToInt32(ds.Rows[0]["TotalShot"]),
                            IsDemoToken = Convert.ToBoolean(ds.Rows[0]["IsShowShotToast"])

                        });
                    }
                    else
                    {
                        result.Add(new ResponcePublish()
                        {
                            Response = "1",
                            IsPublishUpdate = "0",
                            DispenserAlert = ds.Rows[0]["DispenserAlert"].ToString(),
                            TotalShot = Convert.ToInt32(ds.Rows[0]["TotalShot"]),
                            IsDemoToken = Convert.ToBoolean(ds.Rows[0]["IsShowShotToast"])
                        });
                    }



                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePublish()
                {
                    Response = "0",
                    IsPublishUpdate = "0"
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }



        public List<ResponcePublish> UpdateTokenPublish(DataCustomerTokenId data)
        {
            List<ResponcePublish> result = new List<ResponcePublish>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {

                SqlCommand cmd = new SqlCommand("update AMPlayerTokens set IsPublishUpdate = 0 where  tokenid=" + data.Tokenid + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.ExecuteNonQuery();
                result.Add(new ResponcePublish()
                {
                    Response = "1",
                    IsPublishUpdate = "1"
                });
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponcePublish()
                {
                    Response = "0",
                    IsPublishUpdate = "0"
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }



        public List<ResponceDownloadingProcess> PlaylistWiseDownloadedTotalSong(List<DataPlaylistDownloadStatus> data)
        {
            List<ResponceDownloadingProcess> result = new List<ResponceDownloadingProcess>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
            string lType = "";
            int tId = 0;
            try
            {
                foreach (var PlayerLog in data)
                {
                    if (tId != PlayerLog.TokenId)
                    {
                        tId = PlayerLog.TokenId;
                        SqlCommand cmdDel = new SqlCommand("delete from tbDownloadSongStatus_Playlist where tokenid= " + PlayerLog.TokenId + " ", con);
                        cmdDel.CommandType = System.Data.CommandType.Text;
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        cmdDel.ExecuteNonQuery();
                    }
                    lType = "SaveDownloadsongStatus_Playlist " + PlayerLog.TokenId + ", " + PlayerLog.totalSong + "," + PlayerLog.splPlaylistId + "";
                    SqlCommand cmd = new SqlCommand(lType, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    cmd.ExecuteNonQuery();
                }
                result.Add(new ResponceDownloadingProcess()
                {
                    Response = "1",
                });
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponceDownloadingProcess()
                {
                    Response = "0",
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public List<ResponceDownloadingProcess> PlaylistWiseDownloadedSongsDetail(List<DataPlaylistDownloadedSongs> data)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
            List<ResponceDownloadingProcess> result = new List<ResponceDownloadingProcess>();
            string tId = "";
            string pId = "";
            try
            {
                DataTable dtInsert = new DataTable();
                dtInsert.Columns.Add("splPlaylistid", typeof(int));
                dtInsert.Columns.Add("titleid", typeof(int));
                dtInsert.Columns.Add("tokenId", typeof(int));

                //string rSave = "0";
                //rSave = AppDomain.CurrentDomain.BaseDirectory;
                //string path = Path.GetDirectoryName(rSave) + "\\data.txt";
                //string WriteData = "";



                foreach (var Player in data)
                {
                    if (tId != Player.TokenId)
                    {
                        tId = Player.TokenId;

                        SqlCommand cmdDel = new SqlCommand("delete from tbDownloadStatus_PlaylistSong where tokenid= " + Player.TokenId + " ", con);
                        cmdDel.CommandType = System.Data.CommandType.Text;
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        cmdDel.ExecuteNonQuery();
                    }
                    if (pId != Player.splPlaylistId)
                    {
                        pId = Player.splPlaylistId;

                        //if (Player.TokenId.ToString() == "3410")
                        //{
                        //    WriteData = "" + Player.TokenId + ", " + Player.splPlaylistId + " , " + Player.titleIDArray.Length.ToString() + ", {0} ";
                        //    using (StreamWriter writer = new StreamWriter(path, true))
                        //    {
                        //        writer.WriteLine(string.Format(WriteData, ""));
                        //        writer.Close();
                        //    }
                        //}

                        foreach (var PlayerTitle in Player.titleIDArray)
                        {
                            DataRow nr = dtInsert.NewRow();
                            nr["splPlaylistid"] = Player.splPlaylistId;
                            nr["titleid"] = Convert.ToInt32(PlayerTitle);
                            nr["tokenId"] = Convert.ToInt32(Player.TokenId);

                            dtInsert.Rows.Add(nr);
                            //SqlCommand cmd = new SqlCommand("SaveDownloadStatus_PlaylistSong " + Player.TokenId + ", " + PlayerTitle + "," + Player.splPlaylistId + "", con);
                            //cmd.CommandType = System.Data.CommandType.Text;
                            //if (con.State == ConnectionState.Closed)
                            //{
                            //    con.Open();
                            //}
                            //cmd.ExecuteNonQuery();

                        }

                    }

                }
                if (dtInsert.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {

                        SqlBulkCopyColumnMapping mapID =
                         new SqlBulkCopyColumnMapping("splPlaylistid", "splPlaylistid");
                        bulkCopy.ColumnMappings.Add(mapID);

                        SqlBulkCopyColumnMapping mapMumber =
                            new SqlBulkCopyColumnMapping("titleid", "titleid");
                        bulkCopy.ColumnMappings.Add(mapMumber);
                        SqlBulkCopyColumnMapping mapName =
                           new SqlBulkCopyColumnMapping("tokenId", "tokenId");
                        bulkCopy.DestinationTableName = "dbo.tbDownloadStatus_PlaylistSong";

                        bulkCopy.ColumnMappings.Add(mapName);
                        if (con.State == ConnectionState.Open) con.Close();
                        con.Open();
                        bulkCopy.WriteToServer(dtInsert);

                    }
                }
                result.Add(new ResponceDownloadingProcess()
                {
                    Response = "1"
                });
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Add(new ResponceDownloadingProcess()
                {
                    Response = "0",
                    errorMsg = ex.Message.ToString()
                });
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResponseTokenCrashLog TokenCrashLog(DataTokenCrashLog data)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            ResponseTokenCrashLog result = new ResponseTokenCrashLog();
            try
            {
                SqlCommand cmd = new SqlCommand("Save_TokenCrashLog", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@TokenId", data.TokenId));
                cmd.Parameters.Add(new SqlParameter("@crashlog", data.CrashLog));
                cmd.Parameters.Add(new SqlParameter("@crashdatetime", data.CrashDateTime));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.ExecuteNonQuery();
                result.Response = 1;
                result.ErrorMessage = "Success";
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                result.ErrorMessage = ex.ToString();
                result.Response = 0;
            }
            return result;
        }


        #region GetSplPlaylist Live
        public List<ResponceSplSplaylist> GetSplPlaylistDateWiseLive(DataSplPlaylistDateWise data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            List<ResponceSplSplaylist> result = new List<ResponceSplSplaylist>();
            int isShowDefaultVar = 0;
            int isSepration_old = 0;
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {
                string str = "";
                str = "GetSpecialTempPlaylistSchedule " + data.WeekNo + "," + data.TokenId + "," + data.DfClientId + " ,'" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(DateTime.Now)) + "'";

                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["isShowDefault"]) == 1)
                    {
                        isShowDefaultVar = 0;
                    }
                    else
                    {
                        isShowDefaultVar = 1;
                    }
                    if (Convert.ToBoolean(ds.Tables[0].Rows[i]["isShowDefault"]) == true)
                    {

                        isSepration_old = 0;
                    }
                    else
                    {

                        isSepration_old = 1;
                    }
                    /* var e_time="" ;
                     if (string.Format(fi,"{0:hh:mm tt}", ds.Tables[0].Rows[i]["EndTime"]) == "11:59 PM")
                     {
                         e_time = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"]).AddSeconds(59).ToString();
                     }
                     else
                     {
                         e_time = ds.Tables[0].Rows[i]["EndTime"].ToString();
                     }*/
                    result.Add(new ResponceSplSplaylist()
                    {
                        pScid = Convert.ToInt32(ds.Tables[0].Rows[i]["pSchid"]),
                        dfclientid = Convert.ToInt32(ds.Tables[0].Rows[i]["dfClientId"]),
                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        splPlaylistName = ds.Tables[0].Rows[i]["splPlaylistName"].ToString(),
                        StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString(),
                        EndTime = ds.Tables[0].Rows[i]["EndTime"].ToString(),
                        IsSeprationActive = isSepration_old,
                        IsSeprationActive_New = isShowDefaultVar,
                        //IsFadingActive = Convert.ToInt32(ds.Tables[0].Rows[i]["IsFadingActive"]),
                        IsFadingActive = 1,
                        FormatId = 0,
                        IsMute = ds.Tables[0].Rows[i]["IsMute"].ToString(),
                        PercentageValue = ds.Tables[0].Rows[i]["PercentageValue"].ToString(),
                        TotalCount = ds.Tables[0].Rows[i]["TotalCount"].ToString(),
                        VolumeLevel = ds.Tables[0].Rows[i]["VolumeLevel"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        #endregion

        public string[] GetAllPlaylistScheduleSongs(DataCustomerTokenId data)
        {
            List<string> returnArray = new List<string>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("GetTokenAllPlaylistScheduleSongs " + data.Tokenid, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    returnArray.Add(ds.Tables[0].Rows[i]["titleid"].ToString());
                }
                con.Close();
                return returnArray.ToArray();
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return returnArray.ToArray();
            }
        }
        public ResponseTokenCrashLog UpdateFCMId(DataTokenFCMID data)
        {
            ResponseTokenCrashLog result = new ResponseTokenCrashLog();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("update AMPlayerTokens set NotificationDeviceId ='" + data.fcmId + "' where tokenid=" + data.TokenId + "", con);
                cmd.CommandType = CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.ExecuteNonQuery();
                result.Response = 1;
                result.ErrorMessage = "Success";
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                result.ErrorMessage = ex.ToString();
                result.Response = 0;
            }
            return result;
        }
        public ResponseTokenCrashLog SendNoti(ClsNoti data)
        {
            ResponseTokenCrashLog ReturnResult = new ResponseTokenCrashLog();
            try
            {
                string DeviceToken = data.DeviceToken;
                if (DeviceToken == "Desktop")
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ToString());
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "delete from tbTokenPlayNow_Web where tokenId=" + data.tid + "";
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "insert into tbTokenPlayNow_Web (tokenid, cID, PlayType,PlayMode) values(" + data.tid + ", " + data.id + ",'Now','Song')";
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    cmd.ExecuteNonQuery();
                    con.Close();

                    ReturnResult.Response = 1;
                    ReturnResult.ErrorMessage = "Success";
                }
                else
                {


                    var result = "-1";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    httpWebRequest.ContentType = "application/json";
                    if (data.IsVideoToken == "1")
                    {
                        httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAVNhkSB0:APA91bFvqS4tV4d8EBd_R9EPR5OwiSYNAu-WpZoE6u4gsxkurkMscL1Gal-PY_0ZC8j2rl5OV38t531qHK8RTXT1mISNVvVcfdoD7JMRROimfEfnN2ppxEli67eiRGmmfwgJEa_ZK3OP"));
                    }
                    if (data.IsVideoToken == "0")
                    {
                        httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAwL-k27Y:APA91bFMoi8vNJJ310PqIFszZ_fsKqinRQ_U3ZovQwYpuNaYz5R4SkRkOfk5PebyUU6SxMT8gxc81185pZKsxku8Og8vP5foy-jtiOc-LKg-04sO-FFEd-lZpwGE3oeDWzoHE0cwk90d"));
                    }


                    httpWebRequest.Method = "POST";
                    httpWebRequest.UseDefaultCredentials = true;
                    httpWebRequest.PreAuthenticate = true;
                    httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                    var payload = new
                    {
                        to = DeviceToken,
                        priority = "high",
                        content_available = true,
                        notification = new
                        {

                            body = data,
                            title = "MyClaud"
                        },
                    };
                    var serializer = new JavaScriptSerializer();
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = serializer.Serialize(payload);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    var objs = JsonConvert.DeserializeObject<ResNoti>(result);
                    if (objs.success == "0")
                    {
                        ReturnResult.Response = 0;
                        ReturnResult.ErrorMessage = "Error";

                    }
                    else
                    {
                        ReturnResult.Response = 1;
                        ReturnResult.ErrorMessage = "Success";
                    }
                }

                return ReturnResult;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 1;
                ReturnResult.ErrorMessage = ex.ToString();
                ReturnResult.Response = 0;
                return ReturnResult;
            }
        }

        //======================================================================================
        //======================================================================================
        //============================= Mooov ==============================================
        //======================================================================================
        //======================================================================================


        public List<ResponceSetting> GetMoovSettings(DataClientId data)
        {
            List<ResponceSetting> result = new List<ResponceSetting>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MooovConn"].ConnectionString);

            try
            {
                string st = "";
                st = st + " select tbMoovPart1.*, tbDirections.DirectionName , tbDisplayTypes.DisplaytypeName from tbMoovPart1 ";
                st = st + " inner join tbDirections on tbDirections.DirectionId = tbMoovPart1.DirectionId";
                st = st + " inner join tbDisplayTypes on tbDisplayTypes.displaytypeid = tbMoovPart1.displaytypeid";
                st = st + " where tbMoovPart1.PlayerId= " + data.PlayerId + "";
                SqlCommand cmd = new SqlCommand(st, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceSetting()
                    {
                        direction = ds.Tables[0].Rows[i]["DirectionName"].ToString(),
                        defaultSpeed = ds.Tables[0].Rows[i]["defaultSpeed"].ToString(),
                        highlightTime = ds.Tables[0].Rows[i]["highlightTime"].ToString(),
                        zoomRatio = ds.Tables[0].Rows[i]["zoomRatio"].ToString(),
                        moveScaleHitSpeed = ds.Tables[0].Rows[i]["moveScaleHitSpeed"].ToString(),
                        moveHitSpeed = ds.Tables[0].Rows[i]["moveHitSpeed"].ToString(),
                        displayType = ds.Tables[0].Rows[i]["DisplaytypeName"].ToString(),
                        clientid = ds.Tables[0].Rows[i]["clientid"].ToString(),
                        pName = ds.Tables[0].Rows[i]["pName"].ToString(),
                        Interval = ds.Tables[0].Rows[i]["Interval"].ToString(),
                        IsMute = ds.Tables[0].Rows[i]["IsMute"].ToString(),
                        bgColor = "0x" + ds.Tables[0].Rows[i]["bgColor"].ToString(),
                        IsGameActive = ds.Tables[0].Rows[i]["IsGameActive"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                return result;
            }
        }

        public List<ResponceMoovSource> GetMoovSource(PlaylistDetail data)
        {
            List<ResponceMoovSource> result = new List<ResponceMoovSource>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MooovConn"].ConnectionString);

            try
            {
                string st = "select * from tbPlaylistDetail where playlistid=" + data.Playlistid + " ";
                SqlCommand cmd = new SqlCommand(st, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[i]["photoPath"].ToString()))
                    {
                        result.Add(new ResponceMoovSource()
                        {
                            photoPath = "",
                            photoTitle = ds.Tables[0].Rows[i]["photoTitle"].ToString(),
                            photoVideo = ds.Tables[0].Rows[i]["photoVideo"].ToString(),
                            photoId = ds.Tables[0].Rows[i]["id"].ToString()
                        });
                    }
                    else
                    {
                        result.Add(new ResponceMoovSource()
                        {
                            photoPath = ds.Tables[0].Rows[i]["photoPath"].ToString(),
                            photoTitle = ds.Tables[0].Rows[i]["photoTitle"].ToString(),
                            photoVideo = "",
                            photoId = ds.Tables[0].Rows[i]["id"].ToString()
                        });
                    }

                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                return result;
            }
        }
        public List<ResponcePlaylistMoov> GetPlaylistSchedule(DataMooovPlaylist data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            List<ResponcePlaylistMoov> result = new List<ResponcePlaylistMoov>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MooovConn"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("select * from tbPlaylistSchedule where playerid=  " + data.PlayerId + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponcePlaylistMoov()
                    {
                        PlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["playlistid"]),
                        StartTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i]["StartTime"]),
                        EndTime = string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i]["EndTime"]),
                        StartDate = string.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[i]["StartDate"]),
                        EndDate = string.Format("{0:dd-MMM-yyyy}", ds.Tables[0].Rows[i]["EndDate"])
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public ResponcePlayerRegistration PlayerRegistration(DataPlayerRegistration data)
        {
            ResponcePlayerRegistration result = new ResponcePlayerRegistration();
            string str = "";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MooovConn"].ConnectionString);
            try
            {
                str = "spPlayerRegistration '" + data.PlayerName + "', '" + data.TokenCode + "'";
                DataTable dtPlayer = new DataTable();
                dtPlayer = fnFillDataTable(str, con);
                if (con.State == ConnectionState.Closed) { con.Open(); }

                if (dtPlayer.Rows[0][0].ToString() == "0")
                {
                    result.Response = "0";
                    return result;
                }
                result.Response = "1";
                result.PlayerId = dtPlayer.Rows[0][0].ToString();
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                result.Response = "0";
                return result;
            }
        }

        public ResponcePlayerRegistration ClientMessage(DataClientMessage data)
        {
            ResponcePlayerRegistration result = new ResponcePlayerRegistration();
            string str = "";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MooovConn"].ConnectionString);

            try
            {
                str = "SaveClientMessage " + data.playerid + ", '" + data.ClientMsg + "','" + data.email + "'";

                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = con;
                cmd1.CommandText = str;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd1.ExecuteNonQuery();
                con.Close();

                result.Response = "1";
                result.PlayerId = data.playerid;
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                result.Response = "0";
                return result;
            }
        }



        public DataTable fnFillDataTable(string sSql, SqlConnection SqlCon)
        {
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable mldData;
            try
            {
                Adp = new SqlDataAdapter(sSql, SqlCon);
                mldData = new DataTable();
                Adp.Fill(mldData);
            }
            catch (Exception ex)
            {
                mldData = new DataTable();
                // MessageBox.Show(ex.Message);
            }
            return mldData;
        }





        //======================================================================================
        //======================================================================================
        //============================= Web Panel ==============================================
        //======================================================================================
        //======================================================================================
        public List<ResComboQuery> FillQueryCombo(ReqComboQuery data)
        {
            List<ResComboQuery> lstResult = new List<ResComboQuery>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand(data.Query, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstResult.Add(new ResComboQuery()
                    {
                        Id = ds.Tables[0].Rows[i]["id"].ToString(),
                        DisplayName = ds.Tables[0].Rows[i]["DisplayName"].ToString(),
                        check = false,
                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }


        public List<ResTokenInfo> FillTokenInfo(ReqTokenInfo data)
        {
            List<ResTokenInfo> lstResult = new List<ResTokenInfo>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                string str = "";
                if (string.IsNullOrEmpty(data.IsActiveTokens) == true)
                {
                    data.IsActiveTokens = "0";
                }
                if (string.IsNullOrEmpty(data.UserId) == true)
                {
                    str = "GetTokenInfo " + data.clientId + ",0 ," + data.IsActiveTokens;
                }
                else
                {
                    str = "GetTokenInfo " + data.clientId + " , " + data.UserId + "," + data.IsActiveTokens;
                }
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {

                    var tokenid1 = ds.Rows[i]["tokenid"].ToString();
                    var freeSp = "";
                    decimal aSpace = 0;
                    decimal tSpace = 0;
                    int FinalStatus = 0;
                    if (Convert.ToInt32(ds.Rows[i]["fSpace"]) <= 1024)
                    {
                        freeSp = ds.Rows[i]["fSpace"].ToString() + " MB";
                    }
                    else
                    {
                        freeSp = (Convert.ToInt32(ds.Rows[i]["fSpace"]) / 1024).ToString() + " GB";
                    }
                    aSpace = Convert.ToInt32(ds.Rows[i]["fSpace"]) - 2048;
                    tSpace = Convert.ToInt32(ds.Rows[i]["tSpace"]) - 2048;
                    if (Convert.ToInt32(ds.Rows[i]["fSpace"]) == 0)
                    {
                        aSpace = 21504;
                    }
                    if (ds.Rows[i]["PlType"].ToString() == "Desktop")
                    {
                        aSpace = 20480;
                        freeSp = "20 GB";
                        tSpace = 23552;
                    }
                    if (aSpace <= 0)
                    {
                        aSpace = 0;
                    }
                    int perSpace = 0;
                    decimal Persp;
                    aSpace = tSpace - aSpace;
                    Persp = Math.Round((aSpace / tSpace), 2) * 100;
                    perSpace = Convert.ToInt16(Persp);
                    if ((perSpace >= 0) && (perSpace <= 50))
                    {
                        FinalStatus = 50;
                    }
                    else if ((perSpace <= 75) && (perSpace > 50))
                    {
                        FinalStatus = 75;
                    }
                    else if ((perSpace > 75))
                    {
                        FinalStatus = 100;
                    }
                    string PlayerType = "";
                    string tokenStatus = "";
                    if (ds.Rows[i]["PlType"].ToString() == "Desktop")
                    {
                        PlayerType = "Windows";
                    }
                    else
                    {
                        PlayerType = ds.Rows[i]["PlType"].ToString();

                    }
                    if (ds.Rows[i]["token"].ToString() != "used")
                    {
                        tokenStatus = "UnRegsiter";
                    }
                    else
                    {
                        tokenStatus = "Regsiter";
                    }
                    string rebootTime = "";
                    DateTimeFormatInfo fi = new DateTimeFormatInfo();
                    fi.AMDesignator = "AM";
                    fi.PMDesignator = "PM";
                    if (string.Format(fi, "{0:HH:mm}", ds.Rows[i]["RebootTime"]) == "00:00")
                    {
                        rebootTime = "";
                    }
                    else
                    {
                        rebootTime = string.Format(fi, "{0:HH:mm}", ds.Rows[i]["RebootTime"]);
                    }
                    lstResult.Add(new ResTokenInfo()
                    {
                        tokenid = ds.Rows[i]["tokenid"].ToString(),
                        tokenCode = ds.Rows[i]["tNo"].ToString(),
                        Name = ds.Rows[i]["PersonName"].ToString(),
                        location = ds.Rows[i]["Location"].ToString(),
                        city = ds.Rows[i]["CityName"].ToString(),
                        countryName = ds.Rows[i]["CountryName"].ToString(),
                        playerType = PlayerType,
                        LicenceType = ds.Rows[i]["LiType"].ToString(),
                        tInfo = ds.Rows[i]["tInfo"].ToString(),
                        AppLogoId = ds.Rows[i]["AppLogoId"].ToString(),
                        Version = ds.Rows[i]["ver"].ToString(),
                        PublishUpdate = ds.Rows[i]["isPublishUpdate"].ToString(),
                        token = ds.Rows[i]["token"].ToString(),
                        ScheduleType = ds.Rows[i]["shType"].ToString(),
                        fSpace = freeSp,
                        IsIndicatorActive = ds.Rows[i]["IsIndicatorShow"].ToString(),
                        GroupId = ds.Rows[i]["grpId"].ToString(),
                        CountryId = ds.Rows[i]["CountryId"].ToString(),
                        StateId = ds.Rows[i]["StateId"].ToString(),
                        CityId = ds.Rows[i]["CityId"].ToString(),
                        SpaceStatus = FinalStatus,
                        SpacePer = perSpace,
                        MediaType = ds.Rows[i]["nMediaType"].ToString(),
                        State = ds.Rows[i]["StateName"].ToString(),
                        Street = ds.Rows[i]["street"].ToString(),
                        DeviceType = ds.Rows[i]["DeviceType"].ToString(),
                        TokenNoBkp = ds.Rows[i]["tnobkp"].ToString(),
                        CountryFullName = ds.Rows[i]["couName"].ToString(),
                        AlertEmail = ds.Rows[i]["AlertEmail"].ToString(),
                        gName = ds.Rows[i]["gname"].ToString(),
                        tZone = ds.Rows[i]["tZone"].ToString(),
                        TokenStatus = tokenStatus,
                        RebootTime = rebootTime
                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }

        public ResToken FillTokenContent(ReqToken data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            ResToken clsResult = new ResToken();
            List<ResTokenPlaylistSch> lstPlaylist = new List<ResTokenPlaylistSch>();
            List<ResTokenAds> lstAd = new List<ResTokenAds>();
            List<ResTokenPrayer> lstPra = new List<ResTokenPrayer>();
            List<ResTokenData> lstTokend = new List<ResTokenData>();
            List<ReqDispenserAlert> dAlert = new List<ReqDispenserAlert>();
            List<ResTokenPlaylistSch> lstAPKPlaylist = new List<ResTokenPlaylistSch>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                string sQr = "GetCustomerPlaylistSchedule  ' where m.tokenid=" + data.tokenId + " order by StartTime'";
                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstPlaylist.Add(new ResTokenPlaylistSch()
                    {
                        id = ds.Rows[i]["pSchid"].ToString(),
                        formatName = ds.Rows[i]["FormatName"].ToString(),
                        playlistName = ds.Rows[i]["pName"].ToString(),
                        StartTime = string.Format(fi, "{0:HH:mm}", ds.Rows[i]["StartTime"]),
                        EndTime = string.Format(fi, "{0:HH:mm}", ds.Rows[i]["EndTime"]),
                        WeekDay = ds.Rows[i]["wName"].ToString(),
                        formatid = ds.Rows[i]["formatid"].ToString(),
                        splPlaylistId = ds.Rows[i]["splPlaylistid"].ToString(),
                        PercentageValue = ds.Rows[i]["PercentageValue"].ToString(),
                    });
                }

                int wId = 0;
                string CurrentWeekday = DateTime.Now.DayOfWeek.ToString();

                if (CurrentWeekday == "Sunday")
                {
                    wId = 1;
                }
                if (CurrentWeekday == "Monday")
                {
                    wId = 2;
                }
                if (CurrentWeekday == "Tuesday")
                {
                    wId = 3;
                }
                if (CurrentWeekday == "Wednesday")
                {
                    wId = 4;
                }
                if (CurrentWeekday == "Thursday")
                {
                    wId = 5;
                }
                if (CurrentWeekday == "Friday")
                {
                    wId = 6;
                }
                if (CurrentWeekday == "Saturday")
                {
                    wId = 7;
                }
                sQr = "spGetAdvtAdmin_NativeOnly_Web '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','',0," + wId + ", 0,0, 0,0," + data.tokenId + "";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstAd.Add(new ResTokenAds()
                    {
                        id = ds.Rows[i]["AdvtId"].ToString(),
                        adName = ds.Rows[i]["AdvtDisplayName"].ToString(),
                        atype = ds.Rows[i]["pMode"].ToString(),
                        startDate = string.Format("{0:dd-MMM-yyyy}", ds.Rows[i]["AdvtStartDate"]),
                        playingMode = ds.Rows[i]["m"].ToString(),
                    });
                }


                // Prayer
                sQr = "spGetPrayerDataInfo " + DateTime.Now.Date.Month + ", 0,0,0," + data.tokenId;
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstPra.Add(new ResTokenPrayer()
                    {
                        id = (i + 1).ToString(),
                        StartDate = string.Format("{0:dd-MMM-yyyy}", ds.Rows[i]["sDate"]),
                        Time1 = ds.Rows[i]["Time1"].ToString(),
                        Time2 = ds.Rows[i]["Time2"].ToString(),
                        Time3 = ds.Rows[i]["Time3"].ToString(),
                        Time4 = ds.Rows[i]["Time4"].ToString(),
                        Time5 = ds.Rows[i]["Time5"].ToString(),
                    });
                }

                // TokenData
                sQr = "GetTokenInformation " + data.tokenId;
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    var dAt = ds.Rows[i]["DispenserAlert"].ToString().Split(',');
                    var h = ds.Rows[i]["DispenserAlert"].ToString();
                    if (h != "")
                    {
                        foreach (var item in dAt)
                        {
                            dAlert.Add(new ReqDispenserAlert()
                            {
                                id = item,
                                itemName = item + "%",
                            });

                        }
                    }

                    lstTokend.Add(new ResTokenData()
                    {
                        token = ds.Rows[i]["tokenno"].ToString(),
                        personName = ds.Rows[i]["PersonName"].ToString(),
                        country = ds.Rows[i]["CountryId"].ToString(),
                        state = ds.Rows[i]["StateId"].ToString(),
                        city = ds.Rows[i]["CityId"].ToString(),
                        street = ds.Rows[i]["StreetName"].ToString(),
                        location = ds.Rows[i]["Location"].ToString(),
                        ExpiryDate = string.Format("{0:dd-MMM-yyyy}", ds.Rows[i]["cDate"]),
                        PlayerType = ds.Rows[i]["ltype"].ToString(),
                        LicenceType = ds.Rows[i]["ptype"].ToString(),

                        chkMediaType = ds.Rows[i]["MediaType"].ToString(),
                        chkuserRights = ds.Rows[i]["userrights"].ToString(),
                        chkType = ds.Rows[i]["cType"].ToString(),
                        TokenNoBkp = ds.Rows[i]["TokenNoBkp"].ToString(),
                        DeviceId = ds.Rows[i]["deviceId"].ToString(),
                        ScheduleType = ds.Rows[i]["ScheduleType"].ToString(),
                        Indicator = Convert.ToBoolean(ds.Rows[i]["Indicator"]),
                        StateName = ds.Rows[i]["statename"].ToString(),
                        CityName = ds.Rows[i]["cityname"].ToString(),
                        GroupName = ds.Rows[i]["grpName"].ToString(),
                        GroupId = ds.Rows[i]["grpId"].ToString(),
                        ClientId = ds.Rows[i]["ClientId"].ToString(),
                        Rotation = ds.Rows[i]["Rotation"].ToString(),
                        CommunicationType = ds.Rows[i]["CommunicationType"].ToString(),
                        DeviceType = ds.Rows[i]["DeviceType"].ToString(),
                        DispenserAlert = dAlert,
                        TotalShot = ds.Rows[i]["TotalShot"].ToString(),
                        AlertMail = ds.Rows[i]["AlertEmail"].ToString(),
                        IsShowShotToast = Convert.ToBoolean(ds.Rows[i]["IsShowShotToast"]),
                        OsVersion = ds.Rows[i]["OsVersion"].ToString(),
                    });
                }

                sQr = "";
                sQr = "select sp.splPlaylistId, sp.splPlaylistName as pName,sp.Formatid, sf.FormatName from tbInstantPlayPlaylist ip";
                sQr = sQr + " inner join tbSpecialPlaylists sp on sp.splPlaylistId = ip.splPlaylistId ";
                sQr = sQr + " inner join tbSpecialFormat sf on sf.FormatId = sp.Formatid ";
                sQr = sQr + " where ip.TokenId = " + data.tokenId + " ";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstAPKPlaylist.Add(new ResTokenPlaylistSch()
                    {
                        id = "0",
                        formatName = ds.Rows[i]["FormatName"].ToString(),
                        playlistName = ds.Rows[i]["pName"].ToString(),
                        StartTime = "",
                        EndTime = "",
                        WeekDay = "",
                        formatid = ds.Rows[i]["formatid"].ToString(),
                        splPlaylistId = ds.Rows[i]["splPlaylistid"].ToString(),
                    });
                }



                clsResult.lstPlaylistSch = lstPlaylist;
                clsResult.lstAds = lstAd;
                clsResult.lstPrayer = lstPra;
                clsResult.lstTokenData = lstTokend;
                clsResult.APKPlaylist = lstAPKPlaylist;
                con.Close();
                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }

        public ResResponce SaveTokenInformation(ReqSaveTokenInfo data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                var DispenserAlert = "";
                foreach (var iAlert in data.DispenserAlert)
                {
                    if (DispenserAlert == "")
                    {
                        DispenserAlert = iAlert.id.ToString();
                    }
                    else
                    {
                        DispenserAlert = DispenserAlert + "," + iAlert.id.ToString();
                    }
                }

                SqlCommand cmd = new SqlCommand("spTokenInformation", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Tokenid", SqlDbType.BigInt));
                cmd.Parameters["@Tokenid"].Value = data.Tokenid;

                cmd.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.BigInt));
                cmd.Parameters["@CountryId"].Value = data.country;

                cmd.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
                cmd.Parameters["@StateId"].Value = data.state;

                cmd.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
                cmd.Parameters["@CityId"].Value = data.city;

                cmd.Parameters.Add(new SqlParameter("@StreetName", SqlDbType.VarChar));
                cmd.Parameters["@StreetName"].Value = data.street;

                cmd.Parameters.Add(new SqlParameter("@Location", SqlDbType.VarChar));
                cmd.Parameters["@Location"].Value = data.location;


                cmd.Parameters.Add(new SqlParameter("@PersonName", SqlDbType.VarChar));
                cmd.Parameters["@PersonName"].Value = data.personName;

                if (data.chkType == "SF")
                {
                    cmd.Parameters.Add(new SqlParameter("@Store", SqlDbType.Int));
                    cmd.Parameters["@Store"].Value = "1";
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@Store", SqlDbType.Int));
                    cmd.Parameters["@Store"].Value = "0";
                }
                if (data.chkType == "Stream")
                {
                    cmd.Parameters.Add(new SqlParameter("@Stream", SqlDbType.Int));
                    cmd.Parameters["@Stream"].Value = "1";
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@Stream", SqlDbType.Int));
                    cmd.Parameters["@Stream"].Value = "0";
                }

                string p = string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(data.ExpiryDate));

                cmd.Parameters.Add(new SqlParameter("@ExpDate", SqlDbType.DateTime));
                cmd.Parameters["@ExpDate"].Value = string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(data.ExpiryDate));

                cmd.Parameters.Add(new SqlParameter("@IsStopControl", SqlDbType.Int));
                if (data.chkuserRights == "Lock")
                {
                    cmd.Parameters["@IsStopControl"].Value = "1";
                }
                else
                {
                    cmd.Parameters["@IsStopControl"].Value = "0";
                }

                cmd.Parameters.Add(new SqlParameter("@IsVedioActive", SqlDbType.Int));
                if (data.chkMediaType == "Audio")
                {
                    cmd.Parameters["@IsVedioActive"].Value = "0";
                }
                else
                {
                    cmd.Parameters["@IsVedioActive"].Value = "1";
                }

                cmd.Parameters.Add(new SqlParameter("@pType", SqlDbType.VarChar));
                cmd.Parameters["@pType"].Value = data.LicenceType;

                cmd.Parameters.Add(new SqlParameter("@lType", SqlDbType.VarChar));
                cmd.Parameters["@lType"].Value = data.PlayerType;

                cmd.Parameters.Add(new SqlParameter("@TokenNo", SqlDbType.VarChar));
                cmd.Parameters["@TokenNo"].Value = data.token;

                cmd.Parameters.Add(new SqlParameter("@ScheduleType", SqlDbType.VarChar));
                cmd.Parameters["@ScheduleType"].Value = data.ScheduleType;

                cmd.Parameters.Add(new SqlParameter("@IsIndicatorActive", SqlDbType.Int));
                cmd.Parameters["@IsIndicatorActive"].Value = Convert.ToByte(data.chkIndicator);

                cmd.Parameters.Add(new SqlParameter("@groupId", SqlDbType.Int));
                cmd.Parameters["@groupId"].Value = data.GroupId;

                cmd.Parameters.Add(new SqlParameter("@Rotation", SqlDbType.Int));
                cmd.Parameters["@Rotation"].Value = data.Rotation;

                cmd.Parameters.Add(new SqlParameter("@MediaType", SqlDbType.VarChar));
                cmd.Parameters["@MediaType"].Value = data.chkMediaType.Trim();

                cmd.Parameters.Add(new SqlParameter("@CommunicationType", SqlDbType.VarChar));
                cmd.Parameters["@CommunicationType"].Value = data.CommunicationType.Trim();

                cmd.Parameters.Add(new SqlParameter("@DeviceType", SqlDbType.VarChar));
                cmd.Parameters["@DeviceType"].Value = data.DeviceType.Trim();

                cmd.Parameters.Add(new SqlParameter("@DispenserAlert", SqlDbType.NVarChar));
                cmd.Parameters["@DispenserAlert"].Value = DispenserAlert;

                cmd.Parameters.Add(new SqlParameter("@TotalShot", SqlDbType.Int));
                cmd.Parameters["@TotalShot"].Value = data.TotalShot;
                cmd.Parameters.Add(new SqlParameter("@AlertEmail", SqlDbType.NVarChar));
                cmd.Parameters["@AlertEmail"].Value = data.AlertMail;
                cmd.Parameters.Add(new SqlParameter("@IsShowShotToast", SqlDbType.Bit));
                cmd.Parameters["@IsShowShotToast"].Value = Convert.ToByte(data.IsShowShotToast);
                cmd.Parameters.Add(new SqlParameter("@OsVersion", SqlDbType.NVarChar));
                cmd.Parameters["@OsVersion"].Value = data.OsVersion;

                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }

        }
        public ResResponce ResetToken(ReqResetToken data)
        {

            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {

                string strDel = "";
                strDel = "update AMPlayerTokens set token='" + data.tokenCode + "',code=null where tokenid= " + data.tokenId;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                strDel = "";
                strDel = "delete from titlesinplaylists where playlistid in(select distinct playlistid from playlists where tokenid=" + data.tokenId + ")";
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                strDel = "";
                strDel = "delete from playlists where tokenid=" + data.tokenId;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }

        }
        public ResResponce UpdateTokenSchedule(ReqUpdateSchedule data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string PercentageValue = "0";
                if (string.IsNullOrEmpty(data.PercentageValue) == false)
                {
                    PercentageValue = data.PercentageValue.ToString();

                }
                string str = "";
                str = "update tbSpecialPlaylistSchedule set PercentageValue='"+ PercentageValue + "', StartTime='" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(data.ModifyStartTime)) + "', ";
                str = str + " EndTime ='" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(data.ModifyEndTime)) + "' where pschid= " + data.pschid;
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public List<ResCustomerList> FillCustomer(ReqFillCustomer data)
        {
            List<ResCustomerList> lstResult = new List<ResCustomerList>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                string str = "select distinct DFClients.DFClientID,CountryCodes.CountryName, DFClients.ClientName,isnull(DFClients.Email,'') as email,DFClients.orderno , DFClients.DealerNoTotalToken ,DFClients.DealerCode, max(tbdealerlogin.Expirydate) as Expirydate";
                str = str + " , isnull(DFClients.apikey,'') as apikey , isnull(DFClients.IsTemplateActive,'0') as IsTemplateActive from DFClients inner join CountryCodes on DFClients.CountryCode= CountryCodes.CountryCode ";
                str = str + " inner join tbdealerlogin on DFClients.DFClientID= tbdealerlogin.DFClientID  ";
                str = str + " where DFClients.IsDealer=1 and  (DFClients.dbtype='" + data.DBType + "' or DFClients.dbtype='Both') ";
                str = str + " group by DFClients.DFClientID,CountryCodes.CountryName, DFClients.ClientName,DFClients.Email, DFClients.orderno , DFClients.DealerNoTotalToken ,DFClients.DealerCode , DFClients.apikey , DFClients.IsTemplateActive";
                str = str + " order by DFClientID desc ";

                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstResult.Add(new ResCustomerList()
                    {
                        id = ds.Rows[i]["DFClientID"].ToString(),
                        countryName = ds.Rows[i]["CountryName"].ToString(),
                        customerCode = ds.Rows[i]["DealerCode"].ToString(),
                        customerName = ds.Rows[i]["ClientName"].ToString(),
                        customerEmail = ds.Rows[i]["email"].ToString(),
                        totalToken = ds.Rows[i]["DealerNoTotalToken"].ToString(),
                        expiryDate = string.Format("{0:dd/MMM/yyyy}", ds.Rows[i]["Expirydate"]),
                        Key = ds.Rows[i]["apikey"].ToString(),
                        IsTemplateActive = ds.Rows[i]["IsTemplateActive"].ToString(),
                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }

        public ResResponce SaveCustomer(RegCustomer data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";

            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                Int32 SaveDfClientId = 0;
                Int32 User_id = 0;
                Int32 OldToken = 0;
                int IsMain = 0;
                int IsSub = 0;
                if (data.CustomerType == "MainCustomer")
                {
                    IsMain = 1;
                }
                if (data.CustomerType == "SubCustomer")
                {
                    IsSub = 1;
                }
                string ContentType = "";
                if (string.IsNullOrEmpty(data.ContentType) == true)
                {
                    ContentType = "Both";
                }
                else
                {
                    ContentType = data.ContentType;
                }
                if (string.IsNullOrEmpty(data.DfClientId) == true)
                {
                    SqlCommand cmdIns = new SqlCommand("sp_DealerRegistration", con);
                    cmdIns.CommandType = CommandType.StoredProcedure;

                    cmdIns.Parameters.Add(new SqlParameter("@InClientName", SqlDbType.VarChar));
                    cmdIns.Parameters["@InClientName"].Value = data.customerName;

                    cmdIns.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
                    cmdIns.Parameters["@email"].Value = data.customerEmail;

                    cmdIns.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.VarChar));
                    cmdIns.Parameters["@Orderno"].Value = "A-" + DateTime.Now.Year.ToString();

                    cmdIns.Parameters.Add(new SqlParameter("@ResponsiblePersonName", SqlDbType.VarChar));
                    cmdIns.Parameters["@ResponsiblePersonName"].Value = data.personName;

                    cmdIns.Parameters.Add(new SqlParameter("@CountryCode", SqlDbType.BigInt));
                    cmdIns.Parameters["@CountryCode"].Value = data.countryName;

                    cmdIns.Parameters.Add(new SqlParameter("@StreetName", SqlDbType.VarChar));
                    cmdIns.Parameters["@StreetName"].Value = data.Street;

                    cmdIns.Parameters.Add(new SqlParameter("@CityName", SqlDbType.VarChar));
                    cmdIns.Parameters["@CityName"].Value = data.cityName;

                    cmdIns.Parameters.Add(new SqlParameter("@IsDealer", SqlDbType.Bit));
                    cmdIns.Parameters["@IsDealer"].Value = 1;

                    cmdIns.Parameters.Add(new SqlParameter("@DealerNoTotalToken", SqlDbType.Int));
                    cmdIns.Parameters["@DealerNoTotalToken"].Value = data.totalToken;

                    cmdIns.Parameters.Add(new SqlParameter("@DealerCode", SqlDbType.VarChar));
                    cmdIns.Parameters["@DealerCode"].Value = data.cCode.ToUpper();

                    cmdIns.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
                    cmdIns.Parameters["@CityId"].Value = data.cityName;

                    cmdIns.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
                    cmdIns.Parameters["@StateId"].Value = data.stateName;

                    cmdIns.Parameters.Add(new SqlParameter("@IsMainDealer", SqlDbType.Bit));
                    cmdIns.Parameters["@IsMainDealer"].Value = IsMain;

                    cmdIns.Parameters.Add(new SqlParameter("@Vatnumber", SqlDbType.VarChar));
                    cmdIns.Parameters["@Vatnumber"].Value = "Nill";

                    cmdIns.Parameters.Add(new SqlParameter("@IsSubDealer", SqlDbType.Bit));
                    cmdIns.Parameters["@IsSubDealer"].Value = IsSub;

                    cmdIns.Parameters.Add(new SqlParameter("@MainDealerId", SqlDbType.BigInt));
                    cmdIns.Parameters["@MainDealerId"].Value = data.MainCustomer;

                    cmdIns.Parameters.Add(new SqlParameter("@supportEmail", SqlDbType.VarChar));
                    cmdIns.Parameters["@supportEmail"].Value = data.supportEmail;

                    cmdIns.Parameters.Add(new SqlParameter("@supportPhoneNo", SqlDbType.VarChar));
                    cmdIns.Parameters["@supportPhoneNo"].Value = data.supportPhNo;

                    cmdIns.Parameters.Add(new SqlParameter("@dbType", SqlDbType.VarChar));
                    cmdIns.Parameters["@dbType"].Value = data.dbType;

                    cmdIns.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.NVarChar));
                    cmdIns.Parameters["@ContentType"].Value = ContentType;

                    cmdIns.Parameters.Add(new SqlParameter("@ApiKey", SqlDbType.NVarChar));
                    cmdIns.Parameters["@ApiKey"].Value = data.ApiKey.Trim();


                    con.Open();

                    SaveDfClientId = Convert.ToInt32(cmdIns.ExecuteScalar());

                    cmdIns = new SqlCommand();
                    cmdIns.Connection = con;
                    cmdIns.CommandText = "update DFClients set dealerdfclientid= " + Convert.ToInt32(SaveDfClientId) + "  where dfclientid= " + SaveDfClientId;
                    cmdIns.ExecuteNonQuery();
                }
                else
                {
                    string str = "";
                    str = "select dealernototaltoken  from DFClients where DFClientID= " + data.DfClientId;
                    SqlCommand cmdTok = new SqlCommand(str, con);
                    cmdTok.CommandType = System.Data.CommandType.Text;

                    SqlDataAdapter ad = new SqlDataAdapter(cmdTok);
                    DataTable ds = new DataTable();
                    ad.Fill(ds);
                    OldToken = Convert.ToInt32(ds.Rows[0]["dealernototaltoken"]);

                    SqlCommand cmdU = new SqlCommand("sp_DealerRegistration_Modify", con);
                    cmdU.CommandType = CommandType.StoredProcedure;

                    cmdU.Parameters.Add(new SqlParameter("@DFClientID", SqlDbType.BigInt));
                    cmdU.Parameters["@DFClientID"].Value = data.DfClientId;

                    cmdU.Parameters.Add(new SqlParameter("@ClientName", SqlDbType.VarChar));
                    cmdU.Parameters["@ClientName"].Value = data.customerName;

                    cmdU.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
                    cmdU.Parameters["@email"].Value = data.customerEmail;

                    cmdU.Parameters.Add(new SqlParameter("@ResponsiblePersonName", SqlDbType.VarChar));
                    cmdU.Parameters["@ResponsiblePersonName"].Value = data.personName;

                    cmdU.Parameters.Add(new SqlParameter("@CountryCode", SqlDbType.BigInt));
                    cmdU.Parameters["@CountryCode"].Value = data.countryName;

                    cmdU.Parameters.Add(new SqlParameter("@StreetName", SqlDbType.VarChar));
                    cmdU.Parameters["@StreetName"].Value = data.Street;

                    cmdU.Parameters.Add(new SqlParameter("@CityName", SqlDbType.VarChar));
                    cmdU.Parameters["@CityName"].Value = data.cityName;


                    cmdU.Parameters.Add(new SqlParameter("@DealerNoTotalToken", SqlDbType.Int));
                    cmdU.Parameters["@DealerNoTotalToken"].Value = data.totalToken;

                    cmdU.Parameters.Add(new SqlParameter("@DealerCode", SqlDbType.VarChar));
                    cmdU.Parameters["@DealerCode"].Value = data.cCode.ToUpper();

                    cmdU.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
                    cmdU.Parameters["@CityId"].Value = data.cityName;

                    cmdU.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
                    cmdU.Parameters["@StateId"].Value = data.stateName;

                    cmdU.Parameters.Add(new SqlParameter("@IsMainDealer", SqlDbType.Bit));
                    cmdU.Parameters["@IsMainDealer"].Value = IsMain;

                    cmdU.Parameters.Add(new SqlParameter("@Vatnumber", SqlDbType.VarChar));
                    cmdU.Parameters["@Vatnumber"].Value = "Nill";

                    cmdU.Parameters.Add(new SqlParameter("@IsSubDealer", SqlDbType.Bit));
                    cmdU.Parameters["@IsSubDealer"].Value = IsSub;

                    cmdU.Parameters.Add(new SqlParameter("@MainDealerId", SqlDbType.BigInt));
                    cmdU.Parameters["@MainDealerId"].Value = data.MainCustomer;

                    cmdU.Parameters.Add(new SqlParameter("@supportEmail", SqlDbType.VarChar));
                    cmdU.Parameters["@supportEmail"].Value = data.supportEmail;

                    cmdU.Parameters.Add(new SqlParameter("@supportPhoneNo", SqlDbType.VarChar));
                    cmdU.Parameters["@supportPhoneNo"].Value = data.supportPhNo;
                    cmdU.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.NVarChar));
                    cmdU.Parameters["@ContentType"].Value = ContentType;

                    cmdU.Parameters.Add(new SqlParameter("@ApiKey", SqlDbType.NVarChar));
                    cmdU.Parameters["@ApiKey"].Value = data.ApiKey.Trim();

                    con.Open();
                    cmdU.ExecuteNonQuery();
                }
                //================== SaveDealerLogin

                SqlCommand cmd = new SqlCommand("sp_DealerLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@SaveType", SqlDbType.VarChar));
                if (string.IsNullOrEmpty(data.DfClientId) == true)
                {
                    cmd.Parameters["@SaveType"].Value = "Save";
                }
                else
                {
                    cmd.Parameters["@SaveType"].Value = "Modify";
                    SaveDfClientId = Convert.ToInt32(data.DfClientId);
                }

                cmd.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.BigInt));
                if (string.IsNullOrEmpty(data.LoginId) == true)
                {
                    cmd.Parameters["@LoginId"].Value = 0;
                }
                else
                {
                    cmd.Parameters["@LoginId"].Value = data.LoginId;
                }

                cmd.Parameters.Add(new SqlParameter("@LoginName", SqlDbType.VarChar));
                cmd.Parameters["@LoginName"].Value = data.customerEmail;

                cmd.Parameters.Add(new SqlParameter("@DfClientId", SqlDbType.BigInt));
                cmd.Parameters["@DfClientId"].Value = SaveDfClientId;

                cmd.Parameters.Add(new SqlParameter("@LoginPassword", SqlDbType.VarChar));
                cmd.Parameters["@LoginPassword"].Value = "Player!@#" + SaveDfClientId + "player";

                cmd.Parameters.Add(new SqlParameter("@ExpiryDate", SqlDbType.DateTime));
                cmd.Parameters["@ExpiryDate"].Value = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.expiryDate));

                cmd.Parameters.Add(new SqlParameter("@DealerCode", SqlDbType.VarChar));
                cmd.Parameters["@DealerCode"].Value = data.cCode.ToUpper();

                cmd.Parameters.Add(new SqlParameter("@DamTotalToken", SqlDbType.Int));
                cmd.Parameters["@DamTotalToken"].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@CopyrightTotalToken", SqlDbType.Int));
                cmd.Parameters["@CopyrightTotalToken"].Value = data.totalToken;
                cmd.Parameters.Add(new SqlParameter("@SanjivaniTotalToken", SqlDbType.Int));
                cmd.Parameters["@SanjivaniTotalToken"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsDam", SqlDbType.Bit));
                cmd.Parameters["@IsDam"].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@IsCopyright", SqlDbType.Bit));
                cmd.Parameters["@IsCopyright"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@IsSanjivani", SqlDbType.Bit));
                cmd.Parameters["@IsSanjivani"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@AsianTotalToken", SqlDbType.Int));
                cmd.Parameters["@AsianTotalToken"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsAsian", SqlDbType.Bit));
                cmd.Parameters["@IsAsian"].Value = 0;

                cmd.ExecuteNonQuery();
                //======================================

                if (string.IsNullOrEmpty(data.DfClientId) == true)
                {

                    //===================== SaveTokenUser
                    SqlCommand cmdUS = new SqlCommand("InsertUsers", con);
                    cmdUS.CommandType = CommandType.StoredProcedure;

                    cmdUS.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar));
                    cmdUS.Parameters["@UserName"].Value = data.customerName.Substring(3, data.customerName.Length - 3);

                    cmdUS.Parameters.Add(new SqlParameter("@UserEmail", SqlDbType.VarChar));
                    cmdUS.Parameters["@UserEmail"].Value = data.customerEmail;

                    cmdUS.Parameters.Add(new SqlParameter("@NoofToken", SqlDbType.BigInt));
                    cmdUS.Parameters["@NoofToken"].Value = data.totalToken;

                    cmdUS.Parameters.Add(new SqlParameter("@PlayerType", SqlDbType.VarChar));
                    cmdUS.Parameters["@PlayerType"].Value = "Desktop";

                    cmdUS.Parameters.Add(new SqlParameter("@ClientID", SqlDbType.BigInt));
                    cmdUS.Parameters["@ClientID"].Value = SaveDfClientId;

                    cmdUS.Parameters.Add(new SqlParameter("@Street", SqlDbType.VarChar));
                    cmdUS.Parameters["@Street"].Value = data.Street;

                    cmdUS.Parameters.Add(new SqlParameter("@Cityname", SqlDbType.VarChar));
                    cmdUS.Parameters["@Cityname"].Value = data.cityName;

                    cmdUS.Parameters.Add(new SqlParameter("@TeamviewerId", SqlDbType.VarChar));
                    cmdUS.Parameters["@TeamviewerId"].Value = "0";

                    cmdUS.Parameters.Add(new SqlParameter("@TvPassword", SqlDbType.VarChar));
                    cmdUS.Parameters["@TvPassword"].Value = "0";

                    cmdUS.Parameters.Add(new SqlParameter("@MusicType", SqlDbType.VarChar));
                    cmdUS.Parameters["@MusicType"].Value = "Copyright";

                    cmdUS.Parameters.Add(new SqlParameter("@Vatnumber", SqlDbType.VarChar));
                    cmdUS.Parameters["@Vatnumber"].Value = "Nill";

                    cmdUS.Parameters.Add(new SqlParameter("@Location", SqlDbType.VarChar));
                    cmdUS.Parameters["@Location"].Value = data.cityName;

                    cmdUS.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.BigInt));
                    cmdUS.Parameters["@CountryId"].Value = data.countryName;

                    cmdUS.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
                    cmdUS.Parameters["@StateId"].Value = data.stateName;

                    cmdUS.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
                    cmdUS.Parameters["@CityId"].Value = data.cityName;

                    cmdUS.Parameters.Add(new SqlParameter("@PlayerVersion", SqlDbType.VarChar));
                    cmdUS.Parameters["@PlayerVersion"].Value = "NativeCR";

                    User_id = Convert.ToInt32(cmdUS.ExecuteScalar());

                    //===================================
                }
                //=============================== SaveTokenGeneration


                int TotalTokenGenrate = Convert.ToInt32(data.totalToken);
                if (TotalTokenGenrate > OldToken)
                {
                    for (int i = 1; i <= (TotalTokenGenrate - OldToken); i++)
                    {
                        SqlCommand cmdToken = new SqlCommand("spDealer_AMTokensClient", con);
                        cmdToken.CommandType = CommandType.StoredProcedure;

                        cmdToken.Parameters.Add(new SqlParameter("@DFClientID", SqlDbType.BigInt));
                        cmdToken.Parameters["@DFClientID"].Value = SaveDfClientId;

                        cmdToken.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt));
                        cmdToken.Parameters["@UserId"].Value = User_id;

                        cmdToken.Parameters.Add(new SqlParameter("@InNumberofTitles", SqlDbType.BigInt));
                        cmdToken.Parameters["@InNumberofTitles"].Value = 5000;

                        cmdToken.Parameters.Add(new SqlParameter("@isCopyRight", SqlDbType.Int));
                        cmdToken.Parameters["@isCopyRight"].Value = 1;

                        cmdToken.Parameters.Add(new SqlParameter("@InDateExp", SqlDbType.DateTime));

                        cmdToken.Parameters["@InDateExp"].Value = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.expiryDate));

                        cmdToken.Parameters.Add(new SqlParameter("@IsDam", SqlDbType.Int));
                        cmdToken.Parameters["@IsDam"].Value = 0;

                        cmdToken.Parameters.Add(new SqlParameter("@DamExpiryDate", SqlDbType.DateTime));
                        cmdToken.Parameters["@DamExpiryDate"].Value = "01-01-1900";

                        cmdToken.Parameters.Add(new SqlParameter("@IsSanjivani", SqlDbType.Int));
                        cmdToken.Parameters["@IsSanjivani"].Value = 0;
                        cmdToken.Parameters.Add(new SqlParameter("@SanjivaniExpiryDate", SqlDbType.DateTime));
                        cmdToken.Parameters["@SanjivaniExpiryDate"].Value = "01-01-1900";
                        cmdToken.Parameters.Add(new SqlParameter("@IsFitness", SqlDbType.Int));
                        cmdToken.Parameters["@IsFitness"].Value = 0;

                        cmdToken.Parameters.Add(new SqlParameter("@FitnessExpiryDate", SqlDbType.DateTime));
                        cmdToken.Parameters["@FitnessExpiryDate"].Value = "01-01-1900";

                        cmdToken.Parameters.Add(new SqlParameter("@ServiceId", SqlDbType.Int));
                        cmdToken.Parameters["@ServiceId"].Value = 0;

                        cmdToken.Parameters.Add(new SqlParameter("@Dealercode", SqlDbType.VarChar));
                        cmdToken.Parameters["@Dealercode"].Value = data.cCode.ToUpper();

                        cmdToken.Parameters.Add(new SqlParameter("@IsAsian", SqlDbType.Int));
                        cmdToken.Parameters["@IsAsian"].Value = 0;
                        cmdToken.Parameters.Add(new SqlParameter("@AsianExpiryDate", SqlDbType.DateTime));
                        cmdToken.Parameters["@AsianExpiryDate"].Value = "01-01-1900";

                        cmdToken.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                        cmdToken.Parameters["@pVersion"].Value = "NativeCR";

                        cmdToken.ExecuteNonQuery();
                    }
                }
                else if (OldToken > TotalTokenGenrate)
                {
                    int delTotalToken = OldToken - TotalTokenGenrate;
                    string sQr = "";
                    sQr = "delete from AMPlayerTokens where tokenid in (select top (" + delTotalToken + ") TokenID  from AMPlayerTokens where code is null and ClientID=" + SaveDfClientId + " ORDER BY  tokenid desc)";
                    SqlCommand cmdDelete = new SqlCommand(sQr, con);
                    cmdDelete.CommandType = CommandType.Text;
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmdDelete.ExecuteNonQuery();
                    cmdDelete.Dispose();
                }

                //====================================================
                con.Close();
                if (string.IsNullOrEmpty(data.DfClientId) == true)
                {
                    result.Responce = SaveDfClientId.ToString();
                }
                else
                {
                    result.Responce = "1";
                }
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public RegCustomer EditClickCustomer(ReqTokenInfo data)
        {
            RegCustomer result = new RegCustomer();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string sQr = "";
                sQr = "select DFClients.DFClientID,CountryCode , ClientName,isnull(Email,'') as email,orderno,cityname,streetname ,DealerNoTotalToken,DFClients.DealerCode, ";
                sQr = sQr + " Stateid,cityId , isnull(IsMainDealer,0) as IsMainDealer,vatnumber , isnull(issubdealer,0) as isSubdealer, isnull(MainDealerId,0) as MainDealerId, isnull(supportEmail,'') as supportEmail,isnull(supportPhoneNo,'') as supportPhoneNo , tbdealerlogin.ExpiryDate, tbdealerlogin.loginid, isnull(DFClients.ResponsiblePersonName,'') as personName, isnull(contentType,'Both') as ContentType, isnull(APIKey,'') as apikey from DFClients inner join tbdealerlogin on tbdealerlogin.dfclientid= DFClients.dfclientid ";
                sQr = sQr + " where DFClients.DFClientID=" + data.clientId;

                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    result.countryName = ds.Rows[0]["CountryCode"].ToString();
                    result.cCode = ds.Rows[0]["DealerCode"].ToString();
                    result.stateName = ds.Rows[0]["Stateid"].ToString();
                    result.cityName = ds.Rows[0]["cityId"].ToString();
                    result.customerName = ds.Rows[0]["ClientName"].ToString();
                    result.customerEmail = ds.Rows[0]["email"].ToString();
                    result.totalToken = ds.Rows[0]["DealerNoTotalToken"].ToString();
                    result.expiryDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(ds.Rows[0]["ExpiryDate"]));
                    result.supportEmail = ds.Rows[0]["supportEmail"].ToString();
                    result.supportPhNo = ds.Rows[0]["supportPhoneNo"].ToString();
                    result.Street = ds.Rows[0]["streetname"].ToString();
                    result.DfClientId = ds.Rows[0]["DFClientID"].ToString();
                    result.LoginId = ds.Rows[0]["loginid"].ToString();
                    if (Convert.ToBoolean(ds.Rows[0]["IsMainDealer"]) == true)
                    {
                        result.CustomerType = "MainCustomer";
                    }
                    if (Convert.ToBoolean(ds.Rows[0]["isSubdealer"]) == true)
                    {
                        result.CustomerType = "SubCustomer";
                    }
                    result.MainCustomer = ds.Rows[0]["MainDealerId"].ToString();
                    result.personName = ds.Rows[0]["personName"].ToString();
                    result.ContentType = ds.Rows[0]["ContentType"].ToString();
                    result.ApiKey = ds.Rows[0]["apikey"].ToString();
                }


                con.Close();
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }
        public ResResponce DeleteCustomer(ReqTokenInfo data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                strDel = "delete from tbdealerlogin where dfclientid = " + data.clientId;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();


                strDel = "";
                strDel = "delete from DFClients where dfclientid = " + data.clientId;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();

                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {

                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResBestOf BestOf()
        {
            ResBestOf clsResult = new ResBestOf();
            List<ResBestPlaylist> lstBestPlaylist = new List<ResBestPlaylist>();
            List<ResSongList> lstSong = new List<ResSongList>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {

                string sQr = "select PlaylistID,ltrim(rtrim(name)) as pName from Playlists  where ispredefined=1 order by name";
                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                Boolean iCheck = false;
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        iCheck = true;
                    }
                    else
                    {
                        iCheck = false;
                    }
                    lstBestPlaylist.Add(new ResBestPlaylist()
                    {
                        id = ds.Rows[i]["PlaylistID"].ToString(),
                        playlistName = ds.Rows[i]["pName"].ToString(),
                        check = iCheck,
                    });
                }

                sQr = "SELECT TOP (500) Titles.TitleID, Titles.Title, Titles.Time, Artists.Name as ArtistName, Albums.Name AS AlbumName FROM Titles INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID where   mediaType='Audio' order by TitleID desc";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;

                ad = new SqlDataAdapter(cmd);
                ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstSong.Add(new ResSongList()
                    {
                        id = ds.Rows[i]["TitleID"].ToString(),
                        title = ds.Rows[i]["Title"].ToString(),
                        Length = ds.Rows[i]["Time"].ToString(),
                        Artist = ds.Rows[i]["ArtistName"].ToString(),
                        Album = ds.Rows[i]["AlbumName"].ToString(),

                    });
                }
                con.Close();
                clsResult.lstBestPlaylist = lstBestPlaylist;
                clsResult.lstSong = lstSong;
                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }

        public List<ResPlaylistSongList> PlaylistSong(ReqPlaylistSongList data)
        {
            List<ResPlaylistSongList> lstPlaylistSong = new List<ResPlaylistSongList>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string sQr = "";


                sQr = "";
                if (data.IsBestOffPlaylist == "Yes")
                {
                    sQr = "SELECT  Titles.TitleID, rtrim(ltrim(Titles.Title)) as Title, Titles.Time,rtrim(ltrim(Albums.Name)) AS AlbumName ,";
                    sQr = sQr + " Titles.TitleYear ,  rtrim(ltrim(Artists.Name)) as ArtistName,'' as Category  ,Titles.AlbumID, Titles.ArtistID, Titles.mediatype, '0' as srno , isnull(tbGenre.genre,'') as genre, isnull(Titles.label,'') as label, '5' as ImgTimeInterval FROM ((( TitlesInPlaylists  ";
                    sQr = sQr + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID )  ";
                    sQr = sQr + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ) ";
                    sQr = sQr + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ) ";
                    sQr = sQr + " LEFT OUTER JOIN tbGenre ON Titles.GenreId = tbGenre.GenreId  ";
                    sQr = sQr + " where TitlesInPlaylists.PlaylistID= " + data.playlistid;
                }
                else
                {
                    sQr = "SELECT  Titles.TitleID, rtrim(ltrim(Titles.Title)) as Title, Titles.Time,rtrim(ltrim(Albums.Name)) AS AlbumName ,";
                    sQr = sQr + " Titles.TitleYear ,  rtrim(ltrim(Artists.Name)) as ArtistName , isnull(tbGenre.genre,'') as genre, isnull(Titles.tempo,'') as Tempo  , isnull(acategory,'') as Category ,Titles.AlbumID, Titles.ArtistID, Titles.mediatype, tbSpecialPlaylists_Titles.srno, isnull(Titles.label,'') as label, tbSpecialPlaylists_Titles.id ,";
                    sQr = sQr + " iif(tbSpecialPlaylists_Titles.ImgTimeInterval = 0, 5, isnull(tbSpecialPlaylists_Titles.ImgTimeInterval, 5)) as ImgTimeInterval FROM   tbSpecialPlaylists_Titles  ";
                    sQr = sQr + " INNER JOIN Titles ON tbSpecialPlaylists_Titles.TitleID = Titles.TitleID   ";
                    sQr = sQr + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID  ";
                    sQr = sQr + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID  ";
                    sQr = sQr + " LEFT OUTER JOIN tbGenre ON Titles.GenreId = tbGenre.GenreId  ";
                    sQr = sQr + " where tbSpecialPlaylists_Titles.splPlaylistId= " + data.playlistid + " ";

                    sQr = sQr + " order by tbSpecialPlaylists_Titles.srno ";

                }

                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                string url = "", mtypeFormat = "";
                string btnImgAll = "", isImgFind = "No";
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (ds.Rows[i]["MediaType"].ToString().Trim() == "Audio")
                    {
                        mtypeFormat = ".mp3";
                    }
                    if (ds.Rows[i]["MediaType"].ToString().Trim() == "Video")
                    {
                        mtypeFormat = ".mp4";
                    }
                    if (ds.Rows[i]["MediaType"].ToString().Trim() == "Image")
                    {
                        isImgFind = "Yes";
                        mtypeFormat = ".jpg";
                        if (btnImgAll == "")
                        {
                            btnImgAll = "All";
                        }
                        else if (btnImgAll == "All")
                        {
                            btnImgAll = "Hide";
                        }
                    }

                    url = "http://api.advikon.com/mp3files/" + ds.Rows[i]["titleId"].ToString() + mtypeFormat;

                    lstPlaylistSong.Add(new ResPlaylistSongList()
                    {
                        id = ds.Rows[i]["TitleID"].ToString(),
                        title = ds.Rows[i]["Title"].ToString(),
                        Length = ds.Rows[i]["Time"].ToString(),
                        Artist = ds.Rows[i]["ArtistName"].ToString(),
                        Album = ds.Rows[i]["AlbumName"].ToString(),
                        category = ds.Rows[i]["Category"].ToString(),
                        ArtistId = ds.Rows[i]["ArtistID"].ToString(),
                        AlbumId = ds.Rows[i]["AlbumID"].ToString(),
                        MediaType = ds.Rows[i]["MediaType"].ToString(),
                        TitleIdLink = url,
                        SrNo = ds.Rows[i]["srno"].ToString(),
                        GenreName = ds.Rows[i]["genre"].ToString(),
                        Label = ds.Rows[i]["label"].ToString(),
                        sId = ds.Rows[i]["id"].ToString(),
                        ImageTimeInterval = ds.Rows[i]["ImgTimeInterval"].ToString(),
                        ImgAllBtn = btnImgAll,
                        isImgFind = isImgFind,
                    });
                }
                con.Close();
                return lstPlaylistSong;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return lstPlaylistSong;
            }
        }

        public ResResponce SaveBestPlaylist(ReqSaveBestPlaylist data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                if (string.IsNullOrEmpty(data.id) == true)
                {

                    SqlCommand cmd = new SqlCommand("InsertPlayListsNew", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt));
                    cmd.Parameters["@UserID"].Value = 2;

                    cmd.Parameters.Add(new SqlParameter("@IsPredefined", SqlDbType.Bit));
                    cmd.Parameters["@IsPredefined"].Value = 1;

                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Name"].Value = data.plName.Trim();

                    cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Summary"].Value = data.plName.Trim();

                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Description"].Value = " ";

                    cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
                    cmd.Parameters["@TokenId"].Value = 0;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("UpdateUserPlayLists", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PlayListID", SqlDbType.BigInt));
                    cmd.Parameters["@PlayListID"].Value = data.id;

                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Name"].Value = data.plName.Trim();
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                con.Close();

                clsResult.Responce = "1";
                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }


        public ResResponce AddPlaylistSong(ReqAddPlaylistSong data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                var pid = data.playlistid[0];


                if (data.AddSongFrom == "BestOf")
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("PlaylistID", typeof(int));
                    dt.Columns.Add("TitleID", typeof(int));

                    foreach (string iTitle in data.titleid)
                    {
                        string sQr = "select * from TitlesInPlaylists where PlaylistID=" + pid + " and TitleID=" + iTitle;
                        SqlCommand cmd = new SqlCommand(sQr, con);
                        cmd.CommandType = System.Data.CommandType.Text;
                        SqlDataAdapter ad = new SqlDataAdapter(cmd);
                        DataTable ds = new DataTable();
                        ad.Fill(ds);
                        if (ds.Rows.Count == 0)
                        {
                            DataRow nr = dt.NewRow();
                            nr["Playlistid"] = pid;
                            nr["TitleID"] = iTitle;
                            dt.Rows.Add(nr);
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                        {
                            SqlBulkCopyColumnMapping mapID =
                             new SqlBulkCopyColumnMapping("PlaylistID", "PlaylistID");
                            bulkCopy.ColumnMappings.Add(mapID);

                            SqlBulkCopyColumnMapping mapMumber =
                                new SqlBulkCopyColumnMapping("TitleID", "TitleID");
                            bulkCopy.ColumnMappings.Add(mapMumber);

                            //SqlBulkCopyColumnMapping mapName =
                            //   new SqlBulkCopyColumnMapping("Sequence", "Sequence");
                            //bulkCopy.ColumnMappings.Add(mapName);

                            bulkCopy.DestinationTableName = "dbo.TitlesInPlaylists";

                            if (con.State == ConnectionState.Open) con.Close();
                            con.Open();
                            bulkCopy.WriteToServer(dt);
                            con.Close();

                        }
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("splPlaylistId", typeof(int));
                    dt.Columns.Add("titleId", typeof(int));
                    dt.Columns.Add("srNo", typeof(int));
                    foreach (string iTitle in data.titleid)
                    {
                        if (data.IsDuplicate == false)
                        {
                            string sQr = "select * from tbSpecialPlaylists_Titles where splPlaylistId=" + pid + " and TitleID=" + iTitle;
                            SqlCommand cmd = new SqlCommand(sQr, con);
                            cmd.CommandType = System.Data.CommandType.Text;
                            SqlDataAdapter ad = new SqlDataAdapter(cmd);
                            DataTable ds = new DataTable();
                            ad.Fill(ds);
                            if (ds.Rows.Count == 0)
                            {
                                DataRow nr = dt.NewRow();
                                nr["splPlaylistId"] = pid;
                                nr["titleId"] = iTitle;
                                nr["srNo"] = dt.Rows.Count + 1;
                                dt.Rows.Add(nr);
                            }
                        }
                        if (data.IsDuplicate == true)
                        {
                            int sr = 0;
                            sr++;
                            DataRow nr = dt.NewRow();
                            nr["splPlaylistId"] = pid;
                            nr["titleId"] = iTitle;
                            nr["srNo"] = sr;
                            dt.Rows.Add(nr);
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                        {
                            SqlBulkCopyColumnMapping mapID =
                             new SqlBulkCopyColumnMapping("splPlaylistId", "splPlaylistId");
                            bulkCopy.ColumnMappings.Add(mapID);

                            SqlBulkCopyColumnMapping mapMumber =
                                new SqlBulkCopyColumnMapping("titleId", "titleId");
                            bulkCopy.ColumnMappings.Add(mapMumber);

                            SqlBulkCopyColumnMapping mapName =
                             new SqlBulkCopyColumnMapping("srNo", "srNo");
                            bulkCopy.ColumnMappings.Add(mapName);

                            bulkCopy.DestinationTableName = "dbo.tbSpecialPlaylists_Titles";

                            if (con.State == ConnectionState.Open) con.Close();
                            con.Open();
                            bulkCopy.WriteToServer(dt);
                            con.Close();

                        }
                    }
                }



                clsResult.Responce = "1";
                return clsResult;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }
        public List<ResSongList> CommanSearch(ReqCommonSearch data)
        {

            List<ResSongList> lstSong = new List<ResSongList>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string[] genreid = { "325", "324", "297", "303" };

                string sQr = "";
                sQr = " select  top 500 TitleID, ltrim(Title) as Title,Time, ltrim(ArtistName) as ArtistName, ltrim(AlbumName) as AlbumName , isnull(genre,'') as genre, Tempo ,titleyear,Category,AlbumID, ArtistID, mediatype , label,fname ,EngeryLevel, bpm, rdate, lang  , dfclientid from( ";
                sQr = sQr + " SELECT  Titles.TitleID, Titles.Title,Titles.Time, Artists.Name as ArtistName, Albums.Name AS AlbumName, tbGenre.genre, isnull(Titles.tempo,'') as Tempo,Titles.titleyear , isnull(acategory,'') as Category ,Titles.AlbumID, Titles.ArtistID, Titles.mediatype,  isnull(Titles.label ,'') as label,isnuLL(tbFolder.folderName,'') as fName , isnull(Titles.EngeryLevel,0) as EngeryLevel, isnull(Titles.BPM,'') as bpm, isnull(Titles.ReleaseDate,'') as rdate, isnull(Titles.language,'') as lang, isnull(Titles.dfclientid,0) as dfclientid FROM Titles ";
                sQr = sQr + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID  ";
                sQr = sQr + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID  ";
                sQr = sQr + " LEFT OUTER JOIN tbGenre ON Titles.GenreId = tbGenre.GenreId  ";
                sQr = sQr + " LEFT OUTER JOIN tbFolder ON Titles.folderId = tbFolder.folderId  ";
                sQr = "";
                if (data.searchType == "title")
                {
                    //sQr = "spSearch_Title_Comman '" + data.searchText + "','" + data.mediaType + "', " + data.IsRf + "," + Convert.ToByte(data.IsExplicit) + ",'" + data.DBType + "'";
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and  Titles.title like ''%" + data.searchText + "%''  and Titles.mediatype=''" + data.mediaType + "'' and IsRoyaltyFree= " + data.IsRf + " ";
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }


                    sQr = sQr + " ) as d  order by title     ";
                }
                else if (data.searchType == "artist")
                {
                    //sQr = "spSearch_Artist_Comman '" + data.searchText + "','" + data.mediaType + "', " + data.IsRf + "," + Convert.ToByte(data.IsExplicit) + ",'" + data.DBType + "'";
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Artists.Name like ''%" + data.searchText + "%''  and Titles.mediaType=''" + data.mediaType + "'' and IsRoyaltyFree= " + data.IsRf + " ";
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }

                    sQr = sQr + " ) as d  order by ArtistName     ";
                }
                else if (data.searchType == "album")
                {

                    sQr = "spSearch_Album_spl " + data.searchText + " ,'" + data.mediaType + "', " + data.IsRf + "," + Convert.ToByte(data.IsExplicit) + ",'" + data.DBType + "'";
                    sQr = "";
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Titles.AlbumID =''" + data.searchText + "''  and Titles.mediaType=''" + data.mediaType + "'' and IsRoyaltyFree= " + data.IsRf + " ";
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }
                    sQr = sQr + " ) as d  order by TitleID desc     ";
                }
                else if (data.searchType == "Genre")
                {
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Titles.GenreId= " + data.searchText + " and Titles.mediaType=''" + data.mediaType + "''  ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";

                    }
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";

                    if (Array.Exists(genreid, element => element == data.searchText) == true)
                    {
                        //sQr = sQr + " and (Titles.dfclientid=" + data.ClientId + ") ";
                        if (data.IsAdmin == false)
                        {
                            sQr = sQr + " and (Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                        }
                        else
                        {
                            sQr = sQr + " and (Titles.dfclientid=" + data.ClientId + ") ";
                        }
                    }
                    else
                    {
                        if (data.IsAdmin == false)
                        {
                            sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                        }
                        else
                        {
                            sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                        }
                    }



                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);

                    sQr = sQr + " ) as d  order by TitleID desc    ";
                }
                else if (data.searchType == "BPM")
                {
                    var MT = data.searchText.Split('-');
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and cast(Titles.BPM as int) between " + MT[0] + " and " + MT[1] + "  and Titles.mediaType=''" + data.mediaType + "''  ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);

                    sQr = sQr + " ) as d  order by cast(BPM as int) ";
                }
                else if (data.searchType == "ReleaseDate")
                {
                    var MT = data.searchText.Split('-');
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and month(Titles.ReleaseDate)=" + MT[0] + " and year(Titles.ReleaseDate)=" + MT[1] + " and Titles.mediaType=''" + data.mediaType + "''  ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);

                    sQr = sQr + " ) as d  order by TitleID desc    ";
                }
                else if (data.searchType == "EngeryLevel")
                {
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Titles.EngeryLevel= " + data.searchText + " and Titles.mediaType=''" + data.mediaType + "''  ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);

                    sQr = sQr + " ) as d  order by EngeryLevel desc   ";
                }
                else if (data.searchType == "NewVibe")
                {
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Titles.TitleYear between " + DateTime.Now.AddYears(-1).Year + " and  " + DateTime.Now.Year + " and Titles.mediaType=''" + data.mediaType + "''  ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    if (data.ContentType == "Signage")
                    {
                        sQr = sQr + " and Titles.genreid in(303,297, 325,324)";
                    }
                    if (data.ContentType == "MusicMedia")
                    {
                        sQr = sQr + " and Titles.genreid not in(325,324)";
                    }
                    sQr = sQr + " ) as d  order by TitleID desc    ";
                }
                else if (data.searchType == "Label")
                {
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Titles.label= ''" + data.searchText + "'' and Titles.mediaType=''" + data.mediaType + "'' ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }

                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    sQr = sQr + " ) as d  order by TitleID desc     ";
                }
                else if (data.searchType == "Category")
                {
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Titles.acategory= ''" + data.searchText + "''  and Titles.mediaType=''" + data.mediaType + "'' and IsRoyaltyFree= " + data.IsRf + " ";
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }

                    //   sQr = sQr + " ) as d  order by isnull(genre,'')     ";
                }
                else if (data.searchType == "Language")
                {
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Titles.Language= ''" + data.searchText + "''  and Titles.mediaType=''" + data.mediaType + "'' and IsRoyaltyFree= " + data.IsRf + " ";
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }

                    sQr = sQr + " ) as d  order by TitleID desc     ";

                }
                else if (data.searchType == "Year")
                {
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Titles.TitleYear= " + data.searchText + "  and Titles.mediaType=''" + data.mediaType + "'' and IsRoyaltyFree= " + data.IsRf + " ";
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }

                    if (data.IsAdmin == false)
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                    }
                    else
                    {
                        sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                    }

                    sQr = sQr + " ) as d  order by TitleID desc    ";

                }
                else if (data.searchType == "BestOf")
                {
                    sQr = "";
                    sQr = " SELECT Titles.TitleID, Titles.Title,Titles.Time, Artists.Name as ArtistName, Albums.Name AS AlbumName, tbGenre.genre, isnull(Titles.tempo,'') as Tempo,Titles.titleyear , isnull(acategory,'') as Category,Titles.AlbumID, Titles.ArtistID, Titles.mediatype ";
                    sQr = sQr + " FROM TitlesInPlaylists  ";
                    sQr = sQr + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID   ";
                    sQr = sQr + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID  INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ";
                    sQr = sQr + " LEFT OUTER JOIN tbGenre ON Titles.GenreId = tbGenre.GenreId  ";
                    sQr = sQr + " where TitlesInPlaylists.PlaylistID = '" + data.searchText + "' and Titles.mediaType=''" + data.mediaType + "''";
                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    sQr = sQr + "  order by Titles.TitleID desc    ";
                }
                else if (data.searchType == "Folder")
                {
                    sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and Titles.folderId= " + data.searchText + " and Titles.mediaType=''" + data.mediaType + "'' ";
                    if (data.mediaType != "Image")
                    {
                        sQr = sQr + " and IsRoyaltyFree= " + data.IsRf + " ";
                    }
                    sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";
                    if (Array.Exists(genreid, element => element == data.searchText) == true)
                    {
                        //sQr = sQr + " and (Titles.dfclientid=" + data.ClientId + ") ";
                        if (data.IsAdmin == false)
                        {
                            sQr = sQr + " and (Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                        }
                        else
                        {
                            sQr = sQr + " and (Titles.dfclientid=" + data.ClientId + ") ";
                        }
                    }
                    else
                    {
                        if (data.IsAdmin == false)
                        {
                            sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                        }
                        else
                        {
                            sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                        }
                    }


                    sQr = sQr + " and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit);
                    sQr = sQr + " ) as d  order by TitleID desc   ";
                }

                if (data.searchType != "NewVibe")
                {
                    if (string.IsNullOrEmpty(data.searchText) == true)
                    {
                        sQr = "SELECT TOP (500) Titles.TitleID, Titles.Title, Titles.Time, Artists.Name as ArtistName, Albums.Name AS AlbumName, isnull(Titles.tempo,'') as Tempo,isnull(tbGenre.genre,'') as genre , Titles.titleyear ,isnull(acategory,'') as Category, Titles.AlbumID, Titles.ArtistID, Titles.mediatype,  isnull(Titles.label ,'') as label ,isnuLL(tbFolder.folderName,'') as fName , isnull(Titles.EngeryLevel,0) as EngeryLevel, isnull(Titles.BPM,'') as bpm, isnull(Titles.ReleaseDate,'') as rdate, isnull(Titles.language,'') as lang, titles.titleyear, isnull(Titles.dfclientid,0) as dfclientid FROM Titles INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID LEFT OUTER JOIN tbGenre ON Titles.GenreId = tbGenre.GenreId  LEFT OUTER JOIN tbFolder ON Titles.folderId = tbFolder.folderId  ";
                        sQr = "";

                        sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and   Titles.mediaType=''" + data.mediaType + "'' and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit) + " ";
                        sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";

                        if (data.IsAdmin == false)
                        {
                            sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                        }
                        else
                        {
                            sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                        }
                        if (data.mediaType != "Image")
                        {
                            sQr = sQr + " and IsRoyaltyFree=" + data.IsRf + " ";
                        }
                        if (data.ContentType == "Signage")
                        {
                            sQr = sQr + " and Titles.genreid in(303,297, 325,324)";
                        }
                        if (data.ContentType == "MusicMedia")
                        {
                            sQr = sQr + " and Titles.genreid not in(325,324)";
                        }
                        sQr = sQr + " ) as d  order by TitleID desc   ";

                        //sQr = sQr + " order by isnull(tbGenre.genre,'')";
                    }
                }
                string str = "Pagination_CommonSearch " + data.PageNo + ", '" + sQr + "'";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                var format = "";
                string url = "";
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (ds.Rows[i]["MediaType"].ToString() == "Audio")
                    {
                        format = ".mp3";
                    }
                    if (ds.Rows[i]["MediaType"].ToString() == "Video")
                    {
                        format = ".mp4";
                    }
                    if (ds.Rows[i]["MediaType"].ToString() == "Image")
                    {
                        format = ".jpg";
                    }
                    url = "http://api.advikon.com/mp3files/" + ds.Rows[i]["titleId"].ToString() + format;
                    var rDate = "";
                    if (string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(ds.Rows[i]["rDate"])) == "01-Jan-1900")
                    {
                        rDate = "";
                    }
                    else
                    {
                        rDate = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(ds.Rows[i]["rDate"]));
                    }

                    lstSong.Add(new ResSongList()
                    {
                        check = false,
                        id = ds.Rows[i]["TitleID"].ToString(),
                        title = ds.Rows[i]["Title"].ToString().Trim(),
                        Length = ds.Rows[i]["Time"].ToString(),
                        Artist = ds.Rows[i]["ArtistName"].ToString().Trim(),
                        Album = ds.Rows[i]["AlbumName"].ToString().Trim(),
                        category = ds.Rows[i]["Category"].ToString(),
                        genreName = ds.Rows[i]["genre"].ToString(),
                        ArtistId = ds.Rows[i]["ArtistID"].ToString(),
                        AlbumId = ds.Rows[i]["AlbumID"].ToString(),
                        MediaType = ds.Rows[i]["MediaType"].ToString(),
                        TitleIdLink = url,
                        Label = ds.Rows[i]["label"].ToString(),
                        FolderName = ds.Rows[i]["fName"].ToString(),
                        EngeryLevel = ds.Rows[i]["EngeryLevel"].ToString(),
                        rDate = rDate,
                        BPM = ds.Rows[i]["BPM"].ToString(),
                        Language = ds.Rows[i]["lang"].ToString(),
                        titleyear = ds.Rows[i]["titleyear"].ToString(),
                        dfClientId = ds.Rows[i]["dfClientId"].ToString(),
                    });
                }
                con.Close();

                return lstSong;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return lstSong;
            }
        }

        public ResResponce DeleteTitle(ReqDeleteTitle data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                string tid = "";
                foreach (var item in data.titleid)
                {
                    if (tid == "")
                    {
                        tid = item;
                    }
                    else
                    {
                        tid = tid + "," + item;
                    }
                }

                strDel = "delete from tbSpecialPlaylists_Titles where TitleID in(" + tid + ") and splPlaylistId= " + data.playlistid;

                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public ResResponce SavePlaylist(ReqSavePlaylist data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string str = "";
                if (string.IsNullOrEmpty(data.id) == true)
                {
                    str = "select * from tbSpecialPlaylists where splplaylistname ='" + data.plName.ToString().Trim().ToLower() + "' and formatid= " + data.formatid;
                }
                else
                {
                    str = "select * from tbSpecialPlaylists where splplaylistid !="+ data.id + " and  splplaylistname ='" + data.plName.ToString().Trim().ToLower() + "' and formatid= " + data.formatid;
                }
                DataTable dtPl = new DataTable();
                    SqlCommand cmdPL = new SqlCommand(str, con);
                    cmdPL.CommandType = System.Data.CommandType.Text;
                    SqlDataAdapter adPL = new SqlDataAdapter(cmdPL);
                    adPL.Fill(dtPl);
                    adPL.Dispose();
                    cmdPL.Dispose();
                    if (dtPl.Rows.Count > 0)
                    {
                        con.Close();

                        clsResult.Responce = "2";
                        return clsResult;
                    }
               


                SqlCommand cmd = new SqlCommand("spSpecialPlaylists_Save_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@pAction", SqlDbType.VarChar));
                if (string.IsNullOrEmpty(data.id) == true)
                {
                    cmd.Parameters["@pAction"].Value = "New";
                }
                else
                {
                    cmd.Parameters["@pAction"].Value = "Modify";
                }
                cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                if (string.IsNullOrEmpty(data.id) == true)
                {
                    cmd.Parameters["@splPlaylistId"].Value = 0;
                }
                else
                {
                    cmd.Parameters["@splPlaylistId"].Value = data.id;
                }
                cmd.Parameters.Add(new SqlParameter("@splPlaylistName", SqlDbType.VarChar));
                cmd.Parameters["@splPlaylistName"].Value = data.plName.Trim(); ;
                cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                cmd.Parameters["@pVersion"].Value = "c";
                cmd.Parameters.Add(new SqlParameter("@Formatid", SqlDbType.BigInt));
                cmd.Parameters["@Formatid"].Value = data.formatid;
                cmd.Parameters.Add(new SqlParameter("@mType", SqlDbType.VarChar));
                cmd.Parameters["@mType"].Value = "Audio";
               
                cmd.ExecuteNonQuery();
                con.Close();

                clsResult.Responce = "1";
                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }

        public ResResponce SavePlaylistFromBestOf(ReqSavePlaylistFromBestOff data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string str45 = data.plName;
                string[] arr = str45.Split('(');
                SqlCommand cmd = new SqlCommand("spSpecialPlaylists_Save_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@pAction", SqlDbType.VarChar));

                cmd.Parameters["@pAction"].Value = "New";

                cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));

                cmd.Parameters["@splPlaylistId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@splPlaylistName", SqlDbType.VarChar));
                cmd.Parameters["@splPlaylistName"].Value = arr[0].Trim();
                cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                cmd.Parameters["@pVersion"].Value = "c";
                cmd.Parameters.Add(new SqlParameter("@Formatid", SqlDbType.BigInt));
                cmd.Parameters["@Formatid"].Value = data.formatid;
                cmd.Parameters.Add(new SqlParameter("@mType", SqlDbType.VarChar));
                cmd.Parameters["@mType"].Value = "Audio";
                con.Open();
                var lPlaylistId = Convert.ToInt32(cmd.ExecuteScalar());
                string st = "";
                if (data.isBestOff == "1")
                {
                    st = "insert into tbSpecialPlaylists_Titles(splplaylistid,titleid,srno) select " + lPlaylistId + ",titleid,0 from titlesinplaylists where playlistid=" + data.id;
                }
                else
                {
                    st = "insert into tbSpecialPlaylists_Titles(splplaylistid,titleid,srno) select " + lPlaylistId + ",titleid,0 from tbSpecialPlaylists_Titles where splPlaylistid=" + data.id;
                }
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = st;
                cmd.ExecuteNonQuery();
                con.Close();

                clsResult.Responce = "1";
                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }
        public List<ResPlaylist> Playlist(ReqPlaylist data)
        {
            List<ResPlaylist> lstPlaylist = new List<ResPlaylist>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {

                string sQr = "";
                if (string.IsNullOrEmpty(data.ClientId) == true)
                {
                    //sQr = "select splplaylistid  , splplaylistname , isnull(isVideoMute,0) as IsMute  from tbSpecialPlaylists where formatid = " + data.Id;
                    sQr = "GetPlaylistLibrary_new 0, " + data.Id;
                }
                else
                {
                    var weekNo = (int)DateTime.Now.DayOfWeek;
                    sQr = "GetSpecialTempPlaylistSchedule " + weekNo + ", " + data.Id + " ," + data.ClientId + ", '" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "'";
                }

                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                Boolean iCheck = false;
                string spid = "";

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        iCheck = true;
                    }
                    else
                    {
                        iCheck = false;
                    }
                    if (spid != ds.Rows[i]["splplaylistid"].ToString())
                    {
                        spid = ds.Rows[i]["splplaylistid"].ToString();

                        lstPlaylist.Add(new ResPlaylist()
                        {
                            Id = ds.Rows[i]["splplaylistid"].ToString(),
                            DisplayName = ds.Rows[i]["splplaylistname"].ToString(),
                            check = iCheck,
                            IsMute = Convert.ToBoolean(Convert.ToByte(ds.Rows[i]["IsMute"])),
                            IsFixed = Convert.ToBoolean(Convert.ToByte(ds.Rows[i]["isShowDefault"])),
                            IsMixedContent = Convert.ToBoolean(Convert.ToByte(ds.Rows[i]["IsMixedContent"])),
                            tokenIds = ds.Rows[i]["tokenid"].ToString().Split(new Char[] { ',' }),
                            IsDuplicate = Convert.ToBoolean(Convert.ToByte(ds.Rows[i]["IsDuplicate"])),
                            volume = ds.Rows[i]["VolumeLevel"].ToString(),
                        });
                    }
                }
                con.Close();

                return lstPlaylist;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return lstPlaylist;
            }
        }

        public List<ResSongList> SongList(ReqCommonSearch data)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            List<ResSongList> lstSong = new List<ResSongList>();
            try
            {

                string sQr = "";

                sQr = "SELECT TOP (500) Titles.TitleID, Titles.Title, Titles.Time, Artists.Name as ArtistName, Albums.Name AS AlbumName, isnull(Titles.tempo,'') as Tempo,isnull(tbGenre.genre,'') as genre , Titles.titleyear ,isnull(acategory,'') as Category, Titles.AlbumID, Titles.ArtistID, Titles.mediatype,  isnull(Titles.label ,'') as label ,isnuLL(tbFolder.folderName,'') as fName , isnull(Titles.EngeryLevel,0) as EngeryLevel, isnull(Titles.BPM,'') as bpm, isnull(Titles.ReleaseDate,'') as rdate, isnull(Titles.language,'') as lang, titles.titleyear, isnull(Titles.dfclientid,0) as dfclientid FROM Titles INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID LEFT OUTER JOIN tbGenre ON Titles.GenreId = tbGenre.GenreId  LEFT OUTER JOIN tbFolder ON Titles.folderId = tbFolder.folderId   ";
                sQr = "";
                sQr = sQr + " where isnull(titles.IsAnnouncement,0)= " + data.IsAnnouncement + " and  Titles.mediaType=''" + data.mediaType + "'' and isnull(Titles.Explicit,0)= " + Convert.ToByte(data.IsExplicit) + " ";
                sQr = sQr + " and (Titles.dbtype=''" + data.DBType + "'' or Titles.dbtype=''Both'') ";

                if (data.IsAdmin == false)
                {
                    sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + " or Titles.dfclientid=" + data.LoginClientId + ") ";
                }
                else
                {
                    sQr = sQr + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                }
                if (data.mediaType != "Image")
                {
                    sQr = sQr + " and IsRoyaltyFree=" + data.IsRf + " ";
                }
                if (data.ContentType == "Signage")
                {
                    sQr = sQr + " and Titles.genreid in(303,297, 325,324)";
                }
                if (data.ContentType == "MusicMedia")
                {
                    sQr = sQr + " and Titles.genreid not in(325,324)";
                }


                sQr = sQr + " ) as d  order by TitleID desc   ";


                //sQr = "Pagination_Test_2 " + data.mediaType;
                //if (data.mediaType == "Audio")
                //{
                //    sQr = "Pagination_Test_2 1";
                //}
                //else
                //{
                //    sQr = "Pagination_Test_2 " + data.mediaType;
                //}
                string str = "Pagination_CommonSearch " + data.PageNo + ", '" + sQr + "'";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                var format = "";
                string url = "";
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (ds.Rows[i]["MediaType"].ToString() == "Audio")
                    {
                        format = ".mp3";
                    }
                    if (ds.Rows[i]["MediaType"].ToString() == "Video")
                    {
                        format = ".mp4";
                    }
                    if (ds.Rows[i]["MediaType"].ToString() == "Image")
                    {
                        format = ".jpg";
                    }
                    var rDate = "";
                    if (string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(ds.Rows[i]["rDate"])) == "01-Jan-1900")
                    {
                        rDate = "";
                    }
                    else
                    {
                        rDate = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(ds.Rows[i]["rDate"]));
                    }


                    url = "http://api.advikon.com/mp3files/" + ds.Rows[i]["titleId"].ToString() + format;

                    lstSong.Add(new ResSongList()
                    {
                        id = ds.Rows[i]["TitleID"].ToString(),
                        title = ds.Rows[i]["Title"].ToString(),
                        Length = ds.Rows[i]["Time"].ToString(),
                        Artist = ds.Rows[i]["ArtistName"].ToString(),
                        Album = ds.Rows[i]["AlbumName"].ToString(),
                        genreName = ds.Rows[i]["genre"].ToString(),
                        category = ds.Rows[i]["Category"].ToString(),
                        ArtistId = ds.Rows[i]["ArtistID"].ToString(),
                        AlbumId = ds.Rows[i]["AlbumID"].ToString(),
                        MediaType = ds.Rows[i]["MediaType"].ToString(),
                        Label = ds.Rows[i]["label"].ToString(),
                        TitleIdLink = url,
                        FolderName = ds.Rows[i]["fName"].ToString(),
                        EngeryLevel = ds.Rows[i]["EngeryLevel"].ToString(),
                        rDate = rDate,
                        BPM = ds.Rows[i]["BPM"].ToString(),
                        Language = ds.Rows[i]["lang"].ToString(),
                        titleyear = ds.Rows[i]["titleyear"].ToString(),
                        dfClientId = ds.Rows[i]["dfclientid"].ToString(),
                    });
                }
                con.Close();

                return lstSong;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return lstSong;
            }
        }
        public ResResponce SaveSF(ReqSF data)
        {
            ResResponce clsResult = new ResResponce();
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {

                var h = data.EndTime.Split(':');

                if (h[0] == "00")
                {
                    data.EndTime = "23:59";
                }


                con.Open();
                foreach (var iToken in data.TokenList)
                {
                    if (iToken.schType == "Normal")
                    {


                        foreach (var lstWeek in data.wList)
                        {
                            string strTit = "CheckTokenSchedule 0," + iToken.tokenId + "," + lstWeek.id + ",'" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(data.startTime).AddMinutes(1)) + "','" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(data.EndTime).AddMinutes(-1)) + "'";
                            cmd = new SqlCommand(strTit, con);
                            cmd.CommandType = System.Data.CommandType.Text;
                            ad = new SqlDataAdapter(cmd);
                            DataTable ds = new DataTable();
                            ad.Fill(ds);
                            if (ds.Rows.Count > 0)
                            {
                                for (int iTit = 0; iTit <= ds.Rows.Count - 1; iTit++)
                                {
                                    cmd = new SqlCommand();
                                    cmd.Connection = con;
                                    strTit = "";
                                    strTit = "delete from tbSpecialPlaylistSchedule_Token where ";
                                    strTit = strTit + " pSchId = " + ds.Rows[iTit]["pSchId"] + " ";
                                    strTit = strTit + " and tokenid=" + iToken.tokenId + " ";
                                    cmd.CommandText = strTit;

                                    cmd.ExecuteNonQuery();

                                }
                            }
                        }
                    }
                    //=========================== Save Main Data
                    cmd = new SqlCommand("spSaveSpecialPlaylistSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                    cmd.Parameters["@pSchId"].Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                    cmd.Parameters["@pVersion"].Value = "c";

                    cmd.Parameters.Add(new SqlParameter("@dfClientId", SqlDbType.BigInt));
                    cmd.Parameters["@dfClientId"].Value = data.CustomerId;

                    cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                    cmd.Parameters["@splPlaylistId"].Value = data.PlaylistId;

                    //var k = string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(data.startTime));

                    //var k2 = string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(data.EndTime));

                    //              string rSave = "0";
                    //                    rSave = AppDomain.CurrentDomain.BaseDirectory;
                    //                string path = Path.GetDirectoryName(rSave) + "\\data.txt";
                    //string WriteData = "";
                    //                  DateTime custDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "India Standard Time");


                    //WriteData = "" + k + ", " + k2 + ", {0} ";
                    //using (StreamWriter writer = new StreamWriter(path, true))
                    //{
                    //    writer.WriteLine(string.Format(WriteData, custDateTime.ToString("dd/MMM/yyyy hh:mm:ss tt")));
                    //    writer.Close();
                    //}

                    cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime));
                    cmd.Parameters["@StartTime"].Value = string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(data.startTime));

                    cmd.Parameters.Add(new SqlParameter("@EndTime", SqlDbType.DateTime));
                    cmd.Parameters["@EndTime"].Value = string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(data.EndTime));

                    cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                    cmd.Parameters["@FormatId"].Value = data.FormatId;

                    Int32 rtPschId = Convert.ToInt32(cmd.ExecuteScalar());
                    //==========================================
                    cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "delete from tbSpecialPlaylistSchedule_Weekday where pSchId=" + rtPschId;
                    cmd.ExecuteNonQuery();

                    //=============================== Save Week
                    foreach (var lstWeek in data.wList)
                    {
                        cmd = new SqlCommand("spSaveSpecialPlaylistSchedule_Week", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                        cmd.Parameters["@pSchId"].Value = rtPschId;

                        cmd.Parameters.Add(new SqlParameter("@wId", SqlDbType.Int));
                        cmd.Parameters["@wId"].Value = lstWeek.id;

                        cmd.Parameters.Add(new SqlParameter("@IsAllWeek", SqlDbType.Int));
                        cmd.Parameters["@IsAllWeek"].Value = 0;

                        cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                        cmd.Parameters["@FormatId"].Value = data.FormatId;
                        cmd.ExecuteNonQuery();
                    }
                    //=========================================
                    cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "delete from tbSpecialPlaylistSchedule_Token where tokenid=" + iToken.tokenId + " and pSchId= " + rtPschId + " ";
                    cmd.ExecuteNonQuery();

                    //====================== Save Token Detail
                    cmd = new SqlCommand("spSaveSpecialPlaylistSchedule_Token", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                    cmd.Parameters["@pSchId"].Value = rtPschId;

                    cmd.Parameters.Add(new SqlParameter("@tokenId", SqlDbType.BigInt));
                    cmd.Parameters["@tokenId"].Value = iToken.tokenId;

                    cmd.Parameters.Add(new SqlParameter("@IsAllToken", SqlDbType.Int));
                    cmd.Parameters["@IsAllToken"].Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                    cmd.Parameters["@FormatId"].Value = data.FormatId;

                    cmd.Parameters.Add(new SqlParameter("@DfClientid", SqlDbType.BigInt));
                    cmd.Parameters["@DfClientid"].Value = data.CustomerId;

                    cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                    cmd.Parameters["@splPlaylistId"].Value = data.PlaylistId;
                    cmd.ExecuteNonQuery();
                    //========================================

                }
                con.Close();
                clsResult.Responce = "1";
                return clsResult;
            }
            catch (Exception ex)
            {
                clsResult.Responce = "0";
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }

        public List<ResFillSF> FillSF(ReqFillSF data)
        {
            List<ResFillSF> lstSF = new List<ResFillSF>();
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string qtr = "";

                if ((data.clientId != "0") && (data.formatId != "0") && (data.playlistId != "0"))
                {
                    qtr = " where dfclientid= " + data.clientId + " and  formatid=" + data.formatId + " and splPlaylistid=" + data.playlistId + " ";
                }
                if ((data.clientId != "0") && (data.formatId != "0") && (data.playlistId == "0"))
                {
                    qtr = " where dfclientid= " + data.clientId + " and formatid=" + data.formatId + " ";
                }
                if ((data.clientId != "0") && data.formatId == "0" && data.playlistId != "0")
                {
                    qtr = " where dfclientid= " + data.clientId + " and splPlaylistid=" + data.playlistId + " ";
                }
                if (data.clientId == "0" && (data.formatId != "0") && data.playlistId != "0")
                {
                    qtr = " where formatid=" + data.formatId + " and splPlaylistid=" + data.playlistId + " ";
                }
                if ((data.clientId != "0") && data.formatId == "0" && (data.playlistId == "0"))
                {
                    qtr = " where dfclientid= " + data.clientId + " ";
                }
                if (data.clientId == "0" && (data.formatId != "0") && (data.playlistId == "0"))
                {
                    qtr = " where formatid=" + data.formatId + " ";
                }
                if (data.clientId == "0" && data.formatId == "0" && data.playlistId != "0")
                {
                    qtr = " where splPlaylistid=" + data.playlistId + " ";
                }

                if (string.IsNullOrEmpty(data.UserId) == false)
                {
                    if (data.UserId != "0")
                    {
                        qtr = qtr + " and tokenid in(select tokenid from tbuserTokens_web where userid= " + data.UserId + ")";
                    }
                }
                qtr = qtr + " order by tokenid, personname , StartTime ";

                string sQr = "GetCustomerPlaylistSchedule '" + qtr + "'";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;

                ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstSF.Add(new ResFillSF()
                    {
                        id = ds.Rows[i]["pSchid"].ToString(),
                        formatName = ds.Rows[i]["FormatName"].ToString(),
                        playlistName = ds.Rows[i]["pName"].ToString(),
                        token = ds.Rows[i]["Tokenid"].ToString(),
                        StartTime = string.Format(fi, "{0:HH:mm}", Convert.ToDateTime(ds.Rows[i]["StartTime"])),
                        EndTime = string.Format(fi, "{0:HH:mm}", Convert.ToDateTime(ds.Rows[i]["EndTime"])),
                        WeekNo = ds.Rows[i]["wName"].ToString(),
                    });
                }
                con.Close();
                return lstSF;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return lstSF;
            }
        }
        public ResResponce DeleteTokenSch(ReqDeleteSF data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string strDel = "";
                strDel = "delete from tbSpecialPlaylistSchedule_Weekday where pSchid=  " + data.pschid;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();


                strDel = "";
                strDel = "delete from tbSpecialPlaylistSchedule_Token where pSchid=  " + data.pschid;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                strDel = "";
                strDel = "delete from tbSpecialPlaylistSchedule where pSchid=   " + data.pschid;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();

                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }
        public List<ResTokenAds> FillSearchAds(ReqSearchAds data)
        {
            List<ResTokenAds> lstSearchAd = new List<ResTokenAds>();
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string sQr = "";
                if (string.IsNullOrEmpty(data.TokenId) == true)
                {
                    sQr = "spGetAdvertisementAdmin '" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.cDate)) + "'," + data.customerId + ",'NativeCR'";
                }
                else
                {
                    var CurrentWeekday = DateTime.Now.DayOfWeek.ToString();
                    var WeekId = 0;
                    if (CurrentWeekday == "Sunday")
                    {
                        WeekId = 1;
                    }
                    if (CurrentWeekday == "Monday")
                    {
                        WeekId = 2;
                    }
                    if (CurrentWeekday == "Tuesday")
                    {
                        WeekId = 3;
                    }
                    if (CurrentWeekday == "Wednesday")
                    {
                        WeekId = 4;
                    }
                    if (CurrentWeekday == "Thursday")
                    {
                        WeekId = 5;
                    }
                    if (CurrentWeekday == "Friday")
                    {
                        WeekId = 6;
                    }
                    if (CurrentWeekday == "Saturday")
                    {
                        WeekId = 7;
                    }
                    sQr = "spGetAdvtAdmin_NativeOnly_Web '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','NativeCR'," + data.customerId + "," + WeekId + ", 0," + data.customerId + " , 0, 0," + data.TokenId + "";
                }
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;

                ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstSearchAd.Add(new ResTokenAds()
                    {
                        id = ds.Rows[i]["AdvtId"].ToString(),
                        adName = ds.Rows[i]["AdvtName"].ToString(),
                        atype = ds.Rows[i]["pMode"].ToString(),
                        startDate = string.Format("{0:dd-MMM-yyyy}", ds.Rows[i]["StartDate"]),
                        playingMode = ds.Rows[i]["m"].ToString(),
                        adsLink = ds.Rows[i]["AdvtFilePath"].ToString(),

                    });
                }
                con.Close();
                return lstSearchAd;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return lstSearchAd;
            }
        }
        public ResResponce SaveAdsAndUploadFile()
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                var fCom = HttpContext.Current.Request.Form["fcom"];
                var objs = JsonConvert.DeserializeObject<ReqAds>(fCom);

                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";

                SqlDataAdapter ad = new SqlDataAdapter();
                con.Open();
                SqlCommand cmd = new SqlCommand("spTempSaveAdvtAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                Int32 ReturnAdvtId = Convert.ToInt32(cmd.ExecuteScalar());

                DataTable dtInsert = new DataTable();
                dtInsert.Columns.Add("AdvtId", typeof(int));
                dtInsert.Columns.Add("CountryId", typeof(int));
                dtInsert.Columns.Add("StateId", typeof(int));
                dtInsert.Columns.Add("CityId", typeof(int));
                dtInsert.Columns.Add("DealerId", typeof(int));
                dtInsert.Columns.Add("ClientId", typeof(int));
                dtInsert.Columns.Add("IsAllCountry", typeof(int));
                dtInsert.Columns.Add("IsAllState", typeof(int));
                dtInsert.Columns.Add("IsAllCity", typeof(int));
                dtInsert.Columns.Add("IsAllDealer", typeof(int));
                dtInsert.Columns.Add("IsAllClient", typeof(int));
                dtInsert.Columns.Add("WeekId", typeof(int));
                dtInsert.Columns.Add("IsAllWeek", typeof(int));
                dtInsert.Columns.Add("TokenId", typeof(int));
                dtInsert.Columns.Add("IsAllToken", typeof(int));

                cmd = new SqlCommand("spSaveAdvt_Admin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@AdvtId", SqlDbType.BigInt));
                cmd.Parameters["@AdvtId"].Value = ReturnAdvtId;

                cmd.Parameters.Add(new SqlParameter("@AdvtDisplayName", SqlDbType.VarChar));
                cmd.Parameters["@AdvtDisplayName"].Value = objs.aName.Trim();

                cmd.Parameters.Add(new SqlParameter("@AdvtCompanyName", SqlDbType.VarChar));
                cmd.Parameters["@AdvtCompanyName"].Value = objs.cName.Trim();

                cmd.Parameters.Add(new SqlParameter("@AdvtStartDate", SqlDbType.DateTime));
                cmd.Parameters["@AdvtStartDate"].Value = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(objs.sDate));

                cmd.Parameters.Add(new SqlParameter("@AdvtEndDate", SqlDbType.DateTime));
                cmd.Parameters["@AdvtEndDate"].Value = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(objs.eDate));

                if (objs.type.ToString() == "Video")
                {
                    cmd.Parameters.Add(new SqlParameter("@AdvtFilePath", SqlDbType.VarChar));
                    cmd.Parameters["@AdvtFilePath"].Value = "http://api.advikon.com/AdvtSongs/" + ReturnAdvtId + ".mp4";
                }
                else if (objs.type.ToString() == "Picture")
                {
                    cmd.Parameters.Add(new SqlParameter("@AdvtFilePath", SqlDbType.VarChar));
                    cmd.Parameters["@AdvtFilePath"].Value = "http://api.advikon.com/AdvtSongs/" + ReturnAdvtId + ".jpg";
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@AdvtFilePath", SqlDbType.VarChar));
                    cmd.Parameters["@AdvtFilePath"].Value = "http://api.advikon.com/AdvtSongs/" + ReturnAdvtId + ".mp3";
                }
                cmd.Parameters.Add(new SqlParameter("@AdvtPlayertype", SqlDbType.VarChar));
                cmd.Parameters["@AdvtPlayertype"].Value = "NativeCR";

                cmd.Parameters.Add(new SqlParameter("@AdvtTypeId", SqlDbType.BigInt));
                cmd.Parameters["@AdvtTypeId"].Value = objs.category;

                cmd.Parameters.Add(new SqlParameter("@AdvtTime", SqlDbType.DateTime));
                cmd.Parameters["@AdvtTime"].Value = string.Format("{0:hh:mm tt}", Convert.ToDateTime(objs.sTime));

                cmd.Parameters.Add(new SqlParameter("@IsCountry", SqlDbType.Bit));
                cmd.Parameters["@IsCountry"].Value = 0;


                cmd.Parameters.Add(new SqlParameter("@IsState", SqlDbType.Bit));
                cmd.Parameters["@IsState"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsCity", SqlDbType.Bit));
                cmd.Parameters["@IsCity"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsDealer", SqlDbType.Bit));
                cmd.Parameters["@IsDealer"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@IsClient", SqlDbType.Bit));
                cmd.Parameters["@IsClient"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@StateCountryId", SqlDbType.BigInt));
                cmd.Parameters["@StateCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@CityStateId", SqlDbType.BigInt));
                cmd.Parameters["@CityStateId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@CityCountryId", SqlDbType.BigInt));
                cmd.Parameters["@CityCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@DealerCountryId", SqlDbType.BigInt));
                cmd.Parameters["@DealerCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@ClientCountryId", SqlDbType.BigInt));
                cmd.Parameters["@ClientCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsToken", SqlDbType.Bit));
                cmd.Parameters["@IsToken"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@TokenDealerId", SqlDbType.BigInt));
                cmd.Parameters["@TokenDealerId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@DealerId", SqlDbType.BigInt));
                cmd.Parameters["@DealerId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsTime", SqlDbType.Bit));
                if (objs.pMode.ToString() == "Time")
                {
                    cmd.Parameters["@IsTime"].Value = 1;
                }
                else
                {
                    cmd.Parameters["@IsTime"].Value = 0;
                }
                cmd.Parameters.Add(new SqlParameter("@IsMinute", SqlDbType.Bit));
                if (objs.pMode.ToString() == "Minutes")
                {
                    cmd.Parameters["@IsMinute"].Value = 1;
                }
                else
                {
                    cmd.Parameters["@IsMinute"].Value = 0;
                }

                cmd.Parameters.Add(new SqlParameter("@IsSong", SqlDbType.Bit));
                if (objs.pMode.ToString() == "Song")
                {
                    cmd.Parameters["@IsSong"].Value = 1;
                }
                else
                {
                    cmd.Parameters["@IsSong"].Value = 0;
                }

                cmd.Parameters.Add(new SqlParameter("@TotalMinutes", SqlDbType.Int));
                cmd.Parameters["@TotalMinutes"].Value = objs.TotalFrequancy;

                cmd.Parameters.Add(new SqlParameter("@TotalSongs", SqlDbType.Int));
                cmd.Parameters["@TotalSongs"].Value = objs.TotalFrequancy;

                if (objs.type.ToString() == "Video")
                {
                    cmd.Parameters.Add(new SqlParameter("@IsVideo", SqlDbType.Int));
                    cmd.Parameters["@IsVideo"].Value = 1;
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@IsVideo", SqlDbType.Int));
                    cmd.Parameters["@IsVideo"].Value = 0;
                }

                cmd.Parameters.Add(new SqlParameter("@IsVideoMute", SqlDbType.Int));
                cmd.Parameters["@IsVideoMute"].Value = 0;




                if (objs.type.ToString() == "Picture")
                {
                    cmd.Parameters.Add(new SqlParameter("@IsPicture", SqlDbType.Int));
                    cmd.Parameters["@IsPicture"].Value = 1;
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@IsPicture", SqlDbType.Int));
                    cmd.Parameters["@IsPicture"].Value = 0;
                }


                cmd.Parameters.Add(new SqlParameter("@IsBetween", SqlDbType.Bit));
                cmd.Parameters["@IsBetween"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@bStime", SqlDbType.DateTime));
                cmd.Parameters["@bStime"].Value = string.Format("{0:hh:mm tt}", Convert.ToDateTime(objs.sTime));

                cmd.Parameters.Add(new SqlParameter("@bEtime", SqlDbType.DateTime));
                cmd.Parameters["@bEtime"].Value = string.Format("{0:hh:mm tt}", Convert.ToDateTime(objs.sTime));

                cmd.Parameters.Add(new SqlParameter("@playingType", SqlDbType.VarChar));
                cmd.Parameters["@playingType"].Value = objs.pType;

                cmd.Parameters.Add(new SqlParameter("@isEndDateCheck", SqlDbType.Int));
                cmd.Parameters["@isEndDateCheck"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.NVarChar));
                cmd.Parameters["@FileSize"].Value = postedFile.ContentLength.ToString();


                cmd.ExecuteNonQuery();
                //======================================
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "delete from tbAdvtAdminDetail where advtid=" + ReturnAdvtId;
                cmd.ExecuteNonQuery();
                //======================================
                foreach (var iWeek in objs.wList)
                {
                    foreach (var iCountry in objs.CountryLst)
                    {
                        DataRow nr = dtInsert.NewRow();
                        nr["AdvtId"] = ReturnAdvtId;
                        nr["CountryId"] = iCountry;
                        nr["StateId"] = 0;
                        nr["CityId"] = 0;
                        nr["DealerId"] = 0;
                        nr["ClientId"] = 0;
                        nr["IsAllCountry"] = 0;
                        nr["IsAllState"] = 0;
                        nr["IsAllCity"] = 0;
                        nr["IsAllDealer"] = 0;
                        nr["IsAllClient"] = 0;
                        nr["WeekId"] = iWeek.id;
                        nr["IsAllWeek"] = 0;
                        nr["TokenId"] = 0;
                        nr["IsAllToken"] = 0;
                        dtInsert.Rows.Add(nr);

                    }
                    foreach (var iCustomer in objs.CustomerLst)
                    {
                        DataRow nr = dtInsert.NewRow();
                        nr["AdvtId"] = ReturnAdvtId;
                        nr["CountryId"] = 0;
                        nr["StateId"] = 0;
                        nr["CityId"] = 0;
                        nr["DealerId"] = iCustomer;
                        nr["ClientId"] = 0;
                        nr["IsAllCountry"] = 0;
                        nr["IsAllState"] = 0;
                        nr["IsAllCity"] = 0;
                        nr["IsAllDealer"] = 0;
                        nr["IsAllClient"] = 0;
                        nr["WeekId"] = iWeek.id;
                        nr["IsAllWeek"] = 0;
                        nr["TokenId"] = 0;
                        nr["IsAllToken"] = 0;
                        dtInsert.Rows.Add(nr);

                    }
                    foreach (var iToken in objs.TokenLst)
                    {
                        DataRow nr = dtInsert.NewRow();
                        nr["AdvtId"] = ReturnAdvtId;
                        nr["CountryId"] = 0;
                        nr["StateId"] = 0;
                        nr["CityId"] = 0;
                        nr["DealerId"] = 0;
                        nr["ClientId"] = 0;
                        nr["IsAllCountry"] = 0;
                        nr["IsAllState"] = 0;
                        nr["IsAllCity"] = 0;
                        nr["IsAllDealer"] = 0;
                        nr["IsAllClient"] = 0;
                        nr["WeekId"] = iWeek.id;
                        nr["IsAllWeek"] = 0;
                        nr["TokenId"] = iToken;
                        nr["IsAllToken"] = 0;
                        dtInsert.Rows.Add(nr);
                    }
                }
                if (dtInsert.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {

                        SqlBulkCopyColumnMapping AdvtId =
                         new SqlBulkCopyColumnMapping("AdvtId", "AdvtId");
                        bulkCopy.ColumnMappings.Add(AdvtId);

                        SqlBulkCopyColumnMapping CountryId =
                            new SqlBulkCopyColumnMapping("CountryId", "CountryId");
                        bulkCopy.ColumnMappings.Add(CountryId);

                        SqlBulkCopyColumnMapping StateId =
                           new SqlBulkCopyColumnMapping("StateId", "StateId");
                        bulkCopy.ColumnMappings.Add(StateId);

                        SqlBulkCopyColumnMapping CityId =
                            new SqlBulkCopyColumnMapping("CityId", "CityId");
                        bulkCopy.ColumnMappings.Add(CityId);

                        SqlBulkCopyColumnMapping DealerId =
                            new SqlBulkCopyColumnMapping("DealerId", "DealerId");
                        bulkCopy.ColumnMappings.Add(DealerId);

                        SqlBulkCopyColumnMapping ClientId =
                           new SqlBulkCopyColumnMapping("ClientId", "ClientId");
                        bulkCopy.ColumnMappings.Add(ClientId);


                        SqlBulkCopyColumnMapping IsAllCountry =
                         new SqlBulkCopyColumnMapping("IsAllCountry", "IsAllCountry");
                        bulkCopy.ColumnMappings.Add(IsAllCountry);

                        SqlBulkCopyColumnMapping IsAllState =
                            new SqlBulkCopyColumnMapping("IsAllState", "IsAllState");
                        bulkCopy.ColumnMappings.Add(IsAllState);

                        SqlBulkCopyColumnMapping IsAllCity =
                           new SqlBulkCopyColumnMapping("IsAllCity", "IsAllCity");
                        bulkCopy.ColumnMappings.Add(IsAllCity);

                        SqlBulkCopyColumnMapping IsAllDealer =
                            new SqlBulkCopyColumnMapping("IsAllDealer", "IsAllDealer");
                        bulkCopy.ColumnMappings.Add(IsAllDealer);

                        SqlBulkCopyColumnMapping IsAllClient =
                            new SqlBulkCopyColumnMapping("IsAllClient", "IsAllClient");
                        bulkCopy.ColumnMappings.Add(IsAllClient);

                        SqlBulkCopyColumnMapping WeekId =
                           new SqlBulkCopyColumnMapping("WeekId", "WeekId");
                        bulkCopy.ColumnMappings.Add(WeekId);

                        SqlBulkCopyColumnMapping IsAllWeek =
                            new SqlBulkCopyColumnMapping("IsAllWeek", "IsAllWeek");
                        bulkCopy.ColumnMappings.Add(IsAllWeek);

                        SqlBulkCopyColumnMapping TokenId =
                            new SqlBulkCopyColumnMapping("TokenId", "TokenId");
                        bulkCopy.ColumnMappings.Add(TokenId);

                        SqlBulkCopyColumnMapping IsAllToken =
                           new SqlBulkCopyColumnMapping("IsAllToken", "IsAllToken");
                        bulkCopy.ColumnMappings.Add(IsAllToken);

                        bulkCopy.DestinationTableName = "dbo.tbAdvtAdminDetail";



                        bulkCopy.WriteToServer(dtInsert);

                    }
                }


                string fName = ReturnAdvtId.ToString() + Path.GetExtension(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/AdvtSongs/" + fName);
                postedFile.SaveAs(filePath);
                con.Close();
                Result.Responce = "1";
                Result.status = "success";
                Result.message = "uploaded";
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                Result.status = "Error";
                Result.message = ex.Message;
                Result.filePath = "Error File";
                HttpContext.Current.Response.StatusCode = 1;

                return Result;
            }

        }




        public List<ResTokenInfo> FillTokenInfoAds(ReqTokenInfoAds data)
        {
            List<ResTokenInfo> lstResult = new List<ResTokenInfo>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                var cid = "";
                foreach (var item in data.clientId)
                {
                    if (cid == "")
                    {
                        cid = item;
                    }
                    else
                    {
                        cid = cid + "," + item;
                    }
                }
                var h = data.clientId;
                string sQr = "";
                if (string.IsNullOrEmpty(data.UserId) == true)
                {
                    sQr = "SELECT AMPlayerTokens.TokenID ,  iif(AMPlayerTokens.token='used',convert(varchar(100) ,AMPlayerTokens.Tokenid),AMPlayerTokens.token)  as tNo, isnull(AMPlayerTokens.Location,'') as Location,";
                    sQr = sQr + " isnull(tbCity.CityName,'') as CityName, isnull(tbState.StateName,'') as StateName, isnull(CountryCodes.CountryName,'') as CountryName,isnull(AMPlayerTokens.PersonName ,'') as PersonName , AMPlayerTokens.userid, isnull(AMPlayerTokens.IsStore,0) as IsStore, isnull(AMPlayerTokens.IsStream,0) as IsStream, 'false' as [check] ,isnull(AMPlayerTokens.groupid,0) as grpId  , isnull(AMPlayerTokens.CountryId,0) as CountryId, isnull(AMPlayerTokens.StateId,0) as StateId,  isnull(AMPlayerTokens.CityId,0) as CityId FROM  AMPlayerTokens ";
                    sQr = sQr + " LEFT OUTER JOIN tbCity ON AMPlayerTokens.CityId = tbCity.CityId LEFT OUTER JOIN tbState ON AMPlayerTokens.StateId = tbState.StateId LEFT OUTER JOIN CountryCodes ON AMPlayerTokens.CountryId = CountryCodes.CountryCode";
                    sQr = sQr + " where AMPlayerTokens.clientid in( " + cid + " )";
                }
                else if (data.UserId == "0")
                {
                    sQr = "SELECT AMPlayerTokens.TokenID ,  iif(AMPlayerTokens.token='used',convert(varchar(100) ,AMPlayerTokens.Tokenid),AMPlayerTokens.token)  as tNo, isnull(AMPlayerTokens.Location,'') as Location,";
                    sQr = sQr + " isnull(tbCity.CityName,'') as CityName, isnull(tbState.StateName,'') as StateName, isnull(CountryCodes.CountryName,'') as CountryName,isnull(AMPlayerTokens.PersonName ,'') as PersonName , AMPlayerTokens.userid, isnull(AMPlayerTokens.IsStore,0) as IsStore, isnull(AMPlayerTokens.IsStream,0) as IsStream, 'false' as [check],isnull(AMPlayerTokens.groupid,0) as grpId  , isnull(AMPlayerTokens.CountryId,0) as CountryId, isnull(AMPlayerTokens.StateId,0) as StateId,  isnull(AMPlayerTokens.CityId,0) as CityId FROM  AMPlayerTokens ";
                    sQr = sQr + " LEFT OUTER JOIN tbCity ON AMPlayerTokens.CityId = tbCity.CityId LEFT OUTER JOIN tbState ON AMPlayerTokens.StateId = tbState.StateId LEFT OUTER JOIN CountryCodes ON AMPlayerTokens.CountryId = CountryCodes.CountryCode";
                    sQr = sQr + " where AMPlayerTokens.clientid in( " + cid + " )";
                }
                else
                {
                    sQr = "SELECT AMPlayerTokens.TokenID ,  iif(AMPlayerTokens.token='used',convert(varchar(100) ,AMPlayerTokens.Tokenid),AMPlayerTokens.token)  as tNo, isnull(AMPlayerTokens.Location,'') as Location,";
                    sQr = sQr + " isnull(tbCity.CityName,'') as CityName, isnull(tbState.StateName,'') as StateName, isnull(CountryCodes.CountryName,'') as CountryName,isnull(AMPlayerTokens.PersonName ,'') as PersonName , AMPlayerTokens.userid, isnull(AMPlayerTokens.IsStore,0) as IsStore, isnull(AMPlayerTokens.IsStream,0) as IsStream, 'false' as [check], isnull(AMPlayerTokens.groupid,0) as grpId  , isnull(AMPlayerTokens.CountryId,0) as CountryId, isnull(AMPlayerTokens.StateId,0) as StateId,  isnull(AMPlayerTokens.CityId,0) as CityId FROM  AMPlayerTokens ";
                    sQr = sQr + " LEFT OUTER JOIN tbCity ON AMPlayerTokens.CityId = tbCity.CityId LEFT OUTER JOIN tbState ON AMPlayerTokens.StateId = tbState.StateId LEFT OUTER JOIN CountryCodes ON AMPlayerTokens.CountryId = CountryCodes.CountryCode";
                    sQr = sQr + " where AMPlayerTokens.clientid in( " + cid + " )";
                    sQr = sQr + " and AMPlayerTokens.TokenID in (select tokenid from tbuserTokens_web where userid =" + data.UserId + ")";
                }
                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstResult.Add(new ResTokenInfo()
                    {
                        tokenid = ds.Rows[i]["tokenid"].ToString(),
                        tokenCode = ds.Rows[i]["tNo"].ToString(),
                        location = ds.Rows[i]["Location"].ToString() + " , " + ds.Rows[i]["PersonName"].ToString(),
                        GroupId = ds.Rows[i]["grpId"].ToString(),
                        CountryId = ds.Rows[i]["CountryId"].ToString(),
                        StateId = ds.Rows[i]["StateId"].ToString(),
                        CityId = ds.Rows[i]["CityId"].ToString(),

                        check = false,
                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }


        public ResUpdateAds FillSaveAds(ReqAdsId data)
        {
            ResUpdateAds result = new ResUpdateAds();
            List<ReqSFWeek> wLst = new List<ReqSFWeek>();
            List<ResComboQuery> lstCountry = new List<ResComboQuery>();
            List<ResComboQuery> lstCustomer = new List<ResComboQuery>();
            List<ResTokenInfo> lstToken = new List<ResTokenInfo>();
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            List<string> TokenArray = new List<string>();
            List<string> CustomerArray = new List<string>();
            List<string> CountryArray = new List<string>();


            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string st = "select * from tbAdvtAdmin where advtid = " + data.advtid;
                SqlCommand cmd = new SqlCommand(st, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    result.aName = ds.Rows[0]["AdvtName"].ToString();
                    result.pType = ds.Rows[0]["playingType"].ToString();
                    result.category = ds.Rows[0]["AdvtTypeId"].ToString();
                    result.sDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(ds.Rows[0]["StartDate"]));
                    result.eDate = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(ds.Rows[0]["EndDate"]));
                    if (Convert.ToBoolean(ds.Rows[0]["IsTime"]) == true)
                    {
                        result.pMode = "Time";
                    }
                    if (Convert.ToBoolean(ds.Rows[0]["IsMinute"]) == true)
                    {
                        result.pMode = "Minutes";
                        result.TotalFrequancy = ds.Rows[0]["TotalMinutes"].ToString();
                    }
                    if (Convert.ToBoolean(ds.Rows[0]["IsSong"]) == true)
                    {
                        result.pMode = "Song";
                        result.TotalFrequancy = ds.Rows[0]["TotalSongs"].ToString();
                    }
                    result.sTime = string.Format(fi, "{0:dd/MMM/yyyy hh:mm tt}", Convert.ToDateTime(ds.Rows[0]["StartTime"]));

                    result.cName = ds.Rows[0]["CmpName"].ToString();
                    result.FilePath = ds.Rows[0]["FilePath"].ToString();
                    if (ds.Rows[0]["IsVideo"].ToString() == "1")
                    {
                        result.type = "Video";
                    }
                    else if (ds.Rows[0]["IsPicture"].ToString() == "1")
                    {
                        result.type = "Picture";
                    }
                    else
                    {
                        result.type = "Audio";
                    }


                    //====================== Get Week Days
                    st = "select distinct WeekId,IsAllWeek from tbAdvtAdminDetail  where advtid=" + data.advtid + " and WeekId != IsAllWeek";
                    cmd = new SqlCommand(st, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    ds = new DataTable();
                    ad.Fill(ds);
                    if (ds.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(ds.Rows[0]["IsAllWeek"]) == 1)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                wLst.Add(new ReqSFWeek()
                                {
                                    id = (i + 1).ToString(),
                                    itemName = "",
                                });
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ds.Rows.Count; i++)
                            {
                                wLst.Add(new ReqSFWeek()
                                {
                                    id = ds.Rows[i]["WeekId"].ToString(),
                                    itemName = "",
                                });
                            }
                        }

                    }
                    //=====================================
                    //====================== Get Ads Country
                    if (string.IsNullOrEmpty(data.ClientId) == true)
                    {
                        st = "GetAdsCountry " + data.advtid + " ";
                    }
                    else
                    {
                        st = "GetAdsCountry " + data.advtid + " ," + data.ClientId;
                    }
                    cmd = new SqlCommand(st, con);
                    cmd.CommandType = CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    ds = new DataTable();
                    ad.Fill(ds);
                    var countryid = "";
                    if (ds.Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(ds.Rows[i]["check"]) == true)
                            {
                                CountryArray.Add(ds.Rows[i]["id"].ToString());
                                if (countryid == "")
                                {
                                    countryid = ds.Rows[i]["id"].ToString();
                                }
                                else
                                {
                                    countryid = countryid + "," + ds.Rows[i]["id"].ToString();
                                }
                            }
                            lstCountry.Add(new ResComboQuery()
                            {
                                Id = ds.Rows[i]["id"].ToString(),
                                DisplayName = ds.Rows[i]["DisplayName"].ToString(),
                                check = Convert.ToBoolean(ds.Rows[i]["check"]),
                            });
                        }
                    }
                    //=====================================
                    //====================== Get Ads Customer
                    var cid = "";
                    st = "";
                    st = "select id, DisplayName, iif(isnull(cast(icheck as nvarchar(20)), '0') = 0, 'false', 'true') as [check] from(";
                    st = st + " select DFClientID as Id, RIGHT(ClientName, LEN(ClientName) - 3) as DisplayName, ";
                    st = st + " (select distinct advtid from tbAdvtAdminDetail where advtid = " + data.advtid + " and DealerId = DFClientID) as icheck ";
                    st = st + " from( select distinct DFClients.DFClientID, DFClients.ClientName from DFClients ";
                    st = st + " inner join AMPlayerTokens on AMPlayerTokens.clientid = DFClients.dfclientid ";
                    st = st + " where AMPlayerTokens.countryid  in (" + countryid + ") and DFClients.IsDealer = 1 ";
                    st = st + " ) as a ) as m order by DisplayName ";
                    cmd = new SqlCommand(st, con);
                    cmd.CommandType = CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    ds = new DataTable();
                    ad.Fill(ds);
                    if (ds.Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(ds.Rows[i]["check"]) == true)
                            {
                                CustomerArray.Add(ds.Rows[i]["id"].ToString());
                                if (cid == "")
                                {
                                    cid = ds.Rows[i]["id"].ToString();
                                }
                                else
                                {
                                    cid = cid + "," + ds.Rows[i]["id"].ToString();
                                }
                            }
                            lstCustomer.Add(new ResComboQuery()
                            {
                                Id = ds.Rows[i]["id"].ToString(),
                                DisplayName = ds.Rows[i]["DisplayName"].ToString(),
                                check = Convert.ToBoolean(ds.Rows[i]["check"]),
                            });
                        }
                    }
                    //=====================================

                    //============================ Fill Ads Token
                    var sQr = "select TokenID, tNo,Location,personName, iif(isnull(cast(icheck as nvarchar(20)),'0')=0,'false','true') as [check] from (";
                    sQr = sQr + " SELECT AMPlayerTokens.TokenID , iif(isnull(AMPlayerTokens.tokenno,'')='' ,iif(AMPlayerTokens.token='used',convert(varchar(100) ,AMPlayerTokens.Tokenid),AMPlayerTokens.token),AMPlayerTokens.tokenno) as tNo, isnull(AMPlayerTokens.Location,'') as Location, isnull(AMPlayerTokens.personname,'') as personName, ";
                    sQr = sQr + "  (select distinct advtid from tbAdvtAdminDetail where advtid=" + data.advtid + " and tokenid=AMPlayerTokens.TokenID) as icheck FROM  AMPlayerTokens ";
                    sQr = sQr + " where AMPlayerTokens.clientid in( " + cid + " )";
                    sQr = sQr + " ) as m ";
                    cmd = new SqlCommand(sQr, con);
                    cmd.CommandType = CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    ds = new DataTable();
                    ad.Fill(ds);
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(ds.Rows[i]["check"]) == true)
                        {
                            TokenArray.Add(ds.Rows[i]["tokenid"].ToString());
                        }

                        lstToken.Add(new ResTokenInfo()
                        {
                            tokenid = ds.Rows[i]["tokenid"].ToString(),
                            tokenCode = ds.Rows[i]["tNo"].ToString(),
                            location = ds.Rows[i]["Location"].ToString() + " , " + ds.Rows[i]["personName"].ToString(),
                            check = Convert.ToBoolean(ds.Rows[i]["check"]),
                        });
                    }
                    //===========================================
                    result.wList = wLst;
                    result.lstCountry = lstCountry;
                    result.lstCustomer = lstCustomer;
                    result.lstToken = lstToken;

                    result.CountryLst = CountryArray;
                    result.CustomerLst = CustomerArray;
                    result.TokenLst = TokenArray;
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }

        }




        public ResResponce UpdateAds(ReqAds objs)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";

                SqlDataAdapter ad = new SqlDataAdapter();
                con.Open();
                SqlCommand cmd = new SqlCommand();
                Int32 ReturnAdvtId = Convert.ToInt32(objs.aid);

                DataTable dtInsert = new DataTable();
                dtInsert.Columns.Add("AdvtId", typeof(int));
                dtInsert.Columns.Add("CountryId", typeof(int));
                dtInsert.Columns.Add("StateId", typeof(int));
                dtInsert.Columns.Add("CityId", typeof(int));
                dtInsert.Columns.Add("DealerId", typeof(int));
                dtInsert.Columns.Add("ClientId", typeof(int));
                dtInsert.Columns.Add("IsAllCountry", typeof(int));
                dtInsert.Columns.Add("IsAllState", typeof(int));
                dtInsert.Columns.Add("IsAllCity", typeof(int));
                dtInsert.Columns.Add("IsAllDealer", typeof(int));
                dtInsert.Columns.Add("IsAllClient", typeof(int));
                dtInsert.Columns.Add("WeekId", typeof(int));
                dtInsert.Columns.Add("IsAllWeek", typeof(int));
                dtInsert.Columns.Add("TokenId", typeof(int));
                dtInsert.Columns.Add("IsAllToken", typeof(int));

                cmd = new SqlCommand("spSaveAdvt_Admin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@AdvtId", SqlDbType.BigInt));
                cmd.Parameters["@AdvtId"].Value = ReturnAdvtId;

                cmd.Parameters.Add(new SqlParameter("@AdvtDisplayName", SqlDbType.VarChar));
                cmd.Parameters["@AdvtDisplayName"].Value = objs.aName.Trim();

                cmd.Parameters.Add(new SqlParameter("@AdvtCompanyName", SqlDbType.VarChar));
                cmd.Parameters["@AdvtCompanyName"].Value = objs.cName.Trim();

                cmd.Parameters.Add(new SqlParameter("@AdvtStartDate", SqlDbType.DateTime));
                cmd.Parameters["@AdvtStartDate"].Value = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(objs.sDate));

                cmd.Parameters.Add(new SqlParameter("@AdvtEndDate", SqlDbType.DateTime));
                cmd.Parameters["@AdvtEndDate"].Value = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(objs.eDate));

                if (objs.type.ToString() == "Video")
                {
                    cmd.Parameters.Add(new SqlParameter("@AdvtFilePath", SqlDbType.VarChar));
                    cmd.Parameters["@AdvtFilePath"].Value = "http://api.advikon.com/AdvtSongs/" + ReturnAdvtId + ".mp4";
                }
                else if (objs.type.ToString() == "Picture")
                {
                    cmd.Parameters.Add(new SqlParameter("@AdvtFilePath", SqlDbType.VarChar));
                    cmd.Parameters["@AdvtFilePath"].Value = "http://api.advikon.com/AdvtSongs/" + ReturnAdvtId + ".jpg";
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@AdvtFilePath", SqlDbType.VarChar));
                    cmd.Parameters["@AdvtFilePath"].Value = "http://api.advikon.com/AdvtSongs/" + ReturnAdvtId + ".mp3";
                }
                cmd.Parameters.Add(new SqlParameter("@AdvtPlayertype", SqlDbType.VarChar));
                cmd.Parameters["@AdvtPlayertype"].Value = "NativeCR";

                cmd.Parameters.Add(new SqlParameter("@AdvtTypeId", SqlDbType.BigInt));
                cmd.Parameters["@AdvtTypeId"].Value = objs.category;

                cmd.Parameters.Add(new SqlParameter("@AdvtTime", SqlDbType.DateTime));
                cmd.Parameters["@AdvtTime"].Value = string.Format("{0:hh:mm tt}", Convert.ToDateTime(objs.sTime));

                cmd.Parameters.Add(new SqlParameter("@IsCountry", SqlDbType.Bit));
                cmd.Parameters["@IsCountry"].Value = 0;


                cmd.Parameters.Add(new SqlParameter("@IsState", SqlDbType.Bit));
                cmd.Parameters["@IsState"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsCity", SqlDbType.Bit));
                cmd.Parameters["@IsCity"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsDealer", SqlDbType.Bit));
                cmd.Parameters["@IsDealer"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@IsClient", SqlDbType.Bit));
                cmd.Parameters["@IsClient"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@StateCountryId", SqlDbType.BigInt));
                cmd.Parameters["@StateCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@CityStateId", SqlDbType.BigInt));
                cmd.Parameters["@CityStateId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@CityCountryId", SqlDbType.BigInt));
                cmd.Parameters["@CityCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@DealerCountryId", SqlDbType.BigInt));
                cmd.Parameters["@DealerCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@ClientCountryId", SqlDbType.BigInt));
                cmd.Parameters["@ClientCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsToken", SqlDbType.Bit));
                cmd.Parameters["@IsToken"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@TokenDealerId", SqlDbType.BigInt));
                cmd.Parameters["@TokenDealerId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@DealerId", SqlDbType.BigInt));
                cmd.Parameters["@DealerId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsTime", SqlDbType.Bit));
                if (objs.pMode.ToString() == "Time")
                {
                    cmd.Parameters["@IsTime"].Value = 1;
                }
                else
                {
                    cmd.Parameters["@IsTime"].Value = 0;
                }
                cmd.Parameters.Add(new SqlParameter("@IsMinute", SqlDbType.Bit));
                if (objs.pMode.ToString() == "Minutes")
                {
                    cmd.Parameters["@IsMinute"].Value = 1;
                }
                else
                {
                    cmd.Parameters["@IsMinute"].Value = 0;
                }

                cmd.Parameters.Add(new SqlParameter("@IsSong", SqlDbType.Bit));
                if (objs.pMode.ToString() == "Song")
                {
                    cmd.Parameters["@IsSong"].Value = 1;
                }
                else
                {
                    cmd.Parameters["@IsSong"].Value = 0;
                }

                cmd.Parameters.Add(new SqlParameter("@TotalMinutes", SqlDbType.Int));
                cmd.Parameters["@TotalMinutes"].Value = objs.TotalFrequancy;

                cmd.Parameters.Add(new SqlParameter("@TotalSongs", SqlDbType.Int));
                cmd.Parameters["@TotalSongs"].Value = objs.TotalFrequancy;

                if (objs.type.ToString() == "Video")
                {
                    cmd.Parameters.Add(new SqlParameter("@IsVideo", SqlDbType.Int));
                    cmd.Parameters["@IsVideo"].Value = 1;
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@IsVideo", SqlDbType.Int));
                    cmd.Parameters["@IsVideo"].Value = 0;
                }

                cmd.Parameters.Add(new SqlParameter("@IsVideoMute", SqlDbType.Int));
                cmd.Parameters["@IsVideoMute"].Value = 0;




                if (objs.type.ToString() == "Picture")
                {
                    cmd.Parameters.Add(new SqlParameter("@IsPicture", SqlDbType.Int));
                    cmd.Parameters["@IsPicture"].Value = 1;
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@IsPicture", SqlDbType.Int));
                    cmd.Parameters["@IsPicture"].Value = 0;
                }


                cmd.Parameters.Add(new SqlParameter("@IsBetween", SqlDbType.Bit));
                cmd.Parameters["@IsBetween"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@bStime", SqlDbType.DateTime));
                cmd.Parameters["@bStime"].Value = string.Format("{0:hh:mm tt}", Convert.ToDateTime(objs.sTime));

                cmd.Parameters.Add(new SqlParameter("@bEtime", SqlDbType.DateTime));
                cmd.Parameters["@bEtime"].Value = string.Format("{0:hh:mm tt}", Convert.ToDateTime(objs.sTime));

                cmd.Parameters.Add(new SqlParameter("@playingType", SqlDbType.VarChar));
                cmd.Parameters["@playingType"].Value = objs.pType;

                cmd.Parameters.Add(new SqlParameter("@isEndDateCheck", SqlDbType.Int));
                cmd.Parameters["@isEndDateCheck"].Value = 0;


                cmd.ExecuteNonQuery();
                //======================================
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "delete from tbAdvtAdminDetail where advtid=" + ReturnAdvtId;
                cmd.ExecuteNonQuery();
                //======================================
                foreach (var iWeek in objs.wList)
                {
                    foreach (var iCountry in objs.CountryLst)
                    {
                        DataRow nr = dtInsert.NewRow();
                        nr["AdvtId"] = ReturnAdvtId;
                        nr["CountryId"] = iCountry;
                        nr["StateId"] = 0;
                        nr["CityId"] = 0;
                        nr["DealerId"] = 0;
                        nr["ClientId"] = 0;
                        nr["IsAllCountry"] = 0;
                        nr["IsAllState"] = 0;
                        nr["IsAllCity"] = 0;
                        nr["IsAllDealer"] = 0;
                        nr["IsAllClient"] = 0;
                        nr["WeekId"] = iWeek.id;
                        nr["IsAllWeek"] = 0;
                        nr["TokenId"] = 0;
                        nr["IsAllToken"] = 0;
                        dtInsert.Rows.Add(nr);

                    }
                    foreach (var iCustomer in objs.CustomerLst)
                    {
                        DataRow nr = dtInsert.NewRow();
                        nr["AdvtId"] = ReturnAdvtId;
                        nr["CountryId"] = 0;
                        nr["StateId"] = 0;
                        nr["CityId"] = 0;
                        nr["DealerId"] = iCustomer;
                        nr["ClientId"] = 0;
                        nr["IsAllCountry"] = 0;
                        nr["IsAllState"] = 0;
                        nr["IsAllCity"] = 0;
                        nr["IsAllDealer"] = 0;
                        nr["IsAllClient"] = 0;
                        nr["WeekId"] = iWeek.id;
                        nr["IsAllWeek"] = 0;
                        nr["TokenId"] = 0;
                        nr["IsAllToken"] = 0;
                        dtInsert.Rows.Add(nr);

                    }
                    foreach (var iToken in objs.TokenLst)
                    {
                        DataRow nr = dtInsert.NewRow();
                        nr["AdvtId"] = ReturnAdvtId;
                        nr["CountryId"] = 0;
                        nr["StateId"] = 0;
                        nr["CityId"] = 0;
                        nr["DealerId"] = 0;
                        nr["ClientId"] = 0;
                        nr["IsAllCountry"] = 0;
                        nr["IsAllState"] = 0;
                        nr["IsAllCity"] = 0;
                        nr["IsAllDealer"] = 0;
                        nr["IsAllClient"] = 0;
                        nr["WeekId"] = iWeek.id;
                        nr["IsAllWeek"] = 0;
                        nr["TokenId"] = iToken;
                        nr["IsAllToken"] = 0;
                        dtInsert.Rows.Add(nr);
                    }
                }
                if (dtInsert.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {

                        SqlBulkCopyColumnMapping AdvtId =
                         new SqlBulkCopyColumnMapping("AdvtId", "AdvtId");
                        bulkCopy.ColumnMappings.Add(AdvtId);

                        SqlBulkCopyColumnMapping CountryId =
                            new SqlBulkCopyColumnMapping("CountryId", "CountryId");
                        bulkCopy.ColumnMappings.Add(CountryId);

                        SqlBulkCopyColumnMapping StateId =
                           new SqlBulkCopyColumnMapping("StateId", "StateId");
                        bulkCopy.ColumnMappings.Add(StateId);

                        SqlBulkCopyColumnMapping CityId =
                            new SqlBulkCopyColumnMapping("CityId", "CityId");
                        bulkCopy.ColumnMappings.Add(CityId);

                        SqlBulkCopyColumnMapping DealerId =
                            new SqlBulkCopyColumnMapping("DealerId", "DealerId");
                        bulkCopy.ColumnMappings.Add(DealerId);

                        SqlBulkCopyColumnMapping ClientId =
                           new SqlBulkCopyColumnMapping("ClientId", "ClientId");
                        bulkCopy.ColumnMappings.Add(ClientId);


                        SqlBulkCopyColumnMapping IsAllCountry =
                         new SqlBulkCopyColumnMapping("IsAllCountry", "IsAllCountry");
                        bulkCopy.ColumnMappings.Add(IsAllCountry);

                        SqlBulkCopyColumnMapping IsAllState =
                            new SqlBulkCopyColumnMapping("IsAllState", "IsAllState");
                        bulkCopy.ColumnMappings.Add(IsAllState);

                        SqlBulkCopyColumnMapping IsAllCity =
                           new SqlBulkCopyColumnMapping("IsAllCity", "IsAllCity");
                        bulkCopy.ColumnMappings.Add(IsAllCity);

                        SqlBulkCopyColumnMapping IsAllDealer =
                            new SqlBulkCopyColumnMapping("IsAllDealer", "IsAllDealer");
                        bulkCopy.ColumnMappings.Add(IsAllDealer);

                        SqlBulkCopyColumnMapping IsAllClient =
                            new SqlBulkCopyColumnMapping("IsAllClient", "IsAllClient");
                        bulkCopy.ColumnMappings.Add(IsAllClient);

                        SqlBulkCopyColumnMapping WeekId =
                           new SqlBulkCopyColumnMapping("WeekId", "WeekId");
                        bulkCopy.ColumnMappings.Add(WeekId);

                        SqlBulkCopyColumnMapping IsAllWeek =
                            new SqlBulkCopyColumnMapping("IsAllWeek", "IsAllWeek");
                        bulkCopy.ColumnMappings.Add(IsAllWeek);

                        SqlBulkCopyColumnMapping TokenId =
                            new SqlBulkCopyColumnMapping("TokenId", "TokenId");
                        bulkCopy.ColumnMappings.Add(TokenId);

                        SqlBulkCopyColumnMapping IsAllToken =
                           new SqlBulkCopyColumnMapping("IsAllToken", "IsAllToken");
                        bulkCopy.ColumnMappings.Add(IsAllToken);

                        bulkCopy.DestinationTableName = "dbo.tbAdvtAdminDetail";




                        bulkCopy.WriteToServer(dtInsert);

                    }
                }



                con.Close();
                Result.Responce = "1";
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }

        }

        public ResResponce DeleteAds(ReqAdsId data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string strDel = "";
                strDel = "delete from tbAdvtAdmin where advtid=  " + data.advtid;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();


                strDel = "";
                strDel = "delete from tbAdvtAdminDetail where advtid=  " + data.advtid;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                con.Close();

                Result.Responce = "1";

                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }

        }

        public ResResponce SavePrayer(ReqPrayer data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            con.Open();
            DataTable dtInsert = new DataTable();
            dtInsert.Columns.Add("PrayerId", typeof(int));
            dtInsert.Columns.Add("CountryId", typeof(int));
            dtInsert.Columns.Add("StateId", typeof(int));
            dtInsert.Columns.Add("CityId", typeof(int));
            dtInsert.Columns.Add("DealerId", typeof(int));
            dtInsert.Columns.Add("ClientId", typeof(int));
            dtInsert.Columns.Add("IsAllCountry", typeof(int));
            dtInsert.Columns.Add("IsAllState", typeof(int));
            dtInsert.Columns.Add("IsAllCity", typeof(int));
            dtInsert.Columns.Add("IsAllDealer", typeof(int));
            dtInsert.Columns.Add("IsAllClient", typeof(int));
            dtInsert.Columns.Add("TokenId", typeof(int));
            dtInsert.Columns.Add("IsAllToken", typeof(int));
            dtInsert.Columns.Add("AdminTokenId", typeof(int));
            dtInsert.Columns.Add("IsAllAdminToken", typeof(int));
            try
            {
                SqlCommand cmd = new SqlCommand("spSavePrayer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PrayerId", SqlDbType.BigInt));
                cmd.Parameters["@PrayerId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime));
                cmd.Parameters["@StartDate"].Value = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.sDate));

                cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime));
                cmd.Parameters["@EndDate"].Value = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.eDate));

                cmd.Parameters.Add(new SqlParameter("@PlayerType", SqlDbType.VarChar));
                cmd.Parameters["@PlayerType"].Value = "c";

                cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime));
                cmd.Parameters["@StartTime"].Value = string.Format("{0:hh:mm tt}", Convert.ToDateTime(data.startTime));

                var h = string.Format("{0:hh:mm tt}", Convert.ToDateTime(data.startTime).AddMinutes(Convert.ToInt16(data.duration)));

                cmd.Parameters.Add(new SqlParameter("@EndTime", SqlDbType.DateTime));
                cmd.Parameters["@EndTime"].Value = h;

                cmd.Parameters.Add(new SqlParameter("@IsCountry", SqlDbType.Bit));
                cmd.Parameters["@IsCountry"].Value = 0;


                cmd.Parameters.Add(new SqlParameter("@IsState", SqlDbType.Bit));
                cmd.Parameters["@IsState"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsCity", SqlDbType.Bit));
                cmd.Parameters["@IsCity"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsDealer", SqlDbType.Bit));
                cmd.Parameters["@IsDealer"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsClient", SqlDbType.Bit));
                cmd.Parameters["@IsClient"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@StateCountryId", SqlDbType.BigInt));
                cmd.Parameters["@StateCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@CityStateId", SqlDbType.BigInt));
                cmd.Parameters["@CityStateId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@CityCountryId", SqlDbType.BigInt));
                cmd.Parameters["@CityCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@DealerCountryId", SqlDbType.BigInt));
                cmd.Parameters["@DealerCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@ClientCountryId", SqlDbType.BigInt));
                cmd.Parameters["@ClientCountryId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsToken", SqlDbType.Bit));
                cmd.Parameters["@IsToken"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@TokenDealerId", SqlDbType.BigInt));
                cmd.Parameters["@TokenDealerId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@DealerId", SqlDbType.BigInt));
                cmd.Parameters["@DealerId"].Value = data.cId;

                cmd.Parameters.Add(new SqlParameter("@IsAllCountry", SqlDbType.Bit));
                cmd.Parameters["@IsAllCountry"].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@IsAllState", SqlDbType.Bit));
                cmd.Parameters["@IsAllState"].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@IsAllCity", SqlDbType.Bit));
                cmd.Parameters["@IsAllCity"].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@IsAllDealer", SqlDbType.Bit));
                cmd.Parameters["@IsAllDealer"].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@IsAllClient", SqlDbType.Bit));
                cmd.Parameters["@IsAllClient"].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@IsAllToken", SqlDbType.Bit));
                cmd.Parameters["@IsAllToken"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsAllAdminToken", SqlDbType.Bit));
                cmd.Parameters["@IsAllAdminToken"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsAdminToken", SqlDbType.Bit));
                cmd.Parameters["@IsAdminToken"].Value = 0;
                Int32 ReturnPrayerId = Convert.ToInt32(cmd.ExecuteScalar());
                foreach (var iToken in data.tokenid)
                {
                    DataRow nr = dtInsert.NewRow();
                    nr["PrayerId"] = ReturnPrayerId;
                    nr["CountryId"] = 0;
                    nr["StateId"] = 0;
                    nr["CityId"] = 0;
                    nr["DealerId"] = 0;
                    nr["ClientId"] = 0;
                    nr["IsAllCountry"] = 0;
                    nr["IsAllState"] = 0;
                    nr["IsAllCity"] = 0;
                    nr["IsAllDealer"] = 0;
                    nr["IsAllClient"] = 0;
                    nr["TokenId"] = iToken;
                    nr["IsAllToken"] = 0;
                    nr["AdminTokenId"] = 0;
                    nr["IsAllAdminToken"] = 0;
                    dtInsert.Rows.Add(nr);
                }

                if (dtInsert.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {

                        SqlBulkCopyColumnMapping AdvtId =
                         new SqlBulkCopyColumnMapping("PrayerId", "PrayerId");
                        bulkCopy.ColumnMappings.Add(AdvtId);

                        SqlBulkCopyColumnMapping CountryId =
                            new SqlBulkCopyColumnMapping("CountryId", "CountryId");
                        bulkCopy.ColumnMappings.Add(CountryId);

                        SqlBulkCopyColumnMapping StateId =
                           new SqlBulkCopyColumnMapping("StateId", "StateId");
                        bulkCopy.ColumnMappings.Add(StateId);

                        SqlBulkCopyColumnMapping CityId =
                            new SqlBulkCopyColumnMapping("CityId", "CityId");
                        bulkCopy.ColumnMappings.Add(CityId);

                        SqlBulkCopyColumnMapping DealerId =
                            new SqlBulkCopyColumnMapping("DealerId", "DealerId");
                        bulkCopy.ColumnMappings.Add(DealerId);

                        SqlBulkCopyColumnMapping ClientId =
                           new SqlBulkCopyColumnMapping("ClientId", "ClientId");
                        bulkCopy.ColumnMappings.Add(ClientId);


                        SqlBulkCopyColumnMapping IsAllCountry =
                         new SqlBulkCopyColumnMapping("IsAllCountry", "IsAllCountry");
                        bulkCopy.ColumnMappings.Add(IsAllCountry);

                        SqlBulkCopyColumnMapping IsAllState =
                            new SqlBulkCopyColumnMapping("IsAllState", "IsAllState");
                        bulkCopy.ColumnMappings.Add(IsAllState);

                        SqlBulkCopyColumnMapping IsAllCity =
                           new SqlBulkCopyColumnMapping("IsAllCity", "IsAllCity");
                        bulkCopy.ColumnMappings.Add(IsAllCity);

                        SqlBulkCopyColumnMapping IsAllDealer =
                            new SqlBulkCopyColumnMapping("IsAllDealer", "IsAllDealer");
                        bulkCopy.ColumnMappings.Add(IsAllDealer);

                        SqlBulkCopyColumnMapping IsAllClient =
                            new SqlBulkCopyColumnMapping("IsAllClient", "IsAllClient");
                        bulkCopy.ColumnMappings.Add(IsAllClient);

                        SqlBulkCopyColumnMapping AdminTokenId =
                           new SqlBulkCopyColumnMapping("AdminTokenId", "AdminTokenId");
                        bulkCopy.ColumnMappings.Add(AdminTokenId);

                        SqlBulkCopyColumnMapping IsAllAdminToken =
                            new SqlBulkCopyColumnMapping("IsAllAdminToken", "IsAllAdminToken");
                        bulkCopy.ColumnMappings.Add(IsAllAdminToken);

                        SqlBulkCopyColumnMapping TokenId =
                            new SqlBulkCopyColumnMapping("TokenId", "TokenId");
                        bulkCopy.ColumnMappings.Add(TokenId);

                        SqlBulkCopyColumnMapping IsAllToken =
                           new SqlBulkCopyColumnMapping("IsAllToken", "IsAllToken");
                        bulkCopy.ColumnMappings.Add(IsAllToken);

                        bulkCopy.DestinationTableName = "dbo.tbprayerdetail";



                        bulkCopy.WriteToServer(dtInsert);

                    }
                }


                con.Close();
                Result.Responce = "1";
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }


        public List<ResSearchPrayer> FillSearchPayer(ReqSearchPrayer data)
        {
            List<ResSearchPrayer> lstSearchp = new List<ResSearchPrayer>();
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string sQr = "spPrayerData_AdminPanel_Web '" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.cDate)) + "'," + data.tokenid + "";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;

                ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstSearchp.Add(new ResSearchPrayer()
                    {
                        id = ds.Rows[i]["pId"].ToString(),
                        sTime = string.Format(fi, "{0:hh:mm tt}", ds.Rows[i]["sTime"]),
                        eTime = string.Format(fi, "{0:hh:mm tt}", ds.Rows[i]["eTime"]),
                    });
                }
                con.Close();
                return lstSearchp;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstSearchp;
            }
        }

        public ResResponce DeletePrayer(ResDeletePrayer data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string strDel = "";
                strDel = "delete from tbPrayerDetail where id=  " + data.id;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                con.Close();
                Result.Responce = "1";
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }

        }
        public ResResponce uLg(ReqLg data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();

                string sQr = "select * from tbAdministratorLogin where loginid= '" + data.email + "' ";
                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);

                sQr = "select * from tbDealerLogin where LoginName='" + data.email + "' and Loginpassword='" + data.password + "'";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                DataTable dsCustomer = new DataTable();
                ad.Fill(dsCustomer);

                if (ds.Rows.Count > 0)
                {
                    //==============================================
                    string decryptpwd = string.Empty;
                    UTF8Encoding encodepwd = new UTF8Encoding();
                    Decoder Decode = encodepwd.GetDecoder();
                    byte[] todecode_byte = Convert.FromBase64String(ds.Rows[0]["password"].ToString());
                    int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                    char[] decoded_char = new char[charCount];
                    Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                    decryptpwd = new String(decoded_char);
                    //==============================================
                    if (decryptpwd == data.password.Trim())
                    {
                        Result.Responce = "1";
                        Result.dfClientId = "2";
                    }
                    else
                    {
                        Result.Responce = "0";
                    }
                }
                else if (dsCustomer.Rows.Count > 0)
                {
                    Result.Responce = "1";
                    Result.dfClientId = dsCustomer.Rows[0]["dfclientid"].ToString();
                }
                else
                {
                    Result.Responce = "0";
                }
                con.Close();
                return Result;

            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }


        //=============================================== Customer Dashboard
        //===================================================================
        //===================================================================

        public ResDashboard GetCustomerTokenDetailSummary(ReqDashboard data)
        {
            ResDashboard Result = new ResDashboard();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";
                DateTime custDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Central Europe Standard Time");
                //custDateTime = Convert.ToDateTime("07-Jun-2018");
                string sDateTime = "";
                string LastStatus = "";
                List<ResTokenInfo> LstTokenDetail = new List<ResTokenInfo>();
                string str = "";
                if (string.IsNullOrEmpty(data.UserId) == true)
                {
                    str = "GetCustomerTokenDetail " + data.clientId + "";
                }
                else
                {
                    str = "GetCustomerTokenDetail " + data.clientId + ", " + data.UserId;
                }
                SqlCommand cmd = new SqlCommand(str, con);
                con.Open();
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                int TotalPlayers = ds.Rows.Count;
                int OnlinePlayer = 0;
                int OfflinePlayer = 0;
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (DBNull.Value == ds.Rows[i]["sDateTime"])
                    {
                        LastStatus = "01-Jan-1900";
                    }
                    else
                    {
                        LastStatus = string.Format(fi, "{0:dd/MMM/yyyy HH:mm:ss}", Convert.ToDateTime(ds.Rows[i]["sDateTime"]));
                    }

                    sDateTime = string.Format(fi, "{0:dd/MMM/yyyy  HH:mm:ss}", Convert.ToDateTime(LastStatus));

                    var tDays = Convert.ToInt32((custDateTime - Convert.ToDateTime(sDateTime)).TotalMinutes);
                    var pStatus = "";
                    if (LastStatus == "01-Jan-1900")
                    {
                        LastStatus = "";
                    }
                    //if (tDays >= 20)
                    //{
                    //    pStatus = "Away";
                    //    OfflinePlayer = OfflinePlayer + 1;
                    //}
                    //if (tDays == 21)
                    //{
                    //    pStatus = "Away";
                    //    OfflinePlayer = OfflinePlayer + 1;
                    //}
                    if (tDays <= 70)
                    {
                        pStatus = "Online";
                        OnlinePlayer = OnlinePlayer + 1;
                    }
                    else
                    {
                        
                         pStatus = "Away";
                        OfflinePlayer = OfflinePlayer + 1;
                    }
                    if ((data.ftype == "Online") && (pStatus == "Online"))
                    {
                        LstTokenDetail.Add(new ResTokenInfo()
                        {
                            tokenid = ds.Rows[i]["tokenid"].ToString(),
                            tokenCode = ds.Rows[i]["tidNo"].ToString(),
                            location = ds.Rows[i]["loc"].ToString(),
                            city = ds.Rows[i]["ciName"].ToString(),

                            lStatus = LastStatus,
                            TotalDays = tDays,
                            pStatus = pStatus,
                            lPlayed = ds.Rows[i]["title"].ToString(),
                            pName = ds.Rows[i]["splPlaylistName"].ToString(),
                        });
                    }
                    else if ((data.ftype == "Offline") && (pStatus == "Away"))
                    {
                        LstTokenDetail.Add(new ResTokenInfo()
                        {
                            tokenid = ds.Rows[i]["tokenid"].ToString(),
                            tokenCode = ds.Rows[i]["tidNo"].ToString(),
                            location = ds.Rows[i]["loc"].ToString(),
                            city = ds.Rows[i]["ciName"].ToString(),
                            lStatus = LastStatus,
                            TotalDays = tDays,
                            pStatus = pStatus,
                            lPlayed = ds.Rows[i]["title"].ToString(),
                            pName = ds.Rows[i]["splPlaylistName"].ToString(),
                        });
                    }
                    else if (data.ftype == "Total")
                    {
                        LstTokenDetail.Add(new ResTokenInfo()
                        {
                            tokenid = ds.Rows[i]["tokenid"].ToString(),
                            tokenCode = ds.Rows[i]["tidNo"].ToString(),
                            location = ds.Rows[i]["loc"].ToString(),
                            city = ds.Rows[i]["ciName"].ToString(),
                            lStatus = LastStatus,
                            TotalDays = tDays,
                            pStatus = pStatus,
                            lPlayed = ds.Rows[i]["title"].ToString(),
                            pName = ds.Rows[i]["splPlaylistName"].ToString(),
                        });

                    }

                }
                con.Close();
                Result.TotalPlayers = TotalPlayers;
                Result.OnlinePlayers = OnlinePlayer;
                Result.OfflinePlayer = OfflinePlayer;
                Result.lstToken = LstTokenDetail;
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }
        public ResResponce GetFCMID(DataCustomerTokenId data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string sQr = "select isnull(NotificationDeviceId,'') as FcmId, isVedioActive,lType, isnull(mediatype,'') as mtype ,isnull(ptype,'') as ptype from AMPlayerTokens where tokenid=  " + data.Tokenid;
                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    Result.Responce = "1";
                    if (ds.Rows[0]["lType"].ToString() == "Desktop")
                    {
                        Result.FcmId = "Desktop";
                    }
                    else
                    {
                        Result.FcmId = ds.Rows[0]["FcmId"].ToString();
                    }
                    Result.IsVideoToken = ds.Rows[0]["isVedioActive"].ToString();
                    Result.MediaType = ds.Rows[0]["mtype"].ToString();
                    Result.PlayerType = ds.Rows[0]["ptype"].ToString();
                }
                else
                {
                    Result.Responce = "0";
                    Result.FcmId = "";
                    Result.IsVideoToken = "";
                }
                con.Close();
                return Result;

            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }

        public List<ResUser> FillUserList(ReqTokenInfo data)
        {
            List<ResUser> lstUser = new List<ResUser>();

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string sQr = "select id, Username, Password from tbUserLogin_web where dfClientid= " + data.clientId + " order by username";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;

                ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstUser.Add(new ResUser()
                    {
                        id = ds.Rows[i]["id"].ToString(),
                        UserName1 = ds.Rows[i]["username"].ToString(),
                        Password1 = ds.Rows[i]["Password"].ToString(),
                    });
                }
                con.Close();
                return lstUser;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstUser;
            }
        }
        public ResUser EditUser(ReqUserInfo data)
        {
            ResUser Result = new ResUser();
            List<ResTokenInfo> lstTokenInfo = new List<ResTokenInfo>();
            List<string> TokenArray = new List<string>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter ad = new SqlDataAdapter();
                con.Open();
                string sQr = "GetUserInfo " + data.UserId + "";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    Result.Responce = "1";
                    Result.id = ds.Rows[0]["id"].ToString();
                    Result.UserName1 = ds.Rows[0]["username"].ToString();
                    Result.Password1 = ds.Rows[0]["Password"].ToString();
                    Result.chkDashboard = Convert.ToBoolean(ds.Rows[0]["chkDashboard"]);
                    Result.chkPlayerDetail = Convert.ToBoolean(ds.Rows[0]["chkPlayerDetail"]);
                    Result.chkPlaylistLibrary = Convert.ToBoolean(ds.Rows[0]["chkPlaylistLibrary"]);
                    Result.chkScheduling = Convert.ToBoolean(ds.Rows[0]["chkScheduling"]);
                    Result.chkAdvertisement = Convert.ToBoolean(ds.Rows[0]["chkAdvertisement"]);
                    Result.chkInstantPlay = Convert.ToBoolean(ds.Rows[0]["chkInstantPlay"]);
                    Result.chkDeleteSong = Convert.ToBoolean(ds.Rows[0]["chkDeleteSong"]);
                    Result.dfClientid = ds.Rows[0]["dfClientid"].ToString();
                    Result.chkInstantApk = Convert.ToBoolean(ds.Rows[0]["chkInstantApk"]);
                    Result.cmbFormat = ds.Rows[0]["fId"].ToString();
                    Result.cmbPlaylist = ds.Rows[0]["splld"].ToString();
                    Result.chkUserAdmin = Convert.ToBoolean(ds.Rows[0]["chkUserAdmin"]);
                    Result.chkUpload = Convert.ToBoolean(ds.Rows[0]["chkUpload"]);
                    Result.chkStreaming = Convert.ToBoolean(ds.Rows[0]["chkStreaming"]);
                    Result.chkCopyData = Convert.ToBoolean(ds.Rows[0]["chkCopyData"]);
                    Result.chkOfflineAlert = Convert.ToBoolean(ds.Rows[0]["chkOfflineAlert"]);
                    Result.OfflineIntervalHour = ds.Rows[0]["OfflineIntervalHour"].ToString();

                    sQr = "select distinct tokenid from tbUserTokens_web where userid = " + data.UserId + "";
                    cmd = new SqlCommand(sQr, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    DataTable dsUserToken = new DataTable();
                    ad.Fill(dsUserToken);




                    cmd = new SqlCommand("GetTokenInfo " + Result.dfClientid, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    DataTable dsTokenAll = new DataTable();
                    ad.Fill(dsTokenAll);

                    for (int i = 0; i < dsTokenAll.Rows.Count; i++)
                    {
                        bool iCheck = dsUserToken.Select().ToList().Exists(row => row["tokenid"].ToString() == dsTokenAll.Rows[i]["tokenid"].ToString());
                        if (iCheck == true)
                        {
                            TokenArray.Add(dsTokenAll.Rows[i]["tokenid"].ToString());
                        }
                        lstTokenInfo.Add(new ResTokenInfo()
                        {
                            tokenid = dsTokenAll.Rows[i]["tokenid"].ToString(),
                            tokenCode = dsTokenAll.Rows[i]["tNo"].ToString(),
                            Name = dsTokenAll.Rows[i]["PersonName"].ToString(),
                            location = dsTokenAll.Rows[i]["Location"].ToString(),
                            city = dsTokenAll.Rows[i]["CityName"].ToString(),
                            countryName = dsTokenAll.Rows[i]["CountryName"].ToString(),
                            playerType = dsTokenAll.Rows[i]["PlType"].ToString(),
                            LicenceType = dsTokenAll.Rows[i]["LiType"].ToString(),
                            tInfo = dsTokenAll.Rows[i]["tInfo"].ToString(),
                            check = iCheck,
                        });
                    }
                    Result.lstTokenInfo = lstTokenInfo;
                    Result.lstToken = TokenArray;
                }
                else
                {
                    Result.Responce = "0";
                }
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }

        public ResResponce DeleteUser(ReqUserInfo data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string strDel = "";
                strDel = "delete from tbUserLogin_web where id=  " + data.UserId;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                strDel = "delete from tbUserTokens_web where userid=  " + data.UserId;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();

                Result.Responce = "1";
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }

        }
        public ResResponce SaveUpdateUser(ResUser data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string tid = "";
                con.Open();
                DataTable dtInsert = new DataTable();
                dtInsert.Columns.Add("userid", typeof(int));
                dtInsert.Columns.Add("tokenid", typeof(int));

                DataTable dtInsertAPK = new DataTable();
                dtInsertAPK.Columns.Add("userid", typeof(int));
                dtInsertAPK.Columns.Add("TokenId", typeof(int));
                dtInsertAPK.Columns.Add("splPlaylistId", typeof(int));
                dtInsertAPK.Columns.Add("formatid", typeof(int));

                SqlCommand cmd = new SqlCommand("SaveUpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                cmd.Parameters["@id"].Value = data.id;

                cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar));
                cmd.Parameters["@UserName"].Value = data.UserName1;

                cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar));
                cmd.Parameters["@Password"].Value = data.Password1;

                cmd.Parameters.Add(new SqlParameter("@chkDashboard", SqlDbType.Int));
                cmd.Parameters["@chkDashboard"].Value = Convert.ToByte(data.chkDashboard);
                cmd.Parameters.Add(new SqlParameter("@chkPlayerDetail", SqlDbType.Int));
                cmd.Parameters["@chkPlayerDetail"].Value = Convert.ToByte(data.chkPlayerDetail);
                cmd.Parameters.Add(new SqlParameter("@chkPlaylistLibrary", SqlDbType.Int));
                cmd.Parameters["@chkPlaylistLibrary"].Value = Convert.ToByte(data.chkPlaylistLibrary);
                cmd.Parameters.Add(new SqlParameter("@chkScheduling", SqlDbType.Int));
                cmd.Parameters["@chkScheduling"].Value = Convert.ToByte(data.chkScheduling);
                cmd.Parameters.Add(new SqlParameter("@chkAdvertisement", SqlDbType.Int));
                cmd.Parameters["@chkAdvertisement"].Value = Convert.ToByte(data.chkAdvertisement);
                cmd.Parameters.Add(new SqlParameter("@chkInstantPlay", SqlDbType.Int));
                cmd.Parameters["@chkInstantPlay"].Value = Convert.ToByte(data.chkInstantPlay);
                cmd.Parameters.Add(new SqlParameter("@dfClientid", SqlDbType.Int));
                cmd.Parameters["@dfClientid"].Value = data.dfClientid;
                cmd.Parameters.Add(new SqlParameter("@chkDeleteSong", SqlDbType.Int));
                cmd.Parameters["@chkDeleteSong"].Value = Convert.ToByte(data.chkDeleteSong);
                cmd.Parameters.Add(new SqlParameter("@chkInstantApk", SqlDbType.Int));
                cmd.Parameters["@chkInstantApk"].Value = Convert.ToByte(data.chkInstantApk);
                cmd.Parameters.Add(new SqlParameter("@chkUserAdmin", SqlDbType.Int));
                cmd.Parameters["@chkUserAdmin"].Value = Convert.ToByte(data.chkUserAdmin);
                cmd.Parameters.Add(new SqlParameter("@chkUpload", SqlDbType.Int));
                cmd.Parameters["@chkUpload"].Value = Convert.ToByte(data.chkUpload);
                cmd.Parameters.Add(new SqlParameter("@chkCopyData", SqlDbType.Int));
                cmd.Parameters["@chkCopyData"].Value = Convert.ToByte(data.chkCopyData);
                cmd.Parameters.Add(new SqlParameter("@chkStreaming", SqlDbType.Int));
                cmd.Parameters["@chkStreaming"].Value = Convert.ToByte(data.chkStreaming);

                cmd.Parameters.Add(new SqlParameter("@chkOfflineAlert", SqlDbType.Int));
                cmd.Parameters["@chkOfflineAlert"].Value = Convert.ToByte(data.chkOfflineAlert);
                cmd.Parameters.Add(new SqlParameter("@OfflineIntervalHour", SqlDbType.Int));
                cmd.Parameters["@OfflineIntervalHour"].Value = Convert.ToByte(data.OfflineIntervalHour);

                Int32 ReturnId = Convert.ToInt32(cmd.ExecuteScalar());
                foreach (var iToken in data.lstToken)
                {
                    DataRow nr = dtInsert.NewRow();
                    nr["userid"] = ReturnId;
                    nr["tokenid"] = iToken;
                    dtInsert.Rows.Add(nr);
                }
                if (data.chkInstantApk == true)
                {
                    foreach (var iToken in data.lstToken)
                    {

                        DataRow nr = dtInsertAPK.NewRow();
                        nr["userid"] = ReturnId;
                        nr["splPlaylistId"] = data.cmbPlaylist;
                        nr["TokenId"] = iToken;
                        nr["formatid"] = data.cmbFormat;
                        dtInsertAPK.Rows.Add(nr);
                        if (tid == "")
                        {
                            tid = iToken;
                        }
                        else
                        {
                            tid = tid + "," + iToken;
                        }
                    }
                }
                if (dtInsert.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        SqlBulkCopyColumnMapping userid =
                         new SqlBulkCopyColumnMapping("userid", "userid");
                        bulkCopy.ColumnMappings.Add(userid);

                        SqlBulkCopyColumnMapping tokenid =
                        new SqlBulkCopyColumnMapping("tokenid", "tokenid");
                        bulkCopy.ColumnMappings.Add(tokenid);

                        bulkCopy.DestinationTableName = "dbo.tbUserTokens_web";
                        bulkCopy.WriteToServer(dtInsert);
                    }
                }

                if (dtInsertAPK.Rows.Count > 0)
                {


                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        SqlBulkCopyColumnMapping TokenId =
                         new SqlBulkCopyColumnMapping("TokenId", "TokenId");
                        bulkCopy.ColumnMappings.Add(TokenId);

                        SqlBulkCopyColumnMapping splPlaylistId =
                        new SqlBulkCopyColumnMapping("splPlaylistId", "splPlaylistId");
                        bulkCopy.ColumnMappings.Add(splPlaylistId);

                        SqlBulkCopyColumnMapping formatid =
                         new SqlBulkCopyColumnMapping("formatid", "formatid");
                        bulkCopy.ColumnMappings.Add(formatid);

                        SqlBulkCopyColumnMapping userid =
                        new SqlBulkCopyColumnMapping("userid", "userid");
                        bulkCopy.ColumnMappings.Add(userid);

                        bulkCopy.DestinationTableName = "dbo.tbInstantPlayPlaylist";
                        bulkCopy.WriteToServer(dtInsertAPK);
                    }
                }
                con.Close();
                Result.Responce = "1";
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }
        public ResResponce CustomerLogin(ReqLg data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();

                string sQr = "select * from tbLogin_DJ where login= '" + data.email + "' and pwd='" + data.password + "'";
                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                ad.Dispose();
                if (dt.Rows.Count > 0)
                {
                    Result.Responce = "1";
                    Result.dfClientId = "6";
                    Result.IsRf = "0";
                    Result.UserId = "-1";
                    Result.chkDashboard = false;
                    Result.chkPlayerDetail = false;
                    Result.chkPlaylistLibrary = false;
                    Result.chkScheduling = false;
                    Result.chkAdvertisement = false;
                    Result.chkInstantPlay = false;
                    Result.chkUserAdmin = false;
                    Result.ContentType = "Both";
                    Result.chkUpload = false;
                    Result.chkStreaming = false;
                    Result.chkCopyData = false;
                    con.Close();
                    return Result;
                }



                sQr = "spCustomerUserLogin  '" + data.email + "' ,'" + data.password + "'";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                DataTable dsUser = new DataTable();
                ad.Fill(dsUser);
                ad.Dispose();

                sQr = "spCustomerLogin '" + data.email + "', '" + data.password + "'";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                DataTable dsCustomer = new DataTable();
                ad.Fill(dsCustomer);
                ad.Dispose();
                if (dsUser.Rows.Count > 0)
                {
                    if (dsUser.Rows[0]["DBType"].ToString() == data.DBType)
                    {
                        Result.Responce = "1";
                    }
                    else
                    {
                        Result.Responce = "0";
                    }
                    Result.dfClientId = dsUser.Rows[0]["dfclientid"].ToString();
                    Result.UserId = dsUser.Rows[0]["id"].ToString();
                    Result.IsRf = Convert.ToByte(dsUser.Rows[0]["IsDam"]).ToString();
                    Result.chkDashboard = Convert.ToBoolean(dsUser.Rows[0]["chkDashboard"]);
                    Result.chkPlayerDetail = Convert.ToBoolean(dsUser.Rows[0]["chkPlayerDetail"]);
                    Result.chkPlaylistLibrary = Convert.ToBoolean(dsUser.Rows[0]["chkPlaylistLibrary"]);
                    Result.chkScheduling = Convert.ToBoolean(dsUser.Rows[0]["chkScheduling"]);
                    Result.chkAdvertisement = Convert.ToBoolean(dsUser.Rows[0]["chkAdvertisement"]);
                    Result.chkInstantPlay = Convert.ToBoolean(dsUser.Rows[0]["chkInstantPlay"]);
                    Result.chkUserAdmin = Convert.ToBoolean(dsUser.Rows[0]["chkUserAdmin"]);
                    Result.ContentType = dsUser.Rows[0]["ContentType"].ToString();
                    Result.chkUpload = Convert.ToBoolean(dsUser.Rows[0]["chkUpload"]);
                    Result.chkStreaming = Convert.ToBoolean(dsUser.Rows[0]["chkStreaming"]);
                    Result.chkCopyData = Convert.ToBoolean(dsUser.Rows[0]["chkCopyData"]);
                    Result.ClientName = dsUser.Rows[0]["ClientName"].ToString();
                }
                else if (dsCustomer.Rows.Count > 0)
                {
                    if (dsCustomer.Rows[0]["DBType"].ToString() == data.DBType)
                    {
                        Result.Responce = "1";
                    }
                    else
                    {
                        Result.Responce = "0";
                    }
                    Result.dfClientId = dsCustomer.Rows[0]["dfclientid"].ToString();
                    Result.IsRf = Convert.ToByte(dsCustomer.Rows[0]["IsDam"]).ToString();
                    Result.UserId = "0";
                    Result.chkDashboard = true;
                    Result.chkPlayerDetail = true;
                    Result.chkPlaylistLibrary = true;
                    Result.chkScheduling = true;
                    Result.chkAdvertisement = true;
                    Result.chkInstantPlay = true;
                    Result.chkUserAdmin = true;
                    Result.chkUpload = true;
                    Result.chkStreaming = true;
                    Result.chkCopyData = true;
                    Result.ContentType = dsCustomer.Rows[0]["ContentType"].ToString();
                    Result.ClientName = dsCustomer.Rows[0]["ClientName"].ToString();
                }
                else
                {
                    Result.Responce = "0";
                }
                con.Close();
                return Result;

            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }

        public List<ResPlayerLog> FillPlayedSongsLog(ReqPlayerLog data)
        {
            List<ResPlayerLog> lstTd = new List<ResPlayerLog>();
            string OldTitle = "";
            string cs = ConfigurationManager.ConnectionStrings["Panel"].ConnectionString;
            SqlConnection constr = new SqlConnection(cs);
            if (string.IsNullOrEmpty(data.ToDate) == true)
            {
                data.ToDate = data.cDate;
            }
            string str = "GetTokenPlayedSongsDetailPart2_web " + data.tokenid + "  ,'" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.cDate)) + "','" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.ToDate)) + "'";
            SqlCommand cmd = new SqlCommand(str, constr);
            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";
                //                string rSave = "0";
                //              rSave = AppDomain.CurrentDomain.BaseDirectory;
                //            string path = Path.GetDirectoryName(rSave) + "\\data.txt";
                //          string WriteData = "";
                //        DateTime custDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "India Standard Time");



                //      WriteData = "" + str + " , " + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.cDate)) + ", " + data.cDate + ", {0} ";
                //    using (StreamWriter writer = new StreamWriter(path, true))
                //  {
                //    writer.WriteLine(string.Format(WriteData, custDateTime.ToString("dd/MMM/yyyy hh:mm:ss tt")));
                //  writer.Close();
                //}

                constr.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (OldTitle != rdr["title"].ToString())
                    {
                        OldTitle = rdr["title"].ToString();
                        ResPlayerLog td = new ResPlayerLog();
                        td.PlayedDateTime = string.Format(fi, "{0:dd-MMM-yyyy}", rdr["playdate"]) + ' ' + string.Format(fi, "{0:HH:mm:ss}", rdr["pDateTime"]);
                        td.Name = rdr["title"].ToString();
                        td.ArtistName = rdr["name"].ToString();
                        td.SplPlaylistName = rdr["splPlaylistname"].ToString();
                        td.CategoryName = rdr["CategoryName"].ToString();
                        td.pDateTime = rdr["pDateTime"].ToString();
                        lstTd.Add(td);
                    }
                }
                constr.Close();
                return lstTd;
            }
            catch (Exception ex)
            {
                constr.Close();

                HttpContext.Current.Response.StatusCode = 1;
                return lstTd;
            }
        }
        public List<ResPlayerLog> FillPlayedAdsLog(ReqPlayerLog data)
        {
            List<ResPlayerLog> lstTd = new List<ResPlayerLog>();
            string cs = ConfigurationManager.ConnectionStrings["Panel"].ConnectionString;
            SqlConnection constr = new SqlConnection(cs);
            string str = "GetAdvtStatus_Web " + data.tokenid + ",'" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.cDate)) + "','" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.cDate)) + "'";
            //str = "GetAdvtStatus_Web  292,'01-May-2019','01-May-2019'";
            try
            {


                SqlCommand cmd = new SqlCommand(str, constr);

                constr.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    ResPlayerLog td = new ResPlayerLog();
                    td.PlayedDateTime = string.Format("{0:dd-MMM-yyyy HH:mm:ss}", rdr["aDateTime"]);
                    td.Name = rdr["AdvtName"].ToString();
                    td.ArtistName = "";
                    td.SplPlaylistName = "";
                    td.CategoryName = "";
                    td.pDateTime = DateTime.Now.ToString();
                    td.TotalPlayed = "";
                    lstTd.Add(td);

                }
                rdr.Close();
                constr.Close();
                return lstTd;
            }
            catch (Exception ex)
            {
                constr.Close();

                HttpContext.Current.Response.StatusCode = 1;
                return lstTd;
            }
        }


        //public void SaveState(ReqState data)
        //        {
        //            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

        //            try
        //            {
        //                SqlCommand cmd = new SqlCommand("SaveState", con);
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.BigInt));
        //                cmd.Parameters["@CountryId"].Value = data.countryId;

        //                cmd.Parameters.Add(new SqlParameter("@StateName", SqlDbType.VarChar));
        //                cmd.Parameters["@StateName"].Value = data.StateName;

        //                con.Open();
        //               var returnValue = cmd.ExecuteScalar().ToString();
        //                if (returnValue != "-2")
        //                {
        //                    strState = "select * from tbState where countryId= " + Convert.ToInt32(cmbCountryName.SelectedValue);
        //                    objMainClass.fnFillComboBox(strState, cmbStateName, "stateid", "StateName", "");
        //                    cmbStateName.SelectedValue = Convert.ToInt32(returnValue);
        //                    panMainNew.Visible = false;
        //                    lblCaption.Text = "";
        //                }
        //                if (returnValue == "-2")
        //                {
        //                    MessageBox.Show("State Name already exixts", "Management Panel");
        //                    panMainNew.Visible = false;
        //                    lblCaption.Text = "";
        //                    return;
        //                }
        //            }
        //            catch (Exception ex) { }

        //        }



        public List<ResponceSplSplaylistTitle> GetSplPlaylistTitlesLiveFixed(DataSplPlaylistTile data)
        {
            List<ResponceSplSplaylistTitle> result = new List<ResponceSplSplaylistTitle>();

            try
            {


                int it = 0;
                for (int i = 0; i < 10; i++)
                {
                    var mtypeFormat = ".jpg";
                    if (i == 2)
                    {
                        mtypeFormat = ".mp4";
                    }
                    if (i == 6)
                    {
                        mtypeFormat = ".mp4";
                    }
                    it = i + 1;
                    result.Add(new ResponceSplSplaylistTitle()
                    {

                        splPlaylistId = data.splPlaylistId,
                        titleId = i + 1,
                        Title = "Title" + i,
                        tTime = "",
                        ArtistID = i + 2,
                        arName = "arName" + i,
                        AlbumID = i + 3,
                        alName = "alName" + i,
                        srno = i + 1,
                        TitleUrl = "http://85.195.82.94/ImageDemo/" + it + mtypeFormat,

                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }



        public List<ResponceSplSplaylist> GetSplPlaylistVideo1(DataSplPlaylist data)
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            List<ResponceSplSplaylist> result = new List<ResponceSplSplaylist>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["VideoCon"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("GetSpecialPlaylistSchedule " + data.WeekNo + "," + data.TokenId + "," + data.DfClientId + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var e_time = "";
                    if (string.Format(fi, "{0:hh:mm tt}", ds.Tables[0].Rows[i]["EndTime"]) == "11:59 PM")
                    {
                        e_time = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"]).AddSeconds(59).ToString();
                    }
                    else
                    {
                        e_time = ds.Tables[0].Rows[i]["EndTime"].ToString();
                    }
                    result.Add(new ResponceSplSplaylist()
                    {
                        pScid = Convert.ToInt32(ds.Tables[0].Rows[i]["pSchid"]),
                        dfclientid = Convert.ToInt32(ds.Tables[0].Rows[i]["dfClientId"]),
                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        splPlaylistName = ds.Tables[0].Rows[i]["splPlaylistName"].ToString(),
                        StartTime = ds.Tables[0].Rows[i]["StartTime"].ToString(),
                        EndTime = e_time,
                        IsSeprationActive = Convert.ToInt32(ds.Tables[0].Rows[i]["isShowDefault"]),
                        IsFadingActive = Convert.ToInt32(ds.Tables[0].Rows[i]["IsFadingActive"]),
                        FormatId = 0,
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce CustomerLoginDetail(ReqTokenInfo data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string sQr = "select * from tbDealerLogin where dfclientid= " + data.clientId + "";
                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dsUser = new DataTable();
                ad.Fill(dsUser);
                if (dsUser.Rows.Count > 0)
                {
                    Result.Responce = "1";
                    Result.LoginName = dsUser.Rows[0]["LoginName"].ToString();
                    Result.LoginPassword = dsUser.Rows[0]["LoginPassword"].ToString();
                }
                else
                {
                    Result.Responce = "0";
                    Result.LoginName = "";
                    Result.LoginPassword = "";
                }
                con.Close();
                return Result;

            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }

        public ResResponce DeletePlaylist(ReqDeletePlaylistSong data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                con.Open();
                if ((string.IsNullOrEmpty(data.IsForceDelete) == true) || (data.IsForceDelete == "No"))
                {
                    DataTable dtDetail = new DataTable();
                    strDel = "select tbSpecialPlaylistSchedule_Token.* from tbSpecialPlaylistSchedule";
                    strDel = strDel + " inner join tbSpecialPlaylistSchedule_Token on tbSpecialPlaylistSchedule_Token.pschid= tbSpecialPlaylistSchedule.pschid";
                    strDel = strDel + " where tbSpecialPlaylistSchedule.splplaylistid = " + data.playlistid;
                    SqlCommand cmdCheck = new SqlCommand(strDel, con);
                    cmdCheck.CommandType = System.Data.CommandType.Text;

                    SqlDataAdapter ad = new SqlDataAdapter(cmdCheck);
                    DataTable ds = new DataTable();
                    ad.Fill(ds);
                    if (ds.Rows.Count > 0)
                    {
                        con.Close();
                        result.Responce = "2";
                        return result;
                    }
                }

                strDel = "delete from tbSpecialPlaylists_Titles where splPlaylistId= " + data.playlistid;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                strDel = "delete from tbSpecialPlaylists where splPlaylistId= " + data.playlistid;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                strDel = "delete from tbSpecialPlaylistSchedule where splPlaylistId= " + data.playlistid;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }
        public ResResponce SaveFormat(ReqSaveFormat data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("spSaveSpecialFormat", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                //if (btnSaveNew.Text == "Update")
                //{
                //    cmd.Parameters["@FormatId"].Value = Convert.ToInt32(cmbFormat.SelectedValue);
                //}
                //else
                //{
                cmd.Parameters["@FormatId"].Value = data.id;
                // }


                cmd.Parameters.Add(new SqlParameter("@FormatName", SqlDbType.VarChar));
                cmd.Parameters["@FormatName"].Value = data.formatname;

                cmd.Parameters.Add(new SqlParameter("@R", SqlDbType.Int));
                cmd.Parameters["@R"].Value = 255;

                cmd.Parameters.Add(new SqlParameter("@G", SqlDbType.Int));
                cmd.Parameters["@G"].Value = 255;

                cmd.Parameters.Add(new SqlParameter("@B", SqlDbType.Int));
                cmd.Parameters["@B"].Value = 255;

                cmd.Parameters.Add(new SqlParameter("@DfClientId", SqlDbType.BigInt));
                cmd.Parameters["@DfClientId"].Value = data.dfclientId;

                cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                cmd.Parameters["@pVersion"].Value = "c";

                cmd.Parameters.Add(new SqlParameter("@sTime", SqlDbType.DateTime));
                cmd.Parameters["@sTime"].Value = string.Format("{0:hh:mm tt}", DateTime.Now);

                cmd.Parameters.Add(new SqlParameter("@eTime", SqlDbType.DateTime));
                cmd.Parameters["@eTime"].Value = string.Format("{0:hh:mm tt}", DateTime.Now);

                cmd.Parameters.Add(new SqlParameter("@TotalHour", SqlDbType.Int));
                cmd.Parameters["@TotalHour"].Value = "24";

                cmd.Parameters.Add(new SqlParameter("@Is24Hour", SqlDbType.Bit));
                cmd.Parameters["@Is24Hour"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@dbtype", SqlDbType.VarChar));
                cmd.Parameters["@dbtype"].Value = data.DBType;

                cmd.Parameters.Add(new SqlParameter("@MediaType", SqlDbType.NVarChar));
                cmd.Parameters["@MediaType"].Value = data.MediaType;

                con.Open();
                clsResult.Responce = cmd.ExecuteScalar().ToString();

                con.Close();


                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }


        public ResResponce SaveCopySchedule(ReqCopySchedule data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                string str = "";
                foreach (var tlist in data.TokenList)
                {
                    str = "";
                    str = "delete from  tbSpecialPlaylistSchedule_Token where tokenid = " + tlist;
                    cmd = new SqlCommand(str, con);
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                foreach (var schList in data.SchList)
                {
                    str = "";
                    str = "select FormatName, dbtype, DfClientId, isnull(mediatype,'') as mtype from tbSpecialFormat where formatid= " + schList.formatid;
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    DataTable dt = new DataTable();
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(dt);
                    cmd.Dispose();
                    ad.Dispose();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (data.dfClientId.ToString() != dt.Rows[i]["DfClientId"].ToString())
                        {
                            cmd = new SqlCommand("spCloneFormat", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                            cmd.Parameters["@FormatId"].Value = "0";

                            cmd.Parameters.Add(new SqlParameter("@FormatName", SqlDbType.VarChar));
                            cmd.Parameters["@FormatName"].Value = dt.Rows[i]["FormatName"].ToString();

                            cmd.Parameters.Add(new SqlParameter("@R", SqlDbType.Int));
                            cmd.Parameters["@R"].Value = 255;

                            cmd.Parameters.Add(new SqlParameter("@G", SqlDbType.Int));
                            cmd.Parameters["@G"].Value = 255;

                            cmd.Parameters.Add(new SqlParameter("@B", SqlDbType.Int));
                            cmd.Parameters["@B"].Value = 255;

                            cmd.Parameters.Add(new SqlParameter("@DfClientId", SqlDbType.BigInt));
                            cmd.Parameters["@DfClientId"].Value = data.dfClientId;

                            cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                            cmd.Parameters["@pVersion"].Value = "c";

                            cmd.Parameters.Add(new SqlParameter("@sTime", SqlDbType.DateTime));
                            cmd.Parameters["@sTime"].Value = string.Format("{0:hh:mm tt}", DateTime.Now);

                            cmd.Parameters.Add(new SqlParameter("@eTime", SqlDbType.DateTime));
                            cmd.Parameters["@eTime"].Value = string.Format("{0:hh:mm tt}", DateTime.Now);

                            cmd.Parameters.Add(new SqlParameter("@TotalHour", SqlDbType.Int));
                            cmd.Parameters["@TotalHour"].Value = "24";

                            cmd.Parameters.Add(new SqlParameter("@Is24Hour", SqlDbType.Bit));
                            cmd.Parameters["@Is24Hour"].Value = 1;

                            cmd.Parameters.Add(new SqlParameter("@dbtype", SqlDbType.VarChar));
                            cmd.Parameters["@dbtype"].Value = dt.Rows[i]["dbtype"].ToString();

                            cmd.Parameters.Add(new SqlParameter("@MediaType", SqlDbType.NVarChar));
                            cmd.Parameters["@MediaType"].Value = dt.Rows[i]["mtype"].ToString();

                            string formatId_new = cmd.ExecuteScalar().ToString();
                            cmd.Dispose();

                            cmd = new SqlCommand("ClonePlaylist", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@splId", SqlDbType.Int));
                            cmd.Parameters["@splId"].Value = schList.splPlaylistId;
                            cmd.Parameters.Add(new SqlParameter("@Formatid", SqlDbType.Int));
                            cmd.Parameters["@Formatid"].Value = formatId_new;
                            string splId_new = cmd.ExecuteScalar().ToString();
                            cmd.Dispose();

                            schList.formatid = formatId_new;
                            schList.splPlaylistId = splId_new;
                        }
                    }
                }
                foreach (var schList in data.SchList)
                {



                    cmd = new SqlCommand("spSaveSpecialPlaylistSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                    cmd.Parameters["@pSchId"].Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                    cmd.Parameters["@pVersion"].Value = "c";

                    cmd.Parameters.Add(new SqlParameter("@dfClientId", SqlDbType.BigInt));
                    cmd.Parameters["@dfClientId"].Value = data.dfClientId;

                    cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                    cmd.Parameters["@splPlaylistId"].Value = schList.splPlaylistId;


                    cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime));
                    cmd.Parameters["@StartTime"].Value = schList.StartTime;

                    cmd.Parameters.Add(new SqlParameter("@EndTime", SqlDbType.DateTime));
                    cmd.Parameters["@EndTime"].Value = schList.EndTime;

                    cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                    cmd.Parameters["@FormatId"].Value = schList.formatid;

                    Int32 rtPschId = Convert.ToInt32(cmd.ExecuteScalar());

                    str = "";
                    str = "delete from tbSpecialPlaylistSchedule_Weekday where pSchId=" + rtPschId;
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    str = "";
                    str = "select * from tbSpecialPlaylistSchedule_WeekDay where pschid=" + schList.id;
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    DataTable dtWeek = new DataTable();
                    ad.Fill(dtWeek);
                    if (dtWeek.Rows.Count > 0)
                    {
                        for (int iWeek = 0; iWeek < dtWeek.Rows.Count; iWeek++)
                        {
                            cmd = new SqlCommand("spSaveSpecialPlaylistSchedule_Week", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                            cmd.Parameters["@pSchId"].Value = rtPschId;

                            cmd.Parameters.Add(new SqlParameter("@wId", SqlDbType.Int));
                            if (Convert.ToInt32(dtWeek.Rows[iWeek]["wId"]) == 0)
                            {
                                cmd.Parameters["@wId"].Value = 0;
                            }
                            else
                            {
                                cmd.Parameters["@wId"].Value = Convert.ToInt32(dtWeek.Rows[iWeek]["wId"]);
                            }
                            cmd.Parameters.Add(new SqlParameter("@IsAllWeek", SqlDbType.Int));
                            if (Convert.ToInt32(dtWeek.Rows[iWeek]["wId"]) == 0)
                            {
                                cmd.Parameters["@IsAllWeek"].Value = 1;
                            }
                            else
                            {
                                cmd.Parameters["@IsAllWeek"].Value = 0;
                            }
                            cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                            cmd.Parameters["@FormatId"].Value = schList.formatid;
                            cmd.ExecuteNonQuery();
                        }
                    }//Week

                    foreach (var tokenId in data.TokenList)
                    {
                        cmd = new SqlCommand("spSaveSpecialPlaylistSchedule_Token", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                        cmd.Parameters["@pSchId"].Value = rtPschId;

                        cmd.Parameters.Add(new SqlParameter("@tokenId", SqlDbType.BigInt));
                        cmd.Parameters["@tokenId"].Value = tokenId;

                        cmd.Parameters.Add(new SqlParameter("@IsAllToken", SqlDbType.Int));
                        cmd.Parameters["@IsAllToken"].Value = 0;

                        cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                        cmd.Parameters["@FormatId"].Value = schList.formatid;

                        cmd.Parameters.Add(new SqlParameter("@DfClientid", SqlDbType.BigInt));
                        cmd.Parameters["@DfClientid"].Value = data.dfClientId;

                        cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                        cmd.Parameters["@splPlaylistId"].Value = schList.splPlaylistId;
                        cmd.ExecuteNonQuery();
                        result.Responce = "1";
                    }//Token

                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }
        public ResResponce UploadImage()
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            con.Open();
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                var GenreId = HttpContext.Current.Request.Form[0];
                var GenreName = HttpContext.Current.Request.Form[1];
                var CustomerId = HttpContext.Current.Request.Form[2];
                var MediaType = HttpContext.Current.Request.Form[3];
                var folderId = HttpContext.Current.Request.Form[4];
                var dbType = HttpContext.Current.Request.Form[5];
                var IsAnnouncement = HttpContext.Current.Request.Form[6];
                var k = postedFile.ContentLength;
                var Artist = "";
                var Title = "";

                var ext = Path.GetExtension(postedFile.FileName.ToString());
                if ((ext == ".mp4") && (MediaType.Trim() == "Image"))
                {
                    Result.Responce = "0";
                    Result.message = "Media Type is not match with content";
                    con.Close();
                    return Result;
                }


                if (MediaType.Trim() == "Image")
                {
                    Artist = "Image";
                    Title = Path.GetFileNameWithoutExtension(postedFile.FileName.ToString());
                }
                if (MediaType.Trim() == "Video")
                {
                    var ti = Path.GetFileNameWithoutExtension(postedFile.FileName.ToString()).Split('-');
                    if (ti.Length == 2)
                    {
                        Title = ti[1].ToString();
                        Artist = ti[0].ToString();
                    }
                    else
                    {
                        Title = Path.GetFileNameWithoutExtension(postedFile.FileName.ToString());
                        Artist = "Video";

                    }
                }
                SqlCommand cmd = new SqlCommand("InsertContent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@TiTleTiTle", SqlDbType.VarChar));
                cmd.Parameters["@TiTleTiTle"].Value = Title;

                cmd.Parameters.Add(new SqlParameter("@TitleArtistName", SqlDbType.VarChar));
                cmd.Parameters["@TitleArtistName"].Value = Artist;

                cmd.Parameters.Add(new SqlParameter("@AlbumName", SqlDbType.VarChar));
                cmd.Parameters["@AlbumName"].Value = MediaType;

                cmd.Parameters.Add(new SqlParameter("@titlecategoryid", SqlDbType.BigInt));
                cmd.Parameters["@titlecategoryid"].Value = 4;

                cmd.Parameters.Add(new SqlParameter("@titleSubcategoryid", SqlDbType.VarChar));
                cmd.Parameters["@titleSubcategoryid"].Value = 22;

                cmd.Parameters.Add(new SqlParameter("@Time", SqlDbType.VarChar));
                cmd.Parameters["@Time"].Value = "00:00:00";

                cmd.Parameters.Add(new SqlParameter("@AlbumLabel", SqlDbType.VarChar));
                cmd.Parameters["@AlbumLabel"].Value = "0";

                cmd.Parameters.Add(new SqlParameter("@CatalogCode", SqlDbType.VarChar));
                cmd.Parameters["@CatalogCode"].Value = "0";

                cmd.Parameters.Add(new SqlParameter("@titleYear", SqlDbType.Int));
                cmd.Parameters["@titleYear"].Value = 0;


                cmd.Parameters.Add(new SqlParameter("@GenreId", SqlDbType.Int));
                cmd.Parameters["@GenreId"].Value = GenreId;

                cmd.Parameters.Add(new SqlParameter("@tempo", SqlDbType.VarChar));
                cmd.Parameters["@tempo"].Value = "Mid";


                cmd.Parameters.Add(new SqlParameter("@mType", SqlDbType.VarChar));
                cmd.Parameters["@mType"].Value = MediaType.Trim();

                cmd.Parameters.Add(new SqlParameter("@acategory", SqlDbType.VarChar));
                cmd.Parameters["@acategory"].Value = GenreName.Trim();

                cmd.Parameters.Add(new SqlParameter("@language", SqlDbType.VarChar));
                cmd.Parameters["@language"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@isRF", SqlDbType.VarChar));
                cmd.Parameters["@isRF"].Value = "0";

                cmd.Parameters.Add(new SqlParameter("@isrc", SqlDbType.VarChar));
                cmd.Parameters["@isrc"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.VarChar));
                cmd.Parameters["@FileSize"].Value = postedFile.ContentLength;

                cmd.Parameters.Add(new SqlParameter("@dfclientid", SqlDbType.BigInt));
                cmd.Parameters["@dfclientid"].Value = CustomerId;

                cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt));
                cmd.Parameters["@folderid"].Value = folderId;

                cmd.Parameters.Add(new SqlParameter("@dbType", SqlDbType.VarChar));
                cmd.Parameters["@dbType"].Value = dbType.Trim();

                cmd.Parameters.Add(new SqlParameter("@IsAnnouncement", SqlDbType.Int));
                cmd.Parameters["@IsAnnouncement"].Value = IsAnnouncement;

                Int32 Title_Id = Convert.ToInt32(cmd.ExecuteScalar());



                //string fName = Title_Id.ToString() + Path.GetExtension(postedFile.FileName);

                if (Title_Id == 0)
                {
                    Result.Responce = "2";
                    con.Close();
                    return Result;

                }
                string fName = "";
                if (MediaType.Trim() == "Image")
                {
                    fName = Title_Id.ToString() + ".jpg";
                }
                if (MediaType.Trim() == "Video")
                {
                    fName = Title_Id.ToString() + ".mp4";
                }
                var filePath = HttpContext.Current.Server.MapPath("~/mp3files/" + fName);
                postedFile.SaveAs(filePath);


                Result.Responce = "1";
                Result.TitleId = Title_Id.ToString();
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                var g = ex.Message;
                con.Close();
                return Result;
            }
        }

        public ResResponce SettingPlaylist(ReqSettingPlaylistSong data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                DataTable dtDetail = new DataTable();
                var h = data.chkMixed;

                strDel = "update  tbSpecialPlaylists set IsMixedContent=" + Convert.ToByte(data.chkMixed) + " , " +
                    " isVideoMute =" + Convert.ToByte(data.chkMute) + ", isshowdefault=" + Convert.ToByte(data.chkFixed) + " " +
                    " , chkDuplicateContent =" + Convert.ToByte(data.chkDuplicate) + " " +
                    " , VolumeLevel = " + data.volume.ToString() + " " +
                    " where splplaylistid = " + data.playlistid;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce UpdatePlaylistSRNo(ReqUpdatePlaylistSRNo data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string str = "";
                con.Open();
                foreach (var lst in data.lstTitleSR)
                {
                    str = "update tbSpecialPlaylists_Titles set srno=" + lst.index + " where id=" + lst.id + " and  Titleid=" + lst.titleid + " ";
                    str = str + "   and splplaylistid= " + data.playlistid[0] + "";
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce SaveModifyLogs(ReqSaveModifyLogs data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string str = "";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("SaveModifyLogs", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@dfclientid", SqlDbType.BigInt));
                cmd.Parameters["@dfclientid"].Value = data.dfclientid;

                cmd.Parameters.Add(new SqlParameter("@IPAddress", SqlDbType.VarChar));
                cmd.Parameters["@IPAddress"].Value = data.IPAddress;

                cmd.Parameters.Add(new SqlParameter("@ModifyData", SqlDbType.VarChar));
                cmd.Parameters["@ModifyData"].Value = data.ModifyData;

                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int));
                cmd.Parameters["@UserId"].Value = data.UserId;

                cmd.Parameters.Add(new SqlParameter("@EfftectToken", SqlDbType.VarChar));
                cmd.Parameters["@EfftectToken"].Value = data.EffectToken;
                cmd.ExecuteNonQuery();


                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }
        public List<ResAdminLogs> FillAdminLogs(ReqTokenInfo data)
        {
            List<ResAdminLogs> lstResult = new List<ResAdminLogs>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                var str = "";
                str = "";
                str = "select top 300 tbModifyLogs.Ipaddress, modifydata,ModifyDateTime,iif(EfftectToken=0,'',EfftectToken) as effect, dfclients.clientname from tbModifyLogs " +
                 " inner join dfclients on dfclients.dfclientid=tbModifyLogs.dfclientid " +
                 " where dfclients.dfclientid=" + data.clientId + " order by ModifyDateTime desc";

                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstResult.Add(new ResAdminLogs()
                    {
                        Ipaddress = ds.Rows[i]["Ipaddress"].ToString(),
                        modifydata = ds.Rows[i]["modifydata"].ToString(),
                        ModifyDateTime = string.Format("{0:dd-MMM-yyyy HH:mm:ss}", ds.Rows[i]["ModifyDateTime"]),
                        effect = ds.Rows[i]["effect"].ToString(),
                        clientname = ds.Rows[i]["clientname"].ToString(),
                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }

        public List<GenreList> GetGenreList(ReqGenreList data)
        {
            List<GenreList> LstGenreList = new List<GenreList>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {


                var qry = "select tbGenre.GenreId  , genre    from tbGenre ";
                qry = qry + " inner join Titles tit on tit.genreId= tbGenre.genreId ";
                qry = qry + " where tit.mediatype='" + data.mediatype + "' ";
                qry = qry + " and (tit.dbtype='" + data.DBType + "' or tit.dbtype='Both') ";
                if (data.mediatype != "Image")
                {
                    if (data.mediaStyle == "Copyright")
                    {
                        qry = qry + " and isRoyaltyFree=0 ";
                    }
                    else
                    {
                        qry = qry + " and isRoyaltyFree=1 ";
                    }

                }
                if (data.mediatype == "Image")
                {
                    qry = qry + " and tit.GenreId in(303,297, 325,324)";

                }
                else
                {





                    if (data.FilterType == "NewVibe")
                    {
                        qry = qry + " and tit.titleyear between " + DateTime.Now.AddYears(-1).Year + " and " + DateTime.Now.Year + "";
                    }
                    if (data.FilterType == "Language")
                    {
                        qry = qry + " and  tit.Language= '" + data.FilterValue + "' ";
                    }
                    if (data.FilterType == "Year")
                    {
                        qry = qry + " and  tit.titleyear= " + data.FilterValue + " ";
                    }
                    if (data.FilterType == "BPM")
                    {
                        var MT = data.FilterValue.Split('-');
                        qry = qry + " and cast(tit.BPM as int) between " + MT[0] + " and " + MT[1] + " ";
                    }
                    if (data.FilterType == "ReleaseDate")
                    {
                        var MT = data.FilterValue.Split('-');
                        qry = qry + " and month(tit.ReleaseDate)=" + MT[0] + " ";
                    }
                    if (data.FilterType == "EngeryLevel")
                    {
                        qry = qry + " and tit.EngeryLevel= " + data.FilterValue + "";
                    }
                    if (string.IsNullOrEmpty(data.ContentType) == false)
                    {
                        if (data.ContentType == "Signage")
                        {
                            qry = qry + " and tbGenre.GenreId in(303,297, 325,324) ";
                        }
                    }
                }
                qry = qry + " group by tbGenre.GenreId,genre ";
                qry = qry + " order by genre ";



                //string str = "";
                //str = "select tbgenre.genreid, tbgenre.genre from tbgenre";
                //str = str + " inner join Titles on Titles.genreid= tbgenre.genreid ";
                //str = str + " where tbgenre.genreid!=1 ";
                //str = str + " and Titles.mediatype='" + data.mediatype + "' and (Titles.dbtype='" + data.DBType + "' or  Titles.dbtype='Both') ";
                //if (data.IsAdmin == false)
                //{
                //    str = str + " and (isnull(Titles.dfclientid,0)=0 or Titles.dfclientid=" + data.ClientId + ") ";
                //}
                //if (data.mediatype != "Image")
                //{
                //    if (data.mediaStyle == "Copyright")
                //    {
                //        str = str + " and isRoyaltyFree=0 ";
                //    }
                //    else
                //    {
                //        str = str + " and isRoyaltyFree=1 ";
                //    }
                //}
                //str = str + " group by tbgenre.genreid, tbgenre.genre order by tbgenre.genre ";
                SqlCommand cmd = new SqlCommand(qry, con);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    GenreList td = new GenreList();
                    td.iChecked = false;
                    td.genreid = rdr["genreid"].ToString();
                    td.genre = rdr["genre"].ToString();
                    td.GenrePercentage = 0;
                    LstGenreList.Add(td);
                }
                con.Close();
                return LstGenreList;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return LstGenreList;
            }
        }
        public ResResponce NewSavePlaylist(List<ReqNewSavePlaylist> data)
        {

            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                int totalPercentage = 0;
                string IsSongFound = "No";
                string str = "";
                foreach (var itemMain in data)
                {

                    foreach (var item in itemMain.lstGenrePer)
                    {
                        totalPercentage = totalPercentage + Convert.ToInt32(item.GenrePercentage);
                    }
                    foreach (var item in itemMain.lstGenrePer)
                    {
                        int PerGenreSongs = Convert.ToInt32(Convert.ToDecimal(Convert.ToDecimal(item.GenrePercentage) / totalPercentage) * Convert.ToInt32(itemMain.TotalSongs));
                        str = "";
                        str = "select top " + PerGenreSongs.ToString() + "  titles.titleid from Titles ";
                        str = str + " inner join tbgenre on Titles.genreid= tbgenre.genreid ";
                        str = str + " where isnull(titles.IsAnnouncement,0)=0 and ((Titles.dbType='" + itemMain.DBType + "' ) or (Titles.dbType='Both')) and Titles.mediatype='" + item.MediaType + "' and Titles.genreid=" + item.GenreId;
                        //if (itemMain.MediaStyle == "Copyright")
                        //{
                        //    str = str + " and isRoyaltyFree=0 ";
                        //}
                        //else
                        //{
                        //    str = str + " and isRoyaltyFree=1 ";
                        //}
                        str = str + " order by titles.titleid desc  ";
                        DataTable dt = new DataTable();
                        SqlDataAdapter Adp = new SqlDataAdapter();
                        Adp = new SqlDataAdapter(str, con);
                        Adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            IsSongFound = "Yes";
                            break;
                        }

                    }

                    if (IsSongFound == "No")
                    {
                        con.Close();
                        clsResult.Responce = "2";
                        return clsResult;
                    }

                    SqlCommand cmd = new SqlCommand("spSpecialPlaylists_Save_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pAction", SqlDbType.VarChar));
                    cmd.Parameters["@pAction"].Value = "New";

                    cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                    cmd.Parameters["@splPlaylistId"].Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@splPlaylistName", SqlDbType.VarChar));
                    cmd.Parameters["@splPlaylistName"].Value = itemMain.plName;
                    cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                    cmd.Parameters["@pVersion"].Value = "c";
                    cmd.Parameters.Add(new SqlParameter("@Formatid", SqlDbType.BigInt));
                    cmd.Parameters["@Formatid"].Value = itemMain.formatid;
                    cmd.Parameters.Add(new SqlParameter("@mType", SqlDbType.VarChar));
                    cmd.Parameters["@mType"].Value = "Audio";
                    con.Open();
                    Int32 Playlistid = Convert.ToInt32(cmd.ExecuteScalar());
                    foreach (var item in itemMain.lstGenrePer)
                    {
                        int PerGenreSongs = Convert.ToInt32(Convert.ToDecimal(Convert.ToDecimal(item.GenrePercentage) / totalPercentage) * Convert.ToInt32(itemMain.TotalSongs));

                        str = "";
                        str = " insert into tbSpecialPlaylists_Titles (splPlaylistId,titleId,srNo) ";
                        str = str + " select top " + PerGenreSongs.ToString() + " " + Playlistid + ", titles.titleid,titles.titleid from Titles";
                        str = str + " inner join tbgenre on Titles.genreid= tbgenre.genreid ";
                        str = str + " where  ((Titles.dbType='" + itemMain.DBType + "' ) or (Titles.dbType='Both')) and  Titles.mediatype='" + item.MediaType + "' and Titles.genreid=" + item.GenreId;
                        //if (itemMain.MediaStyle == "Copyright")
                        //{
                        //    str = str + " and isRoyaltyFree=0 ";
                        //}
                        //else
                        //{
                        //    str = str + " and isRoyaltyFree=1 ";
                        //}
                        str = str + " order by titles.titleid desc  ";
                        cmd = new SqlCommand(str, con);
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                    }

                }
                con.Close();

                clsResult.Responce = "1";
                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                clsResult.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }
        public ResResponce SaveAdPlaylist(ReqPlaylistAd data)
        {
            ResResponce clsResult = new ResResponce();
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                con.Open();
                foreach (var iToken in data.TokenList)
                {
                    cmd = new SqlCommand("spSavePlaylistAdsSchedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                    cmd.Parameters["@pSchId"].Value = 0;


                    cmd.Parameters.Add(new SqlParameter("@dfClientId", SqlDbType.BigInt));
                    cmd.Parameters["@dfClientId"].Value = data.CustomerId;

                    cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                    cmd.Parameters["@splPlaylistId"].Value = data.PlaylistId;

                    cmd.Parameters.Add(new SqlParameter("@sDate", SqlDbType.DateTime));
                    cmd.Parameters["@sDate"].Value = string.Format(fi, "{0:dd-MMM-yyyy}", Convert.ToDateTime(data.sDate));

                    cmd.Parameters.Add(new SqlParameter("@eDate", SqlDbType.DateTime));
                    cmd.Parameters["@eDate"].Value = string.Format(fi, "{0:dd-MMM-yyyy}", Convert.ToDateTime(data.eDate));

                    cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                    cmd.Parameters["@FormatId"].Value = data.FormatId;


                    cmd.Parameters.Add(new SqlParameter("@pMode", SqlDbType.VarChar));
                    cmd.Parameters["@pMode"].Value = data.pMode;


                    cmd.Parameters.Add(new SqlParameter("@Frequency", SqlDbType.Int));
                    cmd.Parameters["@Frequency"].Value = data.TotalFrequancy;
                    Int32 rtPschId = Convert.ToInt32(cmd.ExecuteScalar());

                    //=============================== Save Week
                    foreach (var lstWeek in data.wList)
                    {
                        cmd = new SqlCommand("spSavePlaylistAdsSchedule_Week", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                        cmd.Parameters["@pSchId"].Value = rtPschId;

                        cmd.Parameters.Add(new SqlParameter("@wId", SqlDbType.Int));
                        cmd.Parameters["@wId"].Value = lstWeek.id;

                        cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                        cmd.Parameters["@FormatId"].Value = data.FormatId;
                        cmd.ExecuteNonQuery();
                    }
                    //=========================================

                    //====================== Save Token Detail
                    cmd = new SqlCommand("spSavePlaylistAdsSchedule_Token", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                    cmd.Parameters["@pSchId"].Value = rtPschId;

                    cmd.Parameters.Add(new SqlParameter("@tokenId", SqlDbType.BigInt));
                    cmd.Parameters["@tokenId"].Value = iToken;

                    cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                    cmd.Parameters["@FormatId"].Value = data.FormatId;

                    cmd.Parameters.Add(new SqlParameter("@DfClientid", SqlDbType.BigInt));
                    cmd.Parameters["@DfClientid"].Value = data.CustomerId;

                    cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                    cmd.Parameters["@splPlaylistId"].Value = data.PlaylistId;
                    cmd.ExecuteNonQuery();
                    //========================================
                }
                con.Close();
                clsResult.Responce = "1";
                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }


        public List<ResFillSF> FillAdPlaylist(ReqFillAdPlaylist data)
        {
            List<ResFillSF> lstSF = new List<ResFillSF>();
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string qtr = "";


                string sQr = "";
                sQr = "GetCustomerPlaylistAdsSchedule " + data.clientId + " , 0";
                if (string.IsNullOrEmpty(data.tokenid) == false)
                {
                    if (data.tokenid != "0")
                    {
                        sQr = "GetCustomerPlaylistAdsSchedule " + data.clientId + " , " + data.tokenid + "";
                    }
                }

                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;

                ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstSF.Add(new ResFillSF()
                    {
                        id = ds.Rows[i]["pSchid"].ToString(),
                        formatName = ds.Rows[i]["FormatName"].ToString(),
                        playlistName = ds.Rows[i]["pName"].ToString(),
                        token = ds.Rows[i]["Tokenid"].ToString(),
                        StartTime = string.Format(fi, "{0:dd-MMM-yyyy}", Convert.ToDateTime(ds.Rows[i]["sDate"])),
                        EndTime = string.Format(fi, "{0:dd-MMM-yyyy}", Convert.ToDateTime(ds.Rows[i]["eDate"])),
                        WeekNo = ds.Rows[i]["wName"].ToString(),
                    });
                }
                con.Close();
                return lstSF;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return lstSF;
            }
        }

        public ResResponce DeleteFormat(ReqDeleteFormatId data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                con.Open();
                if (data.IsForceDelete == "No")
                {
                    DataTable dtDetail = new DataTable();
                    strDel = "select tbSpecialPlaylistSchedule_Token.* from tbSpecialPlaylistSchedule";
                    strDel = strDel + " inner join tbSpecialPlaylistSchedule_Token on tbSpecialPlaylistSchedule_Token.pschid= tbSpecialPlaylistSchedule.pschid";
                    strDel = strDel + " where tbSpecialPlaylistSchedule.formatid = " + data.formatId;
                    SqlCommand cmdCheck = new SqlCommand(strDel, con);
                    cmdCheck.CommandType = System.Data.CommandType.Text;
                    SqlDataAdapter ad = new SqlDataAdapter(cmdCheck);
                    DataTable ds = new DataTable();
                    ad.Fill(ds);
                    if (ds.Rows.Count > 0)
                    {
                        con.Close();
                        result.Responce = "2";
                        return result;
                    }
                }
                strDel = "delete from tbSpecialPlaylists_Titles where splplaylistid in(select splplaylistid from tbSpecialPlaylists where formatid =" + data.formatId + ")";
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                strDel = "delete from tbSpecialPlaylists where formatid =" + data.formatId;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                strDel = "delete from tbSpecialFormat where formatid= " + data.formatId;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce UpdateAppLogo(RegUpdateAppLogo data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string strDel = "";
                strDel = "update AMPlayerTokens set logoid =" + data.LogoId + " where clientId=  " + data.ClientId;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce SetOnlineIndicator(RegSetOnlineIndicator data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string strDel = "";
                strDel = "update AMPlayerTokens set IsIndicatorActive =" + Convert.ToByte(data.chkIndicator) + " where  token='used' and clientId=  " + data.ClientId;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();


                strDel = "update AMPlayerTokens set IsIndicatorActive =0 where  code is null  and clientId=  " + data.ClientId;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                con.Close();
                result.Responce = "1";
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce ForceUpdate(ReqForceUpdate data)
        {
            ResResponce result = new ResResponce();
            ClsNoti clsData = new ClsNoti();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string strDel = "";
                string tokenid = "";

                foreach (var item in data.tokenid)
                {
                    if (tokenid == "")
                    {
                        tokenid = item.ToString();
                    }
                    else
                    {
                        tokenid = tokenid + "," + item.ToString();
                    }
                }

                strDel = "update AMPlayerTokens set isPublishUpdate =1  where token='used' and tokenid in(  " + tokenid + ")";

                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                strDel = "";
                strDel = "select isnull(NotificationDeviceId,'') as FcmId ,isVedioActive from AMPlayerTokens " +
                    " where tokenid in  (" + tokenid + ") and isnull(NotificationDeviceId,'') !=''";

                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {

                    for (int iFCM = 0; iFCM < ds.Rows.Count; iFCM++)
                    {

                        clsData.id = "0";
                        clsData.type = "Publish";
                        clsData.url = "";
                        clsData.DeviceToken = ds.Rows[iFCM]["FcmId"].ToString();
                        clsData.title = "0";
                        clsData.albumid = "0";
                        clsData.artistid = "0";
                        clsData.artistname = "0";
                        clsData.PlayType = "UpdateNow";
                        string DeviceToken = clsData.DeviceToken;

                        var FcmResult = "-1";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                        httpWebRequest.ContentType = "application/json";
                        if (ds.Rows[iFCM]["isVedioActive"].ToString() == "1")
                        {
                            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAVNhkSB0:APA91bFvqS4tV4d8EBd_R9EPR5OwiSYNAu-WpZoE6u4gsxkurkMscL1Gal-PY_0ZC8j2rl5OV38t531qHK8RTXT1mISNVvVcfdoD7JMRROimfEfnN2ppxEli67eiRGmmfwgJEa_ZK3OP"));
                        }
                        if (ds.Rows[iFCM]["isVedioActive"].ToString() == "0")
                        {
                            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAwL-k27Y:APA91bFMoi8vNJJ310PqIFszZ_fsKqinRQ_U3ZovQwYpuNaYz5R4SkRkOfk5PebyUU6SxMT8gxc81185pZKsxku8Og8vP5foy-jtiOc-LKg-04sO-FFEd-lZpwGE3oeDWzoHE0cwk90d"));
                        }

                        httpWebRequest.Method = "POST";
                        httpWebRequest.UseDefaultCredentials = true;
                        httpWebRequest.PreAuthenticate = true;
                        httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                        var payload = new
                        {
                            to = DeviceToken,
                            priority = "high",
                            content_available = true,
                            notification = new
                            {

                                body = clsData,
                                title = "MyClaud"
                            },
                        };
                        var serializer = new JavaScriptSerializer();
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string json = serializer.Serialize(payload);
                            streamWriter.Write(json);
                            streamWriter.Flush();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            FcmResult = streamReader.ReadToEnd();
                        }
                        var objs = JsonConvert.DeserializeObject<ResNoti>(FcmResult);

                    }
                }

                con.Close();
                result.Responce = "1";
                return result;


            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public List<ResTokenInfo> FillTokenInfo_formatANDplaylist(Reg_formatANDplaylist data)
        {
            List<ResTokenInfo> lstResult = new List<ResTokenInfo>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                string str = "";
                str = "GetTokenInfo_formatANDplaylist " + data.formatId + " , " + data.playlistId;
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstResult.Add(new ResTokenInfo()
                    {
                        tokenid = ds.Rows[i]["tokenid"].ToString(),
                        tokenCode = ds.Rows[i]["tNo"].ToString(),
                        Name = ds.Rows[i]["PersonName"].ToString(),
                        location = ds.Rows[i]["Location"].ToString(),
                        city = ds.Rows[i]["CityName"].ToString(),
                        countryName = ds.Rows[i]["CountryName"].ToString(),
                        playerType = ds.Rows[i]["PlType"].ToString(),
                        LicenceType = ds.Rows[i]["LiType"].ToString(),
                        tInfo = ds.Rows[i]["tInfo"].ToString(),
                        AppLogoId = ds.Rows[i]["AppLogoId"].ToString(),
                        Version = ds.Rows[i]["ver"].ToString(),
                        PublishUpdate = ds.Rows[i]["isPublishUpdate"].ToString(),
                        Volume = ds.Rows[i]["volume"].ToString(),
                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }









        public ResResponce DeleteLogo(ReqDeleteLogo data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string strDel = "";
                strDel = "delete titles where titleid =  " + data.logoId;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }




        public ResResponce UploadSheet()
        {
            ResResponce Result = new ResResponce();
            SqlConnection conSql = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            conSql.Open();
            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];
                string fName = DateTime.Now.Day.ToString() + "_" + DateTime.Now.Millisecond.ToString() + Path.GetExtension(postedFile.FileName);

                var filePath = HttpContext.Current.Server.MapPath("~/sheet/" + fName);
                postedFile.SaveAs(filePath);
                string extension = Path.GetExtension(filePath);
                string header = "YES";
                string conStr, sheetName;
                conStr = string.Empty;
                string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conStr = string.Format(Excel03ConString, filePath, header);
                        break;

                    case ".xlsx": //Excel 07
                        conStr = string.Format(Excel07ConString, filePath, header);
                        break;
                }
                using (OleDbConnection con = new OleDbConnection(conStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.Connection = con;
                        con.Open();
                        DataTable dtExcelSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        con.Close();
                    }
                }
                DataTable dtM = new DataTable();
                using (OleDbConnection con = new OleDbConnection(conStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        using (OleDbDataAdapter oda = new OleDbDataAdapter())
                        {
                            cmd.CommandText = "SELECT * From [" + sheetName + "]";
                            cmd.Connection = con;
                            con.Open();
                            oda.SelectCommand = cmd;
                            oda.Fill(dtM);
                            con.Close();
                        }
                    }
                }
                if (dtM.Columns.Count != 9)
                {
                    Result.Responce = "0";
                    Result.message = "Selected excel file is not a correct file. Columns are not match";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[0].ToString().ToLower() != "tokenid")
                {
                    Result.Responce = "0";
                    Result.message = "TokenId column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[1].ToString().ToLower() != "tokencode")
                {
                    Result.Responce = "0";
                    Result.message = "Token code column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[2].ToString().ToLower() != "serial-mac")
                {
                    Result.Responce = "0";
                    Result.message = "Serial-MAC column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[3].ToString().ToLower() != "location")
                {
                    Result.Responce = "0";
                    Result.message = "Location column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[4].ToString().ToLower() != "isandroidplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsAndroidPlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[5].ToString().ToLower() != "iswindowplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsWindowPlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[6].ToString().ToLower() != "isaudioplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsAudioPlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[7].ToString().ToLower() != "isvideoplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsVideoPlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[8].ToString().ToLower() != "issanitizerplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsSanitizerPlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                var k = "";
                var h = "";
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if ((dtM.Rows[i]["TokenId"].ToString() != "") && (dtM.Rows[i]["tokencode"].ToString() != "") && (dtM.Rows[i]["Serial-MAC"].ToString() != ""))
                    {
                        string str = "";

                        str = "update AMPlayerTokens set DateTokenUsed=getdate(), IsStore=1, token='used', code='" + dtM.Rows[i]["Serial-MAC"] + "'";
                        str = str + ", ptype='Copyright', personname = '' ";
                        str = str + ", Location = '" + dtM.Rows[i]["Location"] + "' ";
                        if ((dtM.Rows[i]["IsAndroidPlayer"].ToString() == "1") || (dtM.Rows[i]["IsAndroidPlayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", lType = 'Android' ";
                        }
                        if ((dtM.Rows[i]["IsWindowPlayer"].ToString() == "1") || (dtM.Rows[i]["IsWindowPlayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", lType = 'Desktop' ";
                        }
                        if ((dtM.Rows[i]["isaudioplayer"].ToString() == "1") || (dtM.Rows[i]["isaudioplayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", IsVedioActive = 0 ";
                        }
                        if ((dtM.Rows[i]["IsVideoPlayer"].ToString() == "1") || (dtM.Rows[i]["IsVideoPlayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", IsVedioActive = 1 ";
                        }
                        if ((dtM.Rows[i]["IsSanitizerPlayer"].ToString() == "1") || (dtM.Rows[i]["IsSanitizerPlayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", IsVedioActive = 1, mediatype='Signage',DeviceType='Sanitizer', TotalShot=5000, DispenserAlert='80,90,100' ";
                        }
                        str = str + " Where tokenid = " + dtM.Rows[i]["TokenId"] + " ";

                        SqlCommand cmdOnline = new SqlCommand();
                        cmdOnline.Connection = conSql;
                        cmdOnline.CommandType = CommandType.Text;
                        cmdOnline.CommandText = str;
                        cmdOnline.ExecuteNonQuery();
                        h = "Find";
                    }
                    else
                    {
                        if (k == "")
                        {
                            k = dtM.Rows[i]["TokenId"].ToString();
                        }
                        else
                        {
                            k = k + "," + dtM.Rows[i]["TokenId"].ToString();
                        }
                    }
                }

                if (h == "Find")
                {
                    Result.Responce = "1";
                    //if (k == "")
                    //{
                    Result.message = "Saved";
                    //}
                    //else
                    //{
                    //    Result.message = k + Environment.NewLine + "These tokens columns are not fill proper so these tokens are not activate";
                    //}
                }
                else
                {
                    Result.Responce = "0";
                    Result.message = "All columns are not fill proper. Please fill proper and try again";
                }




                conSql.Close();
                return Result;
            }
            catch (Exception ex)
            {
                Result.Responce = "0";
                var g = ex.Message;
                Result.message = ex.Message;
                conSql.Close();
                return Result;
            }
        }

        public ResResponce SaveGenre(ReqSaveGenre data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("spSaveGenre", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
                cmd.Parameters["@Id"].Value = data.id;

                cmd.Parameters.Add(new SqlParameter("@gName", SqlDbType.VarChar));
                cmd.Parameters["@gName"].Value = data.genrename;

                cmd.Parameters.Add(new SqlParameter("@mediatype", SqlDbType.VarChar));
                cmd.Parameters["@mediatype"].Value = data.mediatype;
                con.Open();
                clsResult.Responce = cmd.ExecuteScalar().ToString();

                con.Close();


                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }



        public ResResponce SaveFolder(ReqSaveFolder data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                bool IsPromoFolder = false;
                var h = data.IsPromoFolder;
                SqlCommand cmd = new SqlCommand("spSaveFolder", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
                cmd.Parameters["@Id"].Value = data.id;

                cmd.Parameters.Add(new SqlParameter("@fName", SqlDbType.VarChar));
                cmd.Parameters["@fName"].Value = data.fname;

                cmd.Parameters.Add(new SqlParameter("@dfClientId", SqlDbType.Int));
                cmd.Parameters["@dfClientId"].Value = data.dfClientId;

                cmd.Parameters.Add(new SqlParameter("@IsPromoFolder", SqlDbType.Int));
                cmd.Parameters["@IsPromoFolder"].Value = data.IsPromoFolder;
                con.Open();
                clsResult.Responce = cmd.ExecuteScalar().ToString();

                con.Close();


                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                clsResult.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }



        public ResponseTokenCrashLog ClientRegistration(DataClientRegistration data)
        {
            ResponseTokenCrashLog result = new ResponseTokenCrashLog();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ALcon"].ConnectionString);
            Int32 SaveDfClientId = 0;
            try
            {
                if (string.IsNullOrEmpty(data.Currency) == true)
                {
                    data.Currency = "USD";
                }
                if (string.IsNullOrEmpty(data.PlayerType) == true)
                {
                    data.PlayerType = "Android";
                }
                string ExpDate = "";
                string PaymentPlan = "";
                string str = "select  DFClientID from DFClients where ClientName='AM-" + data.CompanyName.Trim() + "'";

                SqlCommand cmdValid = new SqlCommand(str, con);
                cmdValid.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmdValid);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    con.Close();
                    result.Response = 0;
                    result.ErrorMessage = "Company is already registered.";
                    return result;
                }

                if (!data.pAmount.All(char.IsDigit))
                {
                    con.Close();
                    result.Response = 0;
                    result.ErrorMessage = "Payment should be in numeric";
                    return result;
                }

                if (data.SubscriptionType == "Month")
                {
                    ExpDate = string.Format("{0:dd/MMM/yyyy}", DateTime.Now.AddMonths(1));
                    PaymentPlan = "Monthly";
                }
                if (data.SubscriptionType == "Annual")
                {
                    ExpDate = string.Format("{0:dd/MMM/yyyy}", DateTime.Now.AddYears(1));
                    PaymentPlan = "One Year";
                }
                if (data.SubscriptionType == "Year")
                {
                    ExpDate = string.Format("{0:dd/MMM/yyyy}", DateTime.Now.AddYears(2));
                    PaymentPlan = "Two Year";

                }
                if (ExpDate == "")
                {
                    con.Close();
                    result.Response = 0;
                    result.ErrorMessage = "Subscription Type is a case sensitive and name should be Month, Annual and Year";
                    return result;

                }
                var ReqOptions = new RequestOptions();
                int amount = Convert.ToInt32(data.pAmount) * 100;
                if ((data.Currency == "USD") || (data.Currency == "CAD"))
                {
                    StripeConfiguration.ApiKey = "sk_test_UszzIuHGVkslaOwThJdRHXtt00QxKpKoCW";
                    ReqOptions = new RequestOptions
                    {
                        ApiKey = "sk_test_UszzIuHGVkslaOwThJdRHXtt00QxKpKoCW"
                    };
                }
                if (data.Currency == "EUR")
                {
                    StripeConfiguration.ApiKey = "sk_test_WTQOPJxxwGVZu5GLHTzYFaOp00wvFn0MYB";
                    ReqOptions = new RequestOptions
                    {
                        ApiKey = "sk_test_WTQOPJxxwGVZu5GLHTzYFaOp00wvFn0MYB"
                    };

                }
                var optiontoken = new TokenCreateOptions
                {
                    Card = new CreditCardOptions
                    {
                        Number = data.cardNo,
                        ExpMonth = data.month,
                        ExpYear = data.year,
                        Cvc = data.cvc
                    }
                };
                var servicetoken = new TokenService();
                Token stripetoken = servicetoken.Create(optiontoken, ReqOptions);
                var customers = new CustomerService();
                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = data.cMail,
                    Source = stripetoken.Id,
                    Metadata = new Dictionary<string, string>()
                    {
                        { "Client Name",data.cName} ,
                        { "Email",data.cMail} ,
                        { "Payment Plan",PaymentPlan} ,
                        { "Copyright Player",data.CRPlayer} ,
                        { "Royalty Free Player",data.RFPlayer} ,
                        { "Total Player",data.TotalPlayer},
                        { "Payment Type","New Payment"},
                        { "Description",data.PlayerType + " player payment"}
                    }
                });

                var options = new ChargeCreateOptions
                {
                    Amount = amount,
                    Currency = data.Currency,
                    Description = "Player payment",
                    Customer = customer.Id,
                    ReceiptEmail = data.cMail,
                    Metadata = new Dictionary<string, string>()
                    {
                        { "Client Name",data.cName} ,
                        { "Email",data.cMail} ,
                        { "Payment Plan",PaymentPlan} ,
                        { "Copyright Player",data.CRPlayer} ,
                        { "Royalty Free Player",data.RFPlayer} ,
                        { "Total Player",data.TotalPlayer},
                        { "Payment Type","New Payment"},
                        { "Description",data.PlayerType + " player payment"}
                    }
                };

                var service = new ChargeService();
                Charge charge = service.Create(options, ReqOptions);
                if (!charge.Paid)
                {
                    con.Close();
                    result.Response = 0;
                    result.ErrorMessage = "Payment request is not complete.Please try again later.";
                    return result;
                }


                SqlCommand cmd = new SqlCommand("sp_DealerRegistration", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@InClientName", SqlDbType.VarChar));
                cmd.Parameters["@InClientName"].Value = "AM-" + data.CompanyName.Trim();

                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
                cmd.Parameters["@email"].Value = data.cMail.Trim();

                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.VarChar));
                cmd.Parameters["@Orderno"].Value = "AM-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

                cmd.Parameters.Add(new SqlParameter("@ResponsiblePersonName", SqlDbType.VarChar));
                cmd.Parameters["@ResponsiblePersonName"].Value = data.cName;

                cmd.Parameters.Add(new SqlParameter("@CountryCode", SqlDbType.BigInt));
                cmd.Parameters["@CountryCode"].Value = data.CountryId;

                cmd.Parameters.Add(new SqlParameter("@StreetName", SqlDbType.VarChar));
                cmd.Parameters["@StreetName"].Value = data.Address.Trim();

                cmd.Parameters.Add(new SqlParameter("@CityName", SqlDbType.VarChar));
                cmd.Parameters["@CityName"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@IsDealer", SqlDbType.Bit));
                cmd.Parameters["@IsDealer"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@DealerNoTotalToken", SqlDbType.Int));
                cmd.Parameters["@DealerNoTotalToken"].Value = data.TotalPlayer;

                cmd.Parameters.Add(new SqlParameter("@DealerCode", SqlDbType.VarChar));
                cmd.Parameters["@DealerCode"].Value = "AM" + data.cName.Substring(0, 2).Trim().ToUpper() + data.CompanyName.Substring(0, 2).Trim().ToUpper() + DateTime.Now.Year.ToString();

                cmd.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
                cmd.Parameters["@CityId"].Value = data.CityId;

                cmd.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
                cmd.Parameters["@StateId"].Value = data.StateId;

                cmd.Parameters.Add(new SqlParameter("@IsMainDealer", SqlDbType.Bit));
                cmd.Parameters["@IsMainDealer"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@Vatnumber", SqlDbType.VarChar));
                cmd.Parameters["@Vatnumber"].Value = data.TaxNo.ToUpper();

                cmd.Parameters.Add(new SqlParameter("@IsSubDealer", SqlDbType.Bit));
                cmd.Parameters["@IsSubDealer"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@MainDealerId", SqlDbType.BigInt));
                cmd.Parameters["@MainDealerId"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@supportEmail", SqlDbType.VarChar));
                cmd.Parameters["@supportEmail"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@supportPhoneNo", SqlDbType.VarChar));
                cmd.Parameters["@supportPhoneNo"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@PostalCode", SqlDbType.VarChar));
                cmd.Parameters["@PostalCode"].Value = data.PostalCode;

                cmd.Parameters.Add(new SqlParameter("@PhoneNo", SqlDbType.VarChar));
                cmd.Parameters["@PhoneNo"].Value = data.PhoneNo;

                SaveDfClientId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("sp_DealerLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@SaveType", SqlDbType.VarChar));
                cmd.Parameters["@SaveType"].Value = "Save";

                cmd.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.BigInt));
                cmd.Parameters["@LoginId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@LoginName", SqlDbType.VarChar));
                cmd.Parameters["@LoginName"].Value = data.cMail.Trim();

                cmd.Parameters.Add(new SqlParameter("@DfClientId", SqlDbType.BigInt));
                cmd.Parameters["@DfClientId"].Value = SaveDfClientId;

                cmd.Parameters.Add(new SqlParameter("@LoginPassword", SqlDbType.VarChar));
                cmd.Parameters["@LoginPassword"].Value = data.lPwd;

                cmd.Parameters.Add(new SqlParameter("@ExpiryDate", SqlDbType.DateTime));
                cmd.Parameters["@ExpiryDate"].Value = ExpDate;

                cmd.Parameters.Add(new SqlParameter("@DealerCode", SqlDbType.VarChar));
                cmd.Parameters["@DealerCode"].Value = "AM" + data.cName.Substring(0, 2).Trim().ToUpper() + data.CompanyName.Substring(0, 2).Trim().ToUpper() + DateTime.Now.Year.ToString();

                cmd.Parameters.Add(new SqlParameter("@DamTotalToken", SqlDbType.Int));
                cmd.Parameters["@DamTotalToken"].Value = data.RFPlayer;
                cmd.Parameters.Add(new SqlParameter("@CopyrightTotalToken", SqlDbType.Int));
                cmd.Parameters["@CopyrightTotalToken"].Value = data.CRPlayer;
                cmd.Parameters.Add(new SqlParameter("@SanjivaniTotalToken", SqlDbType.Int));
                cmd.Parameters["@SanjivaniTotalToken"].Value = 0;

                bool isRF = false;
                bool isCR = false;
                string pType = "";
                if (data.CRPlayer != "0")
                {
                    isCR = true;
                    pType = "Copyright";
                }
                if (data.RFPlayer != "0")
                {
                    isRF = true;
                    pType = "Direct License";
                }

                cmd.Parameters.Add(new SqlParameter("@IsDam", SqlDbType.Bit));
                cmd.Parameters["@IsDam"].Value = isRF;
                cmd.Parameters.Add(new SqlParameter("@IsCopyright", SqlDbType.Bit));
                cmd.Parameters["@IsCopyright"].Value = isCR;
                cmd.Parameters.Add(new SqlParameter("@IsSanjivani", SqlDbType.Bit));
                cmd.Parameters["@IsSanjivani"].Value = false;

                cmd.Parameters.Add(new SqlParameter("@AsianTotalToken", SqlDbType.Int));
                cmd.Parameters["@AsianTotalToken"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsAsian", SqlDbType.Bit));
                cmd.Parameters["@IsAsian"].Value = false;


                cmd.ExecuteNonQuery();

                for (int i = 1; i <= Convert.ToInt32(data.TotalPlayer); i++)
                {
                    cmd = new SqlCommand("spDealer_AMTokensClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@DFClientID", SqlDbType.BigInt));
                    cmd.Parameters["@DFClientID"].Value = SaveDfClientId;

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt));
                    cmd.Parameters["@UserId"].Value = 1;

                    cmd.Parameters.Add(new SqlParameter("@InNumberofTitles", SqlDbType.BigInt));
                    cmd.Parameters["@InNumberofTitles"].Value = 5000;

                    cmd.Parameters.Add(new SqlParameter("@isCopyRight", SqlDbType.Int));
                    cmd.Parameters["@isCopyRight"].Value = 1;

                    cmd.Parameters.Add(new SqlParameter("@InDateExp", SqlDbType.DateTime));
                    cmd.Parameters["@InDateExp"].Value = ExpDate;

                    cmd.Parameters.Add(new SqlParameter("@IsDam", SqlDbType.Int));
                    cmd.Parameters["@IsDam"].Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@DamExpiryDate", SqlDbType.DateTime));
                    cmd.Parameters["@DamExpiryDate"].Value = "01-01-1900";

                    cmd.Parameters.Add(new SqlParameter("@IsSanjivani", SqlDbType.Int));
                    cmd.Parameters["@IsSanjivani"].Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SanjivaniExpiryDate", SqlDbType.DateTime));
                    cmd.Parameters["@SanjivaniExpiryDate"].Value = "01-01-1900";

                    cmd.Parameters.Add(new SqlParameter("@IsFitness", SqlDbType.Int));
                    cmd.Parameters["@IsFitness"].Value = "0";

                    cmd.Parameters.Add(new SqlParameter("@FitnessExpiryDate", SqlDbType.DateTime));
                    cmd.Parameters["@FitnessExpiryDate"].Value = "01-01-1900";

                    cmd.Parameters.Add(new SqlParameter("@ServiceId", SqlDbType.Int));
                    cmd.Parameters["@ServiceId"].Value = "0";

                    cmd.Parameters.Add(new SqlParameter("@Dealercode", SqlDbType.VarChar));
                    cmd.Parameters["@Dealercode"].Value = "AM" + data.cName.Substring(0, 2).Trim().ToUpper() + data.CompanyName.Substring(0, 2).Trim().ToUpper() + DateTime.Now.Year.ToString();

                    cmd.Parameters.Add(new SqlParameter("@IsAsian", SqlDbType.Int));
                    cmd.Parameters["@IsAsian"].Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@AsianExpiryDate", SqlDbType.DateTime));
                    cmd.Parameters["@AsianExpiryDate"].Value = "01-01-1900";
                    cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                    cmd.Parameters["@pVersion"].Value = "NativeCR";
                    cmd.ExecuteNonQuery();
                }

                cmd = new SqlCommand("Save_dfClients_Payment", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@dfClientId", SqlDbType.BigInt));
                cmd.Parameters["@dfClientId"].Value = SaveDfClientId;
                cmd.Parameters.Add(new SqlParameter("@Transactionid", SqlDbType.VarChar));
                cmd.Parameters["@Transactionid"].Value = charge.BalanceTransactionId;
                cmd.Parameters.Add(new SqlParameter("@PaymentMethod", SqlDbType.VarChar));
                cmd.Parameters["@PaymentMethod"].Value = "Card-" + charge.PaymentMethodDetails.Card.Brand.ToUpper();
                cmd.Parameters.Add(new SqlParameter("@PaymentDetail", SqlDbType.VarChar));
                cmd.Parameters["@PaymentDetail"].Value = "****-" + charge.PaymentMethodDetails.Card.Last4;
                cmd.Parameters.Add(new SqlParameter("@stripetokenID", SqlDbType.VarChar));
                cmd.Parameters["@stripetokenID"].Value = stripetoken.Id;
                cmd.Parameters.Add(new SqlParameter("@Payment", SqlDbType.Decimal));
                cmd.Parameters["@Payment"].Value = data.pAmount;
                cmd.Parameters.Add(new SqlParameter("@PaymentType", SqlDbType.VarChar));
                cmd.Parameters["@PaymentType"].Value = "New";
                cmd.Parameters.Add(new SqlParameter("@PaymentPlan", SqlDbType.VarChar));
                cmd.Parameters["@PaymentPlan"].Value = PaymentPlan;

                cmd.ExecuteNonQuery();



                string strQ = "";
                string MailMatter = "";
                strQ = "select token, tokenid from AMPlayerTokens where Clientid=" + SaveDfClientId + " and Code is null";

                SqlCommand cmdToken = new SqlCommand(strQ, con);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataReader rdr = cmdToken.ExecuteReader();
                string TokenId = "0";
                while (rdr.Read())
                {
                    MailMatter = rdr["Token"].ToString() + " \n";
                    TokenId = rdr["tokenid"].ToString();
                }

                rdr.Close();
                cmdToken.Dispose();


                cmd = new SqlCommand("spTokenInformation", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Tokenid", SqlDbType.BigInt));
                cmd.Parameters["@Tokenid"].Value = TokenId;

                cmd.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.BigInt));
                cmd.Parameters["@CountryId"].Value = data.CountryId;

                cmd.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
                cmd.Parameters["@StateId"].Value = data.StateId;

                cmd.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
                cmd.Parameters["@CityId"].Value = data.StateId;

                cmd.Parameters.Add(new SqlParameter("@StreetName", SqlDbType.VarChar));
                cmd.Parameters["@StreetName"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@Location", SqlDbType.VarChar));
                cmd.Parameters["@Location"].Value = "";


                cmd.Parameters.Add(new SqlParameter("@PersonName", SqlDbType.VarChar));
                cmd.Parameters["@PersonName"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@Store", SqlDbType.Int));
                cmd.Parameters["@Store"].Value = 1;

                cmd.Parameters.Add(new SqlParameter("@Stream", SqlDbType.Int));
                cmd.Parameters["@Stream"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@ExpDate", SqlDbType.DateTime));
                cmd.Parameters["@ExpDate"].Value = ExpDate;

                cmd.Parameters.Add(new SqlParameter("@IsStopControl", SqlDbType.Int));
                cmd.Parameters["@IsStopControl"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@IsVedioActive", SqlDbType.Int));
                cmd.Parameters["@IsVedioActive"].Value = 0;


                cmd.Parameters.Add(new SqlParameter("@pType", SqlDbType.VarChar));
                cmd.Parameters["@pType"].Value = pType;

                cmd.Parameters.Add(new SqlParameter("@lType", SqlDbType.VarChar));
                cmd.Parameters["@lType"].Value = data.PlayerType;

                cmd.Parameters.Add(new SqlParameter("@TokenNo", SqlDbType.VarChar));
                cmd.Parameters["@TokenNo"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int));
                cmd.Parameters["@GroupId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@Address1", SqlDbType.VarChar));
                cmd.Parameters["@Address1"].Value = data.Address;

                cmd.Parameters.Add(new SqlParameter("@Address2", SqlDbType.VarChar));
                cmd.Parameters["@Address2"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@Address3", SqlDbType.VarChar));
                cmd.Parameters["@Address3"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@Pincode", SqlDbType.VarChar));
                cmd.Parameters["@Pincode"].Value = data.PostalCode;

                cmd.Parameters.Add(new SqlParameter("@IsPlayPrayerFile", SqlDbType.Int));
                cmd.Parameters["@IsPlayPrayerFile"].Value = 0;

                cmd.ExecuteNonQuery();


                var fromAddress = new MailAddress("noreply.alenkamedia@gmail.com", "AlenkaMedia");
                var toAddress = new MailAddress(data.cMail.Trim());

                const string fromPassword = "Alenka@123";
                string subject = "Notification";
                string body = "Dear Client, \n";
                body += "\n";
                body += "Thank you for registering with Alenka Media.";
                body += "\n";
                body += "\n";
                body += "Client Name: " + "AM-" + data.CompanyName.Trim() + "\n";
                body += "Player License Tokens:: \n";
                body += MailMatter;
                body += "\n";

                body += "License Expiry: " + ExpDate + "   \n";
                body += "\n";

                body += "Regards \n";
                body += "Support Team";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                    try
                    {
                        smtp.Send(message);
                    }
                    catch (Exception ex)
                    {
                        result.Response = 1;
                        result.ErrorMessage = "Saved. But mail is not send. Please write mail to support@alenkamedia.com";
                        con.Close();
                        return result;
                    }
                result.Response = 1;
                result.ErrorMessage = "Saved. Please check your regsiter email.";
                con.Close();
                return result;

            }
            catch (Exception ex)
            {
                result.Response = 0;
                result.ErrorMessage = ex.Message;
                string str = "";
                if (SaveDfClientId != 0)
                {
                    str = "delete from dfclients where dfclientid = " + SaveDfClientId;
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                return result;

            }
        }

        public ResponseTokenCrashLog RenewPayment(DataRenewPayment data)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ALcon"].ConnectionString);
            ResponseTokenCrashLog clsResult = new ResponseTokenCrashLog();
            try
            {
                DataTable dtClient = new DataTable();
                DataTable dtExp = new DataTable();
                if (con.State == ConnectionState.Closed) { con.Open(); }

                string str = "";

                str = "select df.email, df.clientname,dl.damTotalToken, dl.CopyrightTotalToken,dp.payment, dp.paymentPlan from dfclients df";
                str = str + " inner join tbdealerlogin dl on dl.dfclientid = df.dfclientid";
                str = str + " inner join dfClients_Payment dp on dp.dfclientid = df.dfclientid";
                str = str + " where df.dfclientid = " + data.ClientId + " and dl.dealercode is not null";
                SqlCommand cmdToken = new SqlCommand(str, con);
                SqlDataAdapter adToken = new SqlDataAdapter(cmdToken);
                adToken.Fill(dtClient);
                adToken.Dispose();
                cmdToken.Dispose();
                if (dtClient.Rows.Count == 0)
                {
                    clsResult.Response = 0;
                    clsResult.ErrorMessage = "Client details not found";
                    con.Close();
                    return clsResult;
                }
                string cName = "", cMail = "", TotalPlayer = "0", CRPlayer = "0", RFPlayer = "0", PaymentPlan = "", ExpDate = "";
                int pAmount = 0;
                cName = dtClient.Rows[0]["clientname"].ToString();
                cMail = dtClient.Rows[0]["email"].ToString();
                CRPlayer = dtClient.Rows[0]["CopyrightTotalToken"].ToString();
                RFPlayer = dtClient.Rows[0]["damTotalToken"].ToString();
                pAmount = Convert.ToInt32(dtClient.Rows[0]["payment"]);
                PaymentPlan = dtClient.Rows[0]["paymentPlan"].ToString();
                TotalPlayer = (Convert.ToInt32(CRPlayer) + Convert.ToInt32(RFPlayer)).ToString();


                str = "select isnull(max(datetokenexpire),'01-01-1900') as expDate from AMPlayerTokens where clientid= " + data.ClientId;
                cmdToken = new SqlCommand(str, con);
                adToken = new SqlDataAdapter(cmdToken);
                adToken.Fill(dtExp);
                adToken.Dispose();
                cmdToken.Dispose();
                DateTime dtpExp = Convert.ToDateTime(dtExp.Rows[0]["expDate"]);


                if (PaymentPlan == "Monthly")
                {
                    ExpDate = string.Format("{0:dd/MMM/yyyy}", dtpExp.AddMonths(1));
                }
                if (PaymentPlan == "One Year")
                {
                    ExpDate = string.Format("{0:dd/MMM/yyyy}", dtpExp.AddYears(1));
                }
                if (PaymentPlan == "Two Year")
                {
                    ExpDate = string.Format("{0:dd/MMM/yyyy}", dtpExp.AddYears(2));
                }




                int amount = Convert.ToInt32(pAmount) * 100;
                if ((data.Currency == "USD") || (data.Currency == "CAD"))
                {
                    StripeConfiguration.ApiKey = "sk_test_UszzIuHGVkslaOwThJdRHXtt00QxKpKoCW";
                }
                if (data.Currency == "EUR")
                {
                    StripeConfiguration.ApiKey = "sk_test_WTQOPJxxwGVZu5GLHTzYFaOp00wvFn0MYB";
                }

                var optiontoken = new TokenCreateOptions
                {
                    Card = new CreditCardOptions
                    {
                        Number = data.cardNo,
                        ExpMonth = data.month,
                        ExpYear = data.year,
                        Cvc = data.cvv
                    }
                };


                var servicetoken = new TokenService();
                Token stripetoken = servicetoken.Create(optiontoken);
                var customers = new CustomerService();
                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = cMail,
                    Source = stripetoken.Id,
                    Metadata = new Dictionary<string, string>()
                    {
                        { "Client Name",cName} ,
                        { "Email",cMail} ,
                        { "Payment Plan",PaymentPlan} ,
                        { "Copyright Player",CRPlayer} ,
                        { "Royalty Free Player",RFPlayer} ,
                        { "Total Player",TotalPlayer},
                        { "Payment Type","Renew"}
                    }
                });

                var options = new ChargeCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Description = "Renew player payment",
                    Customer = customer.Id,
                    ReceiptEmail = cMail,
                    Metadata = new Dictionary<string, string>()
                    {
                        { "Client Name",cName} ,
                        { "Email",cMail} ,
                        { "Payment Plan",PaymentPlan} ,
                        { "Copyright Player",CRPlayer} ,
                        { "Royalty Free Player",RFPlayer} ,
                        { "Total Player",TotalPlayer},
                        { "Payment Type","Renew"}
                    }
                };

                var service = new ChargeService();
                Charge charge = service.Create(options);
                if (!charge.Paid)
                {
                    con.Close();
                    clsResult.Response = 0;
                    clsResult.ErrorMessage = "Payment request is not complete.Please try again later.";
                    return clsResult;
                }

                str = "update AMPlayerTokens set datetokenexpire= '" + ExpDate + "'  where clientid= " + data.ClientId;
                cmdToken = new SqlCommand(str, con);
                cmdToken.CommandType = CommandType.Text;
                cmdToken.ExecuteNonQuery();
                cmdToken.Dispose();

                SqlCommand cmd = cmd = new SqlCommand("Save_dfClients_Payment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@dfClientId", SqlDbType.BigInt));
                cmd.Parameters["@dfClientId"].Value = data.ClientId;
                cmd.Parameters.Add(new SqlParameter("@Transactionid", SqlDbType.VarChar));
                cmd.Parameters["@Transactionid"].Value = charge.BalanceTransactionId;
                cmd.Parameters.Add(new SqlParameter("@PaymentMethod", SqlDbType.VarChar));
                cmd.Parameters["@PaymentMethod"].Value = "Card-" + charge.PaymentMethodDetails.Card.Brand.ToUpper();
                cmd.Parameters.Add(new SqlParameter("@PaymentDetail", SqlDbType.VarChar));
                cmd.Parameters["@PaymentDetail"].Value = "****-" + charge.PaymentMethodDetails.Card.Last4;
                cmd.Parameters.Add(new SqlParameter("@stripetokenID", SqlDbType.VarChar));
                cmd.Parameters["@stripetokenID"].Value = stripetoken.Id;
                cmd.Parameters.Add(new SqlParameter("@Payment", SqlDbType.Decimal));
                cmd.Parameters["@Payment"].Value = pAmount;
                cmd.Parameters.Add(new SqlParameter("@PaymentType", SqlDbType.VarChar));
                cmd.Parameters["@PaymentType"].Value = "Renew";
                cmd.Parameters.Add(new SqlParameter("@PaymentPlan", SqlDbType.VarChar));
                cmd.Parameters["@PaymentPlan"].Value = PaymentPlan;

                cmd.ExecuteNonQuery();
                clsResult.Response = 1;
                clsResult.ErrorMessage = "Saved";
                con.Close();
                return clsResult;

            }
            catch (Exception ex)
            {
                clsResult.Response = 0;
                clsResult.ErrorMessage = ex.Message.ToString();
                con.Close();
                return clsResult;
            }
        }



        public List<ResPlayerLog> FillPlayedTitleSummary(ReqTitleLog data)
        {
            List<ResPlayerLog> lstTd = new List<ResPlayerLog>();
            string cs = ConfigurationManager.ConnectionStrings["Panel"].ConnectionString;
            SqlConnection constr = new SqlConnection(cs);
            if (string.IsNullOrEmpty(data.ToDate) == true)
            {
                data.ToDate = data.cDate;
            }
            string str = "GetPlayedTitleSummary " + data.ClientId + "," + data.tokenid + "  ,'" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.cDate)) + "','" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.ToDate)) + "'";
            SqlCommand cmd = new SqlCommand(str, constr);
            try
            {
                constr.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ResPlayerLog td = new ResPlayerLog();
                    td.Name = rdr["title"].ToString();
                    td.ArtistName = rdr["name"].ToString();
                    td.TotalPlayed = rdr["TotalPlayed"].ToString();
                    td.PlayedDateTime = string.Format("{0:dd-MMM-yyyy HH:mm}", DateTime.Now);
                    td.SplPlaylistName = "";
                    td.CategoryName = "";
                    td.pDateTime = DateTime.Now.ToString();
                    lstTd.Add(td);
                }
                constr.Close();
                return lstTd;
            }
            catch (Exception ex)
            {
                constr.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstTd;
            }
        }

        public ResResponce SendMail(ReqTokenInfo data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                DataTable dtDetail = new DataTable();
                string strResend = "";
                DataTable dtGetToken = new DataTable();
                string strQ = "";
                int TotalTok = 0;
                string MailMatter = "";
                strQ = "select * from AMPlayerTokens where Clientid=" + data.clientId + " and Code is null";
                SqlCommand cmd = new SqlCommand(strQ, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(dtGetToken);

                if (dtGetToken.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtGetToken.Rows.Count - 1; i++)
                    {
                        TotalTok = TotalTok + 1;
                        MailMatter = MailMatter + TotalTok + ". " + dtGetToken.Rows[i]["Token"].ToString() + " \n";
                    }
                }
                ad.Dispose();
                cmd.Dispose();
                string SupportMatter = "";

                string ClientEmail = "";
                int TotalToken = 0;
                string ExpiryDate = "";
                string ClientName = "";
                string LoginName = "";
                string LoginPassword = "";

                strResend = "  select DFClients.*, tbDealerLogin.LoginPassword,tbDealerLogin.ExpiryDate, tbdealerlogin.DamTotalToken,tbdealerlogin.CopyrightTotalToken,tbdealerlogin.SanjivaniTotalToken ";
                strResend = strResend + "  from DFClients inner join tbDealerLogin on DFClients.DFClientID= tbDealerLogin.DFClientID ";
                strResend = strResend + " where   DFClients.DFClientID = " + data.clientId;
                cmd = new SqlCommand(strResend, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDetail);
                if (dtDetail.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtDetail.Rows.Count - 1; i++)
                    {
                        ClientEmail = dtDetail.Rows[i]["Email"].ToString();
                        TotalToken = Convert.ToInt32(dtDetail.Rows[i]["DealerNoTotalToken"]);
                        ExpiryDate = string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[i]["ExpiryDate"]);
                        ClientName = dtDetail.Rows[i]["ClientName"].ToString();
                        LoginName = dtDetail.Rows[i]["Email"].ToString();
                        LoginPassword = dtDetail.Rows[i]["LoginPassword"].ToString();
                    }
                }
                if (data.DBType == "Advikon")
                {
                    var fromAddress = new MailAddress("advikonservice@gmail.com", "Advikon Service");
                    var toAddress = new MailAddress(ClientEmail);

                    const string fromPassword = "Gilles23789@";
                    string subject = "Notification";
                    string body = "Dear Customer, \n";
                    body += "\n";
                    body += "Thank you for registering with Advikon services. You have registered " + TotalTok + " tokens.";
                    body += "\n";
                    body += "\n";
                    body += "Customer Name: " + ClientName + "\n";

                    body += "\n";
                    body += "You can manage your players and the rest of the services through our web panel. \n";
                    body += "Website: https://advikon.com/ \n";
                    body += "Login Name: " + LoginName + " \n";
                    body += "Password: " + LoginPassword + " \n";
                    body += "License Expiry: " + ExpiryDate + "   \n";
                    body += "\n";
                    body += "The player download links are available on our website https://advikon.com/ under the links menu. \n";
                    body += "\n";
                    body += "Best Regards \n";
                    body += "Your Support Team";
                    body += "\n";
                    body += "\n";
                    //body += "Note: Please Unzip the Windows player file after download in the administration location. Then click on the player exe for the activation of your license adding your customer name with country code and the token you use for the selected player.";
                    body += "\n";
                    body += "More information you can find in the manual what you can download from the login page. \n";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    MailMessage message = new MailMessage();
                    message.Subject = subject;
                    message.Body = body;
                    message.To.Add(toAddress);
                    message.To.Add("jan@advikon.eu");
                    message.To.Add("talwinder@advikon.eu");
                    message.From = fromAddress;
                    string Manualfile = HttpContext.Current.Server.MapPath("~/Manual.pdf");
                    message.Attachments.Add(new Attachment(Manualfile));
                    smtp.Send(message);
                    Result.Responce = "1";
                    con.Close();
                }
                if (data.DBType == "Nusign")
                {
                    var fromAddress = new MailAddress("support@nusign.eu", "Nusign services");
                    var toAddress = new MailAddress(ClientEmail);

                    const string fromPassword = "Talwinder23789@";
                    string subject = "Notification";
                    string body = "Dear Customer, \n";
                    body += "\n";
                    body += "Thank you for registering with Nusign services. You have registered " + TotalTok + " tokens.";
                    body += "\n";
                    body += "\n";
                    body += "Customer Name: " + ClientName + "\n";

                    body += "\n";
                    body += "You can manage your players and the rest of the services through our web panel. \n";
                    body += "Your Login details are: \n";
                    body += "Website: https://nusign.eu/ \n";
                    body += "Login Name: " + LoginName + " \n";
                    body += "Password: " + LoginPassword + " \n";
                    body += "License Expiry: " + ExpiryDate + "   \n";
                    body += "\n";
                    body += "The player download links are available on our website https://nusign.eu/ under the Links menu. \n";
                    body += "\n";
                    body += "Best Regards \n";
                    body += "Your Support Team";
                    body += "\n";
                    body += "\n";
                    //body += "Note: Please Unzip the Windows player file after download in the administration location. Then click on the player exe for the activation of your license adding your customer name with country code and the token you use for the selected player.";
                    body += "\n";
                    body += "More information you can find in the manual what you can download from the login page. \n";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp-auth.mailprotect.be",
                        Port = 2525,
                        //EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword),

                    };
                    MailMessage message = new MailMessage();
                    message.Subject = subject;
                    message.Body = body;
                    message.To.Add(toAddress);
                    string Manualfile = HttpContext.Current.Server.MapPath("~/Manual.pdf");
                    message.Attachments.Add(new Attachment(Manualfile));
                    message.To.Add("info@sanisign.eu");
                    message.To.Add("talwinder@advikon.eu");
                    message.From = fromAddress;
                    smtp.Send(message);
                    Result.Responce = "1";
                    con.Close();
                }
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }



        public ResResponce CitySateNewModify(ReqCitySateNewModify data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            string returnValue = "";
            try
            {
                if (data.type == "State")
                {
                    SqlCommand cmd = new SqlCommand("SaveState", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.BigInt));
                    cmd.Parameters["@CountryId"].Value = data.CountryId;
                    cmd.Parameters.Add(new SqlParameter("@StateName", SqlDbType.VarChar));
                    cmd.Parameters["@StateName"].Value = data.name;
                    cmd.Parameters.Add(new SqlParameter("@Stateid", SqlDbType.BigInt));
                    cmd.Parameters["@Stateid"].Value = data.stateid;
                    if (con.State == ConnectionState.Closed) con.Open();
                    returnValue = cmd.ExecuteScalar().ToString();
                }
                if (data.type == "City")
                {
                    SqlCommand cmd = new SqlCommand("SaveCity", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.BigInt));
                    cmd.Parameters["@CountryId"].Value = data.CountryId;
                    cmd.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
                    cmd.Parameters["@StateId"].Value = data.stateid;
                    cmd.Parameters.Add(new SqlParameter("@CityName", SqlDbType.VarChar));
                    cmd.Parameters["@CityName"].Value = data.name;
                    cmd.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
                    cmd.Parameters["@CityId"].Value = data.id;

                    if (con.State == ConnectionState.Closed) con.Open();
                    returnValue = cmd.ExecuteScalar().ToString();
                }
                if (data.type == "Group")
                {
                    SqlCommand cmd = new SqlCommand("SaveGroup", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@groupId", SqlDbType.BigInt));
                    cmd.Parameters["@groupId"].Value = data.id;
                    cmd.Parameters.Add(new SqlParameter("@groupname", SqlDbType.VarChar));
                    cmd.Parameters["@groupname"].Value = data.name;
                    cmd.Parameters.Add(new SqlParameter("@dfClientId", SqlDbType.BigInt));
                    cmd.Parameters["@dfClientId"].Value = data.dfClientId;

                    if (con.State == ConnectionState.Closed) con.Open();
                    returnValue = cmd.ExecuteScalar().ToString();
                }
                if (returnValue == "-2")
                {
                    clsResult.Responce = returnValue;
                }
                else
                {
                    clsResult.Responce = "1";
                }
                con.Close();
                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }

        public ResResponce CopyFormat(ReqCopyFormat data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                int splId = 0;
                string str = "";
                str = "select  * from tbSpecialPlaylists where formatid= " + data.FormatId;
                DataTable dt = new DataTable();
                SqlCommand cmdDT = new SqlCommand(str, con);
                cmdDT.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmdDT);
                ad.Fill(dt);
                ad.Dispose();
                cmdDT.Dispose();
                con.Open();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str = "";
                        str = "select * from tbSpecialPlaylists where splplaylistname ='" + dt.Rows[i]["splPlaylistName"].ToString().Trim().ToLower() + "' and formatid= " + data.CopyFormatId; 
                        DataTable dtPl = new DataTable();
                        SqlCommand cmdPL = new SqlCommand(str, con);
                        cmdPL.CommandType = System.Data.CommandType.Text;
                        SqlDataAdapter adPL = new SqlDataAdapter(cmdPL);
                        adPL.Fill(dtPl);
                        adPL.Dispose();
                        cmdPL.Dispose();
                        if (dtPl.Rows.Count > 0)
                        {
                            splId =Convert.ToInt32(dtPl.Rows[0]["splPlaylistId"]);
                            str = "";
                            str = "delete from tbSpecialPlaylists_Titles where splPlaylistId =" + splId + "";
                            SqlCommand cmdplDel = new SqlCommand(str, con);
                            cmdplDel.CommandType = System.Data.CommandType.Text;
                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            cmdplDel.ExecuteNonQuery();
                            cmdplDel.Dispose();
                        }
                        else
                        {
                            SqlCommand cmd = new SqlCommand("spSpecialPlaylists_Save_Update", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@pAction", SqlDbType.VarChar));
                            cmd.Parameters["@pAction"].Value = "New";
                            cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                            cmd.Parameters["@splPlaylistId"].Value = 0;
                            cmd.Parameters.Add(new SqlParameter("@splPlaylistName", SqlDbType.VarChar));
                            cmd.Parameters["@splPlaylistName"].Value = dt.Rows[i]["splPlaylistName"].ToString().Trim();
                            cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                            cmd.Parameters["@pVersion"].Value = "c";
                            cmd.Parameters.Add(new SqlParameter("@Formatid", SqlDbType.BigInt));
                            cmd.Parameters["@Formatid"].Value = data.CopyFormatId;
                            cmd.Parameters.Add(new SqlParameter("@mType", SqlDbType.VarChar));
                            cmd.Parameters["@mType"].Value = "Audio";
                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            splId = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        str = "";
                        str = "insert into tbSpecialPlaylists_Titles select " + splId + ", titleid,srno,ImgTimeInterval from tbSpecialPlaylists_Titles where splplaylistid= " + dt.Rows[i]["splPlaylistId"];
                        SqlCommand cmdTitle = new SqlCommand(str, con);
                        cmdTitle.CommandType = System.Data.CommandType.Text;
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        cmdTitle.ExecuteNonQuery();
                        cmdTitle.Dispose();
                    }
                    clsResult.Responce = "1";
                }
                con.Close();
                return clsResult;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                clsResult.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }

        public ResResponce SaveTranferToken(ReqTranferToken data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                foreach (var tTokens in data.TransferTokens)
                {
                    cmd = new SqlCommand("TransferToken", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@tokenId", SqlDbType.BigInt));
                    cmd.Parameters["@tokenId"].Value = tTokens;
                    cmd.Parameters.Add(new SqlParameter("@TransferClientId", SqlDbType.BigInt));
                    cmd.Parameters["@TransferClientId"].Value = data.TransferCustomerId;
                    cmd.Parameters.Add(new SqlParameter("@Clientid", SqlDbType.BigInt));
                    cmd.Parameters["@Clientid"].Value = data.CustomerId;
                    cmd.ExecuteNonQuery();
                }
                result.Responce = "1";
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce UploadStreamImage()
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                var StreamName = HttpContext.Current.Request.Form[0];
                var CustomerId = HttpContext.Current.Request.Form[1];
                var StreamLink = HttpContext.Current.Request.Form[2];
                var k = postedFile.ContentLength;

                SqlCommand cmd = new SqlCommand("sp_AppStream_Save", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StreamId", SqlDbType.BigInt));
                cmd.Parameters["@StreamId"].Value = 0;

                cmd.Parameters.Add(new SqlParameter("@StreamName", SqlDbType.NVarChar));
                cmd.Parameters["@StreamName"].Value = StreamName;

                cmd.Parameters.Add(new SqlParameter("@StreamLink", SqlDbType.NVarChar));
                cmd.Parameters["@StreamLink"].Value = "";

                cmd.Parameters.Add(new SqlParameter("@dfclientid", SqlDbType.BigInt));
                cmd.Parameters["@dfclientid"].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@ImgPath", SqlDbType.NVarChar));
                cmd.Parameters["@ImgPath"].Value = "";

                con.Open();

                Int32 Id = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
                string fName = Id.ToString() + Path.GetExtension(postedFile.FileName);
                var filePath = HttpContext.Current.Server.MapPath("~/AppStreamPic/" + fName);
                postedFile.SaveAs(filePath);

                cmd = new SqlCommand("sp_AppStream_Save", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StreamId", SqlDbType.BigInt));
                cmd.Parameters["@StreamId"].Value = Id;

                cmd.Parameters.Add(new SqlParameter("@StreamName", SqlDbType.NVarChar));
                cmd.Parameters["@StreamName"].Value = StreamName;

                cmd.Parameters.Add(new SqlParameter("@StreamLink", SqlDbType.NVarChar));
                cmd.Parameters["@StreamLink"].Value = StreamLink;

                cmd.Parameters.Add(new SqlParameter("@dfclientid", SqlDbType.BigInt));
                cmd.Parameters["@dfclientid"].Value = CustomerId;
                cmd.Parameters.Add(new SqlParameter("@ImgPath", SqlDbType.NVarChar));
                cmd.Parameters["@ImgPath"].Value = "http://api.advikon.com/AppStreamPic/" + fName;
                cmd.ExecuteNonQuery();

                Result.Responce = "1";
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                var g = ex.Message;
                con.Close();
                return Result;
            }
        }

        public ResResponce UpdateStream(ReqStream data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {

                SqlCommand cmd = new SqlCommand("sp_AppStream_Save", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StreamId", SqlDbType.BigInt));
                cmd.Parameters["@StreamId"].Value = data.sId;

                cmd.Parameters.Add(new SqlParameter("@StreamName", SqlDbType.NVarChar));
                cmd.Parameters["@StreamName"].Value = data.sName.Trim();

                cmd.Parameters.Add(new SqlParameter("@StreamLink", SqlDbType.NVarChar));
                cmd.Parameters["@StreamLink"].Value = data.sLink;

                cmd.Parameters.Add(new SqlParameter("@dfclientid", SqlDbType.BigInt));
                cmd.Parameters["@dfclientid"].Value = data.OwnerId;
                cmd.Parameters.Add(new SqlParameter("@ImgPath", SqlDbType.NVarChar));
                cmd.Parameters["@ImgPath"].Value = data.sImgLink;
                con.Open();
                cmd.ExecuteNonQuery();

                result.Responce = "1";
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public ResResponce DeleteStream(ReqDeleteStream data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {


                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "delete from tblOnlineStreaming_App where streamid=" + data.sId;
                con.Open();
                cmd.ExecuteNonQuery();
                result.Responce = "1";
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce AssignStream(ReqAssignStream data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                SqlCommand cmdOnline = new SqlCommand();
                foreach (var iStream in data.StreamSelected)
                {
                    foreach (var iToken in data.TokenSelected)
                    {
                        cmdOnline = new SqlCommand();
                        cmdOnline.Connection = con;
                        cmdOnline.CommandType = CommandType.Text;
                        cmdOnline.CommandText = "insert into tbAssignMobileStreamToken (tokenid,streamid,StreamOwnerClientid) values (" + iToken + "," + iStream + "," + data.OwnerId + ")";
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmdOnline.ExecuteNonQuery();
                        cmdOnline.Dispose();
                    }
                }

                result.Responce = "1";
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce DeleteAssignStream(ReqDeleteAssignStream data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "delete from tbAssignMobileStreamToken where tokenid=" + data.TokenId + " and streamid=" + data.StreamId;
                con.Open();
                cmd.ExecuteNonQuery();
                result.Responce = "1";
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public List<ResFillMiddleImage> FillMiddleImage(DataStream data)
        {
            List<ResFillMiddleImage> lstResult = new List<ResFillMiddleImage>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                string str = "";
                str = "select distinct  TitleID, ltrim(Title) as Title,Titles.GenreId, isnull(titleimgid,0) as titleImgId ,imgpath FROM Titles ";
                str = str + "  LEFT JOIN (select distinct titleimgid,imgpath from tblMiddleImage_App where tokenid=" + data.TokenId + ") a ";
                str = str + "  on a.titleimgid= Titles.TitleID ";
                str = str + "  where Titles.GenreId= 326  ";
                if (data.IsAdmin == false)
                {
                    str = str + " and dfclientid= " + data.OwnerCustomerId;
                }


                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstResult.Add(new ResFillMiddleImage()
                    {
                        id = ds.Rows[i]["TitleID"].ToString(),
                        IsFind = ds.Rows[i]["titleImgId"].ToString(),
                        TitleIdLink = "http://api.advikon.com/mp3files/" + ds.Rows[i]["TitleID"].ToString() + ".jpg"

                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }

        public ResResponce SetMiddleImg(ReqSetMiddleImg data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                var url = "http://api.advikon.com/mp3files/" + data.TitleId.ToString() + ".jpg";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "insert into tblMiddleImage_App(titleimgid,imgpath,tokenid)" +
                    " values (" + data.TitleId + " ,'" + url + "'," + data.TokenId + ")";
                con.Open();
                cmd.ExecuteNonQuery();
                result.Responce = "1";
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce DeleteMiddleImg(ReqSetMiddleImg data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "delete tblMiddleImage_App where titleimgid= " + data.TitleId + " " +
                    " and tokenid= " + data.TokenId + "";
                con.Open();
                cmd.ExecuteNonQuery();
                result.Responce = "1";
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public List<ResFillMiddleImage> FillSignageLogo(ReqFillSignageLogo data)
        {
            List<ResFillMiddleImage> lstResult = new List<ResFillMiddleImage>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                string str = "";
                str = "select distinct  TitleID, ltrim(Title) as Title,Titles.GenreId FROM Titles ";
                str = str + "  where Titles.GenreId= 326  ";

                str = str + " and dfclientid= " + data.CustomerId;

                if (data.FolderId != "777")
                {
                    str = str + " and isnull(folderId,0)= " + data.FolderId;
                }

                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstResult.Add(new ResFillMiddleImage()
                    {
                        id = ds.Rows[i]["TitleID"].ToString(),
                        IsFind = ds.Rows[i]["TitleID"].ToString(),
                        TitleIdLink = "http://api.advikon.com/mp3files/" + ds.Rows[i]["TitleID"].ToString() + ".jpg"

                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }


        public List<ResponceUserRights> CheckUserRightsLive_bulk(List<DataUserRights_Bulk> lstdata)
        {
            List<ResponceUserRights> result = new List<ResponceUserRights>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string mType = "";
                string IsFind = "No";
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter ad = new SqlDataAdapter();
                DataTable dt = new DataTable();
                foreach (var device in lstdata)
                {
                    cmd = new SqlCommand("spGetTokenExpiryStatus_Mobile '" + device.DeviceId + "'", con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    ad = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    ad.Fill(dt);
                    ad.Dispose();
                    cmd.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        IsFind = "Yes";
                        break;
                    }
                }
                if (IsFind == "No")
                {
                    result.Add(new ResponceUserRights()
                    {
                        Response = "0",
                        LeftDays = "0",
                        TokenId = "0",
                    });
                    con.Close();
                    return result;
                }
                if (Convert.ToInt32(dt.Rows[0]["MediaType"]) == 1)
                {
                    mType = "Video";
                }
                else
                {
                    mType = "Audio";
                }
                string mtypeFormat = "";
                if (dt.Rows[0]["mType"].ToString().Trim() == "Audio")
                {
                    mtypeFormat = ".mp3";
                }
                if (dt.Rows[0]["mType"].ToString().Trim() == "Video")
                {
                    mtypeFormat = ".mp4";
                }
                if (dt.Rows[0]["mType"].ToString().Trim() == "Image")
                {
                    mtypeFormat = ".jpg";
                }
                result.Add(new ResponceUserRights()
                {
                    Response = dt.Rows[0][0].ToString(),
                    LeftDays = dt.Rows[0]["LeftDays"].ToString(),
                    TokenId = dt.Rows[0]["tokenid"].ToString(),
                    dfClientId = dt.Rows[0]["dfClientId"].ToString(),
                    CountryId = Convert.ToInt32(dt.Rows[0]["countryId"]),
                    StateId = Convert.ToInt32(dt.Rows[0]["stateId"]),
                    Cityid = Convert.ToInt32(dt.Rows[0]["cityId"]),
                    IsStopControl = Convert.ToInt32(dt.Rows[0]["IsStopControl"]),
                    MediaType = mType,
                    FcmId = dt.Rows[0]["FcmID"].ToString(),
                    scheduleType = dt.Rows[0]["scheduleType"].ToString(),
                    LogoId = dt.Rows[0]["AppLogoId"].ToString(),
                    IsIndicatorActive = dt.Rows[0]["IsIndicatorActive"].ToString(),
                    Rotation = dt.Rows[0]["Rotation"].ToString(),
                    IsDemoToken = Convert.ToBoolean(dt.Rows[0]["IsDemoToken"]),
                    TotalShot = Convert.ToInt32(dt.Rows[0]["TotalShot"]),
                    DispenserAlert = dt.Rows[0]["DispenserAlert"].ToString(),
                    FireAlertId = dt.Rows[0]["FireAlertId"].ToString(),
                    FireAlertUrl = "http://api.advikon.com/mp3files/" + dt.Rows[0]["FireAlertId"].ToString() + mtypeFormat,

                });

                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce DeleteTitlePercentage(ReqDeleteTitlePercentage data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string str = "";
                str = "DELETE TOP(" + data.titlepercentage + ") PERCENT from tbSpecialPlaylists_Titles where splPlaylistid = " + data.playlistid;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = str;
                con.Open();
                cmd.ExecuteNonQuery();
                result.Responce = "1";
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public List<ResGetMachineAnnouncement> GetMachineAnnouncement(DataCustomerTokenId data)
        {
            List<ResGetMachineAnnouncement> result = new List<ResGetMachineAnnouncement>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string mtypeFormat = "";
                SqlCommand cmd = new SqlCommand("GetMachineAnnouncement " + data.Tokenid + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                string url = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Audio")
                    {
                        mtypeFormat = ".mp3";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Video")
                    {
                        mtypeFormat = ".mp4";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Image")
                    {
                        mtypeFormat = ".jpg";
                    }
                    url = "http://api.advikon.com/mp3files/" + ds.Tables[0].Rows[i]["titleId"].ToString() + mtypeFormat;


                    result.Add(new ResGetMachineAnnouncement()
                    {
                        id = ds.Tables[0].Rows[i]["titleId"].ToString(),
                        url = url,
                        srno = Convert.ToInt32(ds.Tables[0].Rows[i]["srno"]),
                        Artist = ds.Tables[0].Rows[i]["Artist"].ToString(),
                        title = ds.Tables[0].Rows[i]["title"].ToString(),
                        Genre = ds.Tables[0].Rows[i]["genre"].ToString(),
                        aType = ds.Tables[0].Rows[i]["aType"].ToString(),
                        TimeInterval = Convert.ToInt32(ds.Tables[0].Rows[i]["ImgInterval"]),
                        IsLoop = true,
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }



        public ResResponce DeletePlaylistAds(ReqAdsId data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string strDel = "";
                strDel = "delete from tbPlaylistAdsSchedule where pschid=  " + data.advtid;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                strDel = "";
                strDel = "delete from tbPlaylistAdsSchedule_Token where pschid=  " + data.advtid;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                strDel = "";
                strDel = "delete from tbPlaylistAdsSchedule_Week where pschid=  " + data.advtid;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();

                Result.Responce = "1";

                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }

        }

        public List<ResMachineLogs> MachineEventLogs(List<ReqMachineLogs> data)
        {
            SqlConnection conMain = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
            List<ResMachineLogs> result = new List<ResMachineLogs>();
            List<LogsArray> resultSong = new List<LogsArray>();
            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";
                DataTable dtInsert = new DataTable();
                dtInsert.Columns.Add("TokenId", typeof(int));
                dtInsert.Columns.Add("PlayDTP", typeof(DateTime));
                dtInsert.Columns.Add("Event", typeof(string));
                dtInsert.Columns.Add("playdate", typeof(DateTime));
                dtInsert.Columns.Add("command", typeof(string));
                dtInsert.Columns.Add("titleid", typeof(string));
                dtInsert.Columns.Add("aType", typeof(string));

                foreach (var Player in data)
                {

                    if (Player.TokenId != 0)
                    {
                        DataRow nr = dtInsert.NewRow();
                        var k = string.Format(fi, "{0:HH:mm:ss}", Convert.ToDateTime(Player.PlayedDateTime));
                        nr["TokenId"] = Player.TokenId;
                        nr["PlayDTP"] = "01-01-1900 " + k;
                        nr["Event"] = Player.Logs;
                        nr["playdate"] = string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Player.PlayedDateTime));
                        if (string.IsNullOrEmpty(Player.titleId) == false)
                        {
                            nr["titleid"] = Player.titleId;
                        }
                        else
                        {
                            nr["titleid"] = "0";
                        }
                        nr["command"] = Player.command;
                        nr["aType"] = Player.aType;
                        dtInsert.Rows.Add(nr);
                    }
                    resultSong.Add(new LogsArray()
                    {
                        Response = "1",
                        returnEventDateTime = Player.PlayedDateTime
                    });
                }

                if (dtInsert.Rows.Count > 0)
                {

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conMain))
                    {

                        SqlBulkCopyColumnMapping TokenId =
                           new SqlBulkCopyColumnMapping("TokenId", "TokenId");
                        bulkCopy.ColumnMappings.Add(TokenId);
                        SqlBulkCopyColumnMapping PlayDTP =
                           new SqlBulkCopyColumnMapping("PlayDTP", "PlayDTP");
                        bulkCopy.ColumnMappings.Add(PlayDTP);
                        SqlBulkCopyColumnMapping Event =
                           new SqlBulkCopyColumnMapping("Event", "Event");
                        bulkCopy.ColumnMappings.Add(Event);
                        SqlBulkCopyColumnMapping playdate =
                           new SqlBulkCopyColumnMapping("playdate", "playdate");
                        bulkCopy.ColumnMappings.Add(playdate);
                        SqlBulkCopyColumnMapping command =
                           new SqlBulkCopyColumnMapping("command", "command");
                        bulkCopy.ColumnMappings.Add(command);

                        bulkCopy.DestinationTableName = "dbo.tbTokenMachineLogs";
                        if (conMain.State == ConnectionState.Closed)
                        {
                            conMain.Open();
                        }
                        bulkCopy.WriteToServer(dtInsert);

                    }






                    using (SqlBulkCopy bulkCopyTitle = new SqlBulkCopy(conMain))
                    {

                        SqlBulkCopyColumnMapping TokenId =
                           new SqlBulkCopyColumnMapping("TokenId", "tokenid");
                        bulkCopyTitle.ColumnMappings.Add(TokenId);
                        SqlBulkCopyColumnMapping titleid =
                           new SqlBulkCopyColumnMapping("titleid", "titleid");
                        bulkCopyTitle.ColumnMappings.Add(titleid);
                        SqlBulkCopyColumnMapping playdate =
                           new SqlBulkCopyColumnMapping("playdate", "playdate");
                        bulkCopyTitle.ColumnMappings.Add(playdate);
                        SqlBulkCopyColumnMapping playTime =
                         new SqlBulkCopyColumnMapping("PlayDTP", "playTime");
                        bulkCopyTitle.ColumnMappings.Add(playTime);
                        SqlBulkCopyColumnMapping aType =
   new SqlBulkCopyColumnMapping("aType", "aType");
                        bulkCopyTitle.ColumnMappings.Add(aType);
                        bulkCopyTitle.DestinationTableName = "dbo.tbMachinePlayedLogs";
                        if (conMain.State == ConnectionState.Closed)
                        {
                            conMain.Open();
                        }
                        bulkCopyTitle.WriteToServer(dtInsert);


                    }








                }
                result.Add(new ResMachineLogs()
                {
                    Response = "1",
                    EventArray = resultSong
                });

                conMain.Close();
                return result;

            }
            catch (Exception ex)
            {
                var h = ex.Message.ToString();
                result.Add(new ResMachineLogs()
                {
                    Response = "0",
                    EventArray = new List<LogsArray>
                        {
                           new LogsArray { Response = "0" },
                           new LogsArray {  returnEventDateTime ="0"}
                        }
                });
                HttpContext.Current.Response.StatusCode = 1;
                conMain.Close();
                return result;
            }
        }

        public List<ResPlayerLog> FillMachineLogs(ReqTitleLog data)
        {
            List<ResPlayerLog> lstTd = new List<ResPlayerLog>();
            string cs = ConfigurationManager.ConnectionStrings["Panel"].ConnectionString;
            SqlConnection constr = new SqlConnection(cs);
            if (string.IsNullOrEmpty(data.ToDate) == true)
            {
                data.ToDate = data.cDate;
            }
            string str = "GetMachineLogs " + data.tokenid + "  ,'" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.cDate)) + "','" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.ToDate)) + "'," + Convert.ToByte(data.ShowOnlyTankChangeLog);
            SqlCommand cmd = new SqlCommand(str, constr);
            try
            {
                constr.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var k = string.Format("{0:HH:mm:ss}", rdr["PlayDTP"]);
                    ResPlayerLog td = new ResPlayerLog();
                    td.Name = "Dispenser Tank Changed";
                    td.ArtistName = "";
                    td.TotalPlayed = rdr["Event"].ToString();
                    td.PlayedDateTime = string.Format("{0:dd-MMM-yyyy}", rdr["playdate"]);
                    td.SplPlaylistName = "";
                    td.CategoryName = "";
                    td.pDateTime = string.Format("{0:HH:mm:ss}", rdr["PlayDTP"]);
                    lstTd.Add(td);
                }
                constr.Close();
                return lstTd;
            }
            catch (Exception ex)
            {
                constr.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstTd;
            }
        }

        public ResResponce DispenserAlertMail(ReqDispenserAlertMail data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                DataTable dtDetail = new DataTable();
                string strResend = "";
                DataTable dtGetToken = new DataTable();
                string strQ = "";
                strQ = "select clientid,location, isnull(AlertEmail,'') as AlertEmail from AMPlayerTokens where tokenid=" + data.TokenId + " ";
                SqlCommand cmd = new SqlCommand(strQ, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(dtGetToken);

                if (dtGetToken.Rows.Count > 0)
                {


                    ad.Dispose();
                    cmd.Dispose();

                    string ClientEmail = "";
                    string ClientAlertEmail = dtGetToken.Rows[0]["AlertEmail"].ToString().Trim();

                    string Location = dtGetToken.Rows[0]["Location"].ToString();
                    string ClientName = "";

                    strResend = "  select Email,ClientName from   DFClients where DFClientID = " + dtGetToken.Rows[0]["clientid"].ToString();
                    cmd = new SqlCommand(strResend, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    ad.Fill(dtDetail);
                    if (dtDetail.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dtDetail.Rows.Count - 1; i++)
                        {
                            ClientEmail = dtDetail.Rows[i]["Email"].ToString();
                            ClientName = dtDetail.Rows[i]["ClientName"].ToString();
                        }
                    }


                    var fromAddress = new MailAddress("support@nusign.eu", "Nusign services");
                    var toAddress = new MailAddress(ClientEmail);

                    const string fromPassword = "Talwinder23789@";
                    string subject = "! WARNING !";
                    string body = "Dear Customer, \n";
                    body += "\n";
                    body += "You sanitizer dispenser has consumed " + data.TankStatusPercent + "% of its handgel. Please refill at your earliest convenience.\n";
                    body += "Your Player details are: \n";
                    body += "Token Id: " + data.TokenId + " \n";
                    body += "Location: " + Location + " \n";
                    body += "\n";
                    body += "Best Regards \n";
                    body += "Your Support Team";
                    body += "\n";
                    body += "\n";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp-auth.mailprotect.be",
                        Port = 2525,
                        //EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword),

                    };
                    MailMessage message = new MailMessage();
                    message.Subject = subject;
                    message.Body = body;
                    //message.To.Add(toAddress);
                    if (ClientAlertEmail != "")
                    {
                        message.To.Add(ClientAlertEmail);
                    }
                    // message.To.Add("jan@advikon.eu");
                    message.To.Add("talwinder@advikon.eu");
                    message.From = fromAddress;
                    smtp.Send(message);
                    Result.Responce = "1";
                    con.Close();
                    return Result;
                }
                Result.Responce = "0";
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }
        public ResResponce DeleteMachineTitle(ReqDeleteMachineTitle data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                strDel = "delete from tbMachineAnnouncement where TitleID =" + data.titleid + " and tokenid= " + data.Tokenid;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }
        public ResResponce SaveMachineAnnouncement(ReqSaveMachineAnnouncement data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                var pid = data.TokenId;

                DataTable dt = new DataTable();
                dt.Columns.Add("tokenid", typeof(int));
                dt.Columns.Add("titleid", typeof(int));
                dt.Columns.Add("srno", typeof(int));
                string tokenid = "";
                foreach (var tId in data.TokenId)
                {
                    if (tokenid == "")
                    {
                        tokenid = tId.tokenid;
                    }
                    else
                    {
                        tokenid = tokenid + ',' + tId.tokenid;
                    }
                }

                string sQr = "delete from tbMachineAnnouncement where tokenid in(" + tokenid + ")";

                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                if (data.chkWithPrevious == false)
                {
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
                foreach (var tId in data.TokenId)
                {
                    foreach (string iTitle in data.titleid)
                    {
                        sQr = "";
                        sQr = "select * from tbMachineAnnouncement where tokenid=" + tId.tokenid + " and TitleID=" + iTitle;
                        cmd = new SqlCommand(sQr, con);
                        cmd.CommandType = System.Data.CommandType.Text;
                        SqlDataAdapter ad = new SqlDataAdapter(cmd);
                        DataTable ds = new DataTable();
                        ad.Fill(ds);
                        if (ds.Rows.Count == 0)
                        {
                            DataRow nr = dt.NewRow();
                            nr["tokenid"] = tId.tokenid;
                            nr["titleid"] = iTitle;
                            nr["srno"] = dt.Rows.Count + 1;
                            dt.Rows.Add(nr);
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        SqlBulkCopyColumnMapping mapID =
                         new SqlBulkCopyColumnMapping("tokenid", "tokenid");
                        bulkCopy.ColumnMappings.Add(mapID);

                        SqlBulkCopyColumnMapping mapMumber =
                            new SqlBulkCopyColumnMapping("titleid", "titleid");
                        bulkCopy.ColumnMappings.Add(mapMumber);

                        SqlBulkCopyColumnMapping mapName =
                         new SqlBulkCopyColumnMapping("srno", "srno");
                        bulkCopy.ColumnMappings.Add(mapName);

                        bulkCopy.DestinationTableName = "dbo.tbMachineAnnouncement";

                        if (con.State == ConnectionState.Closed)
                        { con.Open(); }
                        bulkCopy.WriteToServer(dt);
                        con.Close();

                    }
                }


                clsResult.Responce = "1";
                return clsResult;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }

        public ResResponce UpdateMachineAnnouncementSRNo(ReqMachineAnnouncementSRNo data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string str = "";
                con.Open();
                foreach (var lst in data.lstTitleSR)
                {
                    str = "update tbMachineAnnouncement set srno=" + lst.index + " where Titleid=" + lst.titleid + " ";
                    str = str + "   and tokenid = " + data.tokenId + "";
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }



        public ResponseTokenCrashLog SendMachineNoti(MachineNoti data)
        {
            ResponseTokenCrashLog ReturnResult = new ResponseTokenCrashLog();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                ClsMachineNoti td = new ClsMachineNoti();

                string sQr = "select NotificationDeviceId, TotalShot, DispenserAlert, IsShowShotToast from AMPlayerTokens where TokenID=" + data.tokenId + " and isnull(NotificationDeviceId,'') !='' ";
                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    td.DispenserAlert = ds.Rows[0]["DispenserAlert"].ToString();
                    td.TotalShot = Convert.ToInt32(ds.Rows[0]["TotalShot"]);
                    td.IsDemoToken = Convert.ToBoolean(ds.Rows[0]["IsShowShotToast"]);
                    td.DeviceToken = ds.Rows[0]["NotificationDeviceId"].ToString();
                    td.type = "Settings";
                    string DeviceToken = td.DeviceToken;


                    var result = "-1";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAVNhkSB0:APA91bFvqS4tV4d8EBd_R9EPR5OwiSYNAu-WpZoE6u4gsxkurkMscL1Gal-PY_0ZC8j2rl5OV38t531qHK8RTXT1mISNVvVcfdoD7JMRROimfEfnN2ppxEli67eiRGmmfwgJEa_ZK3OP"));
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UseDefaultCredentials = true;
                    httpWebRequest.PreAuthenticate = true;
                    httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                    var payload = new
                    {
                        to = DeviceToken,
                        priority = "high",
                        content_available = true,
                        notification = new
                        {

                            body = td,
                            title = "Nusign"
                        },
                    };
                    var serializer = new JavaScriptSerializer();
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = serializer.Serialize(payload);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    var objs = JsonConvert.DeserializeObject<ResNoti>(result);
                    if (objs.success == "0")
                    {
                        ReturnResult.Response = 0;
                        ReturnResult.ErrorMessage = "Error";

                    }
                    else
                    {
                        ReturnResult.Response = 1;
                        ReturnResult.ErrorMessage = "Success";
                    }
                }
                else
                {
                    ReturnResult.Response = 0;
                    ReturnResult.ErrorMessage = "Not found";
                }

                return ReturnResult;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 1;
                ReturnResult.ErrorMessage = ex.ToString();
                ReturnResult.Response = 0;
                return ReturnResult;
            }
        }


        public List<ResponcePlayedSong> MachinePlayedLogs(List<DataPlayedSong> data)
        {
            List<ResponcePlayedSong> result = new List<ResponcePlayedSong>();
            List<SongsArray> resultSong = new List<SongsArray>();
            SqlConnection conMain = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";

                DataTable dtInsert = new DataTable();
                dtInsert.Columns.Add("tokenid", typeof(int));
                dtInsert.Columns.Add("titleid", typeof(int));
                dtInsert.Columns.Add("artistid", typeof(int));
                dtInsert.Columns.Add("playdate", typeof(DateTime));
                dtInsert.Columns.Add("playTime", typeof(DateTime));

                foreach (var Player in data)
                {
                    if (Player.TokenId != 0)
                    {
                        DataRow nr = dtInsert.NewRow();
                        var k = string.Format(fi, "{0:HH:mm:ss}", Convert.ToDateTime(Player.PlayedDateTime));
                        nr["tokenid"] = Player.TokenId;
                        nr["playTime"] = "01-01-1900 " + k;
                        nr["titleid"] = Player.TitleId;
                        nr["artistid"] = Player.ArtistId;
                        nr["playdate"] = string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Player.PlayedDateTime));
                        dtInsert.Rows.Add(nr);
                    }
                    resultSong.Add(new SongsArray()
                    {
                        Response = "1",
                        returnPlayedDateTime = Player.PlayedDateTime,
                        returnTitleId = Player.TitleId.ToString()
                    });
                }
                if (dtInsert.Rows.Count > 0)
                {

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conMain))
                    {

                        SqlBulkCopyColumnMapping TokenId =
                           new SqlBulkCopyColumnMapping("tokenid", "tokenid");
                        bulkCopy.ColumnMappings.Add(TokenId);
                        SqlBulkCopyColumnMapping titleid =
                           new SqlBulkCopyColumnMapping("titleid", "titleid");
                        bulkCopy.ColumnMappings.Add(titleid);
                        SqlBulkCopyColumnMapping artistid =
                           new SqlBulkCopyColumnMapping("artistid", "artistid");
                        bulkCopy.ColumnMappings.Add(artistid);
                        SqlBulkCopyColumnMapping playdate =
                           new SqlBulkCopyColumnMapping("playdate", "playdate");
                        bulkCopy.ColumnMappings.Add(playdate);
                        SqlBulkCopyColumnMapping playTime =
                         new SqlBulkCopyColumnMapping("playTime", "playTime");
                        bulkCopy.ColumnMappings.Add(playTime);
                        bulkCopy.DestinationTableName = "dbo.tbMachinePlayedLogs";
                        if (conMain.State == ConnectionState.Closed)
                        {
                            conMain.Open();
                        }
                        bulkCopy.WriteToServer(dtInsert);

                    }
                }
                result.Add(new ResponcePlayedSong()
                {
                    Response = "1",
                    SongArray = resultSong
                });

                //}
                conMain.Close();
                return result;

            }
            catch (Exception ex)
            {
                result.Add(new ResponcePlayedSong()
                {
                    Response = "0",
                    SongArray = new List<SongsArray>
                        {
                           new SongsArray { Response = "0" },
                           new SongsArray {  returnPlayedDateTime ="0"},
                           new SongsArray {  returnTitleId = "0"}

                        }
                });
                HttpContext.Current.Response.StatusCode = 1;
                conMain.Close();
                return result;
            }
        }
        public List<ResPlayerLog> FillPlayedSanitiserLog(ReqPlayerLog data)
        {
            List<ResPlayerLog> lstTd = new List<ResPlayerLog>();
            string OldTitle = "";
            string cs = ConfigurationManager.ConnectionStrings["Panel"].ConnectionString;
            SqlConnection constr = new SqlConnection(cs);
            if (string.IsNullOrEmpty(data.ToDate) == true)
            {
                data.ToDate = data.cDate;
            }
            string str = "GetTokenMachinePlayedLogs " + data.tokenid + "  ,'" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.cDate)) + "','" + string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(data.ToDate)) + "'";
            SqlCommand cmd = new SqlCommand(str, constr);
            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";
                constr.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    ResPlayerLog td = new ResPlayerLog();
                    td.PlayedDateTime = string.Format(fi, "{0:dd-MMM-yyyy}", rdr["playdate"]) + ' ' + string.Format(fi, "{0:HH:mm:ss}", rdr["pDateTime"]);
                    td.Name = rdr["title"].ToString();
                    td.ArtistName = rdr["name"].ToString();
                    td.SplPlaylistName = rdr["splPlaylistname"].ToString();
                    td.CategoryName = rdr["CategoryName"].ToString();
                    td.pDateTime = rdr["pDateTime"].ToString();
                    lstTd.Add(td);

                }
                constr.Close();
                return lstTd;
            }
            catch (Exception ex)
            {
                constr.Close();

                HttpContext.Current.Response.StatusCode = 1;
                return lstTd;
            }
        }



        public ResResponce UpdateEnergyLevel(ReqUpdateEnergyLevel data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string str = "";
                con.Open();

                str = "update titles set EngeryLevel=" + data.EnergyLevel + " where Titleid=" + data.TitleId + " ";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();

                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public List<ResponceSplSplaylistTitle> GetInstantPlaySpecialPlaylistsTitles(ReqGetInstantPlaySpecialPlaylistsTitles data)
        {
            List<ResponceSplSplaylistTitle> result = new List<ResponceSplSplaylistTitle>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);

            try
            {
                string str = "";
                str = "GetInstantPlaySpecialPlaylistsTitles " + data.Tokenid[0];

                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                string mtypeFormat = "";
                string url = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Audio")
                    {
                        mtypeFormat = ".mp3";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Video")
                    {
                        mtypeFormat = ".mp4";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Image")
                    {
                        mtypeFormat = ".jpg";
                    }

                    url = "http://api.advikon.com/mp3files/" + ds.Tables[0].Rows[i]["titleId"].ToString() + mtypeFormat;

                    result.Add(new ResponceSplSplaylistTitle()
                    {

                        splPlaylistId = Convert.ToInt32(ds.Tables[0].Rows[i]["splPlaylistId"]),
                        titleId = Convert.ToInt32(ds.Tables[0].Rows[i]["titleId"]),
                        Title = ds.Tables[0].Rows[i]["Title"].ToString(),
                        tTime = ds.Tables[0].Rows[i]["Time"].ToString(),
                        ArtistID = Convert.ToInt32(ds.Tables[0].Rows[i]["ArtistID"]),
                        arName = ds.Tables[0].Rows[i]["arName"].ToString(),
                        AlbumID = Convert.ToInt32(ds.Tables[0].Rows[i]["AlbumID"]),
                        alName = ds.Tables[0].Rows[i]["alName"].ToString(),
                        srno = Convert.ToInt32(ds.Tables[0].Rows[i]["srno"]),
                        TitleUrl = url,
                        TitleUrl2 = url,
                        FileSize = ds.Tables[0].Rows[i]["filesize"].ToString(),
                        mediatype = ds.Tables[0].Rows[i]["mType"].ToString().Trim()
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResponseTokenCrashLog InstantPlay(ReqInstantPlay data)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["con"].ConnectionString);
            ResponseTokenCrashLog ReturnResult = new ResponseTokenCrashLog();
            try
            {
                ClsNoti clsData = new ClsNoti();
                string sQr = "";
                string success = "0";
                string tid = "";
                string ext = "";
                if (data.mediatype == "Audio")
                {
                    ext = ".mp3";
                }
                if (data.mediatype == "Video")
                {
                    ext = ".mp4";
                }
                if (data.mediatype == "Image")
                {
                    ext = ".jpg";
                }
                foreach (var item in data.tid)
                {
                    if (tid == "")
                    {
                        tid = item.ToString();
                    }
                    else
                    {
                        tid = tid + "," + item.ToString();
                    }
                }
                sQr = "select isnull(NotificationDeviceId,'') as FcmId, isVedioActive,lType from AMPlayerTokens where tokenid in  (" + tid + ") and isnull(NotificationDeviceId,'') !='' and isVedioActive=1";

                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count == 0)
                {
                    ReturnResult.Response = 0;
                    ReturnResult.ErrorMessage = "Error";
                    return ReturnResult;
                }
                for (int iFCM = 0; iFCM < ds.Rows.Count; iFCM++)
                {

                    clsData.id = data.id;
                    clsData.type = "Song";
                    clsData.url = "http://api.advikon.com/mp3files/" + data.id + ""+ ext;
                    clsData.DeviceToken = ds.Rows[iFCM]["FcmId"].ToString();
                    clsData.title = data.title;
                    clsData.albumid = data.albumid;
                    clsData.artistid = data.artistid;
                    clsData.artistname = data.artistname;
                    clsData.Repeat = data.Repeat;
                    clsData.PlayType = "Next";
                    string DeviceToken = clsData.DeviceToken;

                    var result = "-1";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAVNhkSB0:APA91bFvqS4tV4d8EBd_R9EPR5OwiSYNAu-WpZoE6u4gsxkurkMscL1Gal-PY_0ZC8j2rl5OV38t531qHK8RTXT1mISNVvVcfdoD7JMRROimfEfnN2ppxEli67eiRGmmfwgJEa_ZK3OP"));
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UseDefaultCredentials = true;
                    httpWebRequest.PreAuthenticate = true;
                    httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                    var payload = new
                    {
                        to = DeviceToken,
                        priority = "high",
                        content_available = true,
                        notification = new
                        {

                            body = clsData,
                            title = "MyClaud"
                        },
                    };
                    var serializer = new JavaScriptSerializer();
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = serializer.Serialize(payload);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    var objs = JsonConvert.DeserializeObject<ResNoti>(result);
                    if (objs.success == "0")
                    {
                        if (success == "0")
                        {
                            success = "0";
                        }
                    }
                    else
                    {
                        success = "1";
                    }
                }
                if (success == "0")
                {
                    ReturnResult.Response = 0;
                    ReturnResult.ErrorMessage = "Error";

                }
                else
                {
                    ReturnResult.Response = 1;
                    ReturnResult.ErrorMessage = "Success";
                }
                return ReturnResult;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 1;
                ReturnResult.ErrorMessage = ex.ToString();
                ReturnResult.Response = 0;
                return ReturnResult;
            }
        }

        public List<ResSongList> GetFolderContent(ReqGetFolderContent data)
        {

            List<ResSongList> lstSong = new List<ResSongList>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {


                string sQr = "";
                sQr = " select  TitleID, ltrim(Title) as Title,Time, ltrim(ArtistName) as ArtistName, ltrim(AlbumName) as AlbumName , isnull(genre,'') as genre, Tempo ,titleyear,Category,AlbumID, ArtistID, mediatype , label,fname ,EngeryLevel, bpm, rdate, lang   from( ";
                sQr = sQr + " SELECT  Titles.TitleID, Titles.Title,Titles.Time, Artists.Name as ArtistName, Albums.Name AS AlbumName, tbGenre.genre, isnull(Titles.tempo,'') as Tempo,Titles.titleyear , isnull(acategory,'') as Category ,Titles.AlbumID, Titles.ArtistID, Titles.mediatype,  isnull(Titles.label ,'') as label,isnuLL(tbFolder.folderName,'') as fName , isnull(Titles.EngeryLevel,0) as EngeryLevel, isnull(Titles.BPM,'') as bpm, isnull(Titles.ReleaseDate,'') as rdate, isnull(Titles.language,'') as lang FROM Titles ";
                sQr = sQr + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID  ";
                sQr = sQr + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID  ";
                sQr = sQr + " LEFT OUTER JOIN tbGenre ON Titles.GenreId = tbGenre.GenreId  ";
                sQr = sQr + " LEFT OUTER JOIN tbFolder ON Titles.folderId = tbFolder.folderId  ";
                sQr = sQr + " where Titles.folderId= " + data.FolderId + " and isnull(Titles.IsAnnouncement,0)=0 ";
                sQr = sQr + " and Titles.GenreId !=326 and (Titles.dbtype='" + data.DBType + "' or Titles.dbtype='Both') ";
                sQr = sQr + " and Titles.dfclientid=" + data.ClientId + " ";
                sQr = sQr + " ) as d  order by isnull(genre,'')    ";

                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                var format = "";
                string url = "";
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (ds.Rows[i]["MediaType"].ToString() == "Audio")
                    {
                        format = ".mp3";
                    }
                    if (ds.Rows[i]["MediaType"].ToString() == "Video")
                    {
                        format = ".mp4";
                    }
                    if (ds.Rows[i]["MediaType"].ToString() == "Image")
                    {
                        format = ".jpg";
                    }
                    url = "http://api.advikon.com/mp3files/" + ds.Rows[i]["titleId"].ToString() + format;
                    var rDate = "";
                    if (string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(ds.Rows[i]["rDate"])) == "01-Jan-1900")
                    {
                        rDate = "";
                    }
                    else
                    {
                        rDate = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(ds.Rows[i]["rDate"]));
                    }

                    lstSong.Add(new ResSongList()
                    {
                        check = false,
                        id = ds.Rows[i]["TitleID"].ToString(),
                        title = ds.Rows[i]["Title"].ToString().Trim(),
                        Length = ds.Rows[i]["Time"].ToString(),
                        Artist = ds.Rows[i]["ArtistName"].ToString().Trim(),
                        Album = ds.Rows[i]["AlbumName"].ToString().Trim(),
                        category = ds.Rows[i]["Category"].ToString(),
                        genreName = ds.Rows[i]["genre"].ToString(),
                        ArtistId = ds.Rows[i]["ArtistID"].ToString(),
                        AlbumId = ds.Rows[i]["AlbumID"].ToString(),
                        MediaType = ds.Rows[i]["MediaType"].ToString(),
                        TitleIdLink = url,
                        Label = ds.Rows[i]["label"].ToString(),
                        FolderName = ds.Rows[i]["fName"].ToString(),
                        EngeryLevel = ds.Rows[i]["EngeryLevel"].ToString(),
                        rDate = rDate,
                        BPM = ds.Rows[i]["BPM"].ToString(),
                        Language = ds.Rows[i]["lang"].ToString(),
                        titleyear = ds.Rows[i]["titleyear"].ToString(),
                        dfClientId = "",
                    });
                }
                con.Close();

                return lstSong;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return lstSong;
            }
        }

        public ResResponce SaveTransferContent(ReqTransferContent data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                string str = "";
                string tId = "";

                if (data.FolderId == "0")
                {
                    str = "";
                    str = "update tbFolder set dfclientId  = " + data.dfClientId + " where folderId =" + data.FromFolderId;
                    cmd = new SqlCommand(str, con);
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    data.FolderId = data.FromFolderId;

                }
                foreach (var tlist in data.TitleList)
                {
                    if (tId == "")
                    {
                        tId = tlist;
                    }
                    else
                    {
                        tId = tId + ',' + tlist;
                    }
                }
                if (tId != "")
                {
                    str = "";

                    str = "update titles set dfclientid= " + data.dfClientId + ", folderid= " + data.FolderId + " where titleid in (" + tId + ")";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                con.Close();
                result.Responce = "1";
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public ResResponce UpdateContent(ReqUpdateContent data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string str = "";
                con.Open();

                str = "update titles set title='" + data.titleName + "' where Titleid=" + data.TitleId + " ";
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }



        public List<ResponceUserToen> AppLogin(ReqAppLogin data)
        {
            List<ResponceUserToen> result = new List<ResponceUserToen>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["VideoCon"].ConnectionString);

            try
            {
                string lType = "";

                lType = data.PlayerType;

                SqlCommand cmd = new SqlCommand("spAppLogin '" + data.UserName + "', '" + data.TokenNo + "' , '" + data.DeviceId + "','" + lType + "','" + data.DBType + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    result.Add(new ResponceUserToen()
                    {
                        Response = "0",
                    });
                    con.Close();
                    return result;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ResponceUserToen()
                    {
                        Response = "1",
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public ResResponce SaveKeyboardAnnouncement(ReqKeyboardAnnouncement data)
        {
            ResResponce clsResult = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                var pid = data.TokenId;

                DataTable dt = new DataTable();
                dt.Columns.Add("tokenid", typeof(int));
                dt.Columns.Add("splplaylistid", typeof(int));
                string tokenid = "";
                foreach (var tId in data.TokenId)
                {
                    if (tokenid == "")
                    {
                        tokenid = tId.tokenid;
                    }
                    else
                    {
                        tokenid = tokenid + ',' + tId.tokenid;
                    }
                }

                string sQr = "delete from tbKeyboardAnnouncement where tokenid in(" + tokenid + ")";
                SqlCommand cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                foreach (var tId in data.TokenId)
                {

                    DataRow nr = dt.NewRow();
                    nr["tokenid"] = tId.tokenid;
                    nr["splplaylistid"] = data.splPlaylistId;
                    dt.Rows.Add(nr);

                }
                if (dt.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        SqlBulkCopyColumnMapping mapID =
                         new SqlBulkCopyColumnMapping("tokenid", "tokenid");
                        bulkCopy.ColumnMappings.Add(mapID);

                        SqlBulkCopyColumnMapping mapMumber =
                            new SqlBulkCopyColumnMapping("splplaylistid", "splplaylistid");
                        bulkCopy.ColumnMappings.Add(mapMumber);


                        bulkCopy.DestinationTableName = "dbo.tbKeyboardAnnouncement";

                        if (con.State == ConnectionState.Closed)
                        { con.Open(); }
                        bulkCopy.WriteToServer(dt);
                        con.Close();

                    }
                }


                clsResult.Responce = "1";
                return clsResult;

            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }
        public List<ResGetKeyboardAnnouncement> GetKeyboardAnnouncement(DataCustomerTokenId data)
        {
            List<ResGetKeyboardAnnouncement> result = new List<ResGetKeyboardAnnouncement>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("GetKeyBoardAnnouncement " + data.Tokenid + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {


                    result.Add(new ResGetKeyboardAnnouncement()
                    {
                        id = ds.Tables[0].Rows[i]["id"].ToString(),
                        fName = ds.Tables[0].Rows[i]["FormatName"].ToString(),
                        pName = ds.Tables[0].Rows[i]["splPlaylistName"].ToString(),
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }
        public ResResponce DeleteKeyboardAnnouncement(ReqDeleteKeyboardAnnouncement data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                strDel = "delete from tbKeyboardAnnouncement where id =" + data.id;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce GetClientContenType(ReqTokenInfo data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                var str = "";
                str = "";
                str = "select ContentType from dfclients " +
                 " where dfclientid=" + data.clientId + " ";

                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                Result.Responce = "1";
                Result.ContentType = ds.Rows[0]["ContentType"].ToString();
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }


        public ResResponce SetFireAlert(ReqSetFireAlert data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                foreach (var item in data.tokenId)
                {
                    string strDel = "";
                    strDel = "update AMPlayerTokens set FireAlertId=" + data.titleid + ", FireAlertMediaType= '" + data.MediaType + "' where tokenid =" + item.tokenid;
                    SqlCommand cmd = new SqlCommand(strDel, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public List<ResGetMachineAnnouncement> GetFireAlert(DataCustomerTokenId data)
        {
            List<ResGetMachineAnnouncement> result = new List<ResGetMachineAnnouncement>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);

            try
            {
                string mtypeFormat = "";
                SqlCommand cmd = new SqlCommand("GetFireAlert " + data.Tokenid + " ", con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                string url = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Audio")
                    {
                        mtypeFormat = ".mp3";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Video")
                    {
                        mtypeFormat = ".mp4";
                    }
                    if (ds.Tables[0].Rows[i]["mType"].ToString().Trim() == "Image")
                    {
                        mtypeFormat = ".jpg";
                    }
                    url = "http://api.advikon.com/mp3files/" + ds.Tables[0].Rows[i]["titleId"].ToString() + mtypeFormat;


                    result.Add(new ResGetMachineAnnouncement()
                    {
                        id = ds.Tables[0].Rows[i]["titleId"].ToString(),
                        url = url,
                        srno = Convert.ToInt32(ds.Tables[0].Rows[i]["srno"]),
                        Artist = ds.Tables[0].Rows[i]["Artist"].ToString(),
                        title = ds.Tables[0].Rows[i]["title"].ToString(),
                        Genre = ds.Tables[0].Rows[i]["genre"].ToString(),
                        aType = ds.Tables[0].Rows[i]["aType"].ToString(),
                        TimeInterval = 10000,
                        IsLoop = true,
                    });
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce DeleteFireAlert(ReqDeleteMachineTitle data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                strDel = "update AMPlayerTokens set FireAlertId=null, FireAlertMediaType= null where tokenid =" + data.Tokenid;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public async Task<List<ResGetTemplates>> GetTemplates(ReqGetTemplates data)
        {
            List<ResGetTemplates> lstResult = new List<ResGetTemplates>();

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string aKey = "";
                string str = "select isnull(apikey,'') as aKey from DFClients where DFClientID =" + data.dfClientId;
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    if (ds.Rows[0]["aKey"].ToString() != "")
                    {
                        aKey = ds.Rows[0]["aKey"].ToString();
                    }
                }

                if (string.IsNullOrEmpty(aKey) == true)
                {
                    con.Close();
                    return lstResult;
                }
                //d94fd3f48769dce494014f60901e62b0
                string orientation = "";

                var url = "";
                url = "https://content.nusign.eu/api/my-templates?key=" + aKey.Trim();
                if (data.GenreId == 297)
                {
                    orientation = "landscape";
                }
                if (data.GenreId == 303)
                {
                    orientation = "portrait";
                }
                DateTime dt = new DateTime();
                DateTime dt2 = new DateTime();

                string h = "";

                dt2 = Convert.ToDateTime(string.Format("{0:yyyy-mm-dd}", data.cDate));
                h = dt2.Date.ToString("yyyy-MM-dd");
                dt = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", h));
                if (dt.Date == DateTime.Now.Date)
                {
                    dt = dt.AddDays(-1);
                }

                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var unixDateTime = (dt.ToUniversalTime() - epoch).TotalSeconds;

                if (data.search != "")
                {
                    url = url + "&search=" + data.search;
                }
                else
                {
                    url = url + "&orientation=" + orientation + "&createdAfter=" + unixDateTime;
                }
                // https://content.nusign.eu/api/my-templates?key=d94fd3f48769dce494014f60901e62b0&orientation=portrait
                using (var client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    using (var response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var fileJsonString = await response.Content.ReadAsStringAsync();
                            lstResult = JsonConvert.DeserializeObject<List<ResGetTemplates>>(fileJsonString);

                        }
                    }
                }

                return lstResult;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }

        public async Task<ResResponce> DownloadTemplates(ReqDownloadTemplates data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            Int32 Title_Id= 0;
            try
            {
                con.Open();
                foreach (var item in data.tList)
                {

                    SqlCommand cmd = new SqlCommand("InsertContent", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TiTleTiTle", SqlDbType.VarChar));
                    cmd.Parameters["@TiTleTiTle"].Value = item.TemplateName;

                    cmd.Parameters.Add(new SqlParameter("@TitleArtistName", SqlDbType.VarChar));
                    cmd.Parameters["@TitleArtistName"].Value = item.TemplateName;

                    cmd.Parameters.Add(new SqlParameter("@AlbumName", SqlDbType.VarChar));
                    cmd.Parameters["@AlbumName"].Value = "Templates";

                    cmd.Parameters.Add(new SqlParameter("@titlecategoryid", SqlDbType.BigInt));
                    cmd.Parameters["@titlecategoryid"].Value = 4;

                    cmd.Parameters.Add(new SqlParameter("@titleSubcategoryid", SqlDbType.VarChar));
                    cmd.Parameters["@titleSubcategoryid"].Value = 22;

                    cmd.Parameters.Add(new SqlParameter("@Time", SqlDbType.VarChar));
                    cmd.Parameters["@Time"].Value = "00:00:00";

                    cmd.Parameters.Add(new SqlParameter("@AlbumLabel", SqlDbType.VarChar));
                    cmd.Parameters["@AlbumLabel"].Value = "0";

                    cmd.Parameters.Add(new SqlParameter("@CatalogCode", SqlDbType.VarChar));
                    cmd.Parameters["@CatalogCode"].Value = "0";

                    cmd.Parameters.Add(new SqlParameter("@titleYear", SqlDbType.Int));
                    cmd.Parameters["@titleYear"].Value = 0;


                    cmd.Parameters.Add(new SqlParameter("@GenreId", SqlDbType.Int));
                    cmd.Parameters["@GenreId"].Value = data.GenreId;

                    cmd.Parameters.Add(new SqlParameter("@tempo", SqlDbType.VarChar));
                    cmd.Parameters["@tempo"].Value = "Mid";


                    cmd.Parameters.Add(new SqlParameter("@mType", SqlDbType.VarChar));
                    cmd.Parameters["@mType"].Value = "Video";

                    cmd.Parameters.Add(new SqlParameter("@acategory", SqlDbType.VarChar));
                    cmd.Parameters["@acategory"].Value = "Templates";

                    cmd.Parameters.Add(new SqlParameter("@language", SqlDbType.VarChar));
                    cmd.Parameters["@language"].Value = "";

                    cmd.Parameters.Add(new SqlParameter("@isRF", SqlDbType.VarChar));
                    cmd.Parameters["@isRF"].Value = "0";

                    cmd.Parameters.Add(new SqlParameter("@isrc", SqlDbType.VarChar));
                    cmd.Parameters["@isrc"].Value = "";

                    cmd.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.VarChar));
                    cmd.Parameters["@FileSize"].Value = "0";

                    cmd.Parameters.Add(new SqlParameter("@dfclientid", SqlDbType.BigInt));
                    cmd.Parameters["@dfclientid"].Value = data.dfClientId;

                    cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt));
                    cmd.Parameters["@folderid"].Value = data.FolderId;

                    cmd.Parameters.Add(new SqlParameter("@dbType", SqlDbType.VarChar));
                    cmd.Parameters["@dbType"].Value = data.dbType.Trim();

                    cmd.Parameters.Add(new SqlParameter("@IsAnnouncement", SqlDbType.Int));
                    cmd.Parameters["@IsAnnouncement"].Value = "0";

                    Title_Id = Convert.ToInt32(cmd.ExecuteScalar());
                    //Int32 Title_Id = DateTime.Now.Millisecond;
                    cmd.Dispose();

                    var client = new HttpClient();
                    var response = await client.GetAsync(item.Url);
                    var fsize = "0";
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        fsize = stream.Length.ToString();
                        var fName = "~/mp3files/" + Title_Id.ToString() + ".mp4";
                        var filePath = System.Web.Hosting.HostingEnvironment.MapPath(fName);
                        var fileInfo = new FileInfo(filePath);
                        using (var fileStream = fileInfo.OpenWrite())
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }


                    string strDel = "";
                    strDel = "update titles set filesize='" + fsize + "' where titleid =" + Title_Id.ToString();
                    cmd = new SqlCommand(strDel, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();



                }




                con.Close();
                result.Responce = "1";
                result.TitleId = Title_Id.ToString();
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public ResResponce SaveImageTimeInterval(List<ReqSaveImageTimeInterval> lstData)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                con.Open();
                foreach (var data in lstData)
                {
                    strDel = "";
                    strDel = "update tbSpecialPlaylists_Titles set ImgTimeInterval=" + data.ImgInterval + " where TitleID =" + data.titleid + " and splPlaylistId = " + data.splId;
                    SqlCommand cmd = new SqlCommand(strDel, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce DeleteFolder(ReqDeleteFolder data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                con.Open();
                strDel = "";
                strDel = "delete from tbFolder where folderId = " + data.id;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                strDel = "";
                strDel = "update Titles set folderid=0 where folderid = " + data.id;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce UpdateTokenGroups(ReqUpdateTokenGroups Data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                con.Open();

                foreach (var data in Data.tokenIds)
                {
                    strDel = "";
                    strDel = "update AMPlayerTokens set GroupId=" + Data.GroupId + " where tokenid =" + data;
                    SqlCommand cmd = new SqlCommand(strDel, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }
        public ResResponce DeleteGroup(ReqDeleteFolder data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string strDel = "";
                con.Open();
                strDel = "";
                strDel = "delete from tbGroup where GroupId = " + data.id;
                SqlCommand cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                strDel = "";
                strDel = "update AMPlayerTokens set GroupId=0 where GroupId = " + data.id;
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }



        public ResResponce ClientTemplateRegsiter(ReqTokenInfo data)
        {
            ResClientTemplateRegsiter resAPI = new ResClientTemplateRegsiter();
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            string str = "";
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataTable dtDetail = new DataTable();

                string ClientEmail = "", ClientEmail_Templates = "", ClientName = "", LoginPassword = "", firstName = "", lastName = "";

                str = "  select DFClients.*, tbDealerLogin.LoginPassword,tbDealerLogin.ExpiryDate, tbdealerlogin.DamTotalToken,tbdealerlogin.CopyrightTotalToken,tbdealerlogin.SanjivaniTotalToken ";
                str = str + "  from DFClients inner join tbDealerLogin on DFClients.DFClientID= tbDealerLogin.DFClientID ";
                str = str + " where   DFClients.DFClientID = " + data.clientId;
                cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDetail);
                cmd.Dispose();
                ad.Dispose();
                if (dtDetail.Rows.Count > 0)
                {

                    ClientEmail = dtDetail.Rows[0]["Email"].ToString();
                    ClientName = dtDetail.Rows[0]["ClientName"].ToString();
                    LoginPassword = dtDetail.Rows[0]["LoginPassword"].ToString();
                }
                var obj2 = ClientEmail.Split('@');
                ClientEmail_Templates = "";
                ClientEmail_Templates = obj2[0].ToString() + "_" + dtDetail.Rows[0]["DFClientID"].ToString() + "@" + obj2[1].ToString();

                var obj = ClientName.Split('-');
                firstName = obj[0];
                lastName = obj[1];

                DateTime dt = new DateTime();
                DateTime dt2 = new DateTime();

                string h = "";
                var k = string.Format("{0:yyyy-MM-dd}", dtDetail.Rows[0]["ExpiryDate"]);
                dt2 = Convert.ToDateTime(string.Format("{0:yyyy-mm-dd}", k));
                h = dt2.Date.ToString("yyyy-MM-dd");
                dt = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", h));

                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var unixDateTime_ExpiryDate = (dt.ToUniversalTime() - epoch).TotalSeconds;

                DateTime dtStart = new DateTime();
                dtStart = DateTime.Now.Date;
                var epoch2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var unixDateTime_StartDate = (dtStart.ToUniversalTime() - epoch).TotalSeconds;





                var client = new RestClient("https://content.nusign.eu/api/register");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("companyName", ClientName);
                request.AddParameter("firstName", firstName);
                request.AddParameter("middleName", "");
                request.AddParameter("lastName", lastName);
                request.AddParameter("email", ClientEmail_Templates);
                request.AddParameter("password", LoginPassword);
                request.AddParameter("trialStartDate", unixDateTime_StartDate);
                request.AddParameter("trialEndDate", unixDateTime_ExpiryDate);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                IRestResponse response = client.Execute(request);

                resAPI = JsonConvert.DeserializeObject<ResClientTemplateRegsiter>(response.Content);
                if (resAPI.status == "success")
                {

                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    str = "";
                    str = "update DFClients set apikey='" + resAPI.key + "',IsTemplateActive=1,Template_Login_Email='" + ClientEmail_Templates + "' where DFClientID = " + data.clientId;
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Close();
                    result.Responce = "1";

                    if (data.DBType == "Advikon")
                    {
                        var fromAddress = new MailAddress("advikonservice@gmail.com", "Advikon Service");
                        var toAddress = new MailAddress(ClientEmail);

                        const string fromPassword = "Gilles23789@";
                        string subject = "Notification";
                        string body = "Dear Customer, \n";
                        body += "\n";
                        body += "Thank you for registering with Advikon template services.";
                        body += "\n";
                        body += "\n";
                        body += "Customer Name: " + ClientName + "\n";

                        body += "\n";
                        body += "You can create your own templates through web panel. \n";
                        body += "Website: https://content.nusign.eu/ \n";
                        body += "Login Name: " + ClientEmail_Templates + " \n";
                        body += "Password: " + LoginPassword + " \n";
                        body += "\n";
                        body += "Best Regards \n";
                        body += "Your Support Team";
                        body += "\n";
                        body += "\n";
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                        };
                        MailMessage message = new MailMessage();
                        message.Subject = subject;
                        message.Body = body;
                        message.To.Add(toAddress);
                        message.To.Add("talwinder@advikon.eu");
                        message.From = fromAddress;
                        smtp.Send(message);
                        con.Close();
                    }
                    if (data.DBType == "Nusign")
                    {
                        var fromAddress = new MailAddress("support@nusign.eu", "Nusign services");
                        var toAddress = new MailAddress(ClientEmail);

                        const string fromPassword = "Talwinder23789@";
                        string subject = "Notification";
                        string body = "Dear Customer, \n";
                        body += "\n";
                        body += "Thank you for registering with Nusign template services.";
                        body += "\n";
                        body += "\n";
                        body += "Customer Name: " + ClientName + "\n";

                        body += "\n";
                        body += "You can create your own templates through web panel. \n";
                        body += "Your Login details are: \n";
                        body += "Website: https://content.nusign.eu/ \n";
                        body += "Login Name: " + ClientEmail_Templates + " \n";
                        body += "Password: " + LoginPassword + " \n";
                        body += "\n";
                        body += "\n";
                        body += "Best Regards \n";
                        body += "Your Support Team";
                        body += "\n";
                        body += "\n";
                        var smtp = new SmtpClient
                        {
                            Host = "smtp-auth.mailprotect.be",
                            Port = 2525,
                            //EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromAddress.Address, fromPassword),

                        };
                        MailMessage message = new MailMessage();
                        message.Subject = subject;
                        message.Body = body;
                        message.To.Add(toAddress);
                        message.To.Add("talwinder@advikon.eu");
                        message.From = fromAddress;
                        smtp.Send(message);
                    }






















                }
                else
                {
                    result.Responce = "0";
                }

                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                return result;
            }
        }


        public ResResponce SaveOpeningHours(ReqOpeningHours data)
        {
            ResResponce result = new ResResponce();
            SqlConnection conMain = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";

                DataTable dtInsert = new DataTable();
                dtInsert.Columns.Add("tokenId", typeof(int));
                dtInsert.Columns.Add("wid", typeof(int));
                dtInsert.Columns.Add("starttime", typeof(DateTime));
                dtInsert.Columns.Add("endtime", typeof(DateTime));
                string wid = "";
                foreach (var TokenId in data.TokenList)
                {
                    if (TokenId != "0")
                    {
                        DataRow nr = dtInsert.NewRow();
                        var k = string.Format(fi, "{0:HH:mm:ss}", Convert.ToDateTime(data.startTime));
                        var k2 = string.Format(fi, "{0:HH:mm:ss}", Convert.ToDateTime(data.EndTime));
                        nr["tokenId"] = TokenId;
                        nr["wid"] = "1";
                        nr["starttime"] = "01-01-1900 " + k;
                        nr["endtime"] = "01-01-1900 " + k2;
                        dtInsert.Rows.Add(nr);
                        //foreach (var item in data.wList)
                        //{
                        //    DataRow nr = dtInsert.NewRow();
                        //    var k = string.Format(fi, "{0:HH:mm:ss}", Convert.ToDateTime(data.startTime));
                        //    var k2 = string.Format(fi, "{0:HH:mm:ss}", Convert.ToDateTime(data.EndTime));
                        //    nr["tokenId"] = TokenId;
                        //    nr["wid"] = item.id;
                        //    nr["starttime"] = "01-01-1900 " + k;
                        //    nr["endtime"] = "01-01-1900 " + k2;
                        //    dtInsert.Rows.Add(nr);
                        //    if (wid == "")
                        //    {
                        //        wid = item.id;
                        //    }
                        //    else
                        //    {
                        //        wid = wid+ ","+ item.id;
                        //    }
                        //}

                        string strDel = "";
                        if (conMain.State == ConnectionState.Closed)
                        {
                            conMain.Open();
                        }
                        strDel = "";
                        strDel = "delete from tbTokenOpeningHours where tokenId = " + TokenId;
                        SqlCommand cmd = new SqlCommand(strDel, conMain);
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                }
                if (dtInsert.Rows.Count > 0)
                {

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conMain))
                    {

                        SqlBulkCopyColumnMapping TokenId =
                           new SqlBulkCopyColumnMapping("tokenId", "tokenId");
                        bulkCopy.ColumnMappings.Add(TokenId);
                        SqlBulkCopyColumnMapping weekId =
                           new SqlBulkCopyColumnMapping("wid", "wid");
                        bulkCopy.ColumnMappings.Add(weekId);
                        SqlBulkCopyColumnMapping starttime =
                           new SqlBulkCopyColumnMapping("starttime", "starttime");
                        bulkCopy.ColumnMappings.Add(starttime);
                        SqlBulkCopyColumnMapping endtime =
                           new SqlBulkCopyColumnMapping("endtime", "endtime");
                        bulkCopy.ColumnMappings.Add(endtime);
                        bulkCopy.DestinationTableName = "dbo.tbTokenOpeningHours";
                        if (conMain.State == ConnectionState.Closed)
                        {
                            conMain.Open();
                        }
                        bulkCopy.WriteToServer(dtInsert);

                    }
                    result.Responce = "1";
                }
                return result;
            }
            catch (Exception ex)
            {
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                result.Responce = "0";
                conMain.Close();
                return result;
            }
        }

        public List<ResTokenInfo> FillTokenOpeningHours(ReqTokenInfo data)
        {
            List<ResTokenInfo> lstResult = new List<ResTokenInfo>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";

                string str = "";
                if (string.IsNullOrEmpty(data.UserId) == true)
                {
                    str = "GetTokenOpeningHours " + data.clientId + ",0 ";
                }
                else
                {
                    str = "GetTokenOpeningHours " + data.clientId + " , " + data.UserId + "";
                }
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    var t1 = string.Format(fi, "{0:HH:mm}", ds.Rows[i]["StartTime"]);
                    var t2 = string.Format(fi, "{0:HH:mm}", ds.Rows[i]["EndTime"]);

                    if ((string.Format(fi, "{0:HH:mm}", ds.Rows[i]["StartTime"]) == "00:00")
                        && (string.Format(fi, "{0:HH:mm}", ds.Rows[i]["EndTime"]) == "00:00"))
                    {
                        t1 = "";
                        t2 = "";
                    }

                    lstResult.Add(new ResTokenInfo()
                    {
                        tokenid = ds.Rows[i]["tokenid"].ToString(),
                        tokenCode = ds.Rows[i]["tNo"].ToString(),
                        location = ds.Rows[i]["Location"].ToString(),
                        city = ds.Rows[i]["CityName"].ToString(),
                        countryName = ds.Rows[i]["CountryName"].ToString(),
                        StartTime = t1,
                        EndTime = t2,
                        WeekName = ds.Rows[i]["wName"].ToString(),
                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }

        public ResResponce UpdateTokenInfo()
        {
            ResResponce Result = new ResResponce();
            SqlConnection conSql = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                conSql.Open();

                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];
                string fName = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Millisecond.ToString() + Path.GetExtension(postedFile.FileName);

                var filePath = HttpContext.Current.Server.MapPath("~/sheet/" + fName);
                postedFile.SaveAs(filePath);
                string extension = Path.GetExtension(filePath);
                string header = "YES";
                string conStr, sheetName;
                conStr = string.Empty;
                string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conStr = string.Format(Excel03ConString, filePath, header);
                        break;

                    case ".xlsx": //Excel 07
                        conStr = string.Format(Excel07ConString, filePath, header);
                        break;
                }
                using (OleDbConnection con = new OleDbConnection(conStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.Connection = con;
                        con.Open();
                        DataTable dtExcelSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        con.Close();
                    }
                }
                DataTable dtM = new DataTable();
                using (OleDbConnection con = new OleDbConnection(conStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        using (OleDbDataAdapter oda = new OleDbDataAdapter())
                        {
                            cmd.CommandText = "SELECT * From [" + sheetName + "]";
                            cmd.Connection = con;
                            con.Open();
                            oda.SelectCommand = cmd;
                            oda.Fill(dtM);
                            con.Close();
                        }
                    }
                }
                if (dtM.Columns.Count != 17)
                {
                    Result.Responce = "0";
                    Result.message = "Selected excel file is not a correct file. Columns are not match";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[0].ToString().ToLower() != "tokenid")
                {
                    Result.Responce = "0";
                    Result.message = "TokenId column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[1].ToString().ToLower() != "tokencode")
                {
                    Result.Responce = "0";
                    Result.message = "Token code column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[2].ToString().ToLower() != "country")
                {
                    Result.Responce = "0";
                    Result.message = "Country column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[3].ToString().ToLower() != "state")
                {
                    Result.Responce = "0";
                    Result.message = "State column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[4].ToString().ToLower() != "city")
                {
                    Result.Responce = "0";
                    Result.message = "City column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[5].ToString().ToLower() != "street")
                {
                    Result.Responce = "0";
                    Result.message = "Street column is not match with sequence";
                    conSql.Close();
                    return Result;
                }

                if (dtM.Columns[6].ToString().ToLower() != "location")
                {
                    Result.Responce = "0";
                    Result.message = "Location column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[7].ToString().ToLower() != "iswindowsplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsWindowPlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[8].ToString().ToLower() != "isandroidplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsAndroidPlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }

                if (dtM.Columns[9].ToString().ToLower() != "isaudioplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsAudioPlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[10].ToString().ToLower() != "isvideoplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsVideoPlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[11].ToString().ToLower() != "issignageplayer")
                {
                    Result.Responce = "0";
                    Result.message = "IsSignagePlayer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }

                if (dtM.Columns[12].ToString().ToLower() != "iscopyright")
                {
                    Result.Responce = "0";
                    Result.message = "IsCopyright column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[13].ToString().ToLower() != "isdirectlicence")
                {
                    Result.Responce = "0";
                    Result.message = "IsDirectLicence column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[14].ToString().ToLower() != "isscreen")
                {
                    Result.Responce = "0";
                    Result.message = "IsScreen column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[15].ToString().ToLower() != "issanitizer")
                {
                    Result.Responce = "0";
                    Result.message = "IsSanitizer column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                if (dtM.Columns[16].ToString().ToLower() != "dispenseralertemail")
                {
                    Result.Responce = "0";
                    Result.message = "DispenserAlertEmail column is not match with sequence";
                    conSql.Close();
                    return Result;
                }
                var k = "";
                var h = "";

                string str = "";
                str = "";
                string CountryId = "0";
                string StateId = "0";
                string CityId = "0";
                DataTable dt = new DataTable();
                SqlCommand SqlCmd = new SqlCommand();
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if ((dtM.Rows[i]["TokenId"].ToString() != "") && (dtM.Rows[i]["tokencode"].ToString() != ""))
                    {
                        if ((dtM.Rows[i]["Country"].ToString() != "") && (dtM.Rows[i]["State"].ToString() != "")
                            && (dtM.Rows[i]["City"].ToString() != ""))
                        {
                            k = dtM.Rows[i]["Country"].ToString().Substring(0, 2).Trim().ToUpper();
                            str = "";
                            str = "select CountryCode from CountryCodes where CountryName='" + dtM.Rows[i]["Country"].ToString().Trim() + "' ";
                            SqlCmd = new SqlCommand(str, conSql);
                            SqlCmd.CommandType = CommandType.Text;
                            SqlDataAdapter ad = new SqlDataAdapter(SqlCmd);
                            dt = new DataTable();
                            ad.Fill(dt);
                            ad.Dispose();
                            SqlCmd.Dispose();
                            if (dt.Rows.Count == 0)
                            {
                                SqlCmd = new SqlCommand("SaveCountry", conSql);
                                SqlCmd.CommandType = CommandType.StoredProcedure;
                                SqlCmd.Parameters.Add(new SqlParameter("@CountryName", SqlDbType.BigInt));
                                SqlCmd.Parameters["@CountryName"].Value = dtM.Rows[i]["Country"].ToString().Trim();
                                SqlCmd.Parameters.Add(new SqlParameter("@CountryNameShort", SqlDbType.VarChar));
                                SqlCmd.Parameters["@CountryNameShort"].Value = dtM.Rows[i]["Country"].ToString().Substring(0, 2).Trim().ToUpper();
                                if (conSql.State == ConnectionState.Closed) conSql.Open();
                                CountryId = SqlCmd.ExecuteScalar().ToString();
                            }
                            else
                            {
                                CountryId = dt.Rows[0][0].ToString();
                            }

                            str = "";
                            str = "select stateid from tbState where StateName='" + dtM.Rows[i]["State"].ToString().Trim() + "' and CountryId=" + CountryId;
                            SqlCmd = new SqlCommand(str, conSql);
                            SqlCmd.CommandType = CommandType.Text;
                            ad = new SqlDataAdapter(SqlCmd);
                            dt = new DataTable();
                            ad.Fill(dt);
                            ad.Dispose();
                            SqlCmd.Dispose();
                            if (dt.Rows.Count == 0)
                            {
                                SqlCmd = new SqlCommand("SaveState", conSql);
                                SqlCmd.CommandType = CommandType.StoredProcedure;
                                SqlCmd.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.BigInt));
                                SqlCmd.Parameters["@CountryId"].Value = CountryId;
                                SqlCmd.Parameters.Add(new SqlParameter("@StateName", SqlDbType.VarChar));
                                SqlCmd.Parameters["@StateName"].Value = dtM.Rows[i]["State"].ToString().Trim();
                                SqlCmd.Parameters.Add(new SqlParameter("@Stateid", SqlDbType.BigInt));
                                SqlCmd.Parameters["@Stateid"].Value = 0;
                                if (conSql.State == ConnectionState.Closed) conSql.Open();
                                StateId = SqlCmd.ExecuteScalar().ToString();
                            }
                            else
                            {
                                StateId = dt.Rows[0][0].ToString();
                            }

                            str = "";
                            str = "select CityId from tbCity where CityName='" + dtM.Rows[i]["City"].ToString().Trim() + "' and StateId=" + StateId;
                            SqlCmd = new SqlCommand(str, conSql);
                            SqlCmd.CommandType = CommandType.Text;
                            ad = new SqlDataAdapter(SqlCmd);
                            dt = new DataTable();
                            ad.Fill(dt);
                            ad.Dispose();
                            SqlCmd.Dispose();
                            if (dt.Rows.Count == 0)
                            {
                                SqlCmd = new SqlCommand("SaveCity", conSql);
                                SqlCmd.CommandType = CommandType.StoredProcedure;
                                SqlCmd.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.BigInt));
                                SqlCmd.Parameters["@CountryId"].Value = CountryId;
                                SqlCmd.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
                                SqlCmd.Parameters["@StateId"].Value = StateId;
                                SqlCmd.Parameters.Add(new SqlParameter("@CityName", SqlDbType.VarChar));
                                SqlCmd.Parameters["@CityName"].Value = dtM.Rows[i]["City"].ToString().Trim();
                                SqlCmd.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
                                SqlCmd.Parameters["@CityId"].Value = 0;
                                if (conSql.State == ConnectionState.Closed) conSql.Open();
                                CityId = SqlCmd.ExecuteScalar().ToString();
                            }
                            else
                            {
                                CityId = dt.Rows[0][0].ToString();
                            }
                        }



                        str = "update AMPlayerTokens set CountryId= " + CountryId + ",stateid =" + StateId + ",cityid=" + CityId + " ";
                        str = str + ", Location = '" + dtM.Rows[i]["Location"].ToString().Trim() + "' , streetname='" + dtM.Rows[i]["Street"].ToString().Trim() + "' ";
                        if ((dtM.Rows[i]["IsAndroidPlayer"].ToString() == "1") || (dtM.Rows[i]["IsAndroidPlayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", lType = 'Android' ";
                        }
                        if ((dtM.Rows[i]["IsWindowsPlayer"].ToString() == "1") || (dtM.Rows[i]["IsWindowsPlayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", lType = 'Desktop' ";
                        }
                        if ((dtM.Rows[i]["isaudioplayer"].ToString() == "1") || (dtM.Rows[i]["isaudioplayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", IsVedioActive = 0 , mediatype='Audio'";
                        }
                        if ((dtM.Rows[i]["IsVideoPlayer"].ToString() == "1") || (dtM.Rows[i]["IsVideoPlayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", IsVedioActive = 1 , mediatype='Video'";
                        }
                        if ((dtM.Rows[i]["IsSignagePlayer"].ToString() == "1") || (dtM.Rows[i]["IsSignagePlayer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", IsVedioActive = 1, mediatype='Signage' ";
                        }

                        if ((dtM.Rows[i]["IsDirectLicence"].ToString() == "1") || (dtM.Rows[i]["IsDirectLicence"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", ptype ='DirectLicence' ";
                        }

                        if ((dtM.Rows[i]["IsCopyright"].ToString() == "1") || (dtM.Rows[i]["IsCopyright"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", ptype ='Copyright' ";
                        }


                        if ((dtM.Rows[i]["IsScreen"].ToString() == "1") || (dtM.Rows[i]["IsScreen"].ToString().ToLower() == "yes"))
                        {
                            str = str + ", DeviceType ='Screen' ";
                        }

                        if ((dtM.Rows[i]["IsSanitizer"].ToString() == "1") || (dtM.Rows[i]["IsSanitizer"].ToString().ToLower() == "yes"))
                        {
                            str = str + ",DeviceType ='Sanitizer',CommunicationType='TTL',  TotalShot=5000, DispenserAlert='80,90,100'  ";
                        }

                        str = str + " , AlertEmail='" + dtM.Rows[i]["DispenserAlertEmail"].ToString().Trim() + "' ";


                        str = str + " Where tokenid = " + dtM.Rows[i]["TokenId"] + " ";
                        SqlCmd = new SqlCommand(str, conSql);
                        SqlCmd.CommandType = CommandType.Text;
                        SqlCmd.ExecuteNonQuery();
                        SqlCmd.Dispose();
                    }
                }



                conSql.Close();
                Result.Responce = "1";
                Result.message = "Saved";
                return Result;

            }
            catch (Exception ex)
            {
                var g = ex.Message;
                conSql.Close();
                Result.Responce = "0";
                Result.message = g;
                return Result;
            }
        }


        public ResResponce SaveSF_New(ReqSF_New data)
        {
            ResResponce clsResult = new ResResponce();
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {

                con.Open();
                string tid = "";
                foreach (var iToken in data.TokenList)
                {
                    if (tid == "")
                    {
                        tid = iToken.tokenId.ToString();
                    }
                    else
                    {
                        tid = tid + "," + iToken.tokenId.ToString();
                    }
                }
                if (data.ScheduleType != "Normal")
                {
                    string st = "";
                    st = "delete from tbSpecialPlaylistSchedule where pSchId in(select pSchId from tbSpecialPlaylistSchedule_Token where tokenid in (" + tid + "))";
                    cmd = new SqlCommand(st, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    st = "delete from tbSpecialPlaylistSchedule_Weekday where pSchId in(select pSchId from tbSpecialPlaylistSchedule_Token where tokenid in (" + tid + "))";
                    cmd = new SqlCommand(st, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    st = "delete from tbSpecialPlaylistSchedule_Token where tokenid in (" + tid + ")";
                    cmd = new SqlCommand(st, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                foreach (var iToken in data.TokenList)
                {
                    foreach (var item_PL in data.lstPlaylist)
                    {
                        var h = item_PL.eTime.Split(':');

                        if (h[0] == "00")
                        {
                            item_PL.eTime = "23:59";
                        }
                        var wList = item_PL.wId.Split(',');

                        if (iToken.schType == "Normal")
                        {
                            foreach (var lstWeek in wList)
                            {
                                string strTit = "CheckTokenSchedule 0," + iToken.tokenId + "," + lstWeek + "," +
                                        " '" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(item_PL.sTime).AddMinutes(1)) + "'," +
                                        " '" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(item_PL.eTime).AddMinutes(-1)) + "'";
                                cmd = new SqlCommand(strTit, con);
                                cmd.CommandType = System.Data.CommandType.Text;
                                ad = new SqlDataAdapter(cmd);
                                DataTable ds = new DataTable();
                                ad.Fill(ds);
                                if (ds.Rows.Count > 0)
                                {
                                    for (int iTit = 0; iTit <= ds.Rows.Count - 1; iTit++)
                                    {
                                        cmd = new SqlCommand();
                                        cmd.Connection = con;
                                        strTit = "";
                                        strTit = "delete from tbSpecialPlaylistSchedule_Token where ";
                                        strTit = strTit + " pSchId = " + ds.Rows[iTit]["pSchId"] + " ";
                                        strTit = strTit + " and tokenid=" + iToken.tokenId + " ";
                                        cmd.CommandText = strTit;

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                        //=========================== Save Main Data
                        cmd = new SqlCommand("spSaveSpecialPlaylistSchedule", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                        cmd.Parameters["@pSchId"].Value = 0;

                        cmd.Parameters.Add(new SqlParameter("@pVersion", SqlDbType.VarChar));
                        cmd.Parameters["@pVersion"].Value = "c";

                        cmd.Parameters.Add(new SqlParameter("@dfClientId", SqlDbType.BigInt));
                        cmd.Parameters["@dfClientId"].Value = data.CustomerId;

                        cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                        cmd.Parameters["@splPlaylistId"].Value = item_PL.splId;

                        cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime));
                        cmd.Parameters["@StartTime"].Value = string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(item_PL.sTime));

                        cmd.Parameters.Add(new SqlParameter("@EndTime", SqlDbType.DateTime));
                        cmd.Parameters["@EndTime"].Value = string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(item_PL.eTime));

                        cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                        cmd.Parameters["@FormatId"].Value = data.FormatId;

                        cmd.Parameters.Add(new SqlParameter("@PercentageValue", SqlDbType.Int));
                        cmd.Parameters["@PercentageValue"].Value = item_PL.PercentageValue;

                        cmd.Parameters.Add(new SqlParameter("@volume", SqlDbType.Int));
                        cmd.Parameters["@volume"].Value = item_PL.volume;

                        Int32 rtPschId = Convert.ToInt32(cmd.ExecuteScalar());
                        //==========================================
                        cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "delete from tbSpecialPlaylistSchedule_Weekday where pSchId=" + rtPschId;
                        cmd.ExecuteNonQuery();

                        //=============================== Save Week
                        foreach (var lstWeek in wList)
                        {
                            cmd = new SqlCommand("spSaveSpecialPlaylistSchedule_Week", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                            cmd.Parameters["@pSchId"].Value = rtPschId;

                            cmd.Parameters.Add(new SqlParameter("@wId", SqlDbType.Int));
                            cmd.Parameters["@wId"].Value = lstWeek;

                            cmd.Parameters.Add(new SqlParameter("@IsAllWeek", SqlDbType.Int));
                            cmd.Parameters["@IsAllWeek"].Value = 0;

                            cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                            cmd.Parameters["@FormatId"].Value = data.FormatId;
                            cmd.ExecuteNonQuery();
                        }
                        //=========================================
                        cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "delete from tbSpecialPlaylistSchedule_Token where tokenid=" + iToken.tokenId + " and pSchId= " + rtPschId + " ";
                        cmd.ExecuteNonQuery();

                        //====================== Save Token Detail
                        cmd = new SqlCommand("spSaveSpecialPlaylistSchedule_Token", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pSchId", SqlDbType.BigInt));
                        cmd.Parameters["@pSchId"].Value = rtPschId;

                        cmd.Parameters.Add(new SqlParameter("@tokenId", SqlDbType.BigInt));
                        cmd.Parameters["@tokenId"].Value = iToken.tokenId;

                        cmd.Parameters.Add(new SqlParameter("@IsAllToken", SqlDbType.Int));
                        cmd.Parameters["@IsAllToken"].Value = 0;

                        cmd.Parameters.Add(new SqlParameter("@FormatId", SqlDbType.BigInt));
                        cmd.Parameters["@FormatId"].Value = data.FormatId;

                        cmd.Parameters.Add(new SqlParameter("@DfClientid", SqlDbType.BigInt));
                        cmd.Parameters["@DfClientid"].Value = data.CustomerId;

                        cmd.Parameters.Add(new SqlParameter("@splPlaylistId", SqlDbType.BigInt));
                        cmd.Parameters["@splPlaylistId"].Value = item_PL.splId;
                        cmd.ExecuteNonQuery();
                        //========================================

                    }
                }
                con.Close();
                clsResult.Responce = "1";
                return clsResult;
            }
            catch (Exception ex)
            {
                clsResult.Responce = "0";
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return clsResult;
            }
        }

        public ResResponce DeleteTitleOwn(ReqDeleteTitleOwn data)
        {
            ResResponce result = new ResResponce();
            List<string> TokenArray = new List<string>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                string strDel = "";
                string tid = data.tid;
                var TitlePlaylists = new ArrayList();
                strDel = "";
                if (data.ForceType == "No")
                {
                    strDel = "select distinct splPlaylistName from tbSpecialPlaylists where splplaylistid in (select distinct splplaylistid from tbSpecialPlaylists_Titles where titleid in(" + tid + ") )";
                    cmd = new SqlCommand(strDel, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    DataTable ds = new DataTable();
                    ad.Fill(ds);
                    ad.Dispose();
                    cmd.Dispose();
                    con.Close();
                    if (ds.Rows.Count > 0)
                    {
                        for (int iTit = 0; iTit <= ds.Rows.Count - 1; iTit++)
                        {
                            TokenArray.Add(ds.Rows[iTit]["splPlaylistName"].ToString());
                        }
                        result.Responce = "2";
                        result.TitlePlaylists = TokenArray;
                        return result;
                    }
                }



                strDel = "";
                strDel = "delete from Titles where TitleID in(" + tid + ") ";
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                strDel = "";
                strDel = "delete from tbSpecialPlaylists_Titles where TitleID in(" + tid + ") ";
                cmd = new SqlCommand(strDel, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public List<ResCustomerWithKey> FillCustomerWithKey(ReqComboQuery data)
        {
            List<ResCustomerWithKey> lstResult = new List<ResCustomerWithKey>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand(data.Query, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstResult.Add(new ResCustomerWithKey()
                    {
                        Id = ds.Tables[0].Rows[i]["id"].ToString(),
                        DisplayName = ds.Tables[0].Rows[i]["DisplayName"].ToString(),
                        apikey = ds.Tables[0].Rows[i]["apikey"].ToString(),
                        check = false,
                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }


        public ResResponce SaveCopyContent(ReqTransferContent data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                string str = "";
                string tId = "";

                if (data.FolderId == "0")
                {
                    str = "";
                    str = "update tbFolder set dfclientId  = " + data.dfClientId + " where folderId =" + data.FromFolderId;
                    cmd = new SqlCommand(str, con);
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    data.FolderId = data.FromFolderId;

                }
                foreach (var tlist in data.TitleList)
                {
                    if (tId == "")
                    {
                        tId = tlist;
                    }
                    else
                    {
                        tId = tId + ',' + tlist;
                    }
                }
                if (tId != "")
                {


                    str = "";
                    str = "select Titles.*, Artists.Name as aName , Albums.Name as alName from Titles  inner join Artists on Artists.ArtistID= Titles.ArtistID inner join Albums on Albums.AlbumID= Titles.AlbumID " +
                        " where titleid in (" + tId + ")";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    DataTable dt = new DataTable();
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(dt);
                    cmd.Dispose();
                    ad.Dispose();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (data.dfClientId.ToString() == dt.Rows[i]["dfclientid"].ToString())
                        {
                            str = "";
                            str = "update Titles set folderId=" + data.FolderId + " where TitleID = " + dt.Rows[i]["Titleid"];
                            cmd = new SqlCommand(str, con);
                            cmd.CommandText = str;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }

                        else
                        {
                            cmd = new SqlCommand("InsertContent", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@TiTleTiTle", SqlDbType.VarChar));
                            cmd.Parameters["@TiTleTiTle"].Value = dt.Rows[i]["Title"];

                            cmd.Parameters.Add(new SqlParameter("@TitleArtistName", SqlDbType.VarChar));
                            cmd.Parameters["@TitleArtistName"].Value = dt.Rows[i]["aName"];

                            cmd.Parameters.Add(new SqlParameter("@AlbumName", SqlDbType.VarChar));
                            cmd.Parameters["@AlbumName"].Value = dt.Rows[i]["alName"];

                            cmd.Parameters.Add(new SqlParameter("@titlecategoryid", SqlDbType.BigInt));
                            cmd.Parameters["@titlecategoryid"].Value = 4;

                            cmd.Parameters.Add(new SqlParameter("@titleSubcategoryid", SqlDbType.VarChar));
                            cmd.Parameters["@titleSubcategoryid"].Value = 22;

                            cmd.Parameters.Add(new SqlParameter("@Time", SqlDbType.VarChar));
                            cmd.Parameters["@Time"].Value = dt.Rows[i]["Time"];

                            cmd.Parameters.Add(new SqlParameter("@AlbumLabel", SqlDbType.VarChar));
                            cmd.Parameters["@AlbumLabel"].Value = "0";

                            cmd.Parameters.Add(new SqlParameter("@CatalogCode", SqlDbType.VarChar));
                            cmd.Parameters["@CatalogCode"].Value = "0";

                            cmd.Parameters.Add(new SqlParameter("@titleYear", SqlDbType.Int));
                            cmd.Parameters["@titleYear"].Value = 0;


                            cmd.Parameters.Add(new SqlParameter("@GenreId", SqlDbType.Int));
                            cmd.Parameters["@GenreId"].Value = dt.Rows[i]["GenreId"];

                            cmd.Parameters.Add(new SqlParameter("@tempo", SqlDbType.VarChar));
                            cmd.Parameters["@tempo"].Value = "Mid";


                            cmd.Parameters.Add(new SqlParameter("@mType", SqlDbType.VarChar));
                            cmd.Parameters["@mType"].Value = dt.Rows[i]["mediatype"];

                            cmd.Parameters.Add(new SqlParameter("@acategory", SqlDbType.VarChar));
                            cmd.Parameters["@acategory"].Value = dt.Rows[i]["aCategory"];

                            cmd.Parameters.Add(new SqlParameter("@language", SqlDbType.VarChar));
                            cmd.Parameters["@language"].Value = dt.Rows[i]["language"];

                            cmd.Parameters.Add(new SqlParameter("@isRF", SqlDbType.VarChar));
                            cmd.Parameters["@isRF"].Value = dt.Rows[i]["IsRoyaltyFree"];

                            cmd.Parameters.Add(new SqlParameter("@isrc", SqlDbType.VarChar));
                            cmd.Parameters["@isrc"].Value = "";

                            cmd.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.VarChar));
                            cmd.Parameters["@FileSize"].Value = dt.Rows[i]["FileSize"];

                            cmd.Parameters.Add(new SqlParameter("@dfclientid", SqlDbType.BigInt));
                            cmd.Parameters["@dfclientid"].Value = data.dfClientId;

                            cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt));
                            cmd.Parameters["@folderid"].Value = data.FolderId;

                            cmd.Parameters.Add(new SqlParameter("@dbType", SqlDbType.VarChar));
                            cmd.Parameters["@dbType"].Value = dt.Rows[i]["dbtype"];

                            cmd.Parameters.Add(new SqlParameter("@IsAnnouncement", SqlDbType.Int));
                            cmd.Parameters["@IsAnnouncement"].Value = "0";

                            Int32 Title_Id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                            string ext = "";
                            if (dt.Rows[i]["mediatype"].ToString().Trim() == "Image")
                            {
                                ext = ".jpg";
                            }
                            if (dt.Rows[i]["mediatype"].ToString().Trim() == "Video")
                            {
                                ext = ".mp4";
                            }
                            var fName_old = "~/mp3files/" + dt.Rows[i]["titleid"].ToString() + "" + ext;
                            var filePath = System.Web.Hosting.HostingEnvironment.MapPath(fName_old);
                            var fileInfo = new FileInfo(filePath);

                            var fName_New = Title_Id.ToString() + "" + ext;
                            var filePath_new = HttpContext.Current.Server.MapPath("~/mp3files/" + fName_New);
                            fileInfo.CopyTo(filePath_new);
                        }
                    }
                }
                con.Close();
                result.Responce = "1";
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

        public ResResponce FindToken(ReqFindToken data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                string str = "";
                str = "FillCustomer " + data.IsAdmin + "," + data.ClientId + ", '" + data.DbType + "'";
                cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                ad.Dispose();
                cmd.Dispose();
                string cid = "";
                if (ds.Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        if (cid == "")
                        {
                            cid = ds.Rows[i]["Id"].ToString();
                        }
                        else
                        {
                            cid = cid + "," + ds.Rows[i]["Id"].ToString();
                        }
                    }

                    //str = "select distinct tokenid from AMPlayerTokens where tokenid =" + data.tokenid + " and clientid in(" + cid + ")";
                    str = "select distinct tokenid, (select clientname from dfclients where dfclients.DFClientID= AMPlayerTokens.ClientID) as clientname," +
                        " (select top 1 (playdate+' '+playDTP)  from tbTokenPlayedSongs_Live where tokenid=AMPlayerTokens.tokenid order by playdate desc, playDTP desc) as  laststatus " +
                        " from AMPlayerTokens where tokenid =" + data.tokenid + " and clientid in(" + cid + ")";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    ad.Dispose();
                    cmd.Dispose();
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {

                        result.Responce = "1";
                        result.status = string.Format("{0:dd-MMM-yyyy HH:mm:ss}", dt.Rows[0]["laststatus"]);
                        result.message = dt.Rows[0]["clientname"].ToString();
                        return result;
                    }
                }
                con.Close();
                result.Responce = "0";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public ResResponce UpdateExpiryDate_Template_Creator(ReqUpdateExpiryDate_Template_Creator data)
        {
            ResResponce result = new ResResponce();
            ResClientTemplateRegsiter resAPI = new ResClientTemplateRegsiter();

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string aKey = "", Template_Login_Email = "";
                string str = "select isnull(apikey,'') as aKey, Template_Login_Email  from DFClients where DFClientID =" + data.dfClientId;
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    if (ds.Rows[0]["aKey"].ToString() != "")
                    {
                        aKey = ds.Rows[0]["aKey"].ToString();
                        Template_Login_Email = ds.Rows[0]["Template_Login_Email"].ToString();
                    }
                }

                if (string.IsNullOrEmpty(aKey) == true)
                {
                    con.Close();
                    result.Responce = "0";
                    return result;
                }


                DateTime dt = new DateTime();
                DateTime dt2 = new DateTime();

                string h = "";
                string IsTemplateActive = "1";
                if (data.status == "Disable")
                {
                    data.ExpiryDate = DateTime.Now.ToString();
                    IsTemplateActive = "0";
                }
                if (data.status == "Active")
                {
                     
                    IsTemplateActive = "1";
                }
                dt2 = Convert.ToDateTime(string.Format("{0:yyyy-mm-dd}", data.ExpiryDate));
                h = dt2.Date.ToString("yyyy-MM-dd");
                dt = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", h));
                if (dt.Date < DateTime.Now.Date)
                {

                }

                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var unixDateTime = (dt.ToUniversalTime() - epoch).TotalSeconds;

                var client = new RestClient("https://content.nusign.eu/api/modify-trial-end-date?key=f72c963e19c6ecf88b5e17a8d51ebbf0&email=" + Template_Login_Email.Trim());
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("trialEndDate", unixDateTime);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                IRestResponse response = client.Execute(request);

                resAPI = JsonConvert.DeserializeObject<ResClientTemplateRegsiter>(response.Content);
                if (resAPI.status == "success")
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    str = "";
                    str = "update DFClients set IsTemplateActive=" + IsTemplateActive + " where DFClientID = " + data.dfClientId;
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Close();

                    result.Responce = "1";
                    con.Close();
                    return result;
                }
                con.Close();
                result.Responce = "0";
                return result;
            }
            catch (Exception ex)
            {
                con.Close();
                result.Responce = "0";
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public ResResponce SaveRebootTime(ReqRebootTime data)
        {
            ResResponce result = new ResResponce();
            SqlConnection conMain = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
            try
            {
                DateTimeFormatInfo fi = new DateTimeFormatInfo();
                fi.AMDesignator = "AM";
                fi.PMDesignator = "PM";
                string tid = "";
                foreach (var TokenId in data.TokenList)
                {
                    if (TokenId != "0")
                    {
                        if (tid == "")
                        {
                            tid = TokenId;
                        }
                        else
                        {
                            tid = tid + ',' + TokenId;
                        }

                    }

                }

                if (tid != "")
                {
                    if (conMain.State == ConnectionState.Closed)
                    {
                        conMain.Open();
                    }
                    var k = "01-01-1900 " + string.Format(fi, "{0:HH:mm:ss}", Convert.ToDateTime(data.startTime));

                    string strDel = "";
                    strDel = "";
                    strDel = "update AMPlayerTokens set RebootTime='" + k + "' where tokenId in (" + tid + ")";
                    SqlCommand cmd = new SqlCommand(strDel, conMain);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    result.Responce = "1";
                }
                conMain.Close();
                return result;
            }
            catch (Exception ex)
            {
                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                result.Responce = "0";
                conMain.Close();
                return result;
            }
        }


        public List<ResComboQuery> GetClientFolder(ReqGetClientFolder data)
        {
            List<ResComboQuery> lstResult = new List<ResComboQuery>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                var qry = "select folderId as Id, foldername as DisplayName ,isnull(IsPromoFolder,0) as IsPromoFolder from tbFolder ";
                qry = qry + " where dfclientId=" + data.ClientId + " ";
                qry = qry + " order by foldername ";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstResult.Add(new ResComboQuery()
                    {
                        Id = ds.Tables[0].Rows[i]["id"].ToString(),
                        DisplayName = ds.Tables[0].Rows[i]["DisplayName"].ToString(),
                        check = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsPromoFolder"]),
                    });
                }
                con.Close();
                return lstResult;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstResult;
            }
        }


        public ResResponce ReplaceFolderContent(ReqReplaceFolderContent data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);

            try
            {
                var qry = "getFolderContent " + data.ClientId + " , " + data.FolderId;
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = System.Data.CommandType.Text;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ad.Fill(ds);
                DataTable dtAll = new DataTable();
                DataTable dtPlaylistContent = new DataTable();
                DataTable dtNewContent = new DataTable();
                DataTable dtDeleteContent = new DataTable();

                DataTable dt = new DataTable();
                dt.Columns.Add("splPlaylistId", typeof(int));
                dt.Columns.Add("titleId", typeof(int));
                dt.Columns.Add("srNo", typeof(int));

                dtAll = ds.Tables[0];
                dtPlaylistContent = ds.Tables[1];
                dtNewContent = ds.Tables[2];
                dtDeleteContent = ds.Tables[3];
                string del_titleId = "";
                if (dtNewContent.Rows.Count == 0)
                {
                    result.Responce = "2";
                    con.Close();
                    return result;
                }
                for (int i = 0; i < dtDeleteContent.Rows.Count; i++)
                {
                    if (del_titleId == "")
                    {
                        del_titleId = dtDeleteContent.Rows[i]["titleId"].ToString();
                    }
                    else
                    {
                        del_titleId = del_titleId +','+ dtDeleteContent.Rows[i]["titleId"].ToString();
                    }
                }

                string strDel = "";
                strDel = "";
                strDel = "delete from tbSpecialPlaylists_Titles where titleid in (" + del_titleId + ")";
                SqlCommand cmdDel = new SqlCommand(strDel, con);
                cmdDel.CommandType = CommandType.Text;
                cmdDel.ExecuteNonQuery();
                cmdDel.Dispose();

                strDel = "";
                strDel = "delete from Titles where titleid in (" + del_titleId + ")";
                cmdDel = new SqlCommand(strDel, con);
                cmdDel.CommandType = CommandType.Text;
                cmdDel.ExecuteNonQuery();
                cmdDel.Dispose();

                for (int iPl = 0; iPl < dtPlaylistContent.Rows.Count; iPl++)
                {       
                    for (int iCont = 0; iCont < dtNewContent.Rows.Count; iCont++)
                    {
                        int sr = 0;
                        sr++;
                        DataRow nr = dt.NewRow();
                        nr["splPlaylistId"] = dtPlaylistContent.Rows[iPl]["splPlaylistId"];
                        nr["titleId"] = dtNewContent.Rows[iCont]["titleId"];
                        nr["srNo"] = sr;
                        dt.Rows.Add(nr);
                    }
                }

                 

                if (dt.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        SqlBulkCopyColumnMapping mapID =
                         new SqlBulkCopyColumnMapping("splPlaylistId", "splPlaylistId");
                        bulkCopy.ColumnMappings.Add(mapID);

                        SqlBulkCopyColumnMapping mapMumber =
                            new SqlBulkCopyColumnMapping("titleId", "titleId");
                        bulkCopy.ColumnMappings.Add(mapMumber);

                        SqlBulkCopyColumnMapping mapName =
                         new SqlBulkCopyColumnMapping("srNo", "srNo");
                        bulkCopy.ColumnMappings.Add(mapName);

                        bulkCopy.DestinationTableName = "dbo.tbSpecialPlaylists_Titles";

                        if (con.State == ConnectionState.Open) con.Close();
                        con.Open();
                        bulkCopy.WriteToServer(dt);
                        con.Close();

                    }
                }
                result.Responce = "1";
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                result.Responce = "0";
                result.ContentType = ex.Message.ToString();
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }


        public ResResponce SaveUpdateOfflineAlert(ReqOfflineAlert data)
        {
            ResResponce Result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string tid = "";
                con.Open();
                DataTable dtInsert = new DataTable();
                dtInsert.Columns.Add("userid", typeof(int));
                dtInsert.Columns.Add("tokenid", typeof(int));
                SqlCommand cmd = new SqlCommand("SaveUpdateOfflineAlert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                cmd.Parameters["@id"].Value = data.id;

                cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar));
                cmd.Parameters["@email"].Value = data.email;

                cmd.Parameters.Add(new SqlParameter("@interval", SqlDbType.Int));
                cmd.Parameters["@interval"].Value = data.interval;

                cmd.Parameters.Add(new SqlParameter("@dfClientid", SqlDbType.Int));
                cmd.Parameters["@dfClientid"].Value = data.dfClientid;
                

                Int32 ReturnId = Convert.ToInt32(cmd.ExecuteScalar());
                foreach (var iToken in data.lstToken)
                {
                    DataRow nr = dtInsert.NewRow();
                    nr["userid"] = ReturnId;
                    nr["tokenid"] = iToken;
                    dtInsert.Rows.Add(nr);
                }
                if (dtInsert.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        SqlBulkCopyColumnMapping userid =
                         new SqlBulkCopyColumnMapping("userid", "userid");
                        bulkCopy.ColumnMappings.Add(userid);

                        SqlBulkCopyColumnMapping tokenid =
                        new SqlBulkCopyColumnMapping("tokenid", "tokenid");
                        bulkCopy.ColumnMappings.Add(tokenid);

                        bulkCopy.DestinationTableName = "dbo.tbOfflineAlert_Token";
                        bulkCopy.WriteToServer(dtInsert);
                    }
                }

                
                con.Close();
                Result.Responce = "1";
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }

        public ReqOfflineAlert EditOfflineUser(ReqUserInfo data)
        {
            ReqOfflineAlert Result = new ReqOfflineAlert();
            List<ResTokenInfo> lstTokenInfo = new List<ResTokenInfo>();
            List<string> TokenArray = new List<string>();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter ad = new SqlDataAdapter();
                con.Open();
                string sQr = "GetOfflineAlertInfo " + data.UserId + "";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;
                ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);
                if (ds.Rows.Count > 0)
                {
                    Result.Responce = "1";
                    Result.id = ds.Rows[0]["id"].ToString();
                    Result.email = ds.Rows[0]["email"].ToString();
                    Result.interval = ds.Rows[0]["interval"].ToString();
                    Result.dfClientid = ds.Rows[0]["dfClientid"].ToString();

                    sQr = "select distinct tokenid from tbOfflineAlert_Token where userid = " + data.UserId + "";
                    cmd = new SqlCommand(sQr, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    DataTable dsUserToken = new DataTable();
                    ad.Fill(dsUserToken);




                    cmd = new SqlCommand("GetTokenInfo " + Result.dfClientid, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    ad = new SqlDataAdapter(cmd);
                    DataTable dsTokenAll = new DataTable();
                    ad.Fill(dsTokenAll);

                    for (int i = 0; i < dsTokenAll.Rows.Count; i++)
                    {
                        bool iCheck = dsUserToken.Select().ToList().Exists(row => row["tokenid"].ToString() == dsTokenAll.Rows[i]["tokenid"].ToString());
                        if (iCheck == true)
                        {
                            TokenArray.Add(dsTokenAll.Rows[i]["tokenid"].ToString());
                        }
                        lstTokenInfo.Add(new ResTokenInfo()
                        {
                            tokenid = dsTokenAll.Rows[i]["tokenid"].ToString(),
                            tokenCode = dsTokenAll.Rows[i]["tNo"].ToString(),
                            Name = dsTokenAll.Rows[i]["PersonName"].ToString(),
                            location = dsTokenAll.Rows[i]["Location"].ToString(),
                            city = dsTokenAll.Rows[i]["CityName"].ToString(),
                            countryName = dsTokenAll.Rows[i]["CountryName"].ToString(),
                            playerType = dsTokenAll.Rows[i]["PlType"].ToString(),
                            LicenceType = dsTokenAll.Rows[i]["LiType"].ToString(),
                            tInfo = dsTokenAll.Rows[i]["tInfo"].ToString(),
                            check = iCheck,
                        });
                    }
                    Result.lstTokenInfo = lstTokenInfo;
                    Result.lstToken = TokenArray;
                }
                else
                {
                    Result.Responce = "0";
                }
                con.Close();
                return Result;
            }
            catch (Exception ex)
            {
                con.Close();
                Result.Responce = "0";
                HttpContext.Current.Response.StatusCode = 1;
                return Result;
            }
        }

        public List<ResUser> FillOfflineAlertList(ReqTokenInfo data)
        {
            List<ResUser> lstUser = new List<ResUser>();

            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                con.Open();
                string sQr = "select id, email, interval from tbOfflineUser where dfClientid= " + data.clientId + " order by email";
                cmd = new SqlCommand(sQr, con);
                cmd.CommandType = System.Data.CommandType.Text;

                ad = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                ad.Fill(ds);

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    lstUser.Add(new ResUser()
                    {
                        id = ds.Rows[i]["id"].ToString(),
                        UserName1 = ds.Rows[i]["email"].ToString(),
                        Password1 = ds.Rows[i]["interval"].ToString(),
                    });
                }
                con.Close();
                return lstUser;
            }
            catch (Exception ex)
            {
                con.Close();
                HttpContext.Current.Response.StatusCode = 1;
                return lstUser;
            }
        }
        public ResResponce SavePlaylistTokenVolume(ReqPlaylistTokenVolume Data)
        {
            ResResponce result = new ResResponce();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Panel"].ConnectionString);
            try
            {
                string str = "";
                con.Open();
                string tokenid = "";
                foreach (var data in Data.tokenIds)
                {
                    if (tokenid == "")
                    {
                        tokenid = data;
                    }
                    else
                    {
                        tokenid = tokenid + "," + data;
                    }
                }
                if (tokenid != "")
                {
                    str = "update tbSpecialPlaylistSchedule set volume=" + Data.volume + " where pSchId in(select distinct pSchId from tbSpecialPlaylistSchedule_Token where tokenid=" + tokenid + " and splPlaylistId=" + Data.pid + ")";
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                con.Close();
                result.Responce = "1";
                return result;

            }
            catch (Exception ex)
            {
                con.Close();

                var g = ex.Message;
                HttpContext.Current.Response.StatusCode = 1;
                return result;
            }
        }

    }
}
//spGetAdvtAdmin_NativeOnly_New