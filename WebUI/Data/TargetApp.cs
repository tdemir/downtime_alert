using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebUI.Data
{
    public class TargetApp : BaseTable
    {

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Url { get; set; }

        [Required]
        [MaxLength(256)]
        public string Interval { get; set; }

        //5 4 * * *
        //minute hour day(month) month day(week)


        [NotMapped]
        public string IntervalFormatted
        {
            get
            {
                //CronExpressionDescriptor.ExpressionDescriptor.GetDescription("* * * * *");
                try
                {
                    return CronExpressionDescriptor.ExpressionDescriptor.GetDescription(Interval);
                }
                catch
                {

                }
                return string.Empty;
            }
        }

        


        public bool Status { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string JobId
        {
            get
            {
                return this.Id.ToString("N");
            }
        }
    }
}
