using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Models.DbModels
{
    [Table("SubjectLessons")]
    public class SubjectLessonModel
    {
        public int Id { get; set; }
        public int SubjectGroupId { get; set; }

        public DateTime Date { get; set; }

        public int TypeId { get; set; }

    }
}
