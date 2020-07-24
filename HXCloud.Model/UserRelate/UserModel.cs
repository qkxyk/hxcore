using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.Model
{
    public class UserModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }//用户标示
        [Required]
        [StringLength(25, ErrorMessage = "用户名长度不能超过25个字符", MinimumLength = 6)]
        public string Account { get; set; }//用户账号
        [Required]
        public string Password { get; set; }//用户密码
        public string Phone { get; set; }//联系电话
        //public string FirstName { get; set; }//用户姓名
        //public string LastName { get; set; }//用户名字
        public string UserName { get; set; }
        //public DateTime Create { get; set; }//用户创建时间
        public Nullable<DateTime> LastLogin { get; set; }//上次登录时间
        public string Email { get; set; }//用户邮件

        //public bool IsAdmin { get; set; } = false;//是否管理员
        public string Picture { get; set; }

        public string Salt { get; set; }//用户加密字符串
        public UserStatus Status { get; set; }//用户状态,未激活、有效用户、无效用户
        public string GroupId { get; set; }//组织编号
        public virtual GroupModel Group { get; set; }//组织信息
        public virtual ICollection<UserDepartmentModel> UserDepartments { get; set; }//部门信息
        public virtual ICollection<UserRoleModel> UserRoles { get; set; }//角色信息
    }
    //用户状态,未激活、有效用户、无效用户
    public enum UserStatus
    {
        InActive, Valid, InValid
    }
}
