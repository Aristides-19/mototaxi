# Configuration

Run these commands to enable ***Unity SmartMerge*** (replace with the correct path)
```bash
git config merge.tool unityyamlmerge &&
git config mergetool.unityyamlmerge.cmd '"C:\Program Files <MAY BE x86>\Unity\Hub\Editor\<UNITY VERSION>\Editor\Data\Tools\UnityYAMLMerge.exe" merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"' &&
git config mergetool.unityyamlmerge.trustExitCode false
```

When there is a conflict between Unity-related files when trying to do a merge, you
must execute to call Unity SmartMerge:
```bash
git mergetool
```

### Download [Git LFS](https://git-lfs.com/)

# Try-to
1. Use *\*.local.\** files to test things locally, like *TestScene.local.unity*.
2. Avoid working on same prefabs, scenes, etc. to avoid merging no-text files.
3. Avoid reuploading the same asset package.