using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class GameOptions : OptionsMenu
{
    protected override void CreateMenu(){

        FullScreenMode[] modes = (FullScreenMode[])System.Enum.GetValues(typeof(FullScreenMode));

        OptionsMenuCreator.CreateRowHeading(container, new LocalizedString("options_game_heading_video", false));
        
        OptionsMenuCreator.Item<TMP_Dropdown> fsMode = OptionsMenuCreator.CreateRow<TMP_Dropdown>(
            container, OptionsMenuCreator.ItemType.DROPDOWN,
            new LocalizedString("options_game_fsmode", false),
            modes.Select((s)=>new LocalizedString("options_"+s.ToString().ToLower(), false)).ToArray()
        );

        fsMode.itemClass.value = (int)Screen.fullScreenMode;

        fsMode.itemClass.onValueChanged.AddListener((index) => {
            FullScreenMode selected = modes[index];
            Screen.fullScreenMode = selected;
        });

        Resolution[] resolutions = Screen.resolutions;
        
        OptionsMenuCreator.Item<TMP_Dropdown> resolution = OptionsMenuCreator.CreateRow<TMP_Dropdown>(
            container, OptionsMenuCreator.ItemType.DROPDOWN,
            new LocalizedString("options_game_resolution", false),
            resolutions.Select((res)=>new LocalizedString(res.ToString())).ToArray()
        );
        Resolution currRes = Screen.currentResolution;
        int currResIndex = resolutions.ToList().IndexOf(currRes);
        resolution.itemClass.value = currResIndex >= 0 && currResIndex < resolutions.Length ? currResIndex : 0;

        resolution.itemClass.onValueChanged.AddListener((index)=>{
            Resolution selected = resolutions[index];
            Screen.SetResolution(selected.width, selected.height, Screen.fullScreenMode);
        });

        OptionsMenuCreator.CreateRowHeading(container, new LocalizedString("options_game_heading_sound", false));

        OptionsMenuCreator.Item<Slider> masterVolume = OptionsMenuCreator.CreateRow<Slider>(
            container, OptionsMenuCreator.ItemType.SLIDER,
            new LocalizedString("options_game_mastervolume", false)
        );
        OptionsMenuCreator.Item<Slider> sfxVolume = OptionsMenuCreator.CreateRow<Slider>(
            container, OptionsMenuCreator.ItemType.SLIDER,
            new LocalizedString("options_game_sfxvolume", false)
        );
        OptionsMenuCreator.Item<Slider> musicVolume = OptionsMenuCreator.CreateRow<Slider>(
            container, OptionsMenuCreator.ItemType.SLIDER,
            new LocalizedString("options_game_musicvolume", false)
        );

        OptionsMenuCreator.CreateRowHeading(container, new LocalizedString("options_game_heading_other", false));

        KeyValuePair<string, string>[] langsAndKeys = Locales.LoadMainLocalesKeys();
        OptionsMenuCreator.Item<TMP_Dropdown> languages = OptionsMenuCreator.CreateRow<TMP_Dropdown>(
            container, OptionsMenuCreator.ItemType.DROPDOWN,
            new LocalizedString("options_game_languages", false),
            langsAndKeys.Select((lnk)=>new LocalizedString(lnk.Value)).ToArray()
        );
        OptionsMenuCreator.Item<Toggle> compress = OptionsMenuCreator.CreateRow<Toggle>(
            container, OptionsMenuCreator.ItemType.TOGGLE,
            new LocalizedString("options_game_compress", false)
        );
        compress.itemClass.isOn = PlayerPrefs.GetInt("compress", 1) != 0;
        compress.itemClass.onValueChanged.AddListener((value) => {
            PlayerPrefs.SetInt("compress",value ? 1 : 0);
        });
    }
}
