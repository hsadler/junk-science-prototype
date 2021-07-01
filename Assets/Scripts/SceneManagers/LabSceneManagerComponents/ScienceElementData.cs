using System.Collections;
using System.Collections.Generic;

public class ScienceElementData
{


    public IDictionary<string, string> tagToDisplayName = new Dictionary<string, string>();


    public ScienceElementData()
    {
        tagToDisplayName.Add(Constants.SE_NONE_TAG, Constants.SE_NONE_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_WATER_TAG, Constants.SE_WATER_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_SALT_TAG, Constants.SE_SALT_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_SALINE_TAG, Constants.SE_SALINE_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_STEAM_TAG, Constants.SE_STEAM_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_EARTH_TAG, Constants.SE_EARTH_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_MUD_TAG, Constants.SE_MUD_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_STONE_TAG, Constants.SE_STONE_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_ORE_TAG, Constants.SE_ORE_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_SLAG_TAG, Constants.SE_SLAG_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_MOLTEN_METAL_TAG, Constants.SE_MOLTEN_METAL_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_METAL_TAG, Constants.SE_METAL_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_LAVA_TAG, Constants.SE_LAVA_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_CLAY_TAG, Constants.SE_CLAY_DISPLAY_NAME);
        tagToDisplayName.Add(Constants.SE_BRICK_TAG, Constants.SE_BRICK_DISPLAY_NAME);
    }


}
