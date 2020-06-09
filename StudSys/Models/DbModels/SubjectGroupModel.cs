using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudSys.Models.DbModels
{
    public class SubjectGroupModel
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
    }
}
