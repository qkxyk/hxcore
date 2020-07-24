namespace HXCloud.Common
{
    public enum UserOperateType
    {
        UserNameExist,//用户名已存在
        UserNameError,//用户名不存在
        PasswordDiff,//两次密码不一致
        //用户登录
        LoginSuccess,//用户登录成功
        LoginFailure,//用户登录失败        
        PasswordError,//密码不正确
        //修改用户密码
        ChangePasswordSuccess,//修改用户密码成功
        ChangePasswordFailure,//修改密码失败
        ChangePasswordError,//新密码不符合要求

        //注册用户
        RegisterUserSuccess,//用户注册成功
        RegisterUserFailure,//用户注册失败
        RegisterUserPasswordError,//用户注册密码不符合要求

        //重置用户密码
        ResetUserSuccess,//重置用户密码成功
        ResetUserFailure,//重置用户密码失败
        //修改用户信息
        UpdateUserSuccess,//修改用户信息成功
        UpdateUserFailure//修改用户信息失败
    }

    public enum UserStatus
    {
        AuthenticatedAdmin,
        AuthentucatedUser,
        NonAuthenticatedUser
    }

    public enum UserType
    {
        Error,//用户或者密码不正确
        InValid,//无效用户，用户没有审核
        Valid,//合法用户
        NoRole,//没有分配角色的用户
        Admin,//管理员
        MasterAdmin//主管理员
    }
}