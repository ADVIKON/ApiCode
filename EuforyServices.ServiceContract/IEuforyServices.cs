using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ComponentModel;
using EuforyServices.DataContract;
using System.Web.Http.Cors;
using System.IO;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;

namespace EuforyServices.ServiceContract
{
    [ServiceContract]
    public interface IEuforyServices
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]

        [WebInvoke(Method = "GET", UriTemplate = "GetAllGenre", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get All Genre")]
        List<ResponceGenre> GetAllGenre();

        [WebInvoke(Method = "POST", UriTemplate = "GetGenreTitles", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get All Genre Titles")]
        List<ResponceGenreTitles> GetGenreTitles(DataGenreTitles data);

        [WebInvoke(Method = "GET", UriTemplate = "GetOnlineStream", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get All Stream")]
        List<ResponceStream> GetOnlineStream();


        [WebInvoke(Method = "POST", UriTemplate = "CheckUserLogin", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Check user Login")]
        List<ResponceUserToen> CheckUserLogin(DataClientToken  data);


        [WebInvoke(Method = "POST", UriTemplate = "CheckUserRights", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Check user rights")]
        List<ResponceUserRights> CheckUserRights(DataUserRights data);

        [WebInvoke(Method = "GET", UriTemplate = "GetMiddleImage", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Middle Image")]
        List<ResponceMiddleImage> GetMiddleImage();


        [WebInvoke(Method = "POST", UriTemplate = "GetSplPlaylist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Speical Playlist")]
        List<ResponceSplSplaylist> GetSplPlaylist(DataSplPlaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "GetSplPlaylistTitles", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Speical Playlist Titles")]
        List<ResponceSplSplaylistTitle> GetSplPlaylistTitles(DataSplPlaylistTile data);


        [WebInvoke(Method = "POST", UriTemplate = "GetSplPlaylistLive", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Speical Playlist Live")]
        List<ResponceSplSplaylist> GetSplPlaylistLive(DataSplPlaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "GetSplPlaylistTitlesLive", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Speical Playlist Titles Live")]
        List<ResponceSplSplaylistTitle> GetSplPlaylistTitlesLive(DataSplPlaylistTile data);

        [WebInvoke(Method = "POST", UriTemplate = "CheckUserLoginLive", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Check user Login")]
        List<ResponceUserToen> CheckUserLoginLive(DataClientToken data);

        [WebInvoke(Method = "POST", UriTemplate = "CheckUserRightsLive", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Check user rights")]
        List<ResponceUserRights> CheckUserRightsLive(DataUserRights data);


        [WebInvoke(Method = "POST", UriTemplate = "PrayerTiming", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Prayer Timing")]
        List<ResponcePrayerTiming> PrayerTiming(DataPrayerTiming data);

        [WebInvoke(Method = "POST", UriTemplate = "AdvtSchedule", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Advt Schedule")]
        List<ResponceAdvt> AdvtSchedule(DataAdvtSch data);

        [WebInvoke(Method = "POST", UriTemplate = "AdvtScheduleLinuxOnly", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Advt Schedule Only Linux")]
        List<ResponceAdvtLinux> AdvtScheduleLinuxOnly(DataAdvtSchLinux data);



        

        [WebInvoke(Method = "POST", UriTemplate = "PlayedSongsStatus", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Played Songs Status")]
        List<ResponcePlayedSong> PlayedSongsStatus(DataPlayedSong data);


        [WebInvoke(Method = "POST", UriTemplate = "PlayedAdvertisementStatus", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Played Advertisement Status")]
        List<ResponcePlayedAdvt> PlayedAdvertisementStatus(DataPlayedAdvt data);


        [WebInvoke(Method = "POST", UriTemplate = "PlayedPrayerStatus", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Played Prayer Status")]
        List<ResponcePlayedPrayer> PlayedPrayerStatus(DataPlayedPrayer data);

        [WebInvoke(Method = "POST", UriTemplate = "PlayerLoginStatus", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Player Login Status")]
        List<ResponcePlayerLogin> PlayerLoginStatus(DataPlayerLogin data);

        [WebInvoke(Method = "POST", UriTemplate = "PlayerLogoutStatus", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Player Logout Status")]
        List<ResponcePlayerLogout> PlayerLogoutStatus(DataPlayerLogout data);

        [WebInvoke(Method = "POST", UriTemplate = "PlayerHeartBeatStatus", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Player Heart Beat Status")]
        List<ResponcePlayerHeart> PlayerHeartBeatStatus(DataPlayerHeart data);




      

        #region JSON Services

        [WebInvoke(Method = "POST", UriTemplate = "PlayedSongsStatusJsonArray", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Played Songs Status Stream")]
        List<ResponcePlayedSong> PlayedSongsStatusStream(List<DataPlayedSong> data);


        [WebInvoke(Method = "POST", UriTemplate = "PlayedAdvertisementStatusJsonArray", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Played Advertisement Status Stream")]
        List<ResponcePlayedAdvt> PlayedAdvertisementStatusStream(List<DataPlayedAdvtNew> data);


        [WebInvoke(Method = "POST", UriTemplate = "PlayedPrayerStatusJsonArray", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Played Prayer Status Stream")]
        List<ResponcePlayedPrayer> PlayedPrayerStatusStream(List<DataPlayedPrayer> data);

        [WebInvoke(Method = "POST", UriTemplate = "PlayerLoginStatusJsonArray", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Player Login Status Stream")]
        List<ResponcePlayerLogin> PlayerLoginStatusStream(List<DataPlayerLogin> data);

        [WebInvoke(Method = "POST", UriTemplate = "PlayerLogoutStatusJsonArray", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Player Logout Status Stream")]
        List<ResponcePlayerLogout> PlayerLogoutStatusStream(List<DataPlayerLogout> data);

        [WebInvoke(Method = "POST", UriTemplate = "PlayerHeartBeatStatusJsonArray", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Player Heart Beat Status Stream")]
        List<ResponcePlayerHeart> PlayerHeartBeatStatusStream(List<DataPlayerHeart> data);

        #endregion


        #region Sander Demo Video API

        [WebInvoke(Method = "POST", UriTemplate = "GetSplPlaylistVideo", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Speical Playlist Video")]
        List<ResponceSplSplaylist> GetSplPlaylistVideo(DataSplPlaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "GetSplPlaylistTitlesVideo", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Speical Playlist Titles Video")]
        List<ResponceSplSplaylistTitle> GetSplPlaylistTitlesVideo(DataSplPlaylistTile data);

        [WebInvoke(Method = "POST", UriTemplate = "CheckUserLoginVideo", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Check user Login Video")]
        List<ResponceUserToen> CheckUserLoginVideo(DataClientToken data);

        [WebInvoke(Method = "POST", UriTemplate = "CheckUserRightsVideo", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Check user rights Video")]
        List<ResponceUserRights> CheckUserRightsVideo(DataUserRights data);

        [WebInvoke(Method = "POST", UriTemplate = "AdvtScheduleVideoLinuxOnly", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Advt Schedule Video Only Linux")]
        List<ResponceAdvtLinux> AdvtScheduleVideoLinuxOnly(DataAdvtSchLinux data);


        #endregion



        [WebInvoke(Method = "POST", UriTemplate = "GetCustomerContent", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Customer Content")]
        List<ResponceCustomerContent> GetCustomerContent(DataCustomerContent data);



        [WebInvoke(Method = "GET", UriTemplate = "GetStreamPlaylistSchedule", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Stream Playlist Schedule")]
        List<ResponceStreamPlaylist> GetStreamPlaylistSchedule();

        [WebInvoke(Method = "POST", UriTemplate = "GetCustomerStreams", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Customer Streams")]
        List<ResponceStream> GetCustomerStreams(DataUserRights data);

        [WebInvoke(Method = "POST", UriTemplate = "GetCustomerTokenStreamsUrlLinux", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Customer token Streams url Linux")]
        List<ResponceStreamLinux> GetCustomerTokenStreamsUrlLinux(DataCustomerTokenId data);


        [WebInvoke(Method = "POST", UriTemplate = "GetTokenContent", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Token Content")]
        List<ResponceUserRights> GetTokenContent(DataTokenId data);

        [WebInvoke(Method = "POST", UriTemplate = "DownloadingProcess", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Token Wise Downloading Process")]
        List<ResponceDownloadingProcess> TokenWiseDownloadingProcess(DataDownloadStatus data);

        [WebInvoke(Method = "POST", UriTemplate = "CheckTokenPublish", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Check Token Publish")]
        List<ResponcePublish> CheckTokenPublish(DataCustomerTokenId data);

        [WebInvoke(Method = "POST", UriTemplate = "UpdateTokenPublish", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Update Token Publish")]
        List<ResponcePublish> UpdateTokenPublish(DataCustomerTokenId data);

        [WebInvoke(Method = "POST", UriTemplate = "PlaylistWiseDownloadedTotalSong", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Playlist Wise Downloaded Songs")]
        List<ResponceDownloadingProcess> PlaylistWiseDownloadedTotalSong(List<DataPlaylistDownloadStatus> data);


        [WebInvoke(Method = "POST", UriTemplate = "PlaylistWiseDownloadedSongsDetail", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Playlist Wise Downloaded Songs Detail")]
        List<ResponceDownloadingProcess> PlaylistWiseDownloadedSongsDetail(List<DataPlaylistDownloadedSongs> data);

        [WebInvoke(Method = "POST", UriTemplate = "TokenCrashLog", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Token Crash Log")]
        ResponseTokenCrashLog TokenCrashLog(DataTokenCrashLog data);

        [WebInvoke(Method = "POST", UriTemplate = "GetSplPlaylistDateWiseLive", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Speical Playlist Live")]
        List<ResponceSplSplaylist> GetSplPlaylistDateWiseLive(DataSplPlaylistDateWise data);

        [WebInvoke(Method = "POST", UriTemplate = "GetAllPlaylistScheduleSongs", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get All Playlist Schedule Songs")]
        string[] GetAllPlaylistScheduleSongs(DataCustomerTokenId data);

        [WebInvoke(Method = "POST", UriTemplate = "UpdateFCMId", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Update FCM Id")]
        ResponseTokenCrashLog UpdateFCMId(DataTokenFCMID data);

        [WebInvoke(Method = "POST", UriTemplate = "SendNoti", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SendNoti")]
        ResponseTokenCrashLog SendNoti(ClsNoti data);

        [WebInvoke(Method = "POST", UriTemplate = "GetSplPlaylistTitlesLiveFixed", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("GetSplPlaylistTitlesLive   Fixed")]
        List<ResponceSplSplaylistTitle> GetSplPlaylistTitlesLiveFixed(DataSplPlaylistTile data);

        //======================================================================================
        //======================================================================================
        //============================= Mooov ==============================================
        //======================================================================================
        //======================================================================================


        [WebInvoke(Method = "POST", UriTemplate = "GetMoovSettings", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("GetMoovSettings")]
        List<ResponceSetting> GetMoovSettings(DataClientId data);

        [WebInvoke(Method = "POST", UriTemplate = "GetMoovSource", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("GetMoovSource")]
        List<ResponceMoovSource> GetMoovSource(PlaylistDetail data);

        [WebInvoke(Method = "POST", UriTemplate = "GetPlaylistSchedule", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("GetPlaylistSchedule")]
        List<ResponcePlaylistMoov> GetPlaylistSchedule(DataMooovPlaylist data);


        [WebInvoke(Method = "POST", UriTemplate = "PlayerRegistration", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("PlayerRegistration")]
        ResponcePlayerRegistration PlayerRegistration(DataPlayerRegistration data);

        [WebInvoke(Method = "POST", UriTemplate = "ClientMessage", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("ClientMessage")]
        ResponcePlayerRegistration ClientMessage(DataClientMessage data);

        //======================================================================================
        //======================================================================================
        //============================= Web Panel ==============================================
        //======================================================================================
        //======================================================================================

        [WebInvoke(Method = "POST", UriTemplate = "FillQueryCombo", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Fill Query Combo")]
        List<ResComboQuery> FillQueryCombo(ReqComboQuery data);

        [WebInvoke(Method = "POST", UriTemplate = "FillTokenInfo", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Fill Token Info")]
        List<ResTokenInfo> FillTokenInfo(ReqTokenInfo data);

        [WebInvoke(Method = "POST", UriTemplate = "FillTokenContent", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillTokenContent")]
        ResToken FillTokenContent(ReqToken data);

        [WebInvoke(Method = "POST", UriTemplate = "SaveTokenInformation", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveTokenInformation")]
        ResResponce SaveTokenInformation(ReqSaveTokenInfo data);

        [WebInvoke(Method = "POST", UriTemplate = "ResetToken", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("ResetToken")]
        ResResponce ResetToken(ReqResetToken data);

        [WebInvoke(Method = "POST", UriTemplate = "UpdateTokenSchedule", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("UpdateTokenSchedule")]
        ResResponce UpdateTokenSchedule(ReqUpdateSchedule data);


        [WebInvoke(Method = "GET", UriTemplate = "FillCustomer", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillCustomer")]
        List<ResCustomerList> FillCustomer();

        [WebInvoke(Method = "POST", UriTemplate = "SaveCustomer", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveCustomer")]
        ResResponce SaveCustomer(RegCustomer data);

        [WebInvoke(Method = "POST", UriTemplate = "EditClickCustomer", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("EditClickCustomer")]
        RegCustomer EditClickCustomer(ReqTokenInfo data);

        [WebInvoke(Method = "POST", UriTemplate = "DeleteCustomer", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("DeleteCustomer")]
        ResResponce DeleteCustomer(ReqTokenInfo data);

        [WebInvoke(Method = "GET", UriTemplate = "BestOf", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("BestOf")]
        ResBestOf BestOf();

        [WebInvoke(Method = "POST", UriTemplate = "PlaylistSong", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("PlaylistSong")]
        List<ResPlaylistSongList> PlaylistSong(ReqPlaylistSongList data);

        [WebInvoke(Method = "POST", UriTemplate = "SaveBestPlaylist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveBestPlaylist")]
        ResResponce SaveBestPlaylist(ReqSaveBestPlaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "AddPlaylistSong", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("AddPlaylistSong")]
        ResResponce AddPlaylistSong( ReqAddPlaylistSong  data);

        [WebInvoke(Method = "POST", UriTemplate = "CommanSearch", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("CommanSearch")]
        List<ResSongList> CommanSearch(ReqCommonSearch data);

        [WebInvoke(Method = "POST", UriTemplate = "DeleteTitle", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("DeleteTitle")]
        ResResponce DeleteTitle(ReqDeletePlaylistSong data);

        [WebInvoke(Method = "POST", UriTemplate = "SavePlaylist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SavePlaylist")]
        ResResponce SavePlaylist(ReqSavePlaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "SavePlaylistFromBestOf", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SavePlaylistFromBestOf")]
        ResResponce SavePlaylistFromBestOf(ReqSavePlaylistFromBestOff data);

        [WebInvoke(Method = "POST", UriTemplate = "Playlist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Playlist")]
        List<ResPlaylist> Playlist(ReqPlaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "SongList", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SongList")]
        List<ResSongList> SongList(ReqCommonSearch data);

        [WebInvoke(Method = "POST", UriTemplate = "SaveSF", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveSF")]
        ResResponce SaveSF(ReqSF data);

        [WebInvoke(Method = "POST", UriTemplate = "FillSF", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillSF")]
        List<ResFillSF> FillSF(ReqFillSF data);

        [WebInvoke(Method = "POST", UriTemplate = "DeleteTokenSch", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("DeleteTokenSch")]
        ResResponce DeleteTokenSch(ReqDeleteSF data);

        [WebInvoke(Method = "POST", UriTemplate = "FillSearchAds", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillSearchAds")]
        List<ResTokenAds> FillSearchAds(ReqSearchAds data);

        [WebInvoke(Method = "POST", UriTemplate = "SaveAdsAndUploadFile", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveAdsAndUploadFile")]
        ResResponce SaveAdsAndUploadFile();

        [WebInvoke(Method = "POST", UriTemplate = "FillTokenInfoAds", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillTokenInfoAds")]
        List<ResTokenInfo> FillTokenInfoAds(ReqTokenInfoAds data);

        [WebInvoke(Method = "POST", UriTemplate = "FillSaveAds", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillSaveAds")]
        ResUpdateAds FillSaveAds(ReqAdsId data);

        [WebInvoke(Method = "POST", UriTemplate = "UpdateAds", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("UpdateAds")]
        ResResponce UpdateAds(ReqAds data);

        [WebInvoke(Method = "POST", UriTemplate = "DeleteAds", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("DeleteAds")]
        ResResponce DeleteAds(ReqAdsId data);

        [WebInvoke(Method = "POST", UriTemplate = "SavePrayer", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SavePrayer")]
        ResResponce SavePrayer(ReqPrayer data);

        [WebInvoke(Method = "POST", UriTemplate = "FillSearchPayer", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillSearchPayer")]
        List<ResSearchPrayer> FillSearchPayer(ReqSearchPrayer data);

        [WebInvoke(Method = "POST", UriTemplate = "DeletePrayer", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("DeletePrayer")]
        ResResponce DeletePrayer(ResDeletePrayer data);

        [WebInvoke(Method = "POST", UriTemplate = "uLg", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("uLg")]
        ResResponce uLg(ReqLg data);

        [WebInvoke(Method = "POST", UriTemplate = "GetCustomerTokenDetailSummary", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("GetCustomerTokenDetailSummary")]
        ResDashboard GetCustomerTokenDetailSummary(ReqDashboard data);

        [WebInvoke(Method = "POST", UriTemplate = "GetFCMID", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("GetFCMID")]
        ResResponce GetFCMID(DataCustomerTokenId data);

        [WebInvoke(Method = "POST", UriTemplate = "FillUserList", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillUserList")]
        List<ResUser> FillUserList(ReqTokenInfo data);

        [WebInvoke(Method = "POST", UriTemplate = "EditUser", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("EditUser")]
        ResUser EditUser(ReqUserInfo data);

        [WebInvoke(Method = "POST", UriTemplate = "DeleteUser", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("DeleteUser")]
        ResResponce DeleteUser(ReqUserInfo data);

        [WebInvoke(Method = "POST", UriTemplate = "SaveUpdateUser", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveUpdateUser")]
        ResResponce SaveUpdateUser(ResUser data);

        [WebInvoke(Method = "POST", UriTemplate = "CustomerLogin", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("CustomerLogin")]
        ResResponce CustomerLogin(ReqLg data);

        [WebInvoke(Method = "POST", UriTemplate = "FillPlayedSongsLog", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillPlayedSongsLog")]
        List<ResPlayerLog> FillPlayedSongsLog(ReqPlayerLog data);

        [WebInvoke(Method = "POST", UriTemplate = "FillPlayedAdsLog", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillPlayedAdsLog")]
        List<ResPlayerLog> FillPlayedAdsLog(ReqPlayerLog data);


        [WebInvoke(Method = "POST", UriTemplate = "GetSplPlaylistVideo1", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Get Speical Playlist Video 1")]
        List<ResponceSplSplaylist> GetSplPlaylistVideo1(DataSplPlaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "CustomerLoginDetail", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("CustomerLoginDetail")]
        ResResponce CustomerLoginDetail(ReqTokenInfo data);

        [WebInvoke(Method = "POST", UriTemplate = "DeletePlaylist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("DeletePlaylist")]
        ResResponce DeletePlaylist(ReqDeletePlaylistSong data);

        [WebInvoke(Method = "POST", UriTemplate = "SaveFormat", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveFormat")]
        ResResponce SaveFormat(ReqSaveFormat data);

        [WebInvoke(Method = "POST", UriTemplate = "SaveCopySchedule", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveCopySchedule")]
        ResResponce SaveCopySchedule(ReqCopySchedule data);

        [WebInvoke(Method = "POST", UriTemplate = "UploadImage", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [FaultContract(typeof(FaultException))]
        [Description("UploadImage")]
        ResResponce UploadImage();


        [WebInvoke(Method = "POST", UriTemplate = "SettingPlaylist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SettingPlaylist")]
        ResResponce SettingPlaylist(ReqSettingPlaylistSong data);

        [WebInvoke(Method = "POST", UriTemplate = "UpdatePlaylistSRNo", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("UpdatePlaylistSRNo")]
        ResResponce UpdatePlaylistSRNo(ReqUpdatePlaylistSRNo data);


        [WebInvoke(Method = "POST", UriTemplate = "SaveModifyLogs", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveModifyLogs")]
        ResResponce SaveModifyLogs(ReqSaveModifyLogs data);

        [WebInvoke(Method = "POST", UriTemplate = "FillAdminLogs", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillAdminLogs")]
        List<ResAdminLogs> FillAdminLogs(ReqTokenInfo data);

        [WebInvoke(Method = "POST", UriTemplate = "GetGenreList", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("GetGenreList")]
        List<GenreList> GetGenreList(ReqGenreList data);

        [WebInvoke(Method = "POST", UriTemplate = "NewSavePlaylist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("NewSavePlaylist")]
        ResResponce NewSavePlaylist(List<ReqNewSavePlaylist> data);

        [WebInvoke(Method = "POST", UriTemplate = "SaveAdPlaylist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveAdPlaylist")]
        ResResponce SaveAdPlaylist(ReqPlaylistAd data);

        [WebInvoke(Method = "POST", UriTemplate = "FillAdPlaylist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("FillAdPlaylist")]
        List<ResFillSF> FillAdPlaylist(ReqFillAdPlaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "DeleteFormat", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("DeleteFormat")]
        ResResponce DeleteFormat(ReqDeleteFormatId data);

        [WebInvoke(Method = "POST", UriTemplate = "UpdateAppLogo", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("UpdateAppLogo")]
        ResResponce UpdateAppLogo(RegUpdateAppLogo data);

        [WebInvoke(Method = "POST", UriTemplate = "SetOnlineIndicator", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SetOnlineIndicator")]
        ResResponce SetOnlineIndicator(RegSetOnlineIndicator data);

        [WebInvoke(Method = "POST", UriTemplate = "ForceUpdate", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("ForceUpdate")]
        ResResponce ForceUpdate(ReqFillAdPlaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "FillTokenInfo_formatANDplaylist", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("Fill Token Info")]
        List<ResTokenInfo> FillTokenInfo_formatANDplaylist(Reg_formatANDplaylist data);

        [WebInvoke(Method = "POST", UriTemplate = "ClientRegistration", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("ClientRegistration")]
        ResponseTokenCrashLog ClientRegistration(DataClientRegistration data);


        [WebInvoke(Method = "POST", UriTemplate = "DeleteLogo", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("DeleteLogo")]
        ResResponce DeleteLogo(ReqDeleteLogo data);

        [WebInvoke(Method = "POST", UriTemplate = "UploadSheet", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [FaultContract(typeof(FaultException))]
        [Description("UploadSheet")]
        ResResponce UploadSheet();

        [WebInvoke(Method = "POST", UriTemplate = "RenewPayment", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("RenewPayment")]
        ResponseTokenCrashLog RenewPayment(DataRenewPayment data);

        [WebInvoke(Method = "POST", UriTemplate = "SaveGenre", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(FaultException))]
        [Description("SaveGenre")]
        ResResponce SaveGenre(ReqSaveGenre data);

    }
}
