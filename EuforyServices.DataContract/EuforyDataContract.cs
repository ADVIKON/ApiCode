using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EuforyServices.DataContract
{
    #region Genre
    [DataContract]
    public class ResponceGenre
    {
        [DataMember]
        public int SubcategoryId { get; set; }
        [DataMember]
        public string SubCategoryName { get; set; }
    }

    #endregion

    #region GenreTitles
    [DataContract]
    public class ResponceGenreTitles
    {
        [DataMember]
        public int TitleId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string ArtistName { get; set; }
        [DataMember]
        public string SongUrl { get; set; }
    }

    [DataContract]
    public class DataGenreTitles
    {
        [DataMember]
        public int SubcategoryId { get; set; }
    }
    #endregion

    #region Stream
    [DataContract]
    public class ResponceStream
    {
        [DataMember]
        public string StreamName { get; set; }
        [DataMember]
        public string StreamLink { get; set; }
        [DataMember]
        public string StreamImgPath { get; set; }
        [DataMember]
        public string StreamId { get; set; }
        [DataMember]
        public bool check { get; set; }
    }

    [DataContract]
    public class ResponceStreamLinux
    {
        [DataMember]
        public string StreamName { get; set; }
        [DataMember]
        public string StreamLink { get; set; }

        [DataMember]
        public string StreamId { get; set; }
    }

    #endregion

    #region UserToken
    [DataContract]
    public class ResponceUserToen
    {
        [DataMember]
        public string Response { get; set; }
    }

    [DataContract]
    public class DataClientToken
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string TokenNo { get; set; }
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public string PlayerType { get; set; }
    }
    #endregion

    #region UserToken
    [DataContract]
    public class ResponceUserRights
    {
        [DataMember]
        public string Response { get; set; }

        [DataMember]
        public string LeftDays { get; set; }
        [DataMember]
        public string TokenId { get; set; }
        [DataMember]
        public string dfClientId { get; set; }
        [DataMember]
        public int Cityid { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        public int IsStopControl { get; set; }
        [DataMember]
        public string MediaType { get; set; }
        [DataMember]
        public string FcmId { get; set; }
        [DataMember]
        public string scheduleType { get; set; }
        [DataMember]
        public string LogoId { get; set; }
        [DataMember]
        public string IsIndicatorActive { get; set; }
        [DataMember]
        public string Rotation { get; set; }
        [DataMember]
        public bool IsDemoToken { get; set; }
        [DataMember]
        public int TotalShot { get; set; }
        [DataMember]
        public string DispenserAlert { get; set; }
        [DataMember]
        public string DeviceType { get; set; }
        [DataMember]
        public string FireAlertId { get; set; }
        [DataMember]
        public string FireAlertUrl { get; set; }
    }

    [DataContract]
    public class DataUserRights
    {
        [DataMember]
        public string DeviceId { get; set; }
    }

    #endregion

    #region MiddleImage
    [DataContract]
    public class ResponceMiddleImage
    {
        [DataMember]
        public string ImgPath { get; set; }
    }

    #endregion



    #region SplPlaylist
    [DataContract]
    public class ResponceSplSplaylist
    {
        [DataMember]
        public int pScid { get; set; }
        [DataMember]
        public Int32 dfclientid { get; set; }
        [DataMember]
        public Int32 splPlaylistId { get; set; }
        [DataMember]
        public string splPlaylistName { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public Int32 FormatId { get; set; }
        [DataMember]
        public int IsSeprationActive { get; set; }
        [DataMember]
        public int IsFadingActive { get; set; }
        [DataMember]
        public string IsMute { get; set; }
        [DataMember]
        public int IsSeprationActive_New { get; set; }
        [DataMember]
        public string PercentageValue { get; set; }
    }

    [DataContract]
    public class DataSplPlaylist
    {
        [DataMember]
        public int WeekNo { get; set; }
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public Int32 DfClientId { get; set; }
    }
    #endregion



    #region SplPlaylistTitle
    [DataContract]
    public class ResponceSplSplaylistTitle
    {
        [DataMember]
        public Int32 splPlaylistId { get; set; }
        [DataMember]
        public Int32 titleId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string tTime { get; set; }
        [DataMember]
        public Int32 ArtistID { get; set; }
        [DataMember]
        public string arName { get; set; }
        [DataMember]
        public Int32 AlbumID { get; set; }
        [DataMember]
        public string alName { get; set; }
        [DataMember]
        public string TitleUrl { get; set; }
        [DataMember]
        public int srno { get; set; }
        [DataMember]
        public string TitleUrl2 { get; set; }
        [DataMember]
        public string FileSize { get; set; }
        [DataMember]
        public int TimeInterval { get; set; }
        [DataMember]
        public bool IsLoop { get; set; }
    }

    [DataContract]
    public class DataSplPlaylistTile
    {
        [DataMember]
        public Int32 splPlaylistId { get; set; }
    }
    #endregion


    #region Prayer Timing
    [DataContract]
    public class ResponcePrayerTiming
    {
        [DataMember]
        public string Response { get; set; }

        [DataMember]
        public int pId { get; set; }

        [DataMember]
        public string sDate { get; set; }
        [DataMember]
        public string eDate { get; set; }
        [DataMember]
        public string sTime { get; set; }
        [DataMember]
        public string eTime { get; set; }
    }

    [DataContract]
    public class DataPrayerTiming
    {
        [DataMember]
        public int Month { get; set; }
        [DataMember]
        public int Cityid { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        public int TokenId { get; set; }
    }
    #endregion


    #region Advt Schdeule
    [DataContract]
    public class ResponceAdvt
    {
        [DataMember]
        public string Response { get; set; }

        [DataMember]
        public int AdvtId { get; set; }
        [DataMember]
        public string AdvtName { get; set; }
        [DataMember]
        public string PlayingType { get; set; }
        [DataMember]
        public string sDate { get; set; }
        [DataMember]
        public string eDate { get; set; }
        [DataMember]
        public string AdvtFilePath { get; set; }
        [DataMember]
        public int IsTime { get; set; }
        [DataMember]
        public string sTime { get; set; }
        [DataMember]
        public int IsMinute { get; set; }
        [DataMember]
        public int TotalMinutes { get; set; }
        [DataMember]
        public int IsSong { get; set; }
        [DataMember]
        public int TotalSongs { get; set; }
        [DataMember]
        public int SrNo { get; set; }
    }

    [DataContract]
    public class DataAdvtSch
    {
        [DataMember]
        public string CurrentDate { get; set; }
        [DataMember]
        public int DfClientId { get; set; }
        [DataMember]
        public int WeekNo { get; set; }
        [DataMember]
        public int CityId { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        public int TokenId { get; set; }
    }
    #endregion

    #region Advt Schdeule Linux Only
    [DataContract]
    public class ResponceAdvtLinux
    {
        [DataMember]
        public string Response { get; set; }

        [DataMember]
        public int AdvtId { get; set; }
        [DataMember]
        public string AdvtName { get; set; }
        [DataMember]
        public string PlayingType { get; set; }
        [DataMember]
        public string AdvtFilePath { get; set; }
        [DataMember]
        public int IsTime { get; set; }
        [DataMember]
        public string sTime { get; set; }
        [DataMember]
        public int IsMinute { get; set; }
        [DataMember]
        public int TotalMinutes { get; set; }
        [DataMember]
        public int IsSong { get; set; }
        [DataMember]
        public int TotalSongs { get; set; }
        [DataMember]
        public int SrNo { get; set; }
    }

    [DataContract]
    public class DataAdvtSchLinux
    {
        [DataMember]
        public int MonthNo { get; set; }
        [DataMember]
        public int DfClientId { get; set; }
        [DataMember]
        public int WeekNo { get; set; }
        [DataMember]
        public int CityId { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        public int TokenId { get; set; }
    }
    #endregion




    #region Token Played Songs Status
    [DataContract]
    public class ResponcePlayedSong
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public List<SongsArray> SongArray { get; set; }

    }
    [DataContract]
    public class ResponceDownloadingProcess
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string errorMsg { get; set; }

    }

    [DataContract]
    public class SongsArray
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string returnPlayedDateTime { get; set; }
        [DataMember]
        public string returnTitleId { get; set; }
    }

    [DataContract]
    public class AdsArray
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string returnPlayedDate { get; set; }
        [DataMember]
        public string returnPlayedTime { get; set; }
        [DataMember]
        public string returnAdvtId { get; set; }
    }

    [DataContract]
    public class DataPlayedSong
    {
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public string PlayedDateTime { get; set; }
        [DataMember]
        public Int32 TitleId { get; set; }
        [DataMember]
        public Int32 ArtistId { get; set; }
        [DataMember]
        public Int32 splPlaylistId { get; set; }

    }
    #endregion

    #region Played Advertisement Status
    [DataContract]
    public class ResponcePlayedAdvt
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public List<SongsArray> SongArray { get; set; }

    }

    [DataContract]
    public class DataPlayedAdvt
    {
        [DataMember]
        public Int32 TokenId { get; set; }

        [DataMember]
        public Int32 AdvtId { get; set; }
        [DataMember]
        public string PlayedDate { get; set; }
        [DataMember]
        public string PlayedTime { get; set; }

    }
    [DataContract]
    public class DataPlayedAdvtNew
    {
        [DataMember]
        public Int32 TokenId { get; set; }

        [DataMember]
        public Int32 AdvtId { get; set; }
        [DataMember]
        public string PlayedDate { get; set; }
        [DataMember]
        public string PlayedTime { get; set; }
    }

    #endregion


    #region Played Prayer Status
    [DataContract]
    public class ResponcePlayedPrayer
    {
        [DataMember]
        public string Response { get; set; }
    }

    [DataContract]
    public class DataPlayedPrayer
    {
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public string PlayedDate { get; set; }
        [DataMember]
        public string PlayedTime { get; set; }


    }
    #endregion


    #region Player Login Status
    [DataContract]
    public class ResponcePlayerLogin
    {
        [DataMember]
        public string Response { get; set; }
    }

    [DataContract]
    public class DataPlayerLogin
    {
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public string LoginDate { get; set; }
        [DataMember]
        public string LoginTime { get; set; }


    }
    #endregion


    #region Player Logout Status
    [DataContract]
    public class ResponcePlayerLogout
    {
        [DataMember]
        public string Response { get; set; }
    }

    [DataContract]
    public class DataPlayerLogout
    {
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public string LogoutDate { get; set; }
        [DataMember]
        public string LogoutTime { get; set; }


    }
    #endregion


    #region Player Heart Beat Status
    [DataContract]
    public class ResponcePlayerHeart
    {
        [DataMember]
        public string Response { get; set; }
    }

    [DataContract]
    public class DataPlayerHeart
    {
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public string HeartbeatDateTime { get; set; }



    }
    #endregion


    #region Customer Contents
    [DataContract]
    public class ResponceCustomerContent
    {
        [DataMember]
        public Int32 titleId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string tTime { get; set; }
        [DataMember]
        public Int32 ArtistID { get; set; }
        [DataMember]
        public string arName { get; set; }
        [DataMember]
        public Int32 AlbumID { get; set; }
        [DataMember]
        public string alName { get; set; }
        [DataMember]
        public string TitleUrl { get; set; }
        [DataMember]
        public string MediaType { get; set; }
    }

    [DataContract]
    public class DataCustomerContent
    {
        [DataMember]
        public Int32 dfClientId { get; set; }
        [DataMember]
        public string MediaType { get; set; }



    }
    #endregion


    [DataContract]
    public class ResponceStreamPlaylist
    {
        [DataMember]
        public Int32 pSchid { get; set; }
        [DataMember]
        public Int32 splPlaylistId { get; set; }
        [DataMember]
        public string startTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string StreamName { get; set; }
    }

    [DataContract]
    public class DataCustomerTokenId
    {
        [DataMember]
        public Int32 Tokenid { get; set; }
    }
    [DataContract]
    public class DataTokenId
    {
        [DataMember]
        public string Tokenid { get; set; }
        [DataMember]
        public string WeekId { get; set; }
    }
    [DataContract]
    public class DataDownloadStatus
    {
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public string totalSong { get; set; }
        [DataMember]
        public string verNo { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "FreeSpace", Order = 1)]
        public string FreeSpace { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "TotalSpace", Order = 2)]
        public string TotalSpace { get; set; }

        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "TimeZone", Order = 2)]
        public string TimeZone { get; set; }
    }


    [DataContract]
    public class ResponcePublish
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string IsPublishUpdate { get; set; }
        [DataMember]
        public int TotalShot { get; set; }
        [DataMember]
        public bool IsDemoToken { get; set; }
        [DataMember]
        public string DispenserAlert { get; set; }
    }
    [DataContract]
    public class DataPlaylistDownloadStatus
    {
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public string totalSong { get; set; }
        [DataMember]
        public string splPlaylistId { get; set; }
    }
    [DataContract]
    public class DataPlaylistDownloadedSongs
    {
        [DataMember]
        public string[] titleIDArray { get; set; }
        [DataMember]
        public string TokenId { get; set; }
        [DataMember]
        public string splPlaylistId { get; set; }
    }

    [DataContract]
    public class titleIDArray1
    {
        [DataMember]
        public string titleid { get; set; }
    }

    [DataContract]
    public class DataTokenCrashLog
    {
        [DataMember]
        public int TokenId { get; set; }
        [DataMember]
        public string CrashLog { get; set; }
        [DataMember]
        public string CrashDateTime { get; set; }
    }

    public class ResponseTokenCrashLog
    {
        [DataMember]
        public Int32 Response { get; set; }
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class DataSplPlaylistDateWise
    {
        [DataMember]
        public int WeekNo { get; set; }
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public Int32 DfClientId { get; set; }
        [DataMember]
        public string CurrentDateTime { get; set; }
    }

    [DataContract]
    public class ResponceSetting
    {
        [DataMember]
        public string direction { get; set; }
        [DataMember]
        public string defaultSpeed { get; set; }
        [DataMember]
        public string highlightTime { get; set; }
        [DataMember]
        public string zoomRatio { get; set; }
        [DataMember]
        public string moveScaleHitSpeed { get; set; }
        [DataMember]
        public string moveHitSpeed { get; set; }
        [DataMember]
        public string displayType { get; set; }
        [DataMember]
        public string clientid { get; set; }
        [DataMember]
        public string pName { get; set; }
        [DataMember]
        public string Interval { get; set; }
        [DataMember]
        public string IsMute { get; set; }
        [DataMember]
        public string bgColor { get; set; }
        [DataMember]
        public string IsGameActive { get; set; }
    }
    [DataContract]
    public class ResponceMoovSource
    {
        [DataMember]
        public string photoPath { get; set; }
        [DataMember]
        public string photoTitle { get; set; }
        [DataMember]
        public string photoVideo { get; set; }
        [DataMember]
        public string photoId { get; set; }
    }
    [DataContract]
    public class DataMooovPlaylist
    {
        [DataMember]
        public int PlayerId { get; set; }
    }
    [DataContract]
    public class ResponcePlaylistMoov
    {
        [DataMember]
        public Int32 PlaylistId { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
    }

    [DataContract]
    public class DataPlayerRegistration
    {
        [DataMember]
        public string PlayerName { get; set; }
        [DataMember]
        public string TokenCode { get; set; }
    }
    [DataContract]
    public class ResponcePlayerRegistration
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string PlayerId { get; set; }
    }
    [DataContract]
    public class DataClientMessage
    {
        [DataMember]
        public string ClientMsg { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string playerid { get; set; }
    }
    [DataContract]
    public class DataClientId
    {
        [DataMember]
        public Int32 PlayerId { get; set; }
    }
    [DataContract]
    public class PlaylistDetail
    {

        [DataMember]
        public Int32 Playlistid { get; set; }
    }


    //======================================================================================
    //======================================================================================
    //============================= Web Panel ==============================================
    //======================================================================================
    //======================================================================================

    [DataContract]
    public class ReqComboQuery
    {
        [DataMember]
        public string Query { get; set; }
    }
    [DataContract]
    public class ResComboQuery
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public Boolean check { get; set; }
    }
    [DataContract]
    public class ReqTokenInfo
    {
        [DataMember]
        public string clientId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "UserId", Order = 1)]
        public string UserId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "IsActiveTokens", Order = 1)]
        public string IsActiveTokens { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "DBType", Order = 1)]
        public string DBType { get; set; }
    }
    [DataContract]
    public class ResTokenInfo
    {
        [DataMember]
        public string tokenid { get; set; }
        [DataMember]
        public string tokenCode { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string countryName { get; set; }
        [DataMember]
        public string playerType { get; set; }
        [DataMember]
        public string LicenceType { get; set; }
        [DataMember]
        public string pStatus { get; set; }
        [DataMember]
        public string pName { get; set; }
        [DataMember]
        public string lPlayed { get; set; }
        [DataMember]
        public string lStatus { get; set; }
        [DataMember]
        public Boolean check { get; set; }
        [DataMember]
        public int TotalDays { get; set; }
        [DataMember]
        public string tInfo { get; set; }
        [DataMember]
        public string AppLogoId { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string PublishUpdate { get; set; }
        [DataMember]
        public string token { get; set; }
        [DataMember]
        public string ScheduleType { get; set; }
        [DataMember]
        public string fSpace { get; set; }
        [DataMember]
        public string IsIndicatorActive { get; set; }

        [DataMember]
        public string GroupId { get; set; }
        [DataMember]
        public string CountryId { get; set; }
        [DataMember]
        public string StateId { get; set; }
        [DataMember]
        public string CityId { get; set; }
        [DataMember]
        public int SpaceStatus { get; set; }
        [DataMember]
        public int SpacePer { get; set; }
        [DataMember]
        public string MediaType { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string WeekName { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string DeviceType { get; set; }
        [DataMember]
        public string TokenNoBkp { get; set; }
        [DataMember]
        public string CountryFullName { get; set; }
        [DataMember]
        public string AlertEmail { get; set; }
        [DataMember]
        public string gName { get; set; }
        [DataMember]
        public string tZone { get; set; }
        [DataMember]
        public string TokenStatus { get; set; }

    }
    [DataContract]
    public class ReqToken
    {
        [DataMember]
        public string tokenId { get; set; }
    }
    [DataContract]
    public class ResToken
    {
        [DataMember]
        public List<ResTokenPlaylistSch> lstPlaylistSch { get; set; }
        [DataMember]
        public List<ResTokenAds> lstAds { get; set; }
        [DataMember]
        public List<ResTokenPrayer> lstPrayer { get; set; }
        [DataMember]
        public List<ResTokenData> lstTokenData { get; set; }
        [DataMember]
        public List<ResTokenPlaylistSch> APKPlaylist { get; set; }


    }
    [DataContract]
    public class ResTokenPlaylistSch
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string formatName { get; set; }
        [DataMember]
        public string playlistName { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string WeekDay { get; set; }
        [DataMember]
        public string formatid { get; set; }
        [DataMember]
        public string splPlaylistId { get; set; }
        [DataMember]
        public string PercentageValue { get; set; }
    }
    [DataContract]
    public class ResTokenAds
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string adName { get; set; }
        [DataMember]
        public string atype { get; set; }
        [DataMember]
        public string startDate { get; set; }
        [DataMember]
        public string playingMode { get; set; }
        [DataMember]
        public string adsLink { get; set; }

    }
    [DataContract]
    public class ResTokenPrayer
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string Time1 { get; set; }
        [DataMember]
        public string Time2 { get; set; }
        [DataMember]
        public string Time3 { get; set; }
        [DataMember]
        public string Time4 { get; set; }
        [DataMember]
        public string Time5 { get; set; }
    }
    [DataContract]
    public class ResTokenData
    {
        [DataMember]
        public string token { get; set; }

        [DataMember]
        public string personName { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string street { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string ExpiryDate { get; set; }

        [DataMember]
        public string PlayerType { get; set; }
        [DataMember]
        public string LicenceType { get; set; }
        [DataMember]
        public string chkMediaType { get; set; }
        [DataMember]
        public string chkuserRights { get; set; }
        [DataMember]
        public string chkType { get; set; }

        [DataMember]
        public string TokenNoBkp { get; set; }
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public string ScheduleType { get; set; }
        [DataMember]
        public bool Indicator { get; set; }
        [DataMember]
        public string StateName { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string GroupId { get; set; }
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string Rotation { get; set; }
        [DataMember]
        public string CommunicationType { get; set; }
        [DataMember]
        public string DeviceType { get; set; }
        [DataMember]
        public List<ReqDispenserAlert> DispenserAlert { get; set; }
        [DataMember]
        public string TotalShot { get; set; }
        [DataMember]
        public string AlertMail { get; set; }
        [DataMember]
        public bool IsShowShotToast { get; set; }
        [DataMember]
        public string OsVersion { get; set; }
    }
    [DataContract]
    public class ReqSaveTokenInfo
    {
        [DataMember]
        public string Tokenid { get; set; }
        [DataMember]
        public string token { get; set; }

        [DataMember]
        public string personName { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string street { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string ExpiryDate { get; set; }

        [DataMember]
        public string PlayerType { get; set; }
        [DataMember]
        public string LicenceType { get; set; }
        [DataMember]
        public string chkMediaType { get; set; }
        [DataMember]
        public string chkuserRights { get; set; }
        [DataMember]
        public string chkType { get; set; }
        [DataMember]
        public string ScheduleType { get; set; }
        [DataMember]
        public bool chkIndicator { get; set; }
        [DataMember]
        public string GroupId { get; set; }
        [DataMember]
        public string Rotation { get; set; }
        [DataMember]
        public string CommunicationType { get; set; }
        [DataMember]
        public string DeviceType { get; set; }
        [DataMember]
        public string TotalShot { get; set; }
        [DataMember]
        public List<ReqDispenserAlert> DispenserAlert { get; set; }
        [DataMember]
        public string AlertMail { get; set; }
        [DataMember]
        public bool IsShowShotToast { get; set; }
        [DataMember]
        public string OsVersion { get; set; }
    }
    [DataContract]
    public class ReqDispenserAlert
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string itemName { get; set; }
    }
    [DataContract]
    public class ResResponce
    {
        [DataMember]
        public string Responce { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public string filePath { get; set; }
        [DataMember]
        public string dfClientId { get; set; }
        [DataMember]
        public string FcmId { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public Boolean chkDashboard { get; set; }
        [DataMember]
        public Boolean chkPlayerDetail { get; set; }
        [DataMember]
        public Boolean chkPlaylistLibrary { get; set; }
        [DataMember]
        public Boolean chkScheduling { get; set; }
        [DataMember]
        public Boolean chkAdvertisement { get; set; }

        [DataMember]
        public Boolean chkInstantPlay { get; set; }
        [DataMember]
        public string LoginName { get; set; }
        [DataMember]
        public string LoginPassword { get; set; }
        [DataMember]
        public string IsVideoToken { get; set; }

        [DataMember]
        public string IsRf { get; set; }
        [DataMember]
        public string ContentType { get; set; }
        [DataMember]
        public Boolean chkUserAdmin { get; set; }
        [DataMember]
        public string MediaType { get; set; }
        [DataMember]
        public string PlayerType { get; set; }
        [DataMember]
        public Boolean chkUpload { get; set; }
        [DataMember]
        public Boolean chkCopyData { get; set; }
        [DataMember]
        public Boolean chkStreaming { get; set; }

        [DataMember]
        public List<string> TitlePlaylists { get; set; }
        [DataMember]
        public string TitleId { get; set; }
    }
    [DataContract]
    public class ReqResetToken
    {
        [DataMember]
        public string tokenId { get; set; }
        [DataMember]
        public string tokenCode { get; set; }
    }
    [DataContract]
    public class ReqUpdateSchedule
    {
        [DataMember]
        public string pschid { get; set; }
        [DataMember]
        public string ModifyStartTime { get; set; }
        [DataMember]
        public string ModifyEndTime { get; set; }
    }
    [DataContract]
    public class ResCustomerList
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string countryName { get; set; }
        [DataMember]
        public string customerCode { get; set; }
        [DataMember]
        public string customerName { get; set; }
        [DataMember]
        public string customerEmail { get; set; }
        [DataMember]
        public string totalToken { get; set; }
        [DataMember]
        public string expiryDate { get; set; }
        [DataMember]
        public string Key { get; set; }

    }
    [DataContract]
    public class RegCustomer
    {
        [DataMember]
        public string countryName { get; set; }
        [DataMember]
        public string cCode { get; set; }
        [DataMember]
        public string stateName { get; set; }
        [DataMember]
        public string cityName { get; set; }
        [DataMember]
        public string customerName { get; set; }
        [DataMember]
        public string customerEmail { get; set; }
        [DataMember]
        public string totalToken { get; set; }

        [DataMember]
        public string expiryDate { get; set; }
        [DataMember]
        public string supportEmail { get; set; }
        [DataMember]
        public string supportPhNo { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string DfClientId { get; set; }
        [DataMember]
        public string LoginId { get; set; }
        [DataMember]
        public string CustomerType { get; set; }
        [DataMember]
        public string MainCustomer { get; set; }
        [DataMember]
        public string personName { get; set; }
        [DataMember]
        public string dbType { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "ContentType", Order = 1)]
        public string ContentType { get; set; }
        [DataMember]
        public string ApiKey { get; set; }

    }
    [DataContract]
    public class ResBestOf
    {
        [DataMember]
        public List<ResBestPlaylist> lstBestPlaylist { get; set; }
        [DataMember]
        public List<ResSongList> lstSong { get; set; }
    }

    [DataContract]
    public class ResBestPlaylist
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string playlistName { get; set; }
        [DataMember]
        public Boolean check { get; set; }

    }
    [DataContract]
    public class ResSongList
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string Length { get; set; }
        [DataMember]
        public string Artist { get; set; }
        [DataMember]
        public string Album { get; set; }
        [DataMember]
        public string category { get; set; }
        [DataMember]
        public string genreName { get; set; }
        [DataMember]
        public string ArtistId { get; set; }
        [DataMember]
        public string AlbumId { get; set; }
        [DataMember]
        public string MediaType { get; set; }
        [DataMember]
        public string TitleIdLink { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public string FolderName { get; set; }
        [DataMember]
        public Boolean check { get; set; }
        [DataMember]
        public string EngeryLevel { get; set; }
        [DataMember]
        public string rDate { get; set; }
        [DataMember]
        public string BPM { get; set; }
        [DataMember]
        public string Language { get; set; }
        [DataMember]
        public string titleyear { get; set; }
        [DataMember]
        public string dfClientId { get; set; }


    }
    [DataContract]
    public class ResPlaylistSongList
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string Length { get; set; }
        [DataMember]
        public string Artist { get; set; }
        [DataMember]
        public string Album { get; set; }
        [DataMember]
        public string category { get; set; }
        [DataMember]
        public string ArtistId { get; set; }
        [DataMember]
        public string AlbumId { get; set; }
        [DataMember]
        public string MediaType { get; set; }
        [DataMember]
        public string TitleIdLink { get; set; }
        [DataMember]
        public string SrNo { get; set; }
        [DataMember]
        public string GenreName { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public string sId { get; set; }
        [DataMember]
        public string ImageTimeInterval { get; set; }
        [DataMember]
        public string ImgAllBtn { get; set; }
        [DataMember]
        public string isImgFind { get; set; }
    }

    [DataContract]
    public class ReqPlaylistSongList
    {
        [DataMember]
        public string playlistid { get; set; }
        [DataMember]
        public string IsBestOffPlaylist { get; set; }
    }
    [DataContract]
    public class ReqSaveBestPlaylist
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string plName { get; set; }
    }
    [DataContract]
    public class ReqAddPlaylistSong
    {
        [DataMember]
        public List<string> playlistid { get; set; }
        [DataMember]
        public List<string> titleid { get; set; }
        [DataMember]
        public string AddSongFrom { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "IsDuplicate", Order = 1)]
        public Boolean IsDuplicate { get; set; }
    }
    [DataContract]
    public class ReqCommonSearch
    {
        [DataMember]
        public string searchType { get; set; }
        [DataMember]
        public string searchText { get; set; }
        [DataMember]
        public string mediaType { get; set; }
        [DataMember]
        public string IsRf { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "ClientId", Order = 1)]
        public string ClientId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "IsExplicit", Order = 1)]
        public bool IsExplicit { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
        [DataMember]
        public string DBType { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "ContentType", Order = 1)]
        public string ContentType { get; set; }
        [DataMember]
        public string PageNo { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "LoginClientId", Order = 1)]
        public string LoginClientId { get; set; }
        [DataMember]
        public string IsAnnouncement { get; set; }

    }
    [DataContract]
    public class ReqDeletePlaylistSong
    {
        [DataMember]
        public string playlistid { get; set; }
        [DataMember]
        public string titleid { get; set; }
        [DataMember]
        public string IsBestOffPlaylist { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "IsForceDelete", Order = 1)]
        public string IsForceDelete { get; set; }
    }
    [DataContract]
    public class ReqSavePlaylist
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string plName { get; set; }
        [DataMember]
        public string formatid { get; set; }
    }
    [DataContract]
    public class ReqSavePlaylistFromBestOff
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string plName { get; set; }
        [DataMember]
        public string formatid { get; set; }
        [DataMember]
        public string isBestOff { get; set; }
    }
    [DataContract]
    public class ReqPlaylist
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "ClientId", Order = 1)]
        public string ClientId { get; set; }

    }
    [DataContract]
    public class ResPlaylist
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public Boolean check { get; set; }
        [DataMember]
        public Boolean IsMute { get; set; }
        [DataMember]
        public Boolean IsFixed { get; set; }
        [DataMember]
        public Boolean IsMixedContent { get; set; }

        [DataMember]
        public string[] tokenIds { get; set; }
        [DataMember]
        public Boolean IsDuplicate { get; set; }

    }
    [DataContract]
    public class ReqSF
    {
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string FormatId { get; set; }
        [DataMember]
        public string PlaylistId { get; set; }
        [DataMember]
        public string startTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public List<ReqSFWeek> wList { get; set; }
        [DataMember]
        public List<ReqSFToken> TokenList { get; set; }
    }
    [DataContract]
    public class ReqSFToken
    {
        [DataMember]
        public string tokenId { get; set; }
        [DataMember]
        public string schType { get; set; }
    }
    [DataContract]
    public class ReqSFWeek
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string itemName { get; set; }
    }
    [DataContract]
    public class ReqFillSF
    {
        [DataMember]
        public string clientId { get; set; }
        [DataMember]
        public string formatId { get; set; }
        [DataMember]
        public string playlistId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "UserId", Order = 1)]
        public string UserId { get; set; }
    }
    [DataContract]
    public class ResFillSF
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string formatName { get; set; }
        [DataMember]
        public string playlistName { get; set; }
        [DataMember]
        public string token { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string WeekNo { get; set; }
    }
    [DataContract]
    public class ReqDeleteSF
    {
        [DataMember]
        public string pschid { get; set; }
    }
    public class ReqSearchAds
    {
        [DataMember]
        public string customerId { get; set; }
        [DataMember]
        public string cDate { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "TokenId", Order = 1)]
        public string TokenId { get; set; }
    }
    [DataContract]
    public class ResSearchAds
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string aName { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string sDate { get; set; }
        [DataMember]
        public string Time { get; set; }
    }
    [DataContract]
    public class ReqAds
    {
        [DataMember]
        public string aName { get; set; }
        [DataMember]
        public string cName { get; set; }
        [DataMember]
        public string pType { get; set; }
        [DataMember]
        public string category { get; set; }
        [DataMember]
        public string sDate { get; set; }
        [DataMember]
        public string eDate { get; set; }
        [DataMember]
        public string pMode { get; set; }
        [DataMember]
        public string TotalFrequancy { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public List<ReqSFWeek> wList { get; set; }
        [DataMember]
        public List<string> TokenLst { get; set; }
        [DataMember]
        public List<string> CustomerLst { get; set; }
        [DataMember]
        public string sTime { get; set; }
        [DataMember]
        public List<string> CountryLst { get; set; }
        [DataMember]
        public string aid { get; set; }
    }
    [DataContract]
    public class ReqTokenInfoAds
    {
        [DataMember]
        public List<string> clientId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "UserId", Order = 1)]
        public string UserId { get; set; }
    }
    [DataContract]
    public class ReqAdsId
    {
        [DataMember]
        public string advtid { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "ClientId", Order = 1)]
        public string ClientId { get; set; }
    }
    [DataContract]
    public class ResUpdateAds
    {
        [DataMember]
        public string aName { get; set; }
        [DataMember]
        public string cName { get; set; }
        [DataMember]
        public string pType { get; set; }
        [DataMember]
        public string category { get; set; }
        [DataMember]
        public string sDate { get; set; }
        [DataMember]
        public string eDate { get; set; }
        [DataMember]
        public string pMode { get; set; }
        [DataMember]
        public string TotalFrequancy { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public List<ReqSFWeek> wList { get; set; }
        [DataMember]
        public List<string> TokenLst { get; set; }
        [DataMember]
        public List<string> CustomerLst { get; set; }
        [DataMember]
        public string sTime { get; set; }
        [DataMember]
        public List<string> CountryLst { get; set; }
        [DataMember]
        public List<ResComboQuery> lstCountry { get; set; }
        [DataMember]
        public List<ResComboQuery> lstCustomer { get; set; }
        [DataMember]
        public List<ResTokenInfo> lstToken { get; set; }
    }
    [DataContract]
    public class ReqPrayer
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string sDate { get; set; }
        [DataMember]
        public string eDate { get; set; }
        [DataMember]
        public string startTime { get; set; }
        [DataMember]
        public string duration { get; set; }
        [DataMember]
        public string cId { get; set; }
        [DataMember]
        public List<string> tokenid { get; set; }
    }
    public class ReqSearchPrayer
    {
        [DataMember]
        public string cDate { get; set; }
        [DataMember]
        public string tokenid { get; set; }
    }
    [DataContract]
    public class ResSearchPrayer
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string sTime { get; set; }
        [DataMember]
        public string eTime { get; set; }
    }
    [DataContract]
    public class ResDeletePrayer
    {
        [DataMember]
        public string id { get; set; }
    }
    [DataContract]
    public class ReqLg
    {
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "DBType", Order = 1)]
        public string DBType { get; set; }
    }
    [DataContract]
    public class ReqDashboard
    {
        [DataMember]
        public string clientId { get; set; }
        [DataMember]
        public string ftype { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "UserId", Order = 1)]
        public string UserId { get; set; }
    }
    [DataContract]
    public class ResDashboard
    {
        [DataMember]
        public int TotalPlayers { get; set; }
        [DataMember]
        public int OnlinePlayers { get; set; }
        [DataMember]
        public int OfflinePlayer { get; set; }
        [DataMember]
        public List<ResTokenInfo> lstToken { get; set; }
    }
    [DataContract]
    public class DataTokenFCMID
    {
        [DataMember]
        public int TokenId { get; set; }
        [DataMember]
        public string fcmId { get; set; }
    }
    [DataContract]
    public class ClsNoti
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string DeviceToken { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string artistid { get; set; }
        [DataMember]
        public string albumid { get; set; }
        [DataMember]
        public string artistname { get; set; }
        [DataMember]
        public string IsVideoToken { get; set; }
        [DataMember]
        public string tid { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "PlayType", Order = 1)]
        public string PlayType { get; set; }
    }
    [DataContract]
    public class ResNoti
    {
        [DataMember]
        public string success { get; set; }
    }

    [DataContract]
    public class ReqState
    {
        [DataMember]
        public string countryId { get; set; }
        [DataMember]
        public string StateName { get; set; }
    }
    [DataContract]
    public class ResUser
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string UserName1 { get; set; }
        [DataMember]
        public string Password1 { get; set; }
        [DataMember]
        public Boolean chkDashboard { get; set; }
        [DataMember]
        public Boolean chkPlayerDetail { get; set; }
        [DataMember]
        public Boolean chkPlaylistLibrary { get; set; }
        [DataMember]
        public Boolean chkScheduling { get; set; }
        [DataMember]
        public Boolean chkAdvertisement { get; set; }

        [DataMember]
        public Boolean chkInstantPlay { get; set; }
        [DataMember]
        public string dfClientid { get; set; }

        [DataMember]
        public string Responce { get; set; }

        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "lstTokenInfo", Order = 1)]
        public List<ResTokenInfo> lstTokenInfo { get; set; }

        [DataMember]
        public List<string> lstToken { get; set; }
        [DataMember]
        public Boolean chkDeleteSong { get; set; }
        [DataMember]
        public Boolean chkInstantApk { get; set; }
        [DataMember]
        public string cmbFormat { get; set; }
        [DataMember]
        public string cmbPlaylist { get; set; }
        [DataMember]
        public Boolean chkUserAdmin { get; set; }
        [DataMember]
        public Boolean chkUpload { get; set; }
        [DataMember]
        public Boolean chkCopyData { get; set; }
        [DataMember]
        public Boolean chkStreaming { get; set; }
    }
    [DataContract]
    public class ReqUserInfo
    {
        [DataMember]
        public string UserId { get; set; }
    }
    [DataContract]
    public class ReqPlayerLog
    {
        [DataMember]
        public string cDate { get; set; }
        [DataMember]
        public string tokenid { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "ToDate", Order = 1)]
        public string ToDate { get; set; }

    }
    [DataContract]
    public class ResPlayerLog
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ArtistName { get; set; }
        [DataMember]
        public string PlayedDateTime { get; set; }
        [DataMember]
        public string SplPlaylistName { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public string pDateTime { get; set; }
        [DataMember]
        public string TotalPlayed { get; set; }

    }
    [DataContract]
    public class ReqSaveFormat
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string formatname { get; set; }
        [DataMember]
        public string dfclientId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "DBType", Order = 1)]
        public string DBType { get; set; }
        [DataMember]
        public string MediaType { get; set; }
    }

    [DataContract]
    public class ReqCopyScheduleDetail
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string formatid { get; set; }
        [DataMember]
        public string splPlaylistId { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }

    }
    [DataContract]
    public class ReqCopySchedule
    {
        [DataMember]
        public List<ReqCopyScheduleDetail> SchList { get; set; }
        [DataMember]
        public string[] TokenList { get; set; }
        [DataMember]
        public string dfClientId { get; set; }
    }
    [DataContract]
    public class ReqSettingPlaylistSong
    {
        [DataMember]
        public string playlistid { get; set; }
        [DataMember]
        public Boolean chkMute { get; set; }
        [DataMember]
        public Boolean chkFixed { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "chkMixed", Order = 1)]
        public Boolean chkMixed { get; set; }
        [DataMember]
        public Boolean chkDuplicate { get; set; }
    }
    [DataContract]
    public class ReqUpdatePlaylistSRNo
    {
        [DataMember]
        public List<string> playlistid { get; set; }
        [DataMember]
        public List<ReqUpdatePlaylistSRNo_Detail> lstTitleSR { get; set; }
    }
    [DataContract]
    public class ReqUpdatePlaylistSRNo_Detail
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string titleid { get; set; }
        [DataMember]
        public string id { get; set; }
    }
    [DataContract]
    public class ReqSaveModifyLogs
    {
        [DataMember]
        public string dfclientid { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public string ModifyData { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string EffectToken { get; set; }
    }
    [DataContract]
    public class ResAdminLogs
    {
        [DataMember]
        public string Ipaddress { get; set; }
        [DataMember]
        public string ModifyDateTime { get; set; }
        [DataMember]
        public string effect { get; set; }
        [DataMember]
        public string clientname { get; set; }
        [DataMember]
        public string modifydata { get; set; }

    }
    [DataContract]
    public class ReqGenreList
    {
        [DataMember]
        public string mediatype { get; set; }
        [DataMember]
        public string mediaStyle { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "ClientId", Order = 1)]
        public string ClientId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "DBType", Order = 1)]
        public string DBType { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "IsAdmin", Order = 1)]
        public bool IsAdmin { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "ContentType", Order = 1)]
        public string ContentType { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "FilterType", Order = 1)]
        public string FilterType { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "FilterValue", Order = 1)]
        public string FilterValue { get; set; }

    }
    [DataContract]
    public class GenreList
    {
        [DataMember]
        public Boolean iChecked { get; set; }
        [DataMember]
        public string genreid { get; set; }
        [DataMember]
        public string genre { get; set; }
        [DataMember]
        public int GenrePercentage { get; set; }

    }
    [DataContract]
    public class ReqGenrePer
    {
        [DataMember]
        public string GenreId { get; set; }
        [DataMember]
        public string GenrePercentage { get; set; }
        [DataMember]
        public string MediaType { get; set; }
    }
    [DataContract]
    public class ReqNewSavePlaylist
    {
        [DataMember]
        public string plName { get; set; }
        [DataMember]
        public string TotalSongs { get; set; }
        [DataMember]
        public string MediaType { get; set; }
        [DataMember]
        public string MediaStyle { get; set; }
        [DataMember]
        public string formatid { get; set; }
        [DataMember]
        public string DBType { get; set; }
        [DataMember]
        public List<ReqGenrePer> lstGenrePer { get; set; }
    }
    [DataContract]
    public class ReqPlaylistAd
    {
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string FormatId { get; set; }
        [DataMember]
        public string PlaylistId { get; set; }
        [DataMember]
        public string startTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string sDate { get; set; }
        [DataMember]
        public string eDate { get; set; }
        [DataMember]
        public string pMode { get; set; }
        [DataMember]
        public string TotalFrequancy { get; set; }
        [DataMember]
        public List<ReqSFWeek> wList { get; set; }
        [DataMember]
        public List<string> TokenList { get; set; }
    }
    [DataContract]
    public class ReqFillAdPlaylist
    {
        [DataMember]
        public string clientId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "tokenid", Order = 1)]
        public string tokenid { get; set; }

    }
    [DataContract]
    public class ReqForceUpdate
    {
        [DataMember]
        public string[] tokenid { get; set; }
    }

    [DataContract]
    public class ReqDeleteFormatId
    {
        [DataMember]
        public string formatId { get; set; }
        [DataMember]
        public string IsForceDelete { get; set; }
    }
    [DataContract]
    public class RegUpdateAppLogo
    {
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string LogoId { get; set; }
    }
    [DataContract]
    public class Reg_formatANDplaylist
    {
        [DataMember]
        public string formatId { get; set; }
        [DataMember]
        public string playlistId { get; set; }
    }
    [DataContract]
    public class RegSetOnlineIndicator
    {
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public Boolean chkIndicator { get; set; }
    }



    [DataContract]
    public class DataClientRegistration
    {
        [DataMember]
        public string cName { get; set; }
        [DataMember]
        public string cMail { get; set; }
        [DataMember]
        public string CountryId { get; set; }
        [DataMember]
        public string StateId { get; set; }

        [DataMember]
        public string CityId { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string TotalPlayer { get; set; }
        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string TaxNo { get; set; }
        [DataMember]
        public string lPwd { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string PhoneNo { get; set; }

        [DataMember]
        public string pAmount { get; set; }
        [DataMember]
        public string SubscriptionType { get; set; }
        [DataMember]
        public string CRPlayer { get; set; }
        [DataMember]
        public string RFPlayer { get; set; }

        [DataMember]
        public string cardNo { get; set; }
        [DataMember]
        public int month { get; set; }
        [DataMember]
        public int year { get; set; }
        [DataMember]
        public string cvc { get; set; }

        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "Currency", Order = 1)]
        public string Currency { get; set; }

        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "PlayerType", Order = 1)]
        public string PlayerType { get; set; }
    }

    [DataContract]
    public class ReqDeleteLogo
    {
        [DataMember]
        public string logoId { get; set; }
    }

    [DataContract]
    public class DataRenewPayment
    {
        [DataMember]
        public string cardNo { get; set; }
        [DataMember]
        public int month { get; set; }
        [DataMember]
        public int year { get; set; }
        [DataMember]
        public string cvv { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string ClientId { get; set; }
    }
    [DataContract]
    public class ReqSaveGenre
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string genrename { get; set; }
        [DataMember]
        public string mediatype { get; set; }
    }
    [DataContract]
    public class ReqSaveFolder
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string fname { get; set; }
        [DataMember]
        public string dfClientId { get; set; }
    }
    [DataContract]
    public class ReqTitleLog
    {
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string tokenid { get; set; }
        [DataMember]
        public string cDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public bool ShowOnlyTankChangeLog { get; set; }

    }
    [DataContract]
    public class ReqCitySateNewModify
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string stateid { get; set; }
        [DataMember]
        public string CountryId { get; set; }
        [DataMember]
        public string dfClientId { get; set; }
    }
    [DataContract]
    public class ReqCopyFormat
    {
        [DataMember]
        public string FormatId { get; set; }
        [DataMember]
        public string CopyFormatId { get; set; }
        [DataMember]
        public string dfclientId { get; set; }
    }
    [DataContract]
    public class ReqTranferToken
    {
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string TransferCustomerId { get; set; }
        [DataMember]
        public string[] TransferTokens { get; set; }
    }
    [DataContract]
    public class DataStream
    {
        [DataMember]
        public string TokenId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "OwnerCustomerId", Order = 1)]
        public string OwnerCustomerId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "IsAdmin", Order = 1)]
        public bool IsAdmin { get; set; }
    }
    [DataContract]
    public class ReqStream
    {
        [DataMember]
        public string sId { get; set; }
        [DataMember]
        public string sName { get; set; }
        [DataMember]
        public string sLink { get; set; }
        [DataMember]
        public string OwnerId { get; set; }
        [DataMember]
        public string sImgLink { get; set; }
    }
    [DataContract]
    public class ReqDeleteStream
    {
        [DataMember]
        public string sId { get; set; }

    }
    [DataContract]
    public class ReqAssignStream
    {
        [DataMember]
        public string[] TokenSelected { get; set; }
        [DataMember]
        public string[] StreamSelected { get; set; }
        [DataMember]
        public string OwnerId { get; set; }
    }
    [DataContract]
    public class ReqDeleteAssignStream
    {
        [DataMember]
        public string TokenId { get; set; }
        [DataMember]
        public string StreamId { get; set; }

    }
    [DataContract]
    public class ResFillMiddleImage
    {
        [DataMember]
        public string TitleIdLink { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string IsFind { get; set; }
    }
    [DataContract]
    public class ReqSetMiddleImg
    {
        [DataMember]
        public string TokenId { get; set; }
        [DataMember]
        public string TitleId { get; set; }

    }
    [DataContract]
    public class ReqFillSignageLogo
    {
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string FolderId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "IsAdmin", Order = 1)]
        public bool IsAdmin { get; set; }
    }
    [DataContract]
    public class DataUserRights_Bulk
    {
        [DataMember]
        public string DeviceId { get; set; }
    }
    [DataContract]
    public class ReqDeleteTitlePercentage
    {
        [DataMember]
        public string playlistid { get; set; }
        [DataMember]
        public string titlepercentage { get; set; }

    }
    [DataContract]
    public class ResGetMachineAnnouncement
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public int srno { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string Artist { get; set; }
        [DataMember]
        public string Genre { get; set; }
        [DataMember]
        public string aType { get; set; }
        [DataMember]
        public int TimeInterval { get; set; }
        [DataMember]
        public bool IsLoop { get; set; }

    }
    [DataContract]
    public class ReqMachineLogs
    {
        [DataMember]
        public Int32 TokenId { get; set; }
        [DataMember]
        public string PlayedDateTime { get; set; }
        [DataMember]
        public string Logs { get; set; }
        [DataMember]
        public string command { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "titleId", Order = 1)]
        public string titleId { get; set; }
        [DataMember(EmitDefaultValue = true, IsRequired = false, Name = "aType", Order = 1)]
        public string aType { get; set; }
    }
    [DataContract]
    public class ResMachineLogs
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public List<LogsArray> EventArray { get; set; }
    }
    [DataContract]
    public class LogsArray
    {
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string returnEventDateTime { get; set; }
    }
    [DataContract]
    public class ReqFillCustomer
    {
        [DataMember]
        public string DBType { get; set; }
    }
    [DataContract]
    public class ReqDispenserAlertMail
    {
        [DataMember]
        public string TokenId { get; set; }
        [DataMember]
        public string TankStatusPercent { get; set; }
    }
    [DataContract]
    public class ReqDeleteMachineTitle
    {
        [DataMember]
        public string Tokenid { get; set; }
        [DataMember]
        public string titleid { get; set; }
    }
    [DataContract]
    public class ReqSaveMachineAnnouncement
    {
        [DataMember]
        public List<ReqTokenMachineAnnouncement> TokenId { get; set; }
        [DataMember]
        public List<string> titleid { get; set; }
        [DataMember]
        public Boolean chkWithPrevious { get; set; }


    }
    [DataContract]
    public class ReqTokenMachineAnnouncement
    {
        [DataMember]
        public string tokenid { get; set; }
        [DataMember]
        public string tokenCode { get; set; }
    }
    [DataContract]
    public class ReqMachineAnnouncementSRNo
    {
        [DataMember]
        public string tokenId { get; set; }
        [DataMember]
        public List<ReqUpdatePlaylistSRNo_Detail> lstTitleSR { get; set; }
    }
    [DataContract]
    public class MachineNoti
    {
        [DataMember]
        public string tokenId { get; set; }
    }
    [DataContract]
    public class ClsMachineNoti
    {
        [DataMember]
        public int TotalShot { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public bool IsDemoToken { get; set; }
        [DataMember]
        public string DispenserAlert { get; set; }
        [DataMember]
        public string DeviceToken { get; set; }

    }
    [DataContract]
    public class ReqUpdateEnergyLevel
    {
        [DataMember]
        public string TitleId { get; set; }
        [DataMember]
        public string EnergyLevel { get; set; }
    }
    [DataContract]
    public class ReqGetFolderContent
    {
        [DataMember]
        public string FolderId { get; set; }
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string DBType { get; set; }

    }
    [DataContract]
    public class ReqTransferContent
    {
        [DataMember]
        public string FolderId { get; set; }
        [DataMember]
        public string[] TitleList { get; set; }
        [DataMember]
        public string dfClientId { get; set; }
        [DataMember]
        public string FromFolderId { get; set; }
    }
    [DataContract]
    public class ReqUpdateContent
    {
        [DataMember]
        public string TitleId { get; set; }
        [DataMember]
        public string titleName { get; set; }
    }
    [DataContract]
    public class ReqInstantPlay
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string artistid { get; set; }
        [DataMember]
        public string albumid { get; set; }
        [DataMember]
        public string artistname { get; set; }
        [DataMember]
        public Int32[] tid { get; set; }
    }
    [DataContract]
    public class ReqGetInstantPlaySpecialPlaylistsTitles
    {
        [DataMember]
        public Int32[] Tokenid { get; set; }
    }
    [DataContract]
    public class ReqDeleteTitle
    {
        [DataMember]
        public string playlistid { get; set; }
        [DataMember]
        public string[] titleid { get; set; }
    }
    [DataContract]
    public class ReqAppLogin
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string TokenNo { get; set; }
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public string PlayerType { get; set; }
        [DataMember]
        public string DBType { get; set; }
    }
    [DataContract]
    public class ReqKeyboardAnnouncement
    {
        [DataMember]
        public List<ReqTokenMachineAnnouncement> TokenId { get; set; }
        [DataMember]
        public string splPlaylistId { get; set; }

    }
    [DataContract]
    public class ResGetKeyboardAnnouncement
    {
        [DataMember]
        public string pName { get; set; }
        [DataMember]
        public string fName { get; set; }
        [DataMember]
        public string id { get; set; }
    }
    [DataContract]
    public class ReqDeleteKeyboardAnnouncement
    {
        [DataMember]
        public string id { get; set; }
    }
    [DataContract]
    public class ReqSetFireAlert
    {
        [DataMember]
        public List<ReqTokenMachineAnnouncement> tokenId { get; set; }
        [DataMember]
        public string titleid { get; set; }
        [DataMember]
        public string MediaType { get; set; }
    }
    [DataContract]
    public class ReqGetTemplates
    {
        [DataMember]
        public Int32 dfClientId { get; set; }
        [DataMember]
        public Int32 GenreId { get; set; }
        [DataMember]
        public string cDate { get; set; }
        [DataMember]
        public string search { get; set; }
    }


    [DataContract]
    public class ResGetTemplates
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string orientation { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string createdAt { get; set; }
        [DataMember]
        public string modifiedAt { get; set; }
        [DataMember]
        public string thumbnailUrl { get; set; }
        [DataMember]
        public string thumbnailCreatedAt { get; set; }
        [DataMember]
        public string videoUrl { get; set; }
        [DataMember]
        public string videoCreatedAt { get; set; }

    }
    [DataContract]
    public class ReqDownloadTemplates
    {
        [DataMember]
        public Int32 dfClientId { get; set; }
        [DataMember]
        public Int32 GenreId { get; set; }
        [DataMember]
        public Int32 FolderId { get; set; }
        [DataMember]
        public List<ReqTemplatesList> tList { get; set; }
        [DataMember]
        public string dbType { get; set; }

    }
    [DataContract]
    public class ReqTemplatesList
    {
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string id { get; set; }
    }
    [DataContract]
    public class ReqSaveImageTimeInterval
    {
        [DataMember]
        public string splId { get; set; }
        [DataMember]
        public string titleid { get; set; }
        [DataMember]
        public string ImgInterval { get; set; }
    }
    public class ReqDeleteFolder
    {
        [DataMember]
        public string id { get; set; }
    }
    public class ReqUpdateTokenGroups
    {
        [DataMember]
        public string[] tokenIds { get; set; }
        [DataMember]
        public string GroupId { get; set; }
    }
    public class ReqOpeningHours
    {
        [DataMember]
        public string startTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public List<ReqSFWeek> wList { get; set; }
        [DataMember]
        public List<string> TokenList { get; set; }
    }
    public class ReqUpdateTokenInfo
    {
        [DataMember]
        public string tokenid { get; set; }
        [DataMember]
        public string CountryId { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string LicenceType { get; set; }
        [DataMember]
        public string MediaType { get; set; }
        [DataMember]
        public string playerType { get; set; }
    }
    public class ResClientTemplateRegsiter
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string key { get; set; }
    }
    public class ReqSF_New
    {
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string FormatId { get; set; }
        [DataMember]
        public string PlaylistId1 { get; set; }
        [DataMember]
        public string startTime1 { get; set; }
        [DataMember]
        public string EndTime1 { get; set; }
        [DataMember]
        public List<ReqSFWeek> wList1 { get; set; }
        [DataMember]
        public List<ReqSFToken> TokenList { get; set; }
        [DataMember]
        public List<ReqSF_Playlist_New> lstPlaylist { get; set; }
        [DataMember]
        public string ScheduleType { get; set; }
        
    }
    public class ReqSF_Playlist_New
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string eTime { get; set; }
        [DataMember]
        public string pName { get; set; }
        [DataMember]
        public string sTime { get; set; }
        [DataMember]
        public string splId { get; set; }
        [DataMember]
        public string wId { get; set; }
        [DataMember]
        public string wName { get; set; }
        [DataMember]
        public string PercentageValue { get; set; }

    }
    public class ReqDeleteTitleOwn
    {
        [DataMember]
        public string ForceType { get; set; }
        [DataMember]
        public string tid { get; set; }
    }
    public class ResCustomerWithKey
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public Boolean check { get; set; }
        [DataMember]
        public string apikey { get; set; }
    }

    public class ReqFindToken
    {
        [DataMember]
        public string ClientId { get; set; }
        [DataMember]
        public string IsAdmin { get; set; }
        [DataMember]
        public string DbType { get; set; }
        [DataMember]
        public string tokenid { get; set; }
    }

}

