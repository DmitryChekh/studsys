using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Models.DbModels
{
    [Table("LessonVisits")]
    public class LessonVisitModel
    {
        
        public int Id { get; set; }
        public int LessonId { get; set; }

        public string UserId { get; set; }

        public bool Visited { get; set; }

        UserModel userModel { get; set; }

        SubjectLessonModel subjectLessonModel { get; set; }

    }
}
