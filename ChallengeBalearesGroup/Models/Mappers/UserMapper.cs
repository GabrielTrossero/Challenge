using ChallengeBalearesGroup.Models.DTO;
using ChallengeBalearesGroup.Repository;

namespace ChallengeBalearesGroup.Models.Mappers
{
    public class UserMapper
    {
        public static UserRegisterDTO MapUserToUserRegisterDTO(User user)
        {
            var userRegisterDTO = new UserRegisterDTO();

            foreach (var prop in typeof(User).GetProperties())
            {
                var value = prop.GetValue(user);
                if (value != null)
                {
                    typeof(UserRegisterDTO)
                        .GetProperty(prop.Name)?
                        .SetValue(userRegisterDTO, value);
                }
            }

            return userRegisterDTO;
        }


        public static User MapUserRegisterDTOToUser(UserRegisterDTO userRegisterDTO)
        {
            var user = new User();

            foreach (var prop in typeof(UserRegisterDTO).GetProperties())
            {
                var value = prop.GetValue(userRegisterDTO);
                if (value != null)
                {
                    typeof(User)
                        .GetProperty(prop.Name)?
                        .SetValue(user, value);
                }
            }

            return user;
        }


        public static User MapUserLoginDTOToUser(UserLoginDTO userLoginDTO)
        {
            var user = new User();

            foreach (var prop in typeof(UserLoginDTO).GetProperties())
            {
                var value = prop.GetValue(userLoginDTO);
                if (value != null)
                {
                    typeof(User)
                        .GetProperty(prop.Name)?
                        .SetValue(user, value);
                }
            }

            return user;
        }


        public static User MapUserLogoutDTOToUser(UserLogoutDTO userLogoutDTO)
        {
            var user = new User();

            foreach (var prop in typeof(UserLogoutDTO).GetProperties())
            {
                var value = prop.GetValue(userLogoutDTO);
                if (value != null)
                {
                    typeof(User)
                        .GetProperty(prop.Name)?
                        .SetValue(user, value);
                }
            }

            return user;
        }
    }
}
