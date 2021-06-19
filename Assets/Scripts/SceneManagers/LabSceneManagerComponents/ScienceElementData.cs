using System.Collections;
using System.Collections.Generic;

public class ScienceElementData
{


    public IDictionary<string, string> tagToDisplayName = new Dictionary<string, string>();


    public ScienceElementData() {
        tagToDisplayName.Add(Constants.SE_NONE_TAG, Constants.SE_NONE_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_WATER_TAG, Constants.SE_WATER_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_SALT_TAG, Constants.SE_SALT_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_SALINE_TAG, Constants.SE_SALINE_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_STEAM_TAG, Constants.SE_STEAM_DISPLAY_NAME);
    }


}
