namespace AppAppartamentiApi.Models
{
    using System;
    using System.Security.Principal;

    public partial class UserInfoMapper
    {
        public UserInfoMapper()
        {
        }

        public static UserInfoDto UserInfoToUserInfoDto(UserInfo userInfo, String email)
        {
            UserInfoDto userInfoDto = new UserInfoDto();
            userInfoDto.Cognome = userInfo.Cognome;
            userInfoDto.Nome = userInfo.Nome;
            userInfoDto.DataDiNascita = userInfo.DataDiNascita;
            userInfoDto.FotoProfilo = userInfo.FotoProfilo;
            userInfoDto.IdAspNetUser = userInfo.IdAspNetUser;
            userInfoDto.Email = email;
            userInfoDto.PhotoUrl = userInfo.PhotoUrl;
            userInfoDto.InstallationId = userInfo.InstallationId;
            userInfoDto.OsVersion = userInfo.OsVersion;
            return userInfoDto;
        }
    }
}
