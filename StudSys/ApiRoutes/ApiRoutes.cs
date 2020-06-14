using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRoutes 
{
    public static class Identity
    {
        public const string Login = "api/identity/login";
        public const string Register = "api/identity/registration";
        public const string GetUserInfo = "api/{username}/info";
        public const string CheckJWT = "api/identity/auth";
    }

    public static class Test
    {
        public const string Text = "api/test/getinfo";

    }

    public static class Group
    {
        public const string CreateGroup = "api/group/create";
        public const string MemberList = "api/group/members";
        public const string GetAllSubjectGroup = "api/group/subjects";
    }

    public static class ClientData
    {
        public const string ChangeGroup = "api/{username}/change_group";

    }

    public static class Subject
    {
        public const string CreateSubject = "api/subject/create";
        public const string LinkGroupToSubject = "api/subject/addgroup";
    }

    public static class Lesson
    {
        public const string CreateSubjectLesson = "api/lesson/create";
        public const string LessonVisit = "api/group/lesson/visit";
    }
}
