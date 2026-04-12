using System.ComponentModel.DataAnnotations;

namespace CommonLib1.Models.__Base__;

public class BaseSvcStructure
{
    [Required] public string           Uid           { get; set; } = "";
               public decimal          Cost          { get; set; }
               public string           Details       { get; set; } = "";
               public List<BaseDesign> Designs       { get; set; } = [];
               public BaseSchedSlot    ScheduleSlots { get; set; } = new BaseSchedSlot();
}
