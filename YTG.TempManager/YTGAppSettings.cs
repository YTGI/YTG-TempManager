// --------------------------------------------------------------------------------
/*  Copyright © 2024, Yasgar Technology Group, Inc.
	Any unauthorized review, use, disclosure or distribution is prohibited.

	Purpose: Class for holding AppSetting values

	Description: 

*/
// --------------------------------------------------------------------------------

namespace YTG.TempManager
{
    public class YTGAppSettings
    {

        public string? SourceFolder { get; set; }
        public string? DestinationFolder { get; set; }
        public int ArchiveLookbackDays { get; set; } = 14;
        public string? ApplicationUniqueId { get; set; }
        public string? ApplicationShortName { get; set; }
    }
}
