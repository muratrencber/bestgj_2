public class LocalizedString
{
    public string keyOrText;
    public bool isMod;
    public bool isKey;

    public LocalizedString(string key, bool isMod) => Init(key, isMod, true);
    public LocalizedString(string text) => Init(text, false, false);

    public void Init(string kOT, bool iM, bool iK){
        keyOrText = kOT;
        isMod = iM;
        isKey = iK;
    }

    public string GetValue(params string[] fillIns){
        if(!isKey) return keyOrText;
        if(isMod){
            Locales.TryGetLineMod(keyOrText, out string line, fillIns);
            return line;
        } else{
            Locales.TryGetLineMain(keyOrText, out string line, fillIns);
            return line;
        }
    }
}
