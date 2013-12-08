using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Helpers;
using UltimateSecuritySurvey.Models;

namespace UltimateSecuritySurvey.Utility
{
    public class USSRoleProvider : RoleProvider
    {        
        String[] ROLES = { "Teacher", "Student" };

        public override string[] GetRolesForUser(string userName)
        {
            SecuritySurveyEntities db = new SecuritySurveyEntities();

            string[] role = new string[]{};

            UserAccount user = db.UserAccounts.FirstOrDefault(x => x.userName == userName);
            if (user != null)
            {
                string myRole = user.isTeacher ? ROLES[0] : ROLES[1];
                role = new string[] { myRole };
            }

            return role;
        }

        public override bool IsUserInRole(string userName, string roleName)
        {
            SecuritySurveyEntities db = new SecuritySurveyEntities();

            bool result = false;

            UserAccount user = db.UserAccounts.FirstOrDefault(x => x.userName == userName);
            if (user != null)
            {
                result = user.isTeacher == (roleName == ROLES[0]);
            }

            return result;
        }

        public override string[] GetAllRoles()
        {
            return ROLES;
        }


        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }

        public override void CreateRole(string roleName) { throw new NotImplementedException(); }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) { throw new NotImplementedException(); }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) { throw new NotImplementedException(); }

        public override string[] GetUsersInRole(string roleName) { throw new NotImplementedException(); }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }

        public override bool RoleExists(string roleName) { throw new NotImplementedException(); }
        
    }
}